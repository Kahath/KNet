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
using System;
using System.Collections.Concurrent;

namespace ServerFramework.Managers.Base
{
	public abstract class LogManagerBase<T> : ManagerBase<T> where T : class
	{
		#region Fields

		private static BlockingCollection<Tuple<ConsoleColor, string>> _consoleLogQueue
			= new BlockingCollection<Tuple<ConsoleColor, string>>();

		#endregion

		#region Properties

		protected static BlockingCollection<Tuple<ConsoleColor, string>> ConsoleLogQueue
		{
			get { return _consoleLogQueue; }
			set { _consoleLogQueue = value; }
		}

		#endregion

		#region Methods

		protected abstract void Message(LogType type, string message, params object[] args);
		public abstract void Log(LogType type, string message, params object[] args);

		#endregion
	}
}
