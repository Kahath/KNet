﻿using System;
using System.IO;
using System.Xml;

namespace ServerFramework.Configuration
{
    public sealed class Config
    {

        #region Fields

        XmlNodeList nodes;

        #endregion

        #region Constructor

        /// <summary>
        /// Provides configuration
        /// </summary>
        /// <param name="path">Path to configuration file</param>
        public Config(string path)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Initialising configuration");

            if (!File.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Configuration file is missing!\nExit..");
                Console.ReadLine();
                Environment.Exit(0);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            nodes = doc.DocumentElement.ChildNodes;
            doc = null;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully read configuration file: {0}", path);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads configuration value with specified data type
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="config">name of configuration in xml file</param>
        /// <param name="hex">if value is written as hexadecimal value</param>
        /// <returns>Configuration value of specified data type.</returns>
        public T Read<T>(string config, bool hex = false)
        {
            string nameValue = null;
            T trueValue = default(T);

            try
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.NodeType != XmlNodeType.Comment)
                    {
                        if (node.Attributes[0].Value == config)
                        {
                            nameValue = node.Attributes[1].Value;
                            break;
                        }
                    }
                }

                if (hex)
                    trueValue = (T)Convert.ChangeType(Convert.ToInt32(nameValue, 16), typeof(T));
                else
                    trueValue = (T)Convert.ChangeType(nameValue, typeof(T));
            }
            catch(IndexOutOfRangeException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while reading '{0}' config. Missing argument in line", config);
                Console.ReadLine();
                Environment.Exit(0);

            }
            catch(FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while reading '{0}' config. Cannot convert '{1}' into type '{2}'"
                    , config, nameValue, typeof(T));
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while reading '{0}' config", config);
                Console.ReadLine();
                Environment.Exit(0);
            }

            return trueValue;
        }

        #endregion
    }
}
