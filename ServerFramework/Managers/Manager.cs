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

		private static CommandManager _commandMgr;
		private static SessionManager _sessionMgr;
		private static PacketManager _packetMgr;
		private static BufferManager _bufferMgr;
		private static PacketLogManager _packetLogMgr;
		private static LogManager _logMgr;

		#endregion

		#region Methods

		#region Properties

		public static CommandManager CommandMgr
		{
			get { return _commandMgr; }
			set { _commandMgr = value; }
		}

		public static SessionManager SessionMgr
		{
			get { return _sessionMgr; }
			set { _sessionMgr = value; }
		}

		public static PacketManager PacketMgr
		{
			get { return _packetMgr; }
			set { _packetMgr = value; }
		}

		public static BufferManager BufferMgr
		{
			get { return _bufferMgr; }
			set { _bufferMgr = value; }
		}

		public static PacketLogManager PacketLogMgr
		{
			get { return _packetLogMgr; }
			set { _packetLogMgr = value; }
		}

		public static LogManager LogMgr
		{
			get { return _logMgr; }
			set { _logMgr = value; }
		}

		#endregion

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
			BufferMgr = BufferManager.GetInstance
				(
					ServerConfig.BufferSize * ServerConfig.MaxConnections * 2
				,	ServerConfig.BufferSize
				);
		}

		#endregion

		#endregion
	}
}
