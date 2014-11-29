using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using ServerFramework.Database;
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

        internal static void Init()
        {
            Log.Message(LogType.Init, "Initialising command manager");
            CommandMgr          = CommandManager.GetInstance();
            Log.Message(LogType.Init, "Initialising session manager");
            SessionMgr          = SessionManager.GetInstance();
            Log.Message(LogType.Init, "Initialising packet manager");
            PacketMgr           = PacketManager.GetInstance();
            Log.Message(LogType.Init, "Initialising buffer manager");
            BufferMgr           = BufferManager.GetInstance(ServerConfig.BufferSize * 2
                                    * ServerConfig.MaxConnections, ServerConfig.BufferSize);
        }
    }
}
