/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
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
		private static AssemblyManager _assemblyMgr;

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
			get 
			{
				if (_logMgr == null)
					_logMgr = LogManager.GetInstance();

				return _logMgr; 
			}
		}

		public static AssemblyManager AssemblyMgr
		{
			get { return _assemblyMgr; }
			set { _assemblyMgr = value; }
		}

		#endregion

		#region Init

		internal static void Init()
		{
			LogMgr.Log(LogType.Init, "Initialising assembly manager");
			AssemblyMgr = AssemblyManager.GetInstance();

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
					ServerConfig.BufferSize * ServerConfig.MaxConnections * ServerConfig.NumSocketPerSession
				,	ServerConfig.BufferSize
				);
		}

		#endregion

		#endregion
	}
}
