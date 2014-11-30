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

namespace ServerFramework.Configuration
{
    internal class ServerConfig
    {
        #region Fields

        private static Config config;

        internal static string BindIP;
        internal static int BindPort;

        internal static LogType LogLevel;
        internal static PacketLogType PacketLogLevel;
        internal static OpcodeType OpcodeAllowLevel;

        internal static int BufferSize;
        internal static int MaxConnections;
        internal static int MaxSimultaneousAcceptOps;
        internal static int Backlog;
        internal static int HeaderLength;

        internal static string DBHost;
        internal static int DBPort;
        internal static string DBUser;
        internal static string DBPass;
        internal static string DBName;

        #endregion

        #region Methods

        internal static void Init()
        {
            config                      = new Config("ServerConfig.xml");

            BindIP                      = config.Read<string>("bindip");
            BindPort                    = config.Read<int>("bindport");

            LogLevel                    = (LogType)config.Read<byte>("loglevel", true);
            PacketLogLevel              = (PacketLogType)config.Read<byte>("packetloglevel", true);
            OpcodeAllowLevel            = (OpcodeType)config.Read<byte>("opcodeallowlevel", true);

            BufferSize                  = config.Read<int>("buffersize");
            MaxConnections              = config.Read<int>("maxconnections");
            MaxSimultaneousAcceptOps    = config.Read<int>("maxsimultaneousacceptops");
            Backlog                     = config.Read<int>("backlog");
            HeaderLength = 4;

            DBHost                      = config.Read<string>("dbhost");
            DBPort                      = config.Read<int>("dbport");
            DBUser                      = config.Read<string>("dbuser");
            DBPass                      = config.Read<string>("dbpass");
            DBName                      = config.Read<string>("dbname");
        }

        #endregion      
    }
}
