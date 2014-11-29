using ServerFramework.Constants.Misc;

namespace ServerFramework.Constants.Entities.Console
{
    public sealed class Command
    {
        #region Fields

        private string _name;
        private CommandLevel _commandLevel;
        private Command[] _subCommands;
        private CommandScriptHandler _script;
        private string _description;

        #endregion

        #region Properties

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }


        public CommandScriptHandler Script
        {
            get { return _script; }
            set { _script = value; }
        }

        public Command[] SubCommands
        {
            get { return _subCommands; }
            set { _subCommands = value; }
        }

        public CommandLevel CommandLevel
        {
            get { return _commandLevel; }
            set { _commandLevel = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Constructors

        public Command(string name, CommandLevel commandLevel
            , Command[] subCommands, CommandScriptHandler script, string description)
        {
            Name = name;
            CommandLevel = CommandLevel;
            SubCommands = subCommands;
            Script = script;
            Description = description;
        }

        #endregion
    }
}
