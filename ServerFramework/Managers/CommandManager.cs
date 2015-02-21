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
using ServerFramework.Database;
using ServerFramework.Logging;
using ServerFramework.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ServerFramework.Managers
{
    public sealed class CommandManager : SingletonBase<CommandManager>
    {
        #region Fields

        private List<Command> _commandTable;

        #endregion

        #region Properties

        internal List<Command> CommandTable
        {
            get { return _commandTable; }
            set { _commandTable = value; }
        }

        #endregion

        #region Constructor

        CommandManager()
        {
            CommandTable = new List<Command>();
            Init();
        }

        #endregion

        #region Methods

        #region Init

        internal override void Init()
        {
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in a.GetTypes())
                {
                    foreach (var attr in type.GetCustomAttributes<CommandAttribute>())
                    {
                        if (attr != null)
                        {
                            MethodInfo method = type.GetMethod("GetCommand"
                                , BindingFlags.NonPublic | BindingFlags.Static);

                            if (method != null)
                            {
                                Command c = null;

                                c = method.Invoke(null, null) as Command;

                                if (c != null)
                                    CommandTable.Add(c);
                            }
                        }
                    }
                }
            }

            LogManager.Log(LogType.Normal, "{0} Commands loaded", CommandTable.Count);

            _loadCommandDescriptions();

            base.Init();
        }

        #endregion

        #region InvokeCommand

        public bool InvokeCommand(string command)
        {
            string com = Regex.Replace(command, @"\s+", " ").Trim();
            if(com != "")
                return _invokeCommandHandler(_commandTable.ToArray()
                    , com.Split(' ').ToList(), string.Empty);

            return false;
        }

        #endregion

        #region _invokeCommandHandler

        private bool _invokeCommandHandler(Command[] commandTable,
            List<string> command, string path)
        {
            if (commandTable == null || command == null)
                return false;

            Command c = commandTable.FirstOrDefault(x => x.Name.StartsWith(command[0].Trim()));

            if (c != null)
            {
                path += c.Name + " ";

                if (c.Script == null)
                {
                    if (c.SubCommands != null)
                    {
                        if(command.Count > 0)
                        {
                            command.RemoveAt(0);
                            return _invokeCommandHandler(c.SubCommands, command, path);
                        }
                        else
                        {
                            LogManager.Log(LogType.Command, "Error with '{0}' command."
                                + " Available sub commands:\n{2}", path, _availableSubCommands(c));

                            return false;
                        }
                    }
                    else
                    {
                        LogManager.Log(LogType.Command, "Error with '{0}' command."
                            + " Missing script or subcommands", path);
                        return false;
                    }
                }
                else
                {
                    command.RemoveAt(0);
                    
                    try
                    {
                        return c.Script.Invoke(command.ToArray());
                    }
                    catch (IndexOutOfRangeException)
                    {
                        LogManager.Log(LogType.Error, "Error with '{0}' command. wrong arguments"
                            , path);
                        return false;
                    }
                    catch (Exception)
                    {
                        LogManager.Log(LogType.Error, "Error with '{0}' command. Failed to execute handler"
                            , path);
                        return false;
                    }
                }
            }

            path += command[0];
            LogManager.Log(LogType.Command, "Command '{0}' not found", path);
            return false;
        }

        #endregion

        #region _availableSubCommands

        private string _availableSubCommands(Command c)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Command com in c.SubCommands)
            {
                if (com.SubCommands != null)
                    sb.AppendLine(com.Name + "..");
                else
                    sb.AppendLine(com.Name);
            }

            return sb.ToString();
        }

        #endregion

        #region _loadCommandDescriptions

        private void _loadCommandDescriptions()
        {
            Command c = null;

            using (SqlResult res = DB.Application.Select("SELECT * FROM `command`"))
            {
                for(int i = 0; i < res.Count; i++)
                {
                    c = _getCommand(CommandTable.ToArray()
                        , res.Read<string>(i, "name").Split(' ').ToList());

                    if (c != null)
                    {
                        c.CommandLevel = (CommandLevel)res.Read<ushort>(i, "commandlevel");
                        c.Description = res.Read<string>(i, "description");
                    }
                }
            }
        }

        #endregion

        #region _getCommand

        private Command _getCommand(Command[] commandTable, List<string> command)
        {
            if (commandTable == null || command == null)
                return null;

            Command c = commandTable.FirstOrDefault(x => x.Name.StartsWith(command[0].Trim()));

            if (c != null)
            {
                if (command.Count > 0)
                {
                    command.RemoveAt(0);
                    return _getCommand(c.SubCommands, command);
                }
            }

            return c;
        }

        #endregion

        #endregion
    }
}
