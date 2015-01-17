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
    public enum OpcodeType : byte
    {
        None                    = 0x00,
        Test                    = 0x01,
        Broken                  = 0x02,
        Finished                = 0x04,
        NotUsed                 = 0x08,
        InDevelopment           = 0x10,
    };
}
