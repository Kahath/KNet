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

		private LogType? LogLevel
		{
			get { return ServerConfig.LogLevel ?? (LogType)0xFF; }
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
		private void Message(LogType type, string message, params object[] args)
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
						msg = String.Format(message, args);
						ConsoleLogQueue.Add(Tuple.Create<ConsoleColor, string>(color, msg));
					break;
					case LogType.Init:
					case LogType.DB:
					case LogType.Warning:
					case LogType.Error:
					case LogType.Critical:
					default:
						msg = String.Format
						(
							$"[{DateTime.Now.ToString("HH:mm:ss.fff")}] {String.Format(message, args)}"
						);

						ConsoleLogQueue.Add(Tuple.Create<ConsoleColor, string>(color, msg));

						if (ServerConfig.LogLevel != null)
						{
							LogModel logModel = new LogModel();
							logModel.LogTypeID = (int)type;
							logModel.Message = String.Format(message, args);

							using (ApplicationContext context = new ApplicationContext())
							{
								context.Logs.Add(logModel);
								context.SaveChanges();
							}
						}
					break;
				}
			}

		}

		#endregion

		#region Log

		/// <summary>
		/// Adds message to queue.
		/// </summary>
		/// <param name="type"><see cref="ServerFramework.Constants.Misc.LogType"/> enum type.</param>
		/// <param name="message">Message.</param>
		/// <param name="args">Message arguments.</param>
		public void Log(LogType type, string message, params object[] args)
		{
			Message(type, message, args);
		}

		/// <summary>
		/// Adds message to queue with Normal LogType.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="args">Message arguments</param>
		public void Log(string message, params object[] args)
		{
			Log(LogType.Normal, message, args);
		}

		/// <summary>
		/// Adds Message to queue with Normal LogType.
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
