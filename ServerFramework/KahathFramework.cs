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

using DILibrary.DependencyInjection;
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
    public delegate void PacketHandler(Packet packet);
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

        #region Properties

		public Server Server
        {
			get { return new Server(_socketSettings); }
        }

        #endregion

        #region Constructors

        public KahathFramework()
        {
            DependencyManager.Map(typeof(IConfig), typeof(ConfigInject));
			DependencyManager.Map(typeof(IServer), typeof(ServerInject));

            ServerConfig.Init();

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

            LogManager.Init();

            Console.WriteLine();
            LogManager.Log(LogType.Cmd, "Configuration");
            LogManager.Log(LogType.Cmd, "Bind IP: {0}", ServerConfig.BindIP);
            LogManager.Log(LogType.Cmd, "Bind port: {0}", ServerConfig.BindPort);
            LogManager.Log(LogType.Cmd, "Console log level: {0}", ServerConfig.LogLevel);
            LogManager.Log(LogType.Cmd, "Packet log level: {0}", ServerConfig.PacketLogLevel);
            LogManager.Log(LogType.Cmd, "Opcode allow level: {0}", ServerConfig.OpcodeAllowLevel);
            LogManager.Log(LogType.Cmd, "Buffer size: {0}", ServerConfig.BufferSize);
            LogManager.Log(LogType.Cmd, "Maximum connections: {0}", ServerConfig.MaxConnections);
            LogManager.Log(LogType.Cmd, "Maximum sockets for accept: {0}", ServerConfig.MaxSimultaneousAcceptOps);
            LogManager.Log(LogType.Cmd, "Backlog: {0}", ServerConfig.Backlog);
            LogManager.Log(LogType.Cmd, "Packet header length: {0}", ServerConfig.HeaderLength);
            LogManager.Log(LogType.Cmd, "Database host name: {0}", ServerConfig.DBHost);
            LogManager.Log(LogType.Cmd, "Database port: {0}", ServerConfig.DBPort);
            LogManager.Log(LogType.Cmd, "Database username: {0}", ServerConfig.DBUser);
            LogManager.Log(LogType.Cmd, "Database password: {0}", ServerConfig.DBPass);
            LogManager.Log(LogType.Cmd, "Database name: {0}", ServerConfig.DBName);
            Console.WriteLine();

            LogManager.Log(LogType.Init, "Initialising application database connection.");
            DB.Application.Init
				(
					ServerConfig.DBHost
				,	ServerConfig.DBUser
				,	ServerConfig.DBPass
				,	ServerConfig.DBPort
				,	ServerConfig.DBName
				);

            LogManager.Log(LogType.Init, "Initialising managers.");
            Manager.Init();

            LogManager.Log(LogType.Init, "Initialising server!");
        }



        #endregion 

		#region Methods

		public void Start()
		{
			Server.Init();

			while (true)
			{
				string command = Console.ReadLine();

				if (command != null)
					Manager.CommandMgr.InvokeCommand(command.ToLower());
				else
					LogManager.Log(LogType.Command, "Wrong input");
			}
		}

		#endregion
	}
}
