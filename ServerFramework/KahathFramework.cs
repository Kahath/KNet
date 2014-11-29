using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using ServerFramework.Database;
using ServerFramework.Logging;
using ServerFramework.Managers;
using ServerFramework.Network.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework
{
    public class KahathFramework
    {
        private SocketListenerSettings _socketSettings;

        public KahathFramework()
        {
            ServerConfig.Init();

            Log.Init();

            Log.Message(LogType.Init, "Initialising application database connection.");
            DB.Application.Init(ServerConfig.DBHost, ServerConfig.DBUser, ServerConfig.DBPass
                , ServerConfig.DBPort, ServerConfig.DBName);

            Log.Message(LogType.Init, "Initialising managers.");
            Manager.Init();

            _socketSettings = new SocketListenerSettings(
                ServerConfig.MaxConnections, ServerConfig.Backlog
                , ServerConfig.MaxSimultaneousAcceptOps, ServerConfig.BufferSize
                , ServerConfig.HeaderLength, new IPEndPoint(
                    IPAddress.Parse(ServerConfig.BindIP), ServerConfig.BindPort));

            Log.Message(LogType.Init, "Initialising server!");
            Server.GetInstance(_socketSettings);

        }
    }
}
