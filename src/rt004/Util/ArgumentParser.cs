using rt004.SceneObjects;
using OpenTK.Mathematics;
using System.Text;
using System.Xml.Serialization;
using rt004.Loading;
using rt004.Materials.Loading;
using rt004.Materials;
using System.Text.RegularExpressions;
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
				string? line = inputStream.ReadLine();

				if (line is not null && line.Length != 0)//skip empty lines
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




		/// <summary>
		/// Initialize variables from textReader
		/// </summary>
		/// <param name="reader">Reader to initialize variable from</param>
		/// <returns>Returns pair of TextReader contents without variable definitions and dictionary with variables and their contents</returns>
		/// <exception cref="ArgumentNullException">throws if the reader is empty</exception>
		/// <exception cref="ArgumentException">Throws if there is a problem with variable section delimitation</exception>
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


		/// <summary>
		/// Parses variables in XML format from string.
		/// </summary>
		/// <remarks>
		/// If variable is defined multiple times, then it is registered only as one, associated with last instance contents.
		/// </remarks>
		/// <param name="variableDefinitions">String contatining variable definitions in XML format. Should not contain variable start nor end tags.</param>
		/// <returns>Returns dictionary with variable names and asociated contents.</returns>
		/// <exception cref="ArgumentException"></exception>
		private static Dictionary<string, string> ParseVariables(string variableDefinitions)
		{
			Dictionary<string, string> result = new Dictionary<string, string>();

			string xmlTagPattern = @"\s*\b(\w+)\b.+\1\b";

			foreach (Match match in Regex.Matches(variableDefinitions, xmlTagPattern, RegexOptions.Singleline))
			{
				var variableName = match.Groups[1].Value;
				string variableContentPattern = $"{variableName}>" + @"\s*(.*)\s*" + $"</{variableName}";

				// Match variable content
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


		/// <summary>
		/// Replaces all variable instances in input with variable contents
		/// </summary>
		/// <param name="input">The whole input to replace variabels with</param>
		/// <param name="variableDefinitions">definitions of variables and ther contents</param>
		/// <returns>Returns input with replaced variables</returns>
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

		/// <summary>
		/// Data structure to facilitate loading from XML file
		/// </summary>
		public struct DataLoader
		{
			public string output;
			public RendererSettingsLoader rendererSettings;
			public SceneLoader sceneLoader;
		}
	}
}
