using ServerFramework.Configuration;
using ServerFramework.Logging;
using ServerFramework.Network.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework.Managers
{
    public static class Manager
    {
        internal static CommandManager CommandMgr;
        public static SessionManager SessionMgr;
        internal static PacketManager PacketMgr;
        internal static BufferManager BufferMgr;
        internal static Server Server;

        public static void Init()
        {
            ServerConfig.Init();
            Log.Init();
            CommandMgr          = CommandManager.GetInstance();
            SessionMgr          = SessionManager.GetInstance();
            PacketMgr           = PacketManager.GetInstance();
            BufferMgr           = BufferManager.GetInstance(ServerConfig.BufferSize * 2
                                    * ServerConfig.MaxConnections, ServerConfig.BufferSize);
            Log.Message("Initing server!");
            Server              = Server.GetInstance(ServerConfig.SocketSettings);
        }
    }
}
