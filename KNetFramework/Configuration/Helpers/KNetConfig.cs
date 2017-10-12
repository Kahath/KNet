/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Configuration.Core;
using KNetFramework.Enums;
using System;
using System.IO;
using UMemory.Unmanaged.Enums;

namespace KNetFramework.Configuration.Helpers
{
	public static class KNetConfig
	{
		#region Fields

		private static Config _config;
		private static bool _isInitialised;

		private static string _bindIp;
		private static int _bindPort;

		private static LogTypes _logLevel;
		private static PacketLogTypes _packetLogLevel;
		private static OpcodeTypes _opcodeAllowLevel;
		private static EndiannessType _endianness;
		private static int _packetLogSize;

		private static int _bufferSize;
		private static int _maxConnections;
		private static int _maxSimultaneousAcceptOps;
		private static int _backlog;
		private static int _packetFlagsLength;
		private static int _opcodeLength;
		private static int _messageSizeLength;
		private static int _bigMessageSizeLength;
		private static int _numSocketPerSession;
		private static string _logFilePath;
		private static string _assemblyPath;

		private static string _dbHost;
		private static int _dbPort;
		private static string _dbUser;
		private static string _dbPass;
		private static string _dbName;

		private static bool _isConsole = Console.OpenStandardInput(1) != Stream.Null;

		#endregion

		#region Properties

		private static Config Config
		{
			get { return _config; }
		}

		public static bool IsInitialised
		{
			get { return _isInitialised; }
		}

		public static string BindIP
		{
			get { return _bindIp; }
		}

		public static int BindPort
		{
			get { return _bindPort; }
		}

		public static LogTypes LogLevel
		{
			get { return _logLevel; }
		}

		public static PacketLogTypes PacketLogLevel
		{
			get { return _packetLogLevel; }
		}

		public static OpcodeTypes OpcodeAllowLevel
		{
			get { return _opcodeAllowLevel; }
		}

		public static EndiannessType Endianness
		{
			get { return _endianness; }
		}

		public static int PacketLogSize
		{
			get { return _packetLogSize; }
		}

		public static int BufferSize
		{
			get { return _bufferSize; }
		}

		public static int MaxConnections
		{
			get { return _maxConnections; }
		}

		public static int MaxSimultaneousAcceptOps
		{
			get { return _maxSimultaneousAcceptOps; }
		}

		public static int Backlog
		{
			get { return _backlog; }
		}

		public static int PacketFlagsLength
		{
			get { return _packetFlagsLength; }
		}

		public static int OpcodeLength
		{
			get { return _opcodeLength; }
		}

		public static int MessageSizeLength
		{
			get { return _messageSizeLength; }
		}

		public static int BigMessageSizeLength
		{
			get { return _bigMessageSizeLength; }
		}

		public static int NumSocketPerSession
		{
			get { return _numSocketPerSession; }
		}

		public static bool IsConsole
		{
			get { return _isConsole; }
		}

		public static string LogFilePath
		{
			get { return _logFilePath; }
		}

		public static string AssemblyPath
		{
			get { return _assemblyPath; }
		}

		public static int HeaderLength
		{
			get { return PacketFlagsLength + OpcodeLength + MessageSizeLength; }
		}

		public static int BigHeaderLength
		{
			get { return PacketFlagsLength + OpcodeLength + BigMessageSizeLength; }
		}

		public static string DBHost
		{
			get { return _dbHost; }
		}

		public static int DBPort
		{
			get { return _dbPort; }
		}

		public static string DBUser
		{
			get { return _dbUser; }
		}

		public static string DBPass
		{
			get { return _dbPass; }
		}

		public static string DBName
		{
			get { return _dbName; }
		}

		public static string ConnectionString
		{
			get
			{
				string retVal = String.Empty;

				retVal = String.Format($"Data Source={DBHost},{DBPort};Network Library=DBMSSOCN;Initial Catalog={DBName};User ID={DBUser};Password={DBPass};");

				return retVal;
			}
		}

		#endregion

		#region Methods

		#region Init

		internal static void Init()
		{
			_config = Config.GetInstance(ConfigurationHelper.Path);

			_bindIp = Config.Read<string>(ConfigurationHelper.BindIPKey);
			_bindPort = Config.Read<int>(ConfigurationHelper.BindPortKey);

			_assemblyPath = Config.Read<string>(ConfigurationHelper.AssemblyPath);
			_logFilePath = $"{Config.Read<string>(ConfigurationHelper.LogFolderPath)}\\ServerLog_{DateTime.Now.ToString("yyyy_mm_dd")}.log";
			_logLevel = Config.Read<LogTypes>(ConfigurationHelper.LogLevelKey, true);
			_packetLogLevel = Config.Read<PacketLogTypes>(ConfigurationHelper.PacketLogLevelKey, true);
			_opcodeAllowLevel = Config.Read<OpcodeTypes>(ConfigurationHelper.OpcodeAllowLevelKey, true);
			_endianness = Config.Read<EndiannessType>(ConfigurationHelper.Endianness, true);
			_packetLogSize = Config.Read<int>(ConfigurationHelper.PacketLogSizeKey);

			_bufferSize = Config.Read<int>(ConfigurationHelper.BufferSizeKey);
			_maxConnections = Config.Read<int>(ConfigurationHelper.MaxConnectionsKey);
			_maxSimultaneousAcceptOps = Config.Read<int>(ConfigurationHelper.MaxSimultaneousAcceptOpsKey);
			_backlog = Config.Read<int>(ConfigurationHelper.BacklogKey);

			_dbHost = Config.Read<string>(ConfigurationHelper.DBHostKey);
			_dbPort = Config.Read<int>(ConfigurationHelper.DBPortKey);
			_dbUser = Config.Read<string>(ConfigurationHelper.DBUserKey);
			_dbPass = Config.Read<string>(ConfigurationHelper.DBPassKey);
			_dbName = Config.Read<string>(ConfigurationHelper.DBNameKey);

			_packetFlagsLength = sizeof(byte);
			_opcodeLength = sizeof(ushort);
			_messageSizeLength = sizeof(ushort);
			_bigMessageSizeLength = sizeof(int);
			_numSocketPerSession = 2; // 1 for receive, 1 for send

			_isInitialised = true;
		}

		#endregion

		#endregion
	}
}
