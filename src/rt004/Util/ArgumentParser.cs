using System.Collections.Generic;
using System.Text;

namespace rt004.Util
{
    internal static class ArgumentParser
    {
        /// <summary>
        /// Parses Comand line parameters and if parameter is not specified then it useis configFile to define.
        /// </summary>
        /// <param name="args">command line arguments</param>
        /// <param name="defaultConfigFile">default Path to configuration file</param>
        /// <returns>Inicialized config dictionary containing only specified Keys</returns>
        /// <exception cref="ArgumentException">Thrown when argument is not found in default </exception>
        public static Dictionary<string, string> ParseParameters(string[] args, string defaultConfigFile = "config.txt")
        {
            var config = new Dictionary<string, string>()
            {
                // Default values of all supported parameters
                ["config"] = defaultConfigFile,
                ["width"] = "640",
                ["height"] = "360",
                ["output"] = "output.pfm"
            };

            var argumentLine = String.Join('\n', args);
            byte[] argumentLineBytes = Encoding.Unicode.GetBytes(argumentLine);

            // parse command line arguments to potencialy change config File path
            using (var reader = new StreamReader(new MemoryStream(argumentLineBytes)))
            {
                config = Parse(reader, config);
            }

            // parsing config file arguments
            using (StreamReader reader = File.OpenText(config["config"]))
            {
                config = Parse(reader, config);
            }

            // override config file parameters with command line arguments
            using (StreamReader reader = new StreamReader(new MemoryStream(argumentLineBytes)))
            {
                config = Parse(reader, config);
            }

            return config;
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
                string line = inputStream.ReadLine().Trim();

                if (line.Length != 0)//skip empty lines
                {
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
    }
}
