/*
 * This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
						,	"Available sub commands for '{0}'\n{1}"
						,	command.FullName
						,	Manager.CommandMgr.AvailableSubCommands(command, user.UserLevel)
						);
					}
					else if (!String.IsNullOrEmpty(command.Description))
					{
						Manager.LogMgr.Log(LogType.Command, "{0}", command.Description);
					}
					else
					{
						Manager.LogMgr.Log(LogType.Command, "Command '{0}' is missing description", command.FullName);
					}
				}
				else
				{
					Manager.LogMgr.Log
					(
						LogType.Command
					,	"Command '{0}' doesn't exist"
					,	args[0]
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
