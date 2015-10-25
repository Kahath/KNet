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

namespace ServerFramework.Commands.Handlers
{
	[Command("help", CommandLevel.Ten, "")]
	internal class HelpCommands : CommandHandlerBase
	{
		#region Methods

		#region GetCommand

		protected override Command GetCommand()
		{
			Command retVal = null;

			retVal = new Command(Name, Level, null, HelpCommandHandler, Description);
			
			return retVal;
		}

		#endregion

		#endregion

		#region Handlers

		#region HelpCommandHandler

		public static bool HelpCommandHandler(Client user, params string[] args)
		{
			bool retVal = default(bool);

			if (args != null && args.Any())
			{
				Command command = Manager.CommandMgr.GetCommand(user, args.ToList());

				if (command != null)
				{
					if (command.SubCommands != null)
					{
						Manager.LogMgr.Log
						(
							LogType.Command
						,	$"Available sub commands for '{command.FullName}'{Environment.NewLine}{Manager.CommandMgr.AvailableSubCommands(command, user.UserLevel)}"
						);
					}
					else if (!String.IsNullOrEmpty(command.Description))
					{
						Manager.LogMgr.Log(LogType.Command, $"{command.Description}");
					}
					else
					{
						Manager.LogMgr.Log(LogType.Command, $"Command '{command.FullName}' is missing description");
					}
				}
				else
				{
					Manager.LogMgr.Log
					(
						LogType.Command
					,	$"Command '{args[0]}' doesn't exist"
					);
				}

				retVal = true;
			}

			return retVal;
		}

		#endregion

		#endregion
	}
}
