/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Enums;
using KNetFramework.Managers.Base;
using KNetFramework.Managers.Interface;
using System;

namespace KNetFramework.Managers.Core
{
	public class LogManager : ManagerBase<LogManager, ILogManager>
	{
		#region Methods

		#region Log

		/// <summary>
		/// Adds exception message to queue.
		/// </summary>
		/// <param name="type"><see cref="KNetFramework.Constants.Misc.LogType"/> enum type.</param>
		/// <param name="message">Message.</param>
		/// <param name="exception">Exception</param>
		public void Log(LogTypes type, string message, Exception exception)
		{
			Instance.Log(type, message, exception);
		}

		/// <summary>
		/// Adds exception message to queue.
		/// </summary>
		/// <param name="type"><see cref="KNetFramework.Constants.Misc.LogType"/> enum type.</param>
		/// <param name="exception">Exception</param>
		public void Log(LogTypes type, Exception exception)
		{
			Instance.Log(type, exception);
		}

		/// <summary>
		/// Adds message to queue
		/// </summary>
		/// <param name="type"></param>
		/// <param name="message"></param>
		public void Log(LogTypes type, string message)
		{
			Instance.Log(type, message);
		}

		/// <summary>
		/// Adds message to queue with Normal LogType.
		/// </summary>
		/// <param name="message">Message.</param>
		public void Log(string message)
		{
			Instance.Log(message);
		}

		/// <summary>
		/// Adds empty message to queue with Normal LogType.
		/// </summary>
		public void Log()
		{
			Instance.Log();
		}

		#endregion

		#endregion
	}
}
