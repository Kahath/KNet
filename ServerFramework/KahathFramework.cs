using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using ServerFramework.Database;
using ServerFramework.Logging;
using ServerFramework.Managers;
using ServerFramework.Network.Packets;
using ServerFramework.Network.Socket;
using System;
using System.Net;
using System.Net.Sockets;

namespace ServerFramework
{
    #region Delegates

    public delegate bool CommandScriptHandler(params string[] args);
    public delegate void PacketHandler(UserToken token);
    public delegate void ManagerInitialisationEventHandler(object sender, EventArgs e);
    public delegate void PacketSendEventHandler(object sender, EventArgs e);
    public delegate void ServerEventHandler(object sender, SocketAsyncEventArgs e);
    public delegate void PacketManagerInvokeEventHandler(object sender, EventArgs e);

    #endregion

    public sealed class KahathFramework
    {
        #region Fields

        private SocketListenerSettings _socketSettings;

        #endregion

        #region Constructors

        public KahathFramework()
        {
            ServerConfig.Init();

            LogManager.Log(LogType.Init, "Initialising application database connection.");
            DB.Application.Init(ServerConfig.DBHost, ServerConfig.DBUser, ServerConfig.DBPass
                , ServerConfig.DBPort, ServerConfig.DBName);

            LogManager.Log(LogType.Init, "Initialising managers.");
            Manager.Init();

            _socketSettings = new SocketListenerSettings(
                ServerConfig.MaxConnections, ServerConfig.Backlog
                , ServerConfig.MaxSimultaneousAcceptOps, ServerConfig.BufferSize
                , ServerConfig.HeaderLength, new IPEndPoint(
                    IPAddress.Parse(ServerConfig.BindIP), ServerConfig.BindPort));

            LogManager.Log(LogType.Init, "Initialising server!");
            Server.GetInstance(_socketSettings);

            while(true)
            {
                Manager.CommandMgr.InvokeCommand(Console.ReadLine().ToLower());
            }
        }

        #endregion 
    }
}
