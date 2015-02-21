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

using ServerFramework.Constants.Misc;
using System;

namespace ServerFramework.Constants.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
    public sealed class OpcodeAttribute : Attribute
    {
        public ushort Opcode;
        public string Author;
        public int Version;
        public OpcodeType Type;

        /// <summary>
        /// Attribute used for reading opcodes.
        /// </summary>
        /// <param name="opcode">Client packet opcode</param>
        /// <param name="author">Author of method</param>
        /// <param name="version">Version of method</param>
        /// <param name="type">Opcode type</param>
        public OpcodeAttribute(ushort opcode, string author, int version, OpcodeType type)
        {
            this.Opcode = opcode;
            this.Author = author;
            this.Version = version;
            this.Type = type;
        }
    }
}
