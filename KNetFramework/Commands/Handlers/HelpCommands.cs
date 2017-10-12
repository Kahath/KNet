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

namespace KNetFramework.Commands.Handlers
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
				Command command = Manager.CommandManager.GetCommand(user, args.ToList());

				if (command != null)
				{
					if (command.SubCommands != null)
					{
						Manager.LogManager.Log
						(
							LogTypes.Command,
							$"Available sub commands for '{command.FullName}' {Environment.NewLine}{Manager.CommandManager.AvailableSubCommands(command, user.UserLevel)}"
						);
					}
					else if (!String.IsNullOrEmpty(command.Description))
					{
						Manager.LogManager.Log(LogTypes.Command, $"{command.Description}");
					}
					else
					{
						Manager.LogManager.Log(LogTypes.Command, $"Command '{command.FullName}' is missing description");
					}
				}
				else
				{
					Manager.LogManager.Log(LogTypes.Command, $"Command '{args[0]}' doesn't exist");
				}

				retVal = true;
			}

			return retVal;
		}

		#endregion

		#endregion
	}
}
