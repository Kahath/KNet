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

using ServerFramework.Constants.Attributes;
using ServerFramework.Constants.Entities.Console;
using ServerFramework.Constants.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework.Game.CommandHandlers
{
    [Command]
    public class ClearCommands
    {
        #region Methods

        #region GetCommand

        public static Command GetCommand()
        {
            return new Command("cls", (CommandLevel)0xFF, null, Cls, "");
        }

        #endregion

        #region Handlers

        #region ClsHandler

        private static bool Cls(params string[] args)
        {
            Console.Clear();
            return true;
        }

        #endregion

        #endregion

        #endregion
    }
}
