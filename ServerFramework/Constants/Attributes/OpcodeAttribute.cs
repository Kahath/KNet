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
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class OpcodeAttribute : Attribute
	{
		#region Fields

		private ushort _opcode;
		private string _author;
		private int _version;
		private OpcodeType _type;

		#endregion

		#region Properties

		public ushort Opcode
		{
			get { return _opcode; }
			set { _opcode = value; }
		}

		public string Author
		{
			get { return _author; }
			set { _author = value; }
		}

		public int Version
		{
			get { return _version; }
			set { _version = value; }
		}

		public OpcodeType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ServerFramework.Constants.Attributes.OpcodeAttribute"/> type.
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

		public OpcodeAttribute()
		{

		}

		#endregion
	}
}
