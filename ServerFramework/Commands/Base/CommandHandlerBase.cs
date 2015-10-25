/*
 * Copyright (c) 2015. Kahath.
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
					_attribute = this.GetType().GetCustomAttributes(typeof(CommandAttribute), true).FirstOrDefault() as CommandAttribute;

				return _attribute;
			}
		}

		public string Name
		{
			get
			{
				string retVal = String.Empty;

				if (Attribute != null)
					retVal = Attribute.Name;

				return retVal;
			}
		}

		public CommandLevel Level
		{
			get
			{
				CommandLevel retVal = CommandLevel.Ten;

				if (Attribute != null)
					retVal = Attribute.CommandLevel;

				return retVal;
			}
		}

		public string Description
		{
			get
			{
				string retVal = String.Empty;

				if (Attribute != null)
					retVal = Attribute.Description;

				return retVal;
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
