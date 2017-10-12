/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Commands.Base;
using KNetFramework.Enums;
using KNetFramework.Network.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KNetFramework.Managers.Interface
{
	public interface ICommandManager : IManager, IDisposable
	{
		#region Properties

		BlockingCollection<Command> CommandTable { get; set; }

		#endregion

		#region Events

		event CommandEventHandler BeforeCommandInvoke;

		#endregion

		#region Methods

		bool InvokeCommand(Client user, string command);
		Command GetCommand(Client user, IList<string> path, Command baseCommand = null, IEnumerable<Command> commandTable = null);
		CommandValidation ValidateCommand(Command command);
		string AvailableSubCommands(Command command, CommandLevel userLevel = CommandLevel.Zero);
		IEnumerable<Command> GetSubCommands(Command command, CommandLevel userLevel = CommandLevel.Zero);

		#endregion
	}
}
