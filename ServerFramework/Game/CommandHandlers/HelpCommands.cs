using ServerFramework.Constants.Attributes;
using ServerFramework.Constants.Entities.Console;
using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using ServerFramework.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework.Game.CommandHandlers
{
    [Command]
    internal sealed class HelpCommands
    {
        #region Methods

        #region GetCommand

        public static Command GetCommand()
        {
            return new Command("help", (CommandLevel)0xFFFF, null, HelpCommandHandler
                , "Usage: returns description or subcommands for wanted command");
        }

        #endregion

        #region _getHelpCommand
        
        private static bool _getHelpCommand(Command[] commandTable,
            List<string> command, string path)
        {
            if (commandTable == null || command == null)
                return false;

            if (command.Count == 0)
                command.Add("help");

            if (command.Count == 1)
            {
                foreach (Command c in commandTable)
                {
                    if (c.Name.StartsWith(command[0].Trim()))
                    {
                        if (c.SubCommands != null)
                        {
                            Log.Message(LogType.Command, "Available sub commands for '{0}{1}' command:", path, c.Name);
                            Log.Message(LogType.Command, "{0}", _availableSubCommands(c, path));
                            return true;
                        }
                        else
                        {
                            if (c.Description != null && c.Description != "")
                            {
                                Log.Message(LogType.Command, "{0}", c.Description);
                                return true;
                            }
                            else
                            {
                                Log.Message(LogType.Command, "Command '{0}{1}' is missing description", path, c.Name);
                                return true;
                            }
                        }
                    }
                }

                Log.Message(LogType.Command, "Command '{0}{1}' not found", path, command[0]);
                return false;
            }

            foreach (Command c in commandTable)
            {
                if (c.Name.StartsWith(command[0].Trim()))
                {
                    path += c.Name + " ";
                    command.RemoveAt(0);
                    return _getHelpCommand(c.SubCommands, command, path);
                }
            }

            Log.Message(LogType.Command, "Command '{0}{1}' not found", path, command[0]);
            return false;
        }

        #endregion

        #region _availableSubCommands

        private static string _availableSubCommands(Command c, string path)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Command com in c.SubCommands)
            {
                sb.AppendLine(com.Name);
            }

            return sb.ToString();
        }

	    #endregion

        #region Handlers

        #region HelpCommandHandler

        public static bool HelpCommandHandler(params string[] args)
        {
            return _getHelpCommand(Manager.CommandMgr.CommandTable.ToArray(),
                args.ToList(), string.Empty);
        }

        #endregion

        #endregion

        #endregion
    }
}
