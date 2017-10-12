/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Attributes.Core;
using ServerFramework.Commands.Base;
using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using ServerFramework.Network.Session;
using System;

namespace ServerFramework.Commands.Handlers
{
	[Command("cls", CommandLevel.Ten, "")]
	public class ClearCommands : CommandHandlerBase
	{
		#region Methods

		#region GetCommand

		protected override Command GetCommand()
		{
			Command retVal = null;

			retVal = new Command(Name, Level, null, Cls, Description);

			return retVal;
		}

		#endregion

		#endregion

		#region Handlers

		#region ClsHandler

		private static bool Cls(Client user, params string[] args)
		{
			if(ServerConfig.IsConsole)
				Console.Clear();

			return true;
		}

		#endregion

		#endregion
	}
}
