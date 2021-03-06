﻿/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Enums;
using System;

namespace KNetFramework.Managers.Interface
{
	public interface ILogManager : IManager, IDisposable
	{
		#region Methods

		#region Log

		void Log(LogTypes type, string message, Exception exception);
		void Log(LogTypes type, Exception exception);
		void Log(LogTypes type, string message);
		void Log(string message);
		void Log();

		#endregion

		#endregion
	}
}
