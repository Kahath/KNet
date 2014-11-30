using ServerFramework.Constants.Misc;

namespace ServerFramework.Constants.Entities.Console
{
    public sealed class Command
    {
        #region Fields

        private string                  _name;
        private CommandLevel            _commandLevel;
        private Command[]               _subCommands;
        private CommandScriptHandler    _script;
        private string                  _description;

        #endregion

        #region Properties

        internal string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        internal CommandScriptHandler Script
        {
            get { return _script; }
            set { _script = value; }
        }

        internal Command[] SubCommands
        {
            get { return _subCommands; }
            set { _subCommands = value; }
        }

        internal CommandLevel CommandLevel
        {
            get { return _commandLevel; }
            set { _commandLevel = value; }
        }

        internal string Name
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
