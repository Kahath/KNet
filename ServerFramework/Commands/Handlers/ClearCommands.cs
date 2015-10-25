/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Attributes.Core;
using ServerFramework.Commands.Base;
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
			Console.Clear();

			return true;
		}

		#endregion

		#endregion
	}
}
