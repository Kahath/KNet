using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using ServerFramework.Database;
using ServerFramework.Logging;
using ServerFramework.Logging.Packets;
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
        internal static PacketLogManager PacketLogMgr;

        internal static void Init()
        {
            LogManager.Log(LogType.Init, "Initialising packet log manager");
            PacketLogMgr        = PacketLogManager.GetInstance();
            LogManager.Log(LogType.Init, "Initialising command manager");
            CommandMgr          = CommandManager.GetInstance();
            LogManager.Log(LogType.Init, "Initialising session manager");
            SessionMgr          = SessionManager.GetInstance();
            LogManager.Log(LogType.Init, "Initialising packet manager");
            PacketMgr           = PacketManager.GetInstance();
            LogManager.Log(LogType.Init, "Initialising buffer manager");
            BufferMgr           = BufferManager.GetInstance(ServerConfig.BufferSize * 2
                                    * ServerConfig.MaxConnections, ServerConfig.BufferSize);
        }
    }
}
