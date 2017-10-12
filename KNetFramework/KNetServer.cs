/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using DILibrary.DependencyInjection;
using KNetFramework.Configuration.Base;
using KNetFramework.Configuration.Core;
using KNetFramework.Configuration.Helpers;
using KNetFramework.Database.Context;
using KNetFramework.Database.Model.KNet.Server;
using KNetFramework.Enums;
using KNetFramework.Events;
using KNetFramework.Exceptions;
using KNetFramework.Managers;
using KNetFramework.Managers.Injection;
using KNetFramework.Managers.Interface;
using KNetFramework.Network.Packets;
using KNetFramework.Network.Session;
using KNetFramework.Network.Socket;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace KNetFramework
{
	#region Delegates

	public delegate bool CommandHandler(Client user, params string[] args);
	public delegate void OpcodeHandler(Client pClient, Packet packet);
	public delegate void CommandEventHandler(object sender, EventArgs e);
	public delegate void ServerEventHandler(object sender, SocketAsyncEventArgs e);
	public delegate void AssemblyEventHandler(object sender, AssemblyEventArgs e);

	#endregion

	public sealed class KNetServer : IDisposable
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

			KNetConfig.Init();

			_socketSettings = new SocketListenerSettings
				(
					KNetConfig.MaxConnections,
					KNetConfig.Backlog,
					KNetConfig.MaxSimultaneousAcceptOps,
					KNetConfig.BufferSize,
					KNetConfig.HeaderLength,
					new IPEndPoint(IPAddress.Parse(KNetConfig.BindIP), KNetConfig.BindPort)
				);

			Manager.LogManager.Log(LogTypes.Info, "Configuration");
			Manager.LogManager.Log(LogTypes.Info, $"Bind IP: {KNetConfig.BindIP}");
			Manager.LogManager.Log(LogTypes.Info, $"Bind port: {KNetConfig.BindPort}");
			Manager.LogManager.Log(LogTypes.Info, $"Console log level: {KNetConfig.LogLevel}");
			Manager.LogManager.Log(LogTypes.Info, $"Packet log level: {KNetConfig.PacketLogLevel}");
			Manager.LogManager.Log(LogTypes.Info, $"Opcode allow level: {KNetConfig.OpcodeAllowLevel}");
			Manager.LogManager.Log(LogTypes.Info, $"Buffer size: {KNetConfig.BufferSize}");
			Manager.LogManager.Log(LogTypes.Info, $"Maximum connections: {KNetConfig.MaxConnections}");
			Manager.LogManager.Log(LogTypes.Info, $"Maximum sockets for accept: {KNetConfig.MaxSimultaneousAcceptOps}");
			Manager.LogManager.Log(LogTypes.Info, $"Backlog: {KNetConfig.Backlog}");
			Manager.LogManager.Log(LogTypes.Info, $"Packet header length: {KNetConfig.HeaderLength}");
			Manager.LogManager.Log(LogTypes.Info, $"Big Packet header length: {KNetConfig.BigHeaderLength}");
			Manager.LogManager.Log(LogTypes.Info, $"Database host name: {KNetConfig.DBHost}");
			Manager.LogManager.Log(LogTypes.Info, $"Database port: {KNetConfig.DBPort}");
			Manager.LogManager.Log(LogTypes.Info, $"Database username: {KNetConfig.DBUser}");
			Manager.LogManager.Log(LogTypes.Info, $"Database password: {KNetConfig.DBPass}");
			Manager.LogManager.Log(LogTypes.Info, $"Database name: {KNetConfig.DBName}");
			Manager.LogManager.Log(LogTypes.Info, $"Log filename; {KNetConfig.LogFilePath}");
			Manager.LogManager.Log(LogTypes.Info, $"Is Console: {KNetConfig.IsConsole}");
			Manager.LogManager.Log();

			Manager.LogManager.Log(LogTypes.Init, "Initialising KNet database connection.");

			using (KNetContext context = new KNetContext())
			{
				IEnumerable<DbEntityValidationResult> errors = context.GetValidationErrors();

				if (errors.Any())
				{
					foreach (DbEntityValidationResult result in errors)
						Manager.LogManager.Log(LogTypes.DB, $"{result.ToString()}");

					Manager.LogManager.Log(LogTypes.Critical, "Database error!");
				}

				ServerModel server = new ServerModel() { IsSuccessful = false };
				Manager.DatabaseManager.AddOrUpdate(context, true, server);
			}

			Manager.LogManager.Log(LogTypes.Init, "Successfully tested database connection.");
			Manager.LogManager.Log(LogTypes.Init, "Initializing managers.");
			Manager.Init();

			if (AfterInitialisation != null)
			{
				Manager.LogManager.Log(LogTypes.Init, "Post initialization...");
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

			if (KNetConfig.IsConsole)
			{
				Thread thread = new Thread(new ThreadStart(() =>
				{
					while (Server.IsRunning)
					{
						string command = Console.ReadLine();

						if (!String.IsNullOrEmpty(command))
						{
							Manager.CommandManager.InvokeCommand(ConsoleClient, command.ToLower());
						}
						else
						{
							Manager.LogManager.Log(LogTypes.Command, "Wrong input");
						}
					}
				}))
				{
					IsBackground = true
				};
				thread.Start();

				Manager.LogManager.Log(LogTypes.Init, "Console thread initialised");
			}

			Manager.LogManager.Log(LogTypes.Init, "Server successfully initialized");

			Manager.DatabaseManager.Update<KNetContext, ServerModel>(x =>
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

		#region Dispose

		public void Dispose()
		{
			_waitEvent.Dispose();
		}

		#endregion

		#region UnhandledExceptionHandler

		private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is DatabaseException)
			{
				Manager.LogManager.Log(LogTypes.DB, (DatabaseException)e.ExceptionObject);
			}
			else
			{
				Manager.LogManager.Log(LogTypes.Error, (Exception)e.ExceptionObject);
			}
		}

		#endregion

		#endregion
	}
}
