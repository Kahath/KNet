/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.Log;
using ServerFramework.Enums;
using ServerFramework.Managers.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServerFramework.Managers.Core
{
	public sealed class LogManager : ManagerBase<LogManager>, IDisposable
	{
		#region Fields

		private BlockingCollection<Tuple<ConsoleColor, string>> _consoleLogQueue
			= new BlockingCollection<Tuple<ConsoleColor, string>>();
		private List<LogModel> _logList;

		#endregion

		#region Properties

		private BlockingCollection<Tuple<ConsoleColor, string>> ConsoleLogQueue
		{
			get { return _consoleLogQueue; }
			set { _consoleLogQueue = value; }
		}

		private List<LogModel> LogList
		{
			get
			{
				if (_logList == null)
					_logList = new List<LogModel>();

				return _logList;
			}
		}

		private LogType LogLevel
		{
			get
			{
				LogType logType = ServerConfig.LogLevel;

				if (logType == LogType.None)
					logType = LogType.Error | LogType.Critical;

				return logType;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="ServerFramework.Managers.Core.LogManager"/> type.
		/// </summary>
		LogManager()
		{
			Init();
		}

		#endregion

		#region Methods

		#region Init

		/// <summary>
		/// Initialises LogManager.
		/// </summary>
		protected override void Init()
		{
			Console.InputEncoding = Encoding.UTF8;
			Console.OutputEncoding = Encoding.UTF8;

			Thread logThread = new Thread(() =>
			{
				while (true)
				{
					var item = ConsoleLogQueue.Take();

					if (item != null)
					{
						try
						{
							Console.ForegroundColor = item.Item1;
							Console.WriteLine(item.Item2);
							Console.ResetColor();
						}
						catch (NullReferenceException) { }
					}
				}
			});

			logThread.IsBackground = true;
			logThread.Start();
		}

		#endregion

		#region Message

		/// <summary>
		/// Adds message to queue.
		/// </summary>
		/// <param name="type"><see cref="ServerFramework.Constants.Misc.LogType"/> enum type.</param>
		/// <param name="message">Message.</param>
		/// <param name="args">Message arguments.</param>
		private void Message(LogType type, string message, Exception exception)
		{
			ConsoleColor color;

			switch (type)
			{
				case LogType.Normal:
					color = ConsoleColor.Gray;
					break;
				case LogType.Init:
					color = ConsoleColor.Green;
					break;
				case LogType.DB:
					color = ConsoleColor.DarkMagenta;
					break;
				case LogType.Info:
					color = ConsoleColor.Cyan;
					break;
				case LogType.Command:
					color = ConsoleColor.Blue;
					break;
				case LogType.Warning:
					color = ConsoleColor.Yellow;
					break;
				case LogType.Error:
					color = ConsoleColor.Red;
					break;
				case LogType.Critical:
					color = ConsoleColor.DarkRed;
					break;
				default:
					color = ConsoleColor.White;
					break;
			}

			if ((LogLevel & type) == type)
			{
				string msg = String.Empty;

				switch (type)
				{
					case LogType.Normal:
					case LogType.Info:
					case LogType.Command:
						msg = message;
						break;
					case LogType.Init:
					case LogType.DB:
					case LogType.Warning:
					case LogType.Error:
					case LogType.Critical:
					default:
						msg = String.Format($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] {message}");

						if (ServerConfig.IsInitialised)
						{
							LogModel logModel = new LogModel();
							logModel.LogTypeID = (int)type;
							logModel.Message = message;
							logModel.Description = exception != null ? exception.ToString() : null;

							Manager.DatabaseMgr.AddOrUpdate<ApplicationContext, LogModel>(true, logModel);
						}
						break;
				}

				ConsoleLogQueue.Add(Tuple.Create(color, msg));

				if (type == LogType.Critical)
					Environment.Exit(-1);
			}

		}

		#endregion

		#region Log

		/// <summary>
		/// Adds exception message to queue.
		/// </summary>
		/// <param name="type"><see cref="ServerFramework.Constants.Misc.LogType"/> enum type.</param>
		/// <param name="message">Message.</param>
		/// <param name="exception">Exception</param>
		public void Log(LogType type, string message, Exception exception)
		{
			Message(type, message, exception);
		}

		/// <summary>
		/// Adds exception message to queue.
		/// </summary>
		/// <param name="type"><see cref="ServerFramework.Constants.Misc.LogType"/> enum type.</param>
		/// <param name="exception">Exception</param>
		public void Log(LogType type, Exception exception)
		{
			Log(type, exception.Message, exception);
		}

		/// <summary>
		/// Adds message to queue
		/// </summary>
		/// <param name="type"></param>
		/// <param name="message"></param>
		public void Log(LogType type, string message)
		{
			Message(type, message, null);
		}

		/// <summary>
		/// Adds message to queue with Normal LogType.
		/// </summary>
		/// <param name="message">Message.</param>
		public void Log(string message)
		{
			Log(LogType.Normal, message);
		}

		/// <summary>
		/// Adds empty message to queue with Normal LogType.
		/// </summary>
		public void Log()
		{
			Log(LogType.Normal, "");
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Disposes object.
		/// </summary>
		public void Dispose()
		{
			_consoleLogQueue.Dispose();
		}

		#endregion

		#endregion
	}
}
