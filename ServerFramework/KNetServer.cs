/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using DILibrary.DependencyInjection;
using ServerFramework.Configuration.Base;
using ServerFramework.Configuration.Core;
using ServerFramework.Configuration.Helpers;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.Server;
using ServerFramework.Enums;
using ServerFramework.Events;
using ServerFramework.Exceptions;
using ServerFramework.Managers;
using ServerFramework.Managers.Injection;
using ServerFramework.Managers.Interface;
using ServerFramework.Network.Packets;
using ServerFramework.Network.Session;
using ServerFramework.Network.Socket;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerFramework
{
	#region Delegates

	public delegate bool CommandHandler(Client user, params string[] args);
	public delegate void OpcodeHandler(Client pClient, Packet packet);
	public delegate void CommandEventHandler(object sender, EventArgs e);
	public delegate void ServerEventHandler(object sender, SocketAsyncEventArgs e);
	public delegate void AssemblyEventHandler(object sender, AssemblyEventArgs e);

	#endregion

	public sealed class KNetServer
	{
		#region Constants

		private const string ConsoleName = "Console";

		#endregion

		#region Fields

		private Client _consoleClient;
		private static SocketListenerSettings _socketSettings;
		private static Server _server;
		private ManualResetEvent _waitEvent = new ManualResetEvent(false);

		#endregion

		#region Properties

		public Server Server
		{
			get
			{
				if (_server == null)
					_server = Server.GetInstance(_socketSettings);

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
						UserLevel = CommandLevel.Ten,
						Token = new ConsoleClient()
						{
							SessionID = 0,
							ID = 0,
							Name = ConsoleName
						}
					};
				}

				return _consoleClient;
			}
		}

		private ManualResetEvent WaitEvent
		{
			get { return _waitEvent; }
		}

		#endregion

		#region Events

		public event EventHandler AfterInitialisation;

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates new <see cref="KNetServer"/> type.
		/// </summary>
		public KNetServer()
		{
			Init();
		}

		#endregion

		#region Methods

		#region Init

		/// <summary>
		/// Initialises server.
		/// </summary>
		private void Init()
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

			DependencyManager.Map<IConfig, XmlConfiguration>();
			DependencyManager.Map<IServer, ServerInject>();
			DependencyManager.Map<IAssemblyManager, AssemblyManagerInject>();
			DependencyManager.Map<IBufferManager, BufferManagerInject>();
			DependencyManager.Map<ICommandManager, CommandManagerInject>();
			DependencyManager.Map<IDatabaseManager, DatabaseManagerInject>();
			DependencyManager.Map<ILogManager, LogManagerInject>();
			DependencyManager.Map<IPacketLogManager, PacketLogManagerInject>();
			DependencyManager.Map<IPacketManager, PacketManagerInject>();
			DependencyManager.Map<ISessionManager, SessionManagerInject>();

			ServerConfig.Init();

			_socketSettings = new SocketListenerSettings
				(
					ServerConfig.MaxConnections,
					ServerConfig.Backlog,
					ServerConfig.MaxSimultaneousAcceptOps,
					ServerConfig.BufferSize,
					ServerConfig.HeaderLength,
					new IPEndPoint(IPAddress.Parse(ServerConfig.BindIP), ServerConfig.BindPort)
				);

			Manager.LogMgr.Log(LogTypes.Info, "Configuration");
			Manager.LogMgr.Log(LogTypes.Info, $"Bind IP: {ServerConfig.BindIP}");
			Manager.LogMgr.Log(LogTypes.Info, $"Bind port: {ServerConfig.BindPort}");
			Manager.LogMgr.Log(LogTypes.Info, $"Console log level: {ServerConfig.LogLevel}");
			Manager.LogMgr.Log(LogTypes.Info, $"Packet log level: {ServerConfig.PacketLogLevel}");
			Manager.LogMgr.Log(LogTypes.Info, $"Opcode allow level: {ServerConfig.OpcodeAllowLevel}");
			Manager.LogMgr.Log(LogTypes.Info, $"Buffer size: {ServerConfig.BufferSize}");
			Manager.LogMgr.Log(LogTypes.Info, $"Maximum connections: {ServerConfig.MaxConnections}");
			Manager.LogMgr.Log(LogTypes.Info, $"Maximum sockets for accept: {ServerConfig.MaxSimultaneousAcceptOps}");
			Manager.LogMgr.Log(LogTypes.Info, $"Backlog: {ServerConfig.Backlog}");
			Manager.LogMgr.Log(LogTypes.Info, $"Packet header length: {ServerConfig.HeaderLength}");
			Manager.LogMgr.Log(LogTypes.Info, $"Big Packet header length: {ServerConfig.BigHeaderLength}");
			Manager.LogMgr.Log(LogTypes.Info, $"Database host name: {ServerConfig.DBHost}");
			Manager.LogMgr.Log(LogTypes.Info, $"Database port: {ServerConfig.DBPort}");
			Manager.LogMgr.Log(LogTypes.Info, $"Database username: {ServerConfig.DBUser}");
			Manager.LogMgr.Log(LogTypes.Info, $"Database password: {ServerConfig.DBPass}");
			Manager.LogMgr.Log(LogTypes.Info, $"Database name: {ServerConfig.DBName}");
			Manager.LogMgr.Log(LogTypes.Info, $"Log filename; {ServerConfig.LogFilePath}");
			Manager.LogMgr.Log(LogTypes.Info, $"Is Console: {ServerConfig.IsConsole}");
			Manager.LogMgr.Log();

			Manager.LogMgr.Log(LogTypes.Init, "Initialising application database connection.");

			using (ApplicationContext context = new ApplicationContext())
			{
				IEnumerable<DbEntityValidationResult> errors = context.GetValidationErrors();

				if (errors.Any())
				{
					foreach (DbEntityValidationResult result in errors)
						Manager.LogMgr.Log(LogTypes.DB, $"{result.ToString()}");

					Manager.LogMgr.Log(LogTypes.Critical, "Database error!");
				}

				ServerModel server = new ServerModel() { IsSuccessful = false };
				Manager.DatabaseMgr.AddOrUpdate(context, true, server);
			}

			Manager.LogMgr.Log(LogTypes.Init, "Successfully tested database connection.");
			Manager.LogMgr.Log(LogTypes.Init, "Initialising managers.");
			Manager.Init();

			if (AfterInitialisation != null)
			{
				Manager.LogMgr.Log(LogTypes.Init, "Post initialisation...");
				AfterInitialisation(new object(), new EventArgs());
			}
		}

		#endregion

		#region Start

		/// <summary>
		/// Starts server loop.
		/// </summary>
		public void Start()
		{
			Server.Init();

			if (ServerConfig.IsConsole)
			{
				Thread thread = new Thread(new ThreadStart(() =>
				{
					while (Server.IsRunning)
					{
						string command = Console.ReadLine();

						if (!String.IsNullOrEmpty(command))
						{
							Manager.CommandMgr.InvokeCommand(ConsoleClient, command.ToLower());
						}
						else
						{
							Manager.LogMgr.Log(LogTypes.Command, "Wrong input");
						}
					}
				}));
				thread.IsBackground = true;
				thread.Start();

				Manager.LogMgr.Log(LogTypes.Init, "Console thread initialised");
			}

			Manager.LogMgr.Log(LogTypes.Init, "Server successfully initialised");

			Manager.DatabaseMgr.Update<ApplicationContext, ServerModel>(x =>
				x.OrderByDescending(y => y.ID).First(),
				x => x.IsSuccessful = true);

			WaitEvent.WaitOne();
		}

		#endregion

		#region Quit

		public void Quit()
		{
			Server.Quit();
			WaitEvent.Set();
		}

		#endregion

		#region UnhandledExceptionHandler

		private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is DatabaseException)
			{
				Manager.LogMgr.Log(LogTypes.DB, (DatabaseException)e.ExceptionObject);
			}
			else
			{
				Manager.LogMgr.Log(LogTypes.Error, (Exception)e.ExceptionObject);
			}
		}

		#endregion

		#endregion
	}
}
