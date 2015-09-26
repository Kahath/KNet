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

using ServerFramework.Commands.Base;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.Command;
using ServerFramework.Database.Repository;
using ServerFramework.Enums;
using ServerFramework.Extensions;
using ServerFramework.Managers.Base;
using ServerFramework.Network.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ServerFramework.Managers.Core
{
	public sealed class CommandManager : ManagerBase<CommandManager>, IDisposable
	{
		#region Fields

		private BlockingCollection<Command> _commandTable;

		#endregion

		#region Properties

		internal BlockingCollection<Command> CommandTable
		{
			get { return _commandTable; }
			set { _commandTable = value; }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Creates instance of <see cref="ServerFramework.Managers.Core.CommandManager"/> type.
		/// </summary>
		CommandManager()
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
		protected override void Init()
		{
			using(CommandRepository cRepo = new CommandRepository(new ApplicationContext()))
			{
				IEnumerable<CommandModel> commands = cRepo.Context.Commands.Where(x => x.Active && x.ParentID == null).ToList();

				foreach(CommandModel command in commands)
				{
					Command c = null;
					Type type = Manager.AssemblyMgr.GetType(command.AssemblyName, command.TypeName);
					MethodInfo method = type.GetMethodByName(command.MethodName);

					if(method != null)
					{
						object obj = Manager.AssemblyMgr.InvokeConstructor(type);

						if(obj != null && command != null)
							c = Manager.AssemblyMgr.InvokeMethod<Command>(obj, method);

						if (c != null)
						{
							UpdateBase(c);

							cRepo.UpdateCommandInfo(c, command);

							CommandTable.Add(c);
						}
					}
				}
			}

			Manager.LogMgr.Log(LogType.Normal, "{0} Commands loaded", CommandTable.Count);
		}

		#endregion

		#region InvokeCommand

		/// <summary>
		/// Invokes command
		/// </summary>
		/// <param name="user">Instance of <see cref="ServerFramework.Constants.Entities.Session.Client"/> type invoking command.</param>
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
				result = GetCommand
					(
						user
					,	commandPath
					);

				if (result != null)
				{
					if (result.Script != null)
					{
						try
						{
							retVal = result.Invoke(user);
						}
						catch (IndexOutOfRangeException)
						{
							Manager.LogMgr.Log(LogType.Error, "Error with '{0}' command. wrong arguments"
								, result.FullName);
						}
						catch (Exception)
						{
							Manager.LogMgr.Log(LogType.Error, "Error with '{0}' command. Failed to execute handler"
								, result.FullName);
						}

						if (retVal)
						{
							CommandLogModel commandLog = new CommandLogModel();
							commandLog.UserID = user.Token.ID;
							commandLog.UserName = user.Token.Name;
							commandLog.CommandName = result.FullName + " " + result.Arguments;
							commandLog.CommandID = result.Model.ID;

							using (ApplicationContext context = new ApplicationContext())
							{
								context.CommandLogs.Add(commandLog);
								context.SaveChanges();
							}
						}
					}
					else
					{
						Manager.LogMgr.Log
						(
							LogType.Command
						,	"Available sub commands for '{0}'\n{1}"
						,	result.FullName
						,	AvailableSubCommands(result, user.UserLevel)
						);
					}
				}
				else
				{
					Manager.LogMgr.Log
					(
						LogType.Command
					,	"Command '{0}' doesn't exist"
					,	commandPath[0]
					);
				}
			}

			return retVal;
		}

		#endregion

		#region GetCommand

		/// <summary>
		/// Invokes Command script
		/// </summary>
		/// <param name="user">Instance of <see cref="ServerFramework.Constants.Entities.Session.Client"/> type invoking command.</param>
		/// <param name="commandTable">Array of available commands.</param>
		/// <param name="path">Command name as string list.</param>
		/// <param name="command">Clean command.</param>
		/// <returns>True if executed.</returns>
		internal Command GetCommand(Client user, IList<string> path
			, Command baseCommand = null, IEnumerable<Command> commandTable = null)
		{
			Command retVal = null;

			if(commandTable == null)
				commandTable = CommandTable;

			if (path != null && path.Any())
			{
				Command command = commandTable
					.Where
					(x =>
						user.UserLevel >= x.CommandLevel
						&& x.IsValid
					)
					.FirstOrDefault(x => x.Name.StartsWith(path.First().Trim()));

				if (command != null)
				{
					path.RemoveAt(0);

					if (command.Script != null)
					{
						command.Arguments = String.Join(" ", path);
						retVal = command;
					}
					else if (command.SubCommands != null)
					{
						if (path.Count > 0)
							retVal = GetCommand(user, path, command, command.SubCommands);
						else
							retVal = command;
					}
				}
				else
					retVal = baseCommand;
			}

			return retVal;
		}

		#endregion

		#region UpdateBase

		internal void UpdateBase(Command command)
		{
			command.Validation = ValidateCommand(command);

			if (!command.IsValid)
			{
				Manager.LogMgr.Log
				(
					LogType.Warning
				,	"Command '{0}' failed '{1}' validation"
				,	command.FullName
				,	command.Validation
				);
			}

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
				retVal = CommandValidation.WrongLevel;
			else if (command.SubCommands == null && command.Script == null)
				retVal = CommandValidation.NoSubCommandsAndScript;
			else if (command.SubCommands != null && command.Script != null)
				retVal = CommandValidation.HasSubCommandsAndScrpit;

			return retVal;
		}

		#endregion

		#region AvailableSubCommands

		/// <summary>
		/// Gets available sub commands
		/// </summary>
		/// <param name="command">Instance of <see cref="ServerFramework.Commands.Base.Command"/> type.</param>
		/// <param name="userLevel">User level.</param>
		/// <returns>String formated available commands based on user level.</returns>
		public string AvailableSubCommands(Command command, CommandLevel userLevel = CommandLevel.Zero)
		{
			string retVal = String.Empty;

			retVal = String.Join("\n", GetSubCommands(command, userLevel)
					.Select(x => x.SubCommands != null ? String.Format("{0}..", x.Name) : x.Name));

			return retVal;
		}

		#region GetSubCommands

		internal IEnumerable<Command> GetSubCommands(Command command, CommandLevel userLevel = CommandLevel.Zero)
		{
			IEnumerable<Command> retVal = null;

			if (command.SubCommands != null && command.SubCommands.Any())
				retVal = command.SubCommands.Where(x => x.CommandLevel == userLevel && x.IsValid);

			return retVal;
		}

		#endregion

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
