/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Attributes.Core;
using ServerFramework.Enums;
using System;
using System.Linq;

namespace ServerFramework.Commands.Base
{
	public abstract class CommandHandlerBase
	{
		#region Fields

		private CommandAttribute _attribute;

		#endregion

		#region Properties

		private CommandAttribute Attribute
		{
			get
			{
				if(_attribute == null)
					_attribute = GetType().GetCustomAttributes(typeof(CommandAttribute), true).FirstOrDefault() as CommandAttribute;

				return _attribute;
			}
		}

		public string Name
		{
			get
			{
				return Attribute?.Name ?? String.Empty;
			}
		}

		public CommandLevel Level
		{
			get
			{
				return Attribute?.CommandLevel ?? CommandLevel.Ten;
			}
		}

		public string Description
		{
			get
			{
				return Attribute?.Description ?? String.Empty;
			}
		}

		#endregion

		#region Methods

		#region GetCommand

		protected abstract Command GetCommand();

		#endregion

		#endregion
	}
}
