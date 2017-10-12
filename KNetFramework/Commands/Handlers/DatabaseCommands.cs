/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Attributes.Core;
using KNetFramework.Commands.Base;
using KNetFramework.Enums;

namespace KNetFramework.Game.Handlers
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
