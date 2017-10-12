/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Commands.Base;
using KNetFramework.Database.Context;
using KNetFramework.Database.Model.KNet.Command;
using KNetFramework.Database.Repository;
using KNetFramework.Enums;
using KNetFramework.Extensions;
using KNetFramework.Managers.Interface;
using KNetFramework.Network.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace KNetFramework.Managers.Injection
{
	public sealed class CommandManagerInject : ICommandManager, IDisposable
	{
		#region Fields

		private BlockingCollection<Command> _commandTable;

		#endregion

		#region Properties

		public BlockingCollection<Command> CommandTable
		{
			get { return _commandTable; }
			set { _commandTable = value; }
		}

		#endregion

		#region Events

		public event CommandEventHandler BeforeCommandInvoke;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates instance of <see cref="KNetFramework.Managers.Core.CommandManager"/> type.
		/// </summary>
		CommandManagerInject()
		{
			_commandTable = new BlockingCollection<Command>();
			Init();
		}

		#endregion

		#region Methods

		#region Init

		/// <summary>
		/// Initialises CommandManager.
		/// </summary>
		public void Init()
		{
			using (CommandRepository cRepo = new CommandRepository(new KNetContext()))
			{
				IEnumerable<CommandModel> commands = Manager.DatabaseManager.Get<CommandModel>(cRepo.Context, x =>
					x.Where(y => y.Active && y.ParentID == null).ToList());

				foreach (CommandModel command in commands)
				{
					Command c = null;
					Type type = Manager.AssemblyManager.GetType(command.AssemblyName, command.TypeName);
					MethodInfo method = type.GetMethodByName(command.MethodName);

					if (method != null)
					{
						object obj = Manager.AssemblyManager.InvokeConstructor(type);

						if (obj != null && command != null)
							c = Manager.AssemblyManager.InvokeMethod<Command>(obj, method);

						if (c != null)
						{
							UpdateBase(c);

							cRepo.UpdateCommandInfo(c, command);

							CommandTable.Add(c);
						}
					}
				}
			}

			Manager.LogManager.Log(LogTypes.Normal, $"{CommandTable.Count} Commands loaded");
		}

		#endregion

		#region InvokeCommand

		/// <summary>
		/// Invokes command
		/// </summary>
		/// <param name="user">Instance of <see cref="KNetFramework.Constants.Entities.Session.Client"/> type invoking command.</param>
		/// <param name="command">Command name.</param>
		/// <returns></returns>
		public bool InvokeCommand(Client user, string command)
		{
			bool retVal = default(bool);
			Command result = null;

			string com = Regex.Replace(command, @"\s+", " ").Trim();
			IList<string> commandPath = com.Split(' ').ToList();

			if (commandPath != null && commandPath.Any())
			{
				result = GetCommand(user, commandPath);

				if (result != null)
				{
					if (result.Script != null)
					{
						try
						{
							BeforeCommandInvoke?.Invoke(result, new EventArgs());
							retVal = result.Invoke(user, commandPath.ToArray());
						}
						catch (IndexOutOfRangeException e)
						{
							Manager.LogManager.Log(LogTypes.Error, $"Error with '{result.FullName}' command. wrong arguments", e);
						}
						catch (Exception e)
						{
							Manager.LogManager.Log(LogTypes.Error, $"Error with '{result.FullName}' command. Failed to execute handler", e);
						}

						if (retVal)
						{
							CommandLogModel commandLog = new CommandLogModel()
							{
								UserID = user.Token.ID,
								UserName = user.Token.Name,
								CommandName = String.Format($"{result.FullName} {String.Join(" ", commandPath)}"),
								CommandID = result.Model.ID
							};

							Manager.DatabaseManager.AddOrUpdate<KNetContext, CommandLogModel>(commandLog);
						}
					}
					else
					{
						Manager.LogManager.Log(LogTypes.Command, $"Available sub commands for '{result.FullName}'\n{AvailableSubCommands(result, user.UserLevel)}");
					}
				}
				else
				{
					Manager.LogManager.Log(LogTypes.Command, $"Command '{commandPath[0]}' doesn't exist");
				}
			}

			return retVal;
		}

		#endregion

		#region GetCommand

		/// <summary>
		/// Invokes Command script
		/// </summary>
		/// <param name="user">Instance of <see cref="KNetFramework.Constants.Entities.Session.Client"/> type invoking command.</param>
		/// <param name="commandTable">Array of available commands.</param>
		/// <param name="path">Command name as string list.</param>
		/// <param name="command">Clean command.</param>
		/// <returns>True if executed.</returns>
		public Command GetCommand(Client user, IList<string> path, Command baseCommand = null, IEnumerable<Command> commandTable = null)
		{
			Command retVal = null;

			if (commandTable == null)
				commandTable = CommandTable;

			if (path != null && path.Any())
			{
				Command command = commandTable
					.Where(x => user.UserLevel >= x.CommandLevel && x.IsValid)
					.FirstOrDefault(x => x.Name.StartsWith(path.First().Trim()));

				if (command != null)
				{
					path.RemoveAt(0);

					if (command.Script != null)
					{
						retVal = command;
					}
					else if (command.SubCommands != null)
					{
						retVal = path.Count > 0 ? GetCommand(user, path, command, command.SubCommands) : command;
					}
				}
				else
				{
					retVal = baseCommand;
				}
			}

			return retVal;
		}

		#endregion

		#region UpdateBase

		private void UpdateBase(Command command)
		{
			command.Validation = ValidateCommand(command);

			if (!command.IsValid)
				Manager.LogManager.Log(LogTypes.Warning, $"Command '{command.FullName}' failed '{command.Validation}' validation");

			if (command.SubCommands != null)
			{
				foreach (Command c in command.SubCommands)
				{
					c.BaseCommand = command;
					UpdateBase(c);
				}
			}
		}

		#endregion

		#region ValidateCommand

		public CommandValidation ValidateCommand(Command command)
		{
			CommandValidation retVal = CommandValidation.Successful;

			if (command.BaseCommand != null && command.CommandLevel < command.BaseCommand.CommandLevel)
			{
				retVal = CommandValidation.WrongLevel;
			}
			else if (command.SubCommands == null && command.Script == null)
			{
				retVal = CommandValidation.NoSubCommandsAndScript;
			}
			else if (command.SubCommands != null && command.Script != null)
			{
				retVal = CommandValidation.HasSubCommandsAndScrpit;
			}

			return retVal;
		}

		#endregion

		#region AvailableSubCommands

		/// <summary>
		/// Gets available sub commands
		/// </summary>
		/// <param name="command">Instance of <see cref="KNetFramework.Commands.Base.Command"/> type.</param>
		/// <param name="userLevel">User level.</param>
		/// <returns>String formated available commands based on user level.</returns>
		public string AvailableSubCommands(Command command, CommandLevel userLevel = CommandLevel.Zero)
		{
			string retVal = String.Empty;

			retVal = String.Join("\n", GetSubCommands(command, userLevel)
					.Select(x => x.SubCommands != null ? String.Format($"{x.Name}..", x.Name) : x.Name));

			return retVal;
		}

		#endregion

		#region GetSubCommands

		public IEnumerable<Command> GetSubCommands(Command command, CommandLevel userLevel = CommandLevel.Zero)
		{
			IEnumerable<Command> retVal = null;

			if (command.SubCommands != null && command.SubCommands.Any())
				retVal = command.SubCommands.Where(x => x.CommandLevel == userLevel && x.IsValid);

			return retVal;
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Disposes object.
		/// </summary>
		public void Dispose()
		{
			_commandTable.Dispose();
		}

		#endregion

		#endregion
	}
}
