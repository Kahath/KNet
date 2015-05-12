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

using ServerFramework.Constants.Attributes;
using ServerFramework.Constants.Entities.Console;
using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using ServerFramework.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFramework.Game.CommandHandlers
{
    [Command]
    internal static class HelpCommands
    {
        #region Methods

        #region GetCommand

        private static Command GetCommand()
        {
            return new Command("help", CommandLevel.Ten, null, HelpCommandHandler
                , "");
        }

        #endregion

        #region GetHelpCommand
        
        private static bool GetHelpCommand(CommandLevel userLevel, Command[] commandTable,
            List<string> command, string path)
        {
            if (commandTable == null || command == null)
                return false;

            if(command.Count == 0)
                command.Add("help");

            Command c = commandTable.Where(x => userLevel >= x.CommandLevel).FirstOrDefault(x => x.Name.StartsWith(command[0].Trim()));

            if(c != null)
            {
                path += c.Name + " ";

                if(command.Count > 1)
                {
                    command.RemoveAt(0);
                    return GetHelpCommand(userLevel, c.SubCommands, command, path);
                }

                if (c.SubCommands != null)
                {
                    Manager.LogMgr.Log(LogType.Command, "Available sub commands for '{0}' command:", path);
                    Manager.LogMgr.Log(LogType.Command, "{0}", AvailableSubCommands(userLevel, c, path));
                    return true;
                }
                else
                {
                    if (c.Description != null && c.Description != "")
                    {
                        Manager.LogMgr.Log(LogType.Command, "{0}", c.Description);
                        return true;
                    }
                    else
                    {
                        Manager.LogMgr.Log(LogType.Command, "Command '{0}' is missing description", path);
                        return true;
                    }
                }    
            }

            path += command[0];

            Manager.LogMgr.Log(LogType.Command, "Command '{0}' not found", path);
            return false;
        }

        #endregion

        #region AvailableSubCommands

        private static string AvailableSubCommands(CommandLevel userLevel, Command c, string path)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Command com in c.SubCommands)
            {
                if(com.SubCommands != null && userLevel >= com.CommandLevel)
                    sb.AppendLine(com.Name + "..");
                else if(userLevel >= com.CommandLevel)
                    sb.AppendLine(com.Name);
            }

            return sb.ToString();
        }

	    #endregion

        #endregion

        #region Handlers

        #region HelpCommandHandler

        public static bool HelpCommandHandler(Client user, params string[] args)
        {
            return GetHelpCommand(user.UserLevel, Manager.CommandMgr.CommandTable.ToArray(),
                args.ToList(), string.Empty);
        }

        #endregion

        #endregion
    }
}
