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

namespace ServerFramework.Commands.Handlers
{
	[Command("assembly", CommandLevel.Ten, "")]
	public class AssemblyCommands : CommandHandlerBase
	{
		#region Methods

		#region GetCommand

		protected override Command GetCommand()
		{
			Command retVal = null;

			Command[] AssemblyCommands =
			{
				new Command("load", CommandLevel.Ten, null, AssemblyLoadHandler, ""),
			};

			retVal = new Command(Name, Level, AssemblyCommands, null, Description);
				
			return retVal;
		}

		#endregion

		#region Handlers

		#region AssemblyLoadHandler

		private static bool AssemblyLoadHandler(Client client, params string[] args)
		{
			string path = args[0];

			Manager.AssemblyMgr.Load(path);

			return true;
		}

		#endregion

		#endregion

		#endregion
	}
}
