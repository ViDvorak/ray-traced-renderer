using rt004.SceneObjects;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using rt004.SceneObjects.Loading;
using rt004.Materials.Loading;
using rt004.Materials;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using rt004.UtilLoading;
using rt004.MaterialLoading;

namespace rt004.Util
{
	public static class ArgumentParser
	{
		/// <summary>
		/// Parses Comand line parameters. If parameter is not specified, then it uses XML configFile to define it.
		/// </summary>
		/// <param name="args">command line arguments</param>
		/// <param name="configFile">default Path to XML configuration file</param>
		/// <returns>loaded data</returns>
		/// <exception cref="ArgumentException">Thrown when argument is not found in default </exception>
		private static DataLoader ParseParameters(string[] args, string configFile)
		{

			var config = new Dictionary<string, string>()
			{
				// Default values of all supported parameters
				["config"] = configFile,
				["output"] = "output.pfm",
			};

			var argumentLine = String.Join('\n', args);
			byte[] argumentLineBytes = Encoding.Unicode.GetBytes(argumentLine);

			// parse command line arguments to potencialy change config File path
			using (var reader = new StreamReader(new MemoryStream(argumentLineBytes)))
			{
				config = Parse(reader, config);
			}

			var serializer = new XmlSerializer(typeof(DataLoader));

			DataLoader data = new DataLoader();
			
			
			// tests
			data.output = "output.pfm";
			
			SceneLoader scene = new SceneLoader();
			TextureLoader whiteUniform = new UniformTextureLoader(Color4.White);

			/*
			var rendererSetting = new RendererSettingsLoader();
			rendererSetting.lightModel = "PhongModel";
			var materialYellow = new PhongMaterialLoader(new Color4(1f, 1f, 0.2f, 1f), 0.2f, 0.5f, 10f, 1f);
			var materialBlue = new PhongMaterialLoader	(new Color4(0.2f, 0.3f, 1f, 1f), 0.5f, 0.5f, 150f, 1f);
			var materialRed = new PhongMaterialLoader	(new Color4(0.8f, 0.2f, 0.2f, 1f), 0.4f, 0.5f, 80f, 1f);
			var materialGold = new PhongMaterialLoader	(new Color4(0.3f, 0.2f, 0f, 1f), 0.8f, 0.5f, 400f, 1f);
			var materialWhite = new PhongMaterialLoader	(new Color4(0.9f, 0.9f, 0.9f, 1f), 0.4f, 0.5f, 80f, 1f);

			scene.solidLoaders.Add(new SphereLoader(new Point3D(0, 0, 0), new Vector3(0, 0, 0), materialYellow, 1f));
			scene.solidLoaders.Add(new SphereLoader(new Point3D(1.4f, -0.7, -0.5), new Vector3(0, 0, 0), materialBlue, 0.6f));
			scene.solidLoaders.Add(new SphereLoader(new Point3D(-0.7f, 0.7f, -0.8f), new Vector3(0, 0, 0), materialRed, 0.1f));
			scene.solidLoaders.Add(new SphereLoader(new Point3D(1.5f, 0.6f, 0.1f), new Vector3(0, 0, 0), materialGold, 0.5f));
			scene.solidLoaders.Add(new PlaneLoader (new Point3D(0, -1.3f, 0), new Vector3(0, 0, 0), materialWhite));

			scene.ambientLightIntensity = 1f;
			scene.ambientLightColor = Color4.White;
			scene.lightLoaders.Add(new PointLightLoader(new Point3D(-10f, 8f, -6f), Vector3.Zero, Color4.White, 1, 1f, 1f));
			scene.lightLoaders.Add(new PointLightLoader(new Point3D( 0f, 20f, -3f), Vector3.Zero, new Color4(0.3f, 0.3f, 0.3f, 1f), 1, 1f, 1f));

			scene.cameraLoaders.Add( new CameraLoader(new Point3D(0.6f, 0f, -5.6f), new Vector3(0f, -0.03f, 0), new Color4(0.1f, 0.2f, 0.3f, 0f), MathF.PI * 4f / 9f, 600, 450));
			data.sceneLoader = scene;

			serializer.Serialize(Console.Out, data);
			*/


			// parsing XML config file
			using (Stream stream = File.Open(config["config"], FileMode.Open))
			{
                (string inputWithoutVariables, Dictionary<string,string> variables) = InitializeVariables(new StreamReader(stream));
				var xmlData = replaceVariables(inputWithoutVariables, variables);

				data = (DataLoader)serializer.Deserialize(new StringReader(xmlData));
			}

			data.output = config["output"];

			return data;
		}

		/// <summary>
		/// Parses parameters from streamReader.
		/// </summary>
		/// <param name="inputStream">Stream to parse</param>
		/// <param name="config">configuration dictionary to save the data</param>
		/// <returns>Returns input config dictionary with filled config parameters.</returns>
		/// <exception cref="ArgumentException">thrown when there is any problem with format of arguments</exception>
		private static Dictionary<string, string> Parse(StreamReader inputStream, Dictionary<string, string> config)
		{
			uint lineNumber = 0;
			while (!inputStream.EndOfStream)
			{
				lineNumber++;
				string line = inputStream.ReadLine();

				if (line.Length != 0)//skip empty lines
				{
					line = line.Trim();
					string[] paramValues = line.Split('=', 2, StringSplitOptions.TrimEntries);
					for (int i = 0; i < paramValues.Length; i++)
					{
						paramValues[i] = paramValues[i].Trim('"');
					}


					if (paramValues.Length != 2)
					{
						throw new ArgumentException($"Argument format problem at line {lineNumber}", line);
					}
					else if (config.ContainsKey(paramValues[0]))
					{
						config[paramValues[0].ToLower()] = paramValues[1];
					}
					else
					{
						throw new ArgumentException($"unknown parameter name at line {lineNumber}", paramValues[0]);
					}
				}
			}
			return config;
		}





		private static (string, Dictionary<string,string>) InitializeVariables(TextReader reader)
		{
            Dictionary<string, string> variables = new Dictionary<string, string>();

            string? input = reader.ReadToEnd();

			if (input == null)
				throw new ArgumentNullException("io input has been loaded");


            int startIndex = input.IndexOf("<variables>");
            int endIndex = input.IndexOf("</variables>");


			string variableDefinitions = "";
			if (startIndex != -1 && endIndex != -1)
			{
				startIndex +="<variables>\n".Length + 1;

                variableDefinitions = input.Substring(startIndex, endIndex - startIndex);
			}
			else if (startIndex != -1 ^ endIndex != -1)
				throw new ArgumentException("section for variables is not correctly delimited");

            variables = ParseVariables(variableDefinitions);
			return (input, variables);
		}


		private static Dictionary<string, string> ParseVariables(string variableDefinitions)
		{
			Dictionary<string, string> result = new Dictionary<string, string>();

			string xmlTagPattern = @"\s*\b(\w+)\b.+\1\b";

			foreach (Match match in Regex.Matches(variableDefinitions, xmlTagPattern, RegexOptions.Singleline))
			{
				var variableName = match.Groups[1].Value;
				string variableContentPattern = $"{variableName}>" + @"\s*(.*)\s*" + $"</{variableName}";

				//Match variable content
				Match variableContentMatch = Regex.Match(match.Groups[0].Value, variableContentPattern, RegexOptions.Singleline);
				if (variableContentMatch.Success)
				{
					result.Add(variableName, variableContentMatch.Groups[1].Value);
				}
				else
				{
					throw new ArgumentException("variable content not found");
				}
			}
			return result;
		}



		public static string replaceVariables(string input, Dictionary<string, string> variableDefinitions)
		{
			foreach ( (string name, string content) in variableDefinitions)
			{
				input = Regex.Replace(input, $"<{name}/>", content);
			}
			return input;
		}




		/// <summary>
		/// Parses config file to scene, output path and saves rendererSettings.
		/// </summary>
		/// <param name="args">Command line parameters</param>
		/// <param name="configFile">Path to config file</param>
		/// <returns>Returns tuple (initialized Scean, output path where to save rendered image)</returns>
		public static (Scene, string) ParseScene( string[] args, string configFile = "config.xml")
		{
			DataLoader loadedData = ParseParameters(args, configFile);

			loadedData.rendererSettings.SaveLoadedSettings();

			return (loadedData.sceneLoader.CreateInstance(), loadedData.output);
		}

		public struct DataLoader
		{
			public string output;
			public RendererSettingsLoader rendererSettings;
			public SceneLoader sceneLoader;
		}
	}
}
