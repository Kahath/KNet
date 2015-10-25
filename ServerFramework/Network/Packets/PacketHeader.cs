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

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using System;

namespace ServerFramework.Network.Packets
{
	public class PacketHeader
	{
		#region Fields

		private int _length;
		private ushort _opcode;
		private byte _flags;

		#endregion

		#region Properties

		public int Length
		{
			get { return _length; }
			set { _length = value; }
		}

		public ushort Opcode
		{
			get { return _opcode; }
			set { _opcode = value; }
		}

		public byte Flags
		{
			get { return _flags; }
			set { _flags = value; }
		}

		/// <summary>
		/// Is big packet (>64kb)
		/// </summary>
		public bool IsBigHeader
		{
			get { return Convert.ToBoolean(Flags & (byte)PacketFlag.BigPacket); }
		}

		/// <summary>
		/// Is packet loged in database
		/// </summary>
		public bool IsForLog
		{
			get { return Convert.ToBoolean(Flags & (byte)PacketFlag.Log); }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="PacketHeader"/> type.
		/// </summary>
		/// <param name="header">Header byte array.</param>
		public PacketHeader(byte flags, int length, ushort opcode)
		{
			Flags = flags;
			Length = length;
			Opcode = opcode;
		}

		#endregion
	}
}
