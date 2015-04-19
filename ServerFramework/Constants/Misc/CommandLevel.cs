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

using System;

namespace ServerFramework.Constants.Misc
{
    [Flags]
    public enum CommandLevel : short
    {
        None                    = 0x0000,
        CommandLevelOne         = 0x0001,
        CommandLevelTwo         = 0x0002,
        CommandLevelThree       = 0x0004,
        CommandLevelFour        = 0x0008,
        CommandLevelFive        = 0x0010,
        CommandLevelSix         = 0x0020,
        CommandLevelSeven       = 0x0040,
        CommandLevelEight       = 0x0080,
        CommandLevelNine        = 0x0100,
        CommandLevelTen         = 0x0200,
    };
}
