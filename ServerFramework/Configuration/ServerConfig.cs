using ServerFramework.Constants.Misc;
using ServerFramework.Network.Socket;
using System.Net;

namespace ServerFramework.Configuration
{
    internal class ServerConfig
    {
        private static Config config;
        internal static string BindIP;
        internal static int BindPort ;
        internal static LogType LogLevel;
        internal static PacketLogType PacketLogLevel;
        internal static int BufferSize;
        internal static int MaxConnections;
        internal static int MaxSimultaneousAcceptOps;
        internal static int Backlog;
        internal static int HeaderLength;
        public static SocketListenerSettings SocketSettings;

        internal static void Init()
        {
            config = new Config("ServerConfig.xml");

            BindIP = config.Read<string>("bindip");
            BindPort = config.Read<int>("bindport");

            LogLevel = (LogType)config.Read<byte>("loglevel", true);
            PacketLogLevel = (PacketLogType)config.Read<byte>("packetloglevel", true);
            BufferSize = config.Read<int>("buffersize");
            MaxConnections = config.Read<int>("maxconnections");
            MaxSimultaneousAcceptOps = config.Read<int>("maxsimultaneousacceptops");
            Backlog = config.Read<int>("backlog");
            HeaderLength = config.Read<int>("headerlength");

            SocketSettings = new SocketListenerSettings(MaxConnections, Backlog
                    , MaxSimultaneousAcceptOps, BufferSize, HeaderLength
                    , new IPEndPoint(IPAddress.Parse(BindIP), BindPort));
        }
    }
}
