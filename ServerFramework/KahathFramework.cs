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
using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using ServerFramework.Database.Context;
using ServerFramework.Managers;
using ServerFramework.Managers.Core;
using ServerFramework.Network.Packets;
using ServerFramework.Network.Socket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ServerFramework
{
	#region Delegates

	public delegate bool CommandHandler(Client user, params string[] args);
	public delegate void OpcodeHandler(Client pClient, Packet packet);
	public delegate void ServerEventHandler(object sender, SocketAsyncEventArgs e);
	public delegate void PacketEventHandler(object sender, EventArgs e);

	#endregion

	public sealed class KahathFramework
	{
		#region Fields

		private Client _consoleClient;
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

		private Client ConsoleClient
		{
			get
			{
				if (_consoleClient == null)
				{
					_consoleClient = new Client()
					{
						UserLevel = CommandLevel.Ten
					,	Token = new ConsoleClient()
						{
							SessionId = 0
						,	ID = 0
						,	Name = "Console"
						}
					};
				}

				return _consoleClient;
			}
		}

		#endregion

		#region Constructors

		public KahathFramework()
		{
			DependencyManager.Map<IConfig, ConfigInject>();
			DependencyManager.Map<IServer, ServerInject>();

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

			Manager.LogMgr.Log(LogType.Info, "Configuration");
			Manager.LogMgr.Log(LogType.Info, "Bind IP: {0}", ServerConfig.BindIP);
			Manager.LogMgr.Log(LogType.Info, "Bind port: {0}", ServerConfig.BindPort);
			Manager.LogMgr.Log(LogType.Info, "Console log level: {0}", ServerConfig.LogLevel);
			Manager.LogMgr.Log(LogType.Info, "Packet log level: {0}", ServerConfig.PacketLogLevel);
			Manager.LogMgr.Log(LogType.Info, "Opcode allow level: {0}", ServerConfig.OpcodeAllowLevel);
			Manager.LogMgr.Log(LogType.Info, "Buffer size: {0}", ServerConfig.BufferSize);
			Manager.LogMgr.Log(LogType.Info, "Maximum connections: {0}", ServerConfig.MaxConnections);
			Manager.LogMgr.Log(LogType.Info, "Maximum sockets for accept: {0}", ServerConfig.MaxSimultaneousAcceptOps);
			Manager.LogMgr.Log(LogType.Info, "Backlog: {0}", ServerConfig.Backlog);
			Manager.LogMgr.Log(LogType.Info, "Packet header length: {0}", ServerConfig.HeaderLength);
			Manager.LogMgr.Log(LogType.Info, "Database host name: {0}", ServerConfig.DBHost);
			Manager.LogMgr.Log(LogType.Info, "Database port: {0}", ServerConfig.DBPort);
			Manager.LogMgr.Log(LogType.Info, "Database username: {0}", ServerConfig.DBUser);
			Manager.LogMgr.Log(LogType.Info, "Database password: {0}", ServerConfig.DBPass);
			Manager.LogMgr.Log(LogType.Info, "Database name: {0}", ServerConfig.DBName);
			Manager.LogMgr.Log();

			Manager.LogMgr.Log(LogType.Init, "Initialising application database connection.");

			using (ApplicationContext context = new ApplicationContext())
			{
				IEnumerable<DbEntityValidationResult> errors = context.GetValidationErrors();

				if (errors.Any())
				{
					foreach (DbEntityValidationResult result in errors)
						Manager.LogMgr.Log(LogType.DB, "{0}", result.ToString());

					Console.ReadLine();
					Environment.Exit(0);
				}
			}

			Manager.LogMgr.Log(LogType.Init, "Successfully tested database connection.");

			Manager.LogMgr.Log(LogType.Init, "Initialising managers.");
			Manager.Init();
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

				if (!String.IsNullOrEmpty(command))
					Manager.CommandMgr.InvokeCommand(ConsoleClient, command.ToLower());
				else
					Manager.LogMgr.Log(LogType.Command, "Wrong input");
			}
		}

		#endregion

		#endregion
	}
}
