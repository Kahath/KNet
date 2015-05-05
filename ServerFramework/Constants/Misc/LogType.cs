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
    public enum LogType : byte
    {
        None        = 0x00,
        Normal      = 0x01,
        Init        = 0x02,
        Command     = 0x04,
        DB          = 0x08,
        Info        = 0x10,
        Warning     = 0x20,
        Error       = 0x40,
        Critical    = 0x80,
    };
}
