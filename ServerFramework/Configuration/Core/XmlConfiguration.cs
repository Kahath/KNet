/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Base;
using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using ServerFramework.Managers;
using System;
using System.IO;
using System.Xml;

namespace ServerFramework.Configuration.Core
{
	/// <summary>
	/// Provides xml configuration injection.
	/// </summary>
	public sealed class XmlConfiguration : IConfig
	{
		#region Fields

		private XmlNodeList _nodes;

		#endregion

		#region Properties

		private XmlNodeList Nodes
		{
			get { return _nodes; }
			set { _nodes = value; }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Creates instance of <see cref="ServerFramework.Configuration.XmlConfiguration"/> type.
		/// </summary>
		/// <param name="path">Path to configuration file</param>
		XmlConfiguration(string path)
		{
			if (!File.Exists(path))
			{
				Environment.Exit(0);
			}

			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			Nodes = doc.DocumentElement.ChildNodes;
		}

		#endregion

		#region Methods

		#region Read

		/// <summary>
		/// Reads generic value from xml configuration file.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="config">Name of configuration in xml file.</param>
		/// <param name="hex">Is value is written as hexadecimal value.</param>
		/// <returns>Configuration value of specified data type.</returns>
		public T Read<T>(string config, bool hex = false)
		{
			string nameValue = null;
			T trueValue = default(T);

			try
			{
				foreach (XmlNode node in Nodes)
				{
					if (node.NodeType == XmlNodeType.Element)
					{
						if (node.Attributes[ConfigurationHelper.Key].Value == config)
						{
							nameValue = node.Attributes[ConfigurationHelper.Value].Value;
							break;
						}
					}
				}

				if (hex)
					trueValue = (T)Convert.ChangeType(Convert.ToInt32(nameValue, 16), typeof(T));
				else
					trueValue = (T)Convert.ChangeType(nameValue, typeof(T));
			}
			catch (IndexOutOfRangeException)
			{
				Manager.LogMgr.Log(LogType.Error, $"Error while reading '{config}' config. Missing argument in line");
				Console.ReadLine();
				Environment.Exit(0);

			}
			catch (NullReferenceException)
			{
				Manager.LogMgr.Log(LogType.Error, $"Error while reading '{config}' config. Argument is null");
				Console.ReadLine();
				Environment.Exit(0);
			}
			catch (FormatException)
			{
				Manager.LogMgr.Log(LogType.Error, $"Error while reading '{config}' config. Cannot convert '{nameValue}' into type '{typeof(T)}'");
				Console.ReadLine();
				Environment.Exit(0);
			}
			catch (Exception)
			{
				Manager.LogMgr.Log(LogType.Error, $"Error while reading '{config}' config");
				Console.ReadLine();
				Environment.Exit(0);
			}

			return trueValue;
		}

		#endregion

		#endregion
	}
}
