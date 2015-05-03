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
using ServerFramework.Database.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ServerFramework.Managers.Base
{
	public abstract class LogManagerBase<T> : ManagerBase<T> where T : class
	{
		#region Fields

		private BlockingCollection<Tuple<ConsoleColor, string>> _consoleLogQueue
			= new BlockingCollection<Tuple<ConsoleColor, string>>();
        private List<LogModel> _logList;

		#endregion

		#region Properties

		protected BlockingCollection<Tuple<ConsoleColor, string>> ConsoleLogQueue
		{
			get { return _consoleLogQueue; }
			set { _consoleLogQueue = value; }
		}

        protected List<LogModel> LogList
        {
            get 
            {
                if (_logList == null)
                    _logList = new List<LogModel>();

                return _logList; 
            }
        }

		#endregion

		#region Methods

		protected abstract void Message(LogType type, string message, params object[] args);
		public abstract void Log(LogType type, string message, params object[] args);

		#endregion
	}
}
