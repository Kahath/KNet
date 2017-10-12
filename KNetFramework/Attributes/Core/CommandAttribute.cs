/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Attributes.Base;
using KNetFramework.Enums;
using System;

namespace KNetFramework.Attributes.Core
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
	public sealed class CommandAttribute : Attribute, ICustomAttribute
	{
		#region Fields

		private string _name;
		private CommandLevel _commandLevel;
		private string _description;

		#endregion

		#region Properties

		public string Name
		{
			get { return _name; }
		}

		public CommandLevel CommandLevel
		{
			get { return _commandLevel; }
		}

		public string Description
		{
			get { return _description; }
		}

		#endregion

		#region Constructors

		public CommandAttribute()
		{

		}

		public CommandAttribute(string name, CommandLevel commandLevel, string description)
		{
			_name = name;
			_commandLevel = commandLevel;
			_description = description;
		}

		#endregion
	}
}
