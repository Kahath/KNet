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

using ServerFramework.Constants.Entities.Console;
using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using System.Collections.Generic;

namespace ServerFramework.Managers.Base
{
	public abstract class CommandManagerBase<T> : ManagerBase<T> where T : class
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

		protected CommandManagerBase()
		{
			_commandTable = new List<Command>();
		}

		#endregion

		#region Methods

		public abstract bool InvokeCommand(Client user, string command);
		protected abstract bool InvokeCommandHandler(Client user, Command[] commandTable,
			IList<string> command, string path);
		protected abstract string AvailableSubCommands(CommandLevel userLevel, Command c);
		protected abstract void LoadCommandDescriptions();
		protected abstract Command GetCommandByPath(Command[] commandTable, IList<string> command);

		#endregion
	}
}
