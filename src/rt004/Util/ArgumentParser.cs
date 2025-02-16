using OpenTK.Mathematics;
using System.Text;
using System.Xml.Serialization;
using rt004.Loading;
using rt004.Materials.Loading;
using System.Text.RegularExpressions;
using rt004.UtilLoading;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;

namespace rt004.Util
{
	enum ParsingStage
	{
		CommaLoading,
		ParameterLoading,
		ArgumentLoading
	}

	public static class ArgumentParser
	{
		/// <summary>
		/// Parses Comand line parameters. If parameter is not specified, then it uses XML configFile to define it.
		/// </summary>
		/// <param name="args">command line arguments</param>
		/// <param name="configFile">Default path to XML configuration file</param>
		/// <returns>loaded data</returns>
		/// <exception cref="ArgumentException">Thrown when argument is not found in default </exception>
		private static DataLoader ParseParameters(string[] args, string configFile)
		{

			var config = new Dictionary<string, string>()
			{
				// Default values of all supported parameters
				["config"] = configFile,
                ["output"] = RendererSettings.DefaultOutputFile,
            };


			StringReader reader = new StringReader( String.Join(" ", args));
            // parse command line arguments to potencialy change config File path
            Parse( reader, config);

			var serializer = new XmlSerializer(typeof(DataLoader));

			DataLoader data = new DataLoader();
			
			

			// parsing XML config file
			using (Stream stream = File.Open(config["config"], FileMode.Open))
			{
                (string inputWithoutVariables, Dictionary<string,string> variables) = InitializeVariables(new StreamReader(stream));
				var xmlData = replaceVariables(inputWithoutVariables, variables);

				var d = (DataLoader?)(serializer.Deserialize(new StringReader(xmlData)));

                data = d ?? throw new ArgumentException("Configurtion file is empty");
			}

			if ( config["output"] != RendererSettings.DefaultOutputFile )
				data.output = config["output"];

			return data;
		}


        private static Dictionary<string, string> Parse(StringReader reader, Dictionary<string, string> config)
		{
			ParsingStage currentLoading = ParsingStage.CommaLoading;
			string parameter = "";

            var delimiters = new HashSet<char>() { '-', '=', '\'', '"' };
            (var word, var delim) = ReadUntil(reader, delimiters);


			while(delim != -1 || (delim == -1 && currentLoading == ParsingStage.ArgumentLoading))
			{
				switch (currentLoading)
				{
					case ParsingStage.CommaLoading:
						if (delim == '-' && String.IsNullOrEmpty( word.Trim() ))
						{
							currentLoading = ParsingStage.ParameterLoading;

							(word, delim) = ReadUntil(reader, delimiters);
						}
						else 
							throw new ArgumentException("Expected -, but not found.");
						break;
					case ParsingStage.ParameterLoading:
						if (delim == '=')
						{
							parameter = word.Trim();
						}
						else
							throw new ArgumentException("Expected =, but not found.");

						currentLoading = ParsingStage.ArgumentLoading;
						//(word, delim) = ReadUntil(reader, delimiters);
						break;
					case ParsingStage.ArgumentLoading:
                        (word, delim) = ReadUntil(reader, delimiters);
                        if (delim == '"' || delim == '\'' )
						{
							if (!String.IsNullOrEmpty(word.Trim()))
								throw new ArgumentException("Argument name can not contain \" nor ' symobols outside the begining or the end");

							int previusDelimiter = delim;

							if (delim == '"')
								(word, delim) = ReadUntil(reader, '"');
							else
								(word, delim) = ReadUntil(reader, '\'');

							word = word.Trim();

							
							if (delim == -1)
								throw new ArgumentException($"Expected {(int)delim} but not found");

                            if (String.IsNullOrEmpty(word))
                                throw new ArgumentException("Argument can not be empty");

							if (word.Contains(' '))// word has space inside
								throw new ArgumentException("Argument should not contain spaces. Probably '-' is missing");


							config[parameter.ToLower()] = word.Trim();

							(word, delim) = ReadUntil(reader, delimiters);
                        }
						
						else if (delim == '-' || delim == -1)
						{
							config[parameter.ToLower()] = word.Trim();
							word = String.Empty;
						}
						else
							throw new ArgumentException($"Symbol {(delim == -1 ? "EndOfInput" : (char)delim) } not expected at the begining of an argument");
						
						
						currentLoading = ParsingStage.CommaLoading;
						break;
				}
			}

			return config;
		}



        private static (string word, int delim) ReadUntil(StringReader reader, char delimiter)
        {

            StringBuilder word = new StringBuilder();
            int c = reader.Read();

            while (delimiter != c && c != -1)
            {
                word.Append((char)c);
                c = reader.Read();
            }
            return (word.ToString(), c);
        }



        private static (string readString, int encounteredDelimiter) ReadUntil(StringReader reader, HashSet<char> delimiters)
		{

			StringBuilder word = new StringBuilder();
			int c = reader.Read();

            while ( c != -1 && !delimiters.Contains((char)c))
			{
				word.Append((char)c);
				c = reader.Read();
			}
			return (word.ToString(), c);
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
            string? input = reader.ReadToEnd();

			if (input == null)
				throw new ArgumentNullException("io input has been loaded");



            int startIndex = input.IndexOf("<variables>");
            int endIndex = input.IndexOf("</variables>");

			// contains at least 3 <variables> or </variables> tags than throw an error
			if (input.IndexOf("<variables>", startIndex + 1) != -1 || input.IndexOf("</variables>", endIndex + 1) != -1)
				throw new ArgumentException("There is to much variables tegs");

			string variableDefinitions = "";
			if (startIndex != -1 && endIndex != -1)
			{
				startIndex +="<variables>\n".Length + 1;

                variableDefinitions = input.Substring(startIndex, endIndex - startIndex);
			}
			else if (startIndex != -1 ^ endIndex != -1)
				throw new ArgumentException("section for variables is not correctly delimited");

            Dictionary<string, string> variables = ParseVariables(variableDefinitions);
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
		/// <param name="configFile">Default path to config file</param>
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
