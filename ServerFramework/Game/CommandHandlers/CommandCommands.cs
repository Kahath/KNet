using ServerFramework.Constants.Attributes;
using ServerFramework.Constants.Entities.Console;
using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using ServerFramework.Managers;

namespace ServerFramework.Game.CommandHandlers
{
    [Command]
    public sealed class CommandCommands
    {
        #region Methods

        #region GetCommand

        public static Command GetCommand()
        {
            Command[] CommandCommandTable = 
            {
                new Command("list", (CommandLevel)0xFF, null, CommandListHandler, "")
            };

            return new Command("command", (CommandLevel)0xFF,
                CommandCommandTable, null, "");
        }

        #endregion

        #region Handlers

        #region CommandListHandler

        private static bool CommandListHandler(params string[] args)
        {
            if (args.Length > 0)
                return false;

            foreach (Command c in Manager.CommandMgr.CommandTable)
                Log.Message(LogType.Default, "{0}", c.Name);

            return true;
        }

        #endregion

        #endregion

        #endregion     
    }
}