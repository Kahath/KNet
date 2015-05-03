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
using ServerFramework.Database.Context;
using ServerFramework.Database.Model;
using ServerFramework.Managers.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ServerFramework.Managers.Core
{
    public sealed class CommandManager : CommandManagerBase<CommandManager>
    {
        #region Constructor

        CommandManager()
        {
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

                                try
                                {
                                    c = method.Invoke(null, null) as Command;
                                }
                                catch(Exception)
                                {
                                    Manager.LogMgr.Log(LogType.Error, "Error creating command type {0}", type.ToString());
                                }

                                if (c != null)
                                    CommandTable.Add(c);
                            }
                        }
                    }
                }
            }

            Manager.LogMgr.Log(LogType.Normal, "{0} Commands loaded", CommandTable.Count);

            _loadCommandDescriptions();
        }

        #endregion

        #region InvokeCommand

        public override bool InvokeCommand(string command)
        {
			bool retVal = false;

            string com = Regex.Replace(command, @"\s+", " ").Trim();

            if (com != "")
            {
                retVal = _invokeCommandHandler(CommandTable.ToArray()
                    , com.Split(' ').ToList(), string.Empty);
            }

            return retVal;
        }

        #endregion

        #region _invokeCommandHandler

		protected override bool _invokeCommandHandler(Command[] commandTable,
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
                        command.RemoveAt(0);
                        if(command.Count > 0)
                        {
                            return _invokeCommandHandler(c.SubCommands, command, path);
                        }
                        else
                        {
                            Manager.LogMgr.Log(LogType.Command, "Error with '{0}' command."
                                + " Available sub commands:\n{1}", path, _availableSubCommands(c));

                            return false;
                        }
                    }
                    else
                    {
                        Manager.LogMgr.Log(LogType.Command, "Error with '{0}' command."
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
                        Manager.LogMgr.Log(LogType.Error, "Error with '{0}' command. wrong arguments"
                            , path);
                        return false;
                    }
                    catch (Exception)
                    {
                        Manager.LogMgr.Log(LogType.Error, "Error with '{0}' command. Failed to execute handler"
                            , path);
                        return false;
                    }
                }
            }

            path += command[0];
            Manager.LogMgr.Log(LogType.Command, "Command '{0}' not found", path);
            return false;
        }

        #endregion

        #region _availableSubCommands

		protected override string _availableSubCommands(Command c)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Command com in c.SubCommands)
            {
                if (com.SubCommands != null)
                {
                    sb.AppendLine(com.Name + "..");
                }
                else
                {
                    sb.AppendLine(com.Name);
                }
            }

            return sb.ToString();
        }

        #endregion

        #region _loadCommandDescriptions

		protected override void _loadCommandDescriptions()
        {
            Command c = null;

            using (ApplicationContext context = new ApplicationContext())
            {
                IEnumerable<CommandModel> commands = context.Commands.Where(x => x.Active);

                foreach (CommandModel cdo in commands)
                {
                    c = _getCommand(CommandTable.ToArray()
                        , cdo.Name.Split(' ').ToList());

                    if (c != null)
                    {
                        c.CommandLevel = (CommandLevel)cdo.CommandLevel;
                        c.Description = cdo.Description;
                    }
                }
            }
        }

        #endregion

        #region _getCommand

		protected override Command _getCommand(Command[] commandTable, List<string> command)
        {
            if (commandTable == null || command == null)
                return null;

            Command c = commandTable.FirstOrDefault(x => x.Name.StartsWith(command[0].Trim()));

            if (c != null)
            {
                command.RemoveAt(0);
                if (command.Count > 0)
                {
                    return _getCommand(c.SubCommands, command);
                }
            }

            return c;
        }

        #endregion

        #endregion
    }
}
