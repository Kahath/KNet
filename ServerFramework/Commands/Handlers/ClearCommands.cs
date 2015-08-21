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
using ServerFramework.Network.Session;
using System;

namespace ServerFramework.Commands.Handlers
{
	[Command("cls", CommandLevel.Ten, "")]
	public class ClearCommands : CommandHandlerBase
	{
		#region Methods

		#region GetCommand

		protected override Command GetCommand()
		{
			Command retVal = null;

			retVal = new Command(Name, Level, null, Cls, Description);

			return retVal;
		}

		#endregion

		#endregion

		#region Handlers

		#region ClsHandler

		private static bool Cls(Client user, params string[] args)
		{
			Console.Clear();

			return true;
		}

		#endregion

		#endregion
	}
}
