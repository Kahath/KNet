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

namespace ServerFramework.Configuration
{
	internal static class ServerConfig
	{
		#region Fields

		private static Config _config;

		private static string _bindIp;
		private static int _bindPort;

		private static LogType _logLevel;
		private static PacketLogType _packetLogLevel;
		private static OpcodeType _opcodeAllowLevel;
		private static byte _packetLogSize;

		private static int _bufferSize;
		private static int _maxConnections;
		private static int _maxSimultaneousAcceptOps;
		private static int _backlog;
		private static int _opcodeLength;
		private static int _headerLength;
		private static int _bigHeaderLength;
		private static int _packetFlagsLength;

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

		internal static LogType LogLevel
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

		internal static byte PacketLogSize
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

		internal static int OpcodeLength
		{
			get { return _opcodeLength; }
			set { _opcodeLength = value; }
		}

		internal static int HeaderLength
		{
			get { return _headerLength; }
			set { _headerLength = value; }
		}

		internal static int BigHeaderLength
		{
			get { return _bigHeaderLength; }
			set { _bigHeaderLength = value; }
		}

		public static int PacketFlagsLength
		{
			get { return _packetFlagsLength; }
			set { _packetFlagsLength = value; }
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

				retVal = String.Format("Data Source={0},{1};Network Library=DBMSSOCN;Initial Catalog={2};User ID={3};Password={4};"
					, DBHost, DBPort, DBName, DBUser, DBPass);

				return retVal;
			}
		}

		#endregion

		#region Methods

		#region Init

		internal static void Init()
		{
			Config = new Config("ServerConfig.xml");

			BindIP = Config.Read<string>("bindip");
			BindPort = Config.Read<int>("bindport");

			LogLevel = (LogType)Config.Read<byte>("loglevel", true);
			PacketLogLevel = (PacketLogType)Config.Read<byte>("packetloglevel", true);
			OpcodeAllowLevel = (OpcodeType)Config.Read<byte>("opcodeallowlevel", true);
			PacketLogSize = Config.Read<byte>("packetlogsize");

			BufferSize = Config.Read<int>("buffersize");
			MaxConnections = Config.Read<int>("maxconnections");
			MaxSimultaneousAcceptOps = Config.Read<int>("maxsimultaneousacceptops");
			Backlog = Config.Read<int>("backlog");

			DBHost = Config.Read<string>("dbhost");
			DBPort = Config.Read<int>("dbport");
			DBUser = Config.Read<string>("dbuser");
			DBPass = Config.Read<string>("dbpass");
			DBName = Config.Read<string>("dbname");

			PacketFlagsLength = 1;
			OpcodeLength = 2;
			HeaderLength = 5;
			BigHeaderLength = 7;
		}

		#endregion

		#endregion
	}
}
