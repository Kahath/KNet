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

using ServerFramework.Constants.Entities.Console;
using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.Command;
using ServerFramework.Managers.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
		internal override void Init()
		{
			using(ApplicationContext context = new ApplicationContext())
			{
				IEnumerable<CommandModel> commands = context.Commands.Where(x => x.Active);

				foreach(CommandModel command in commands)
				{
					MethodInfo method = Manager.AssemblyMgr.GetMethod(command.AssemblyName, command.TypeName, command.MethodName);

					if(method != null)
					{
						Command c = Manager.AssemblyMgr.InvokeStaticMethod<Command>(method);

						if (c != null)
							CommandTable.Add(c);
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
			StringBuilder sb = new StringBuilder();
			bool retVal = default(bool);

			sb.Append("Command: ");
			string com = Regex.Replace(command, @"\s+", " ").Trim();
			List<string> commandPath = com.Split(' ').ToList();

			if (!String.IsNullOrEmpty(com))
			{
				retVal = InvokeCommandHandler
					(
						user
					,	CommandTable.ToArray()
					,	commandPath
					,	sb
					);

				if(retVal)
				{
					if(commandPath.Any())
						sb.AppendFormat("\nParameters: {0}", String.Join(", ", commandPath));

					CommandLogModel commandLog = new CommandLogModel();
					commandLog.UserID = user.Token.ID;
					commandLog.UserName = user.Token.Name;
					commandLog.Command = sb.ToString();

					using(ApplicationContext context = new ApplicationContext())
					{
						context.CommandLogs.Add(commandLog);
						context.SaveChanges();
					}
				}
			}

			return retVal;
		}

		#endregion

		#region InvokeCommandHandler

		/// <summary>
		/// Invokes Command script
		/// </summary>
		/// <param name="user">Instance of <see cref="ServerFramework.Constants.Entities.Session.Client"/> type invoking command.</param>
		/// <param name="commandTable">Array of available commands.</param>
		/// <param name="path">Command name as string list.</param>
		/// <param name="command">Clean command.</param>
		/// <returns>True if executed.</returns>
		private bool InvokeCommandHandler(Client user
			, Command[] commandTable, IList<string> path, StringBuilder command)
		{
			if (commandTable == null || path == null)
				return false;

			Command c = commandTable
				.Where(x => user.UserLevel >= x.CommandLevel)
				.FirstOrDefault(x => x.Name.StartsWith(path[0].Trim()));

			if (c != null)
			{
				command.AppendFormat("{0} ", c.Name);

				if (c.Script == null)
				{
					if (c.SubCommands != null)
					{
						path.RemoveAt(0);
						if (path.Count > 0)
						{
							return InvokeCommandHandler(user, c.SubCommands, path, command);
						}
						else
						{
							Manager.LogMgr.Log
								(
									LogType.Command
								,	"Error with '{0}' command. Available sub commands:\n{1}"
								,	command
								,	c.AvailableSubCommands(user.UserLevel)
								);

							return false;
						}
					}
					else
					{
						Manager.LogMgr.Log(LogType.Command, "Error with '{0}' command."
							+ " Missing script or subcommands", command);

						return false;
					}
				}
				else
				{
					path.RemoveAt(0);

					try
					{
						return c.Invoke(user, path.ToArray());
					}
					catch (IndexOutOfRangeException)
					{
						Manager.LogMgr.Log(LogType.Error, "Error with '{0}' command. wrong arguments"
							, command);
						return false;
					}
					catch (Exception)
					{
						Manager.LogMgr.Log(LogType.Error, "Error with '{0}' command. Failed to execute handler"
							, command);
						return false;
					}
				}
			}

			command.Append(path[0]);
			Manager.LogMgr.Log(LogType.Command, "Command '{0}' not found", command);
			return false;
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
