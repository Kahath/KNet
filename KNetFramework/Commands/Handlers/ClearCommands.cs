/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Attributes.Core;
using KNetFramework.Commands.Base;
using KNetFramework.Configuration.Helpers;
using KNetFramework.Enums;
using KNetFramework.Network.Session;
using System;

namespace KNetFramework.Commands.Handlers
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
			if(KNetConfig.IsConsole)
				Console.Clear();

			return true;
		}

		#endregion

		#endregion
	}
}
