/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Commands.Base;
using ServerFramework.Enums;
using ServerFramework.Managers.Base;
using ServerFramework.Managers.Interface;
using ServerFramework.Network.Session;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ServerFramework.Managers.Core
{
	public class CommandManager : ManagerBase<CommandManager, ICommandManager>
	{
		#region Properties

		internal BlockingCollection<Command> CommandTable
		{
			get { return Instance.CommandTable; }
			set { Instance.CommandTable = value; }
		}

		#endregion

		#region Events

		public event CommandEventHandler BeforeCommandInvoke
		{
			add { Instance.BeforeCommandInvoke += value; }
			remove { Instance.BeforeCommandInvoke -= value; }
		}

		#endregion

		#region Methods

		public bool InvokeCommand(Client user, string command)
		{
			return Instance.InvokeCommand(user, command);
		}

		internal Command GetCommand(Client user, IList<string> path, Command baseCommand = null, IEnumerable<Command> commandTable = null)
		{
			return Instance.GetCommand(user, path, baseCommand, commandTable);
		}

		public CommandValidation ValidateCommand(Command command)
		{
			return Instance.ValidateCommand(command);
		}

		public string AvailableSubCommands(Command command, CommandLevel userLevel = CommandLevel.Zero)
		{
			return Instance.AvailableSubCommands(command, userLevel);
		}

		public IEnumerable<Command> GetSubCommands(Command command, CommandLevel userLevel = CommandLevel.Zero)
		{
			return Instance.GetSubCommands(command, userLevel);
		}

		#endregion
	}
}
