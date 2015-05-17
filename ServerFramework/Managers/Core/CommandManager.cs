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
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.Command;
using ServerFramework.Managers.Base;
using System;
using System.Collections.Generic;
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
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies().
                Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(AssemblyServerAttribute))))
            {
                foreach (Type type in a.GetTypes())
                {
                    foreach (CommandAttribute attr in type.GetCustomAttributes<CommandAttribute>())
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

            LoadCommandDescriptions();
        }

        #endregion

        #region InvokeCommand

        public override bool InvokeCommand(Client user, string command)
        {
			bool retVal = false;

            string com = Regex.Replace(command, @"\s+", " ").Trim();

            if (!String.IsNullOrEmpty(com))
            {
                retVal = InvokeCommandHandler(user, CommandTable.ToArray()
                    , com.Split(' ').ToList(), String.Empty);
            }

            return retVal;
        }

        #endregion

        #region InvokeCommandHandler

		protected override bool InvokeCommandHandler(Client user, Command[] commandTable,
            List<string> command, string path)
        {
            if (commandTable == null || command == null)
                return false;

            Command c = commandTable.Where(x => user.UserLevel >= x.CommandLevel).FirstOrDefault(x => x.Name.StartsWith(command[0].Trim()));

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
                            return InvokeCommandHandler(user, c.SubCommands, command, path);
                        }
                        else
                        {
                            Manager.LogMgr.Log(LogType.Command, "Error with '{0}' command."
                                + " Available sub commands:\n{1}", path, AvailableSubCommands(user.UserLevel, c));

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
                        return c.Script.Invoke(user, command.ToArray());
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

        #region AvailableSubCommands

		protected override string AvailableSubCommands(CommandLevel userLevel, Command c)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Command com in c.SubCommands)
            {
                if (com.SubCommands != null && userLevel >= com.CommandLevel)
                {
                    sb.AppendLine(com.Name + "..");
                }
                else if (userLevel >= com.CommandLevel)
                {
                    sb.AppendLine(com.Name);
                }
            }

            return sb.ToString();
        }

        #endregion

        #region LoadCommandDescriptions

		protected override void LoadCommandDescriptions()
        {
            Command c = null;

            using (ApplicationContext context = new ApplicationContext())
            {
                IEnumerable<CommandModel> commands = context.Commands.Where(x => x.Active);

                foreach (CommandModel cdo in commands)
                {
                    c = GetCommand(CommandTable.ToArray()
                        , cdo.Name.Split(' ').ToList());

                    if (c != null)
                    {
                        c.CommandLevel = (CommandLevel)cdo.CommandLevelID.GetValueOrDefault();
                        c.Description = cdo.Description;
                    }
                }
            }
        }

        #endregion

        #region GetCommand

		protected override Command GetCommand(Command[] commandTable, List<string> command)
        {
            if (commandTable == null || command == null)
                return null;

            Command c = commandTable.FirstOrDefault(x => x.Name.StartsWith(command[0].Trim()));

            if (c != null)
            {
                command.RemoveAt(0);
                if (command.Count > 0)
                {
                    return GetCommand(c.SubCommands, command);
                }
            }

            return c;
        }

        #endregion

        #endregion
    }
}
