using ServerFramework.Constants.Attributes;
using ServerFramework.Constants.Entities.Console;
using ServerFramework.Constants.Misc;
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
                            MethodInfo method = type.GetMethod("GetCommand");
                            Command c = null;

                            if (method != null)
                            {
                                c = method.Invoke(null, null) as Command;
                                if (c != null)
                                    CommandTable.Add(c);
                            }
                        }
                    }
                }
            }

            LogManager.Log(LogType.Normal, "{0} Commands loaded", CommandTable.Count);

            /*new Thread(() =>
                {
                    while (true)
                        InvokeCommand(Console.ReadLine());

                }).Start();*/

            base.Init();
        }

        #endregion

        #region _invokeCommand

        public bool InvokeCommand(string command)
        {
            string com = Regex.Replace(command, @"\s+", " ").Trim();
            if(com != "")
                return _getCommandHandler(_commandTable.ToArray()
                    , com.Split(' ').ToList(), string.Empty);

            return false;
        }

        #endregion

        #region _getCommandHandler

        private bool _getCommandHandler(Command[] commandTable,
            List<string> command, string path)
        {
            if (commandTable == null || command == null)
                return false;

            if (command.Count == 1)
            {
                foreach (Command c in commandTable)
                {
                    if (c.Name.StartsWith(command[0].Trim()))
                    {
                        if (c.Script == null)
                        {
                            if (c.SubCommands == null)
                            {
                                LogManager.Log(LogType.Command, "Error with '{0}{1}' command."
                                + " Missing script or subcommands", path, c.Name);
                                return false;
                            }
                            else
                            {
                                LogManager.Log(LogType.Command, "Available sub commands for '{0}{1}': {2}"
                                    , path, c.Name, _availableSubCommands(c));
                                return false;
                            }
                        }
                        else
                            return c.Script.Invoke();
                    }
                }

                LogManager.Log(LogType.Command, "Command '{0}{1}' not found", path, command[0]);
                return false;
            }

            foreach (Command c in commandTable)
            {
                if (c.Name.StartsWith(command[0].Trim()))
                {
                    if (c.Script == null)
                    {
                        if (c.SubCommands != null)
                        {
                            command.RemoveAt(0);
                            path += c.Name + " ";
                            return _getCommandHandler(c.SubCommands, command, path);
                        }
                        else
                        {
                            LogManager.Log(LogType.Command, "Error with '{0}{1}' command."
                                + " Missing script or subcommands", path, c.Name);
                            return false;
                        }
                    }
                    else
                    {
                        command.RemoveAt(0);
                        return c.Script.Invoke(command.ToArray());
                    }
                }
            }

            LogManager.Log(LogType.Command, "Command '{0}{1}' not found", path, command[0]);
            return false;
        }

        #endregion

        #region _availableSubCommands

        private string _availableSubCommands(Command c)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Command com in c.SubCommands)
            {
                sb.AppendLine(com.Name);
            }

            return sb.ToString();
        }

        #endregion
        
        #endregion
    }
}
