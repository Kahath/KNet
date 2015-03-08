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

using DILibrary.Constants;
using DILibrary.DependencyInjection;
using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using ServerFramework.Database;
using ServerFramework.Managers.Core;
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
    public delegate void PacketHandler(Packet packet);
    public delegate void ManagerInitialisationEventHandler(object sender, EventArgs e);
    public delegate void PacketSendEventHandler(object sender, EventArgs e);
    public delegate void ServerEventHandler(object sender, SocketAsyncEventArgs e);
    public delegate void PacketManagerInvokeEventHandler(object sender, EventArgs e);

    #endregion

    public sealed class KahathFramework
    {
        #region Fields

        private static SocketListenerSettings _socketSettings;
		private static Server _server;

        #endregion

        #region Properties

		public static Server Server
        {
			get 
			{
				if (_server == null)
					_server = new Server(_socketSettings);

				return _server;
			}
        }

        #endregion

        #region Constructors

        public KahathFramework()
        {
            DependencyManager.Map(typeof(IConfig), typeof(ConfigInject));
			DependencyManager.Map(typeof(IServer), typeof(ServerInject));

            ServerConfig.Init();
			Manager.LogMgr = LogManager.GetInstance();

            _socketSettings = new SocketListenerSettings
				(
					ServerConfig.MaxConnections
				,	ServerConfig.Backlog
				,	ServerConfig.MaxSimultaneousAcceptOps
				,	ServerConfig.BufferSize
				,	ServerConfig.HeaderLength
				,	new IPEndPoint
						(
							IPAddress.Parse(ServerConfig.BindIP)
						,	ServerConfig.BindPort
						)
				);

            Console.WriteLine();
            Manager.LogMgr.Log(LogType.Cmd, "Configuration");
            Manager.LogMgr.Log(LogType.Cmd, "Bind IP: {0}", ServerConfig.BindIP);
            Manager.LogMgr.Log(LogType.Cmd, "Bind port: {0}", ServerConfig.BindPort);
            Manager.LogMgr.Log(LogType.Cmd, "Console log level: {0}", ServerConfig.LogLevel);
            Manager.LogMgr.Log(LogType.Cmd, "Packet log level: {0}", ServerConfig.PacketLogLevel);
            Manager.LogMgr.Log(LogType.Cmd, "Opcode allow level: {0}", ServerConfig.OpcodeAllowLevel);
            Manager.LogMgr.Log(LogType.Cmd, "Buffer size: {0}", ServerConfig.BufferSize);
            Manager.LogMgr.Log(LogType.Cmd, "Maximum connections: {0}", ServerConfig.MaxConnections);
            Manager.LogMgr.Log(LogType.Cmd, "Maximum sockets for accept: {0}", ServerConfig.MaxSimultaneousAcceptOps);
            Manager.LogMgr.Log(LogType.Cmd, "Backlog: {0}", ServerConfig.Backlog);
            Manager.LogMgr.Log(LogType.Cmd, "Packet header length: {0}", ServerConfig.HeaderLength);
            Manager.LogMgr.Log(LogType.Cmd, "Database host name: {0}", ServerConfig.DBHost);
            Manager.LogMgr.Log(LogType.Cmd, "Database port: {0}", ServerConfig.DBPort);
            Manager.LogMgr.Log(LogType.Cmd, "Database username: {0}", ServerConfig.DBUser);
            Manager.LogMgr.Log(LogType.Cmd, "Database password: {0}", ServerConfig.DBPass);
            Manager.LogMgr.Log(LogType.Cmd, "Database name: {0}", ServerConfig.DBName);
            Console.WriteLine();

            Manager.LogMgr.Log(LogType.Init, "Initialising application database connection.");
			
			DB.Application.Init
				(
					ServerConfig.DBHost
				,	ServerConfig.DBUser
				,	ServerConfig.DBPass
				,	ServerConfig.DBPort
				,	ServerConfig.DBName
				);

			//Manager.DatabaseMgr = DatabaseManager.GetInstance();

            Manager.LogMgr.Log(LogType.Init, "Initialising managers.");
            Manager.Init();

            Manager.LogMgr.Log(LogType.Init, "Initialising server!");
        }



        #endregion 

		#region Methods

		#region Start

		public void Start()
		{
			Server.Init();

			while (true)
			{
				string command = Console.ReadLine();

				if (command != null)
					Manager.CommandMgr.InvokeCommand(command.ToLower());
				else
					Manager.LogMgr.Log(LogType.Command, "Wrong input");
			}
		}

		#endregion

		#endregion
	}
}
