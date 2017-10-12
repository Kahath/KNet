/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Configuration.Helpers;
using KNetFramework.Enums;
using KNetFramework.Managers.Core;

namespace KNetFramework.Managers
{
	public static class Manager
	{
		#region Fields

		private static CommandManager _commandManager;
		private static SessionManager _sessionManager;
		private static PacketManager _packetManager;
		private static BufferManager _bufferManager;
		private static PacketLogManager _packetLogManager;
		private static LogManager _logManager;
		private static AssemblyManager _assemblyManager;
		private static DatabaseManager _databaseManager;

		#endregion

		#region Methods

		#region Properties

		public static CommandManager CommandManager
		{
			get { return _commandManager; }
			set { _commandManager = value; }
		}

		public static SessionManager SessionManager
		{
			get { return _sessionManager; }
			set { _sessionManager = value; }
		}

		public static PacketManager PacketManager
		{
			get { return _packetManager; }
			set { _packetManager = value; }
		}

		public static BufferManager BufferManager
		{
			get { return _bufferManager; }
			set { _bufferManager = value; }
		}

		public static PacketLogManager PacketLogManager
		{
			get { return _packetLogManager; }
			set { _packetLogManager = value; }
		}

		public static LogManager LogManager
		{
			get 
			{
				if (_logManager == null)
					_logManager = LogManager.GetInstance();

				return _logManager; 
			}
		}

		public static AssemblyManager AssemblyManager
		{
			get { return _assemblyManager; }
			set { _assemblyManager = value; }
		}

		public static DatabaseManager DatabaseManager
		{
			get
			{
				if (_databaseManager == null)
					_databaseManager = DatabaseManager.GetInstance();

				return _databaseManager;
			}
		}

		#endregion

		#region Init

		internal static void Init()
		{
			LogManager.Log(LogTypes.Init, "Initializing assembly manager");
			AssemblyManager = AssemblyManager.GetInstance();

			LogManager.Log(LogTypes.Init, "Initializing packet log manager");
			PacketLogManager = PacketLogManager.GetInstance();

			LogManager.Log(LogTypes.Init, "Initializing command manager");
			CommandManager = CommandManager.GetInstance();

			LogManager.Log(LogTypes.Init, "Initializing session manager");
			SessionManager = SessionManager.GetInstance();

			LogManager.Log(LogTypes.Init, "Initializing packet manager");
			PacketManager = PacketManager.GetInstance();

			LogManager.Log(LogTypes.Init, "Initializing buffer manager");
			BufferManager = BufferManager.GetInstance
				(
					KNetConfig.BufferSize * KNetConfig.MaxConnections * KNetConfig.NumSocketPerSession
				,	KNetConfig.BufferSize
				);
		}

		#endregion

		#endregion
	}
}
