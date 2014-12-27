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
using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using ServerFramework.Managers;

namespace ServerFramework.Game.CommandHandlers
{
    [Command]
    internal sealed class CommandCommands
    {
        #region Methods

        #region GetCommand

        private static Command GetCommand()
        {
            Command[] CommandCommandTable = 
            {
                new Command("list", (CommandLevel)0xFFFF, null, CommandListHandler, "")
            };

            return new Command("command", (CommandLevel)0xFFFF,
                CommandCommandTable, null, "");
        }

        #endregion

        #endregion    
 
        #region Handlers

        #region CommandListHandler

        private static bool CommandListHandler(params string[] args)
        {
            LogManager.Log(LogType.Command, "List of all commands:");
            foreach (Command c in Manager.CommandMgr.CommandTable)
            {
                if (c.SubCommands != null)
                    LogManager.Log(LogType.Command, "{0}..", c.Name);
                else
                    LogManager.Log(LogType.Command, "{0}", c.Name);
            }

            return true;
        }

        #endregion

        #endregion
    }
}