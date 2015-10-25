/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Attributes.Core;
using ServerFramework.Commands.Base;
using ServerFramework.Enums;
using ServerFramework.Managers;
using ServerFramework.Network.Session;
using System;
using System.Linq;
using System.Text;

namespace ServerFramework.Commands.Handlers
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
			Manager.LogMgr.Log(LogType.Command, "List of all commands:");

			sb.AppendLine(String.Join("\n", Manager.CommandMgr.CommandTable
				.Where
				(x => 
					user.UserLevel >= x.CommandLevel
					&& x.IsValid
				)
				.Select(x => x.SubCommands != null ? String.Format($"{x.Name}..") : x.Name)));

			Manager.LogMgr.Log(LogType.Command, $"{sb.ToString()}");

			return true;
		}

		#endregion

		#endregion
	}
}