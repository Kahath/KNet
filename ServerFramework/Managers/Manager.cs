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

using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using ServerFramework.Managers.Core;

namespace ServerFramework.Managers
{
    public static class Manager
	{
		#region Fields

		public static CommandManager CommandMgr;
		public static SessionManager SessionMgr;
		public static PacketManager PacketMgr;
		public static BufferManager BufferMgr;
		public static PacketLogManager PacketLogMgr;
		public static LogManager LogMgr;

		#endregion

		#region Methods

		#region Init

		internal static void Init()
		{
			LogMgr = LogManager.GetInstance();

			LogMgr.Log(LogType.Init, "Initialising packet log manager");
			PacketLogMgr = PacketLogManager.GetInstance();

			LogMgr.Log(LogType.Init, "Initialising command manager");
			CommandMgr = CommandManager.GetInstance();

			LogMgr.Log(LogType.Init, "Initialising session manager");
			SessionMgr = SessionManager.GetInstance();

			LogMgr.Log(LogType.Init, "Initialising packet manager");
			PacketMgr = PacketManager.GetInstance();

			LogMgr.Log(LogType.Init, "Initialising buffer manager");
			BufferMgr = BufferManager.GetInstance(ServerConfig.BufferSize * 2
				* ServerConfig.MaxConnections, ServerConfig.BufferSize);
		}

		#endregion

		#endregion		
    }
}
