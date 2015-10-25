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

using ServerFramework.Configuration.Core;
using ServerFramework.Enums;
using System;

namespace ServerFramework.Configuration.Helpers
{
	internal static class ServerConfig
	{
		#region Fields

		private static Config _config;

		private static string _bindIp;
		private static int _bindPort;

		private static LogType? _logLevel;
		private static PacketLogType _packetLogLevel;
		private static OpcodeType _opcodeAllowLevel;
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
		private static string _assemblyPath;

		private static string _dbHost;
		private static int _dbPort;
		private static string _dbUser;
		private static string _dbPass;
		private static string _dbName;

		#endregion

		#region Properties

		private static Config Config
		{
			get { return _config; }
			set { _config = value; }
		}

		internal static string BindIP
		{
			get { return _bindIp; }
			set { _bindIp = value; }
		}

		internal static int BindPort
		{
			get { return _bindPort; }
			set { _bindPort = value; }
		}

		internal static LogType? LogLevel
		{
			get { return _logLevel; }
			set { _logLevel = value; }
		}

		internal static PacketLogType PacketLogLevel
		{
			get { return _packetLogLevel; }
			set { _packetLogLevel = value; }
		}

		internal static OpcodeType OpcodeAllowLevel
		{
			get { return _opcodeAllowLevel; }
			set { _opcodeAllowLevel = value; }
		}

		internal static int PacketLogSize
		{
			get { return _packetLogSize; }
			set { _packetLogSize = value; }
		}

		internal static int BufferSize
		{
			get { return _bufferSize; }
			set { _bufferSize = value; }
		}

		internal static int MaxConnections
		{
			get { return _maxConnections; }
			set { _maxConnections = value; }
		}

		internal static int MaxSimultaneousAcceptOps
		{
			get { return _maxSimultaneousAcceptOps; }
			set { _maxSimultaneousAcceptOps = value; }
		}

		internal static int Backlog
		{
			get { return _backlog; }
			set { _backlog = value; }
		}

		public static int PacketFlagsLength
		{
			get { return _packetFlagsLength; }
			set { _packetFlagsLength = value; }
		}

		internal static int OpcodeLength
		{
			get { return _opcodeLength; }
			set { _opcodeLength = value; }
		}

		internal static int MessageSizeLength
		{
			get { return _messageSizeLength; }
			set { _messageSizeLength = value; }
		}

		internal static int BigMessageSizeLength
		{
			get { return _bigMessageSizeLength; }
			set { _bigMessageSizeLength = value; }
		}

		internal static int NumSocketPerSession
		{
			get { return _numSocketPerSession; }
			set { _numSocketPerSession = value; }
		}

		internal static string AssemblyPath
		{
			get { return _assemblyPath; }
			set { _assemblyPath = value; }
		}

		internal static int HeaderLength
		{
			get { return PacketFlagsLength + OpcodeLength + MessageSizeLength; }
		}

		internal static int BigHeaderLength
		{
			get { return PacketFlagsLength + OpcodeLength + BigMessageSizeLength; }
		}

		internal static string DBHost
		{
			get { return _dbHost; }
			set { _dbHost = value; }
		}

		internal static int DBPort
		{
			get { return _dbPort; }
			set { _dbPort = value; }
		}

		internal static string DBUser
		{
			get { return _dbUser; }
			set { _dbUser = value; }
		}

		internal static string DBPass
		{
			get { return _dbPass; }
			set { _dbPass = value; }
		}

		internal static string DBName
		{
			get { return _dbName; }
			set { _dbName = value; }
		}

		internal static string ConnectionString
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
			Config = new Config(ConfigurationHelper.Path);

			BindIP = Config.Read<string>(ConfigurationHelper.BindIPKey);
			BindPort = Config.Read<int>(ConfigurationHelper.BindPortKey);

			LogLevel = (LogType?)Config.Read<byte>(ConfigurationHelper.LogLevelKey, true) ?? LogType.None;
			PacketLogLevel = (PacketLogType)Config.Read<byte>(ConfigurationHelper.PacketLogLevelKey, true);
			OpcodeAllowLevel = (OpcodeType)Config.Read<byte>(ConfigurationHelper.OpcodeAllowLevelKey, true);
			PacketLogSize = Config.Read<int>(ConfigurationHelper.PacketLogSizeKey);

			BufferSize = Config.Read<int>(ConfigurationHelper.BufferSizeKey);
			MaxConnections = Config.Read<int>(ConfigurationHelper.MaxConnectionsKey);
			MaxSimultaneousAcceptOps = Config.Read<int>(ConfigurationHelper.MaxSimultaneousAcceptOpsKey);
			Backlog = Config.Read<int>(ConfigurationHelper.BacklogKey);
			AssemblyPath = Config.Read<string>(ConfigurationHelper.AssemblyPath);

			DBHost = Config.Read<string>(ConfigurationHelper.DBHostKey);
			DBPort = Config.Read<int>(ConfigurationHelper.DBPortKey);
			DBUser = Config.Read<string>(ConfigurationHelper.DBUserKey);
			DBPass = Config.Read<string>(ConfigurationHelper.DBPassKey);
			DBName = Config.Read<string>(ConfigurationHelper.DBNameKey);

			PacketFlagsLength = sizeof(byte);
			OpcodeLength = sizeof(ushort);
			MessageSizeLength = sizeof(ushort);
			BigMessageSizeLength = sizeof(int);
			NumSocketPerSession = 2;
		}

		#endregion

		#endregion
	}
}
