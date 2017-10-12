/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Configuration.Helpers;
using KNetFramework.Database.Context;
using KNetFramework.Database.Model.KNet.Log;
using KNetFramework.Enums;
using KNetFramework.Managers.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace KNetFramework.Managers.Injection
{
	public sealed class LogManagerInject : ILogManager, IDisposable
	{
		#region Fields

		private BlockingCollection<Tuple<ConsoleColor, string>> _logQueue = new BlockingCollection<Tuple<ConsoleColor, string>>();
		private List<LogModel> _logList;
		private string _logFilePath = $"{DateTime.Now.ToString("yyyy-mm-dd")}.log";

		#endregion

		#region Properties

		private BlockingCollection<Tuple<ConsoleColor, string>> LogQueue
		{
			get { return _logQueue; }
			set { _logQueue = value; }
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

		private LogTypes LogLevel
		{
			get
			{
				LogTypes logType = KNetConfig.LogLevel;

				if (logType == LogTypes.None)
					logType = LogTypes.Error | LogTypes.Critical;

				return logType;
			}
		}

		private string LogFilePath
		{
			get { return _logFilePath; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="Core.LogManager"/> type.
		/// </summary>
		LogManagerInject()
		{
			Init();
		}

		#endregion

		#region Methods

		#region Init

		/// <summary>
		/// Initialises LogManager.
		/// </summary>
		public void Init()
		{
			if (KNetConfig.IsConsole)
			{
				Console.InputEncoding = Encoding.UTF8;
				Console.OutputEncoding = Encoding.UTF8;
			}

			Thread logThread = new Thread(() =>
			{
				while (true)
				{
					Tuple<ConsoleColor, string> item = LogQueue.Take();

					if (item != null)
					{
						try
						{
							if (KNetConfig.IsInitialised)
								File.AppendAllText(KNetConfig.LogFilePath, $"{item.Item2}\n");
							else
								File.AppendAllText(LogFilePath, $"{item.Item2}\n");

							if (KNetConfig.IsConsole)
							{
								Console.ForegroundColor = item.Item1;
								Console.WriteLine(item.Item2);
								Console.ResetColor();
							}
						}
						catch (NullReferenceException) { }
					}
				}
			})
			{
				IsBackground = true
			};
			logThread.Start();
		}

		#endregion

		#region Message

		/// <summary>
		/// Adds message to queue.
		/// </summary>
		/// <param name="type"><see cref="KNetFramework.Constants.Misc.LogType"/> enum type.</param>
		/// <param name="message">Message.</param>
		/// <param name="args">Message arguments.</param>
		private void Message(LogTypes type, string message, Exception exception)
		{
			ConsoleColor color = ConsoleColor.White;

			if (KNetConfig.IsConsole)
			{
				switch (type)
				{
					case LogTypes.Normal:
						color = ConsoleColor.Gray;
						break;
					case LogTypes.Init:
						color = ConsoleColor.Green;
						break;
					case LogTypes.DB:
						color = ConsoleColor.DarkMagenta;
						break;
					case LogTypes.Info:
						color = ConsoleColor.Cyan;
						break;
					case LogTypes.Command:
						color = ConsoleColor.Blue;
						break;
					case LogTypes.Warning:
						color = ConsoleColor.Yellow;
						break;
					case LogTypes.Error:
						color = ConsoleColor.Red;
						break;
					case LogTypes.Critical:
						color = ConsoleColor.DarkRed;
						break;
					default:
						color = ConsoleColor.White;
						break;
				}
			}

			if ((LogLevel & type) == type)
			{
				string msg = String.Format($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] [{type}] {message}");

				if (KNetConfig.IsInitialised)
				{
					LogModel logModel = new LogModel()
					{
						LogTypeID = (int)type,
						Message = message,
						Description = exception?.ToString(),
					};

					Manager.DatabaseManager.AddOrUpdate<KNetContext, LogModel>(logModel);
				}

				LogQueue.Add(Tuple.Create(color, msg));

				if (type == LogTypes.Critical)
					Environment.Exit(-1);
			}
		}

		#endregion

		#region Log

		/// <summary>
		/// Adds exception message to queue.
		/// </summary>
		/// <param name="type"><see cref="KNetFramework.Constants.Misc.LogType"/> enum type.</param>
		/// <param name="message">Message.</param>
		/// <param name="exception">Exception</param>
		public void Log(LogTypes type, string message, Exception exception)
		{
			Message(type, message, exception);
		}

		/// <summary>
		/// Adds exception message to queue.
		/// </summary>
		/// <param name="type"><see cref="KNetFramework.Constants.Misc.LogType"/> enum type.</param>
		/// <param name="exception">Exception</param>
		public void Log(LogTypes type, Exception exception)
		{
			Log(type, exception.Message, exception);
		}

		/// <summary>
		/// Adds message to queue
		/// </summary>
		/// <param name="type"></param>
		/// <param name="message"></param>
		public void Log(LogTypes type, string message)
		{
			Message(type, message, null);
		}

		/// <summary>
		/// Adds message to queue with Normal LogType.
		/// </summary>
		/// <param name="message">Message.</param>
		public void Log(string message)
		{
			Log(LogTypes.Normal, message);
		}

		/// <summary>
		/// Adds empty message to queue with Normal LogType.
		/// </summary>
		public void Log()
		{
			Log(LogTypes.Normal, "");
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Disposes object.
		/// </summary>
		public void Dispose()
		{
			_logQueue.Dispose();
		}

		#endregion

		#endregion
	}
}
