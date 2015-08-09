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

using ServerFramework.Attributes.Core;
using ServerFramework.Commands.Base;
using ServerFramework.Enums;
using ServerFramework.Managers;
using ServerFramework.Network.Session;
using System;
using System.Linq;
using System.Text;

namespace ServerFramework.Commands.Handlers
{
	[Command]
	internal static class CommandCommands
	{
		#region Methods

		#region GetCommand

		private static Command GetCommand()
		{
			Command[] CommandCommandTable = 
			{
				new Command("list", CommandLevel.Nine, null, CommandListHandler, "")
			};

			return new Command("command", CommandLevel.Nine,
				CommandCommandTable, null, "");
		}

		#endregion

		#endregion

		#region Handlers

		#region CommandListHandler

		private static bool CommandListHandler(Client user, params string[] args)
		{
			StringBuilder sb = new StringBuilder();
			Manager.LogMgr.Log(LogType.Command, "List of all commands:");

			sb.AppendLine(String.Join("\n", Manager.CommandMgr.CommandTable
				.Where(x => user.UserLevel >= x.CommandLevel)
				.Select(x => x.SubCommands != null ? String.Format("{0}..", x.Name) : x.Name)));

			Manager.LogMgr.Log(LogType.Command, "{0}", sb.ToString());
			return true;
		}

		#endregion

		#endregion
	}
}