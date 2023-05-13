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
			
			/*
			// tests
			data.output = "output.pfm";
			
			SceneLoader scene = new SceneLoader();
			TextureLoader whiteUniform = new UniformTextureLoader(Color4.White);

			
			var material = new MaterialLoader(Color4.Gold, 0.5f, 0.5f, 1f);

			scene.solidLoaders.Add(new SphereLoader(new Point3D(1, 2, 3), new Vector3(3, 2, 1), material, 1));
			scene.solidLoaders.Add(new PlaneLoader(new Point3D(0, 0, 0), new Vector3(0, 0, 0), material));

			scene.lightLoaders.Add(new PointLightLoader(new Point3D(5,5,5), Vector3.Zero, Color4.Azure, 10, 0.5f, 0.5f));
			scene.cameraLoaders.Add( new CameraLoader(Point3D.Zero, Vector3.Zero, Color4.DimGray, MathF.PI / 2, 780, 600));
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


            int startIndex = input.IndexOf("<variables>") + "<variables>\n".Length + 1;
            int endIndex = input.IndexOf("</variables>");

            string variableDefinitions = input.Substring(startIndex, endIndex - startIndex);

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
		/// Parses config file to scene and output path.
		/// </summary>
		/// <param name="args">Command line parameters</param>
		/// <param name="configFile">Path to config file</param>
		/// <returns>Returns tuple (initialized Scean, output path where to save rendered image)</returns>
		public static (Scene, string) ParseScene( string[] args, string configFile = "config.xml")
		{
			DataLoader loadedData = ParseParameters(args, configFile);

			return (loadedData.sceneLoader.CreateInstance(), loadedData.output);
		}

		public struct DataLoader
		{
			public string output;
			public SceneLoader sceneLoader;
		}
	}
}
