/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Attributes.Core;
using KNetFramework.Commands.Base;
using KNetFramework.Enums;
using KNetFramework.Managers;
using KNetFramework.Network.Session;
using System;
using System.Linq;
using System.Text;

namespace KNetFramework.Commands.Handlers
{
	[Command("command", CommandLevel.Ten, "")]
	internal class CommandCommands : CommandHandlerBase
	{
		#region Methods

		#region GetCommand

		protected override Command GetCommand()
		{
			Command retVal = null;

			Command[] CommandCommandTable = 
			{
				new Command("list", CommandLevel.Ten, null, CommandListHandler, "")
			};

			retVal = new Command(Name, Level, CommandCommandTable, null, Description);

			return retVal;
		}

		#endregion

		#endregion

		#region Handlers

		#region CommandListHandler

		private static bool CommandListHandler(Client user, params string[] args)
		{
			StringBuilder sb = new StringBuilder();
			Manager.LogManager.Log(LogTypes.Command, "List of all commands:");

			sb.AppendLine(String.Join("\n", Manager.CommandManager.CommandTable
				.Where(x => user.UserLevel >= x.CommandLevel && x.IsValid)
				.Select(x => x.SubCommands != null ? String.Format($"{x.Name}..") : x.Name)));

			Manager.LogManager.Log(LogTypes.Command, $"{sb.ToString()}");

			return true;
		}

		#endregion

		#endregion
	}
}