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

        public static Command GetCommand()
        {
            Command[] CommandCommandTable = 
            {
                new Command("list", (CommandLevel)0xFFFF, null, CommandListHandler, "")
            };

            return new Command("command", (CommandLevel)0xFFFF,
                CommandCommandTable, null, "");
        }

        #endregion

        #region Handlers

        #region CommandListHandler

        private static bool CommandListHandler(params string[] args)
        {
            if (args.Length > 0)
                return false;
            
            Log.Message(LogType.Command, "List of all commands:");
            foreach (Command c in Manager.CommandMgr.CommandTable)
                Log.Message(LogType.Command, "{0}", c.Name);

            return true;
        }

        #endregion

        #endregion

        #endregion     
    }
}