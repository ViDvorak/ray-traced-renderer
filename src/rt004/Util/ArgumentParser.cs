﻿using rt004.SceneObjects;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using rt004.SceneObjects.Loading;

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
            /*// tests
            data.output = "output.pfm";
            
            SceneLoader scene = new SceneLoader();
            scene.solidLoaders.Add(new SphereLoader(new Vector3(1, 2, 3), new Vector3(3, 2, 1), new Color4(255, 255, 255, 255), 1) );
            scene.lightLoaders.Add(new PointLightLoader(new Vector3(5,5,5), Vector3.Zero, Color4.Azure, 10));
            scene.cameraLoaders.Add( new CameraLoader(Vector3.Zero, Vector3.Zero, Color4.DimGray, MathF.PI / 2, 780, 600));
            data.sceneLoader = scene;

            serializer.Serialize(Console.Out, data); // Problem not Serialized, result is {}
            */


            // parsing XML config file
            using (Stream stream = File.Open(config["config"], FileMode.Open))
            {
                data = (DataLoader)serializer.Deserialize(stream);
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