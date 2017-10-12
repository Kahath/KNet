/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Attributes.Base;
using ServerFramework.Enums;
using System;

namespace ServerFramework.Attributes.Core
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
			set { _name = value; }
		}

		public CommandLevel CommandLevel
		{
			get { return _commandLevel; }
			set { _commandLevel = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		#endregion

		#region Constructors

		public CommandAttribute()
		{

		}

		public CommandAttribute(string name, CommandLevel commandLevel, string description)
		{
			Name = name;
			CommandLevel = commandLevel;
			Description = description;
		}

		#endregion
	}
}
