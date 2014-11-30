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
