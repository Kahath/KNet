/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Attributes.Core;
using ServerFramework.Commands.Base;
using ServerFramework.Enums;

namespace ServerFramework.Game.Handlers
{
	[Command("database", CommandLevel.Ten, "")]
	public class DatabaseCommands : CommandHandlerBase
	{
		#region Methods

		#region GetCommand

		protected override Command GetCommand()
		{
			Command retVal = null;

			retVal = new Command(Name, Level, null, null, Description);

			return retVal;
		}

		#endregion

		#endregion
	}
}
