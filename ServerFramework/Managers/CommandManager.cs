using ServerFramework.Constants.Attributes;
using ServerFramework.Constants.Entities.Console;
using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using ServerFramework.Singleton;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;

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
            //Log.Message(LogType.Debug, "Initing...");
            CommandTable = new List<Command>();
            Init();
        }

        #endregion

        #region Methods

        #region Init

        internal void Init()
        {
            var assm = Assembly.GetExecutingAssembly();

            foreach (var type in assm.GetTypes())
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

                        CommandTable.Add((Command)method.Invoke(null, null));
                    }
                }
            }

            Log.Message(LogType.Init, "{0} Commands loaded", CommandTable.Count);

            new Thread(() =>
                {
                    while (true)
                        InvokeCommand(Console.ReadLine());

                }).Start();
        }

        #endregion

        #region InvokeCommand

        internal bool InvokeCommand(string command)
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
                                Console.WriteLine("Error with {0}{1} command."
                                + " Missing script or subcommands", path, c.Name);
                                return false;
                            }
                            else
                            {
                                Console.WriteLine("Available sub commands for {0}{1}: {2}"
                                    , path, c.Name, _availableSubCommands(c));
                                return false;
                            }
                        }
                        else
                            return c.Script.Invoke();
                    }
                }

                Console.WriteLine("Command \'{0}\' not found", command[0]);
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
                            Console.WriteLine("Error with {0}{1} command."
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

            Console.WriteLine("Command \'{0}\' not found", command[0]);
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
