/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Base;
using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using ServerFramework.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace ServerFramework.Configuration.Core
{
	/// <summary>
	/// Provides xml configuration injection.
	/// </summary>
	public sealed class XmlConfiguration : IConfig
	{
		#region Fields

		private IEnumerable<XmlNode> _nodes;

		#endregion

		#region Properties

		private IEnumerable<XmlNode> Nodes
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
			if (File.Exists(path))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(path);
				Nodes = doc.DocumentElement.ChildNodes.Cast<XmlNode>();
			}
			else
			{
				Environment.Exit(0);
			}
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
			bool isEnum = typeof(T).IsEnum;

			try
			{
				XmlNode node = Nodes.FirstOrDefault(x => x.NodeType == XmlNodeType.Element
					&& x.Attributes[ConfigurationHelper.Key].Value == config);

				if (node != null)
					nameValue = node.Attributes[ConfigurationHelper.Value].Value;

				if (hex)
				{
					if (isEnum)
					{
						trueValue = (T)Enum.ToObject(typeof(T), Convert.ToInt32(nameValue, 16));
					}
					else
					{
						trueValue = (T)Convert.ChangeType(Convert.ToInt32(nameValue, 16), typeof(T));
					}

				}
				else
				{
					if (isEnum)
					{
						trueValue = (T)Enum.ToObject(typeof(T), nameValue);
					}
					else
					{
						trueValue = (T)Convert.ChangeType(nameValue, typeof(T));
					}
				}
			}
			catch (IndexOutOfRangeException e)
			{
				Manager.LogMgr.Log(LogType.Critical, $"Error while reading '{config}' config. Missing argument in line", e);
			}
			catch (NullReferenceException e)
			{
				Manager.LogMgr.Log(LogType.Critical, $"Error while reading '{config}' config. Argument is null", e);
			}
			catch (FormatException e)
			{
				Manager.LogMgr.Log(LogType.Critical, $"Error while reading '{config}' config. Cannot convert '{nameValue}' into type '{typeof(T)}'", e);
			}
			catch (Exception e)
			{
				Manager.LogMgr.Log(LogType.Critical, $"Error while reading '{config}' config", e);
			}

			return trueValue;
		}

		#endregion

		#endregion
	}
}
