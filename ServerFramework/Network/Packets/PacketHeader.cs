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

using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using System;

namespace ServerFramework.Network.Packets
{
	public class PacketHeader
	{
		#region Fields

		private int _size;
		private ushort _opcode;
		private byte _flags;

		#endregion

		#region Properties

		public int Size
		{
			get { return _size; }
			set { _size = value; }
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

		public bool IsBigHeader
		{
			get { return Convert.ToBoolean(Flags & (byte)PacketFlag.BigPacket); }
		}

		public bool IsUnicode
		{
			get { return Convert.ToBoolean(Flags & (byte)PacketFlag.Unicode); }
		}

		#endregion

		#region Constructors

		public PacketHeader(byte[] header)
		{
			Flags = header[0];

			Size = IsBigHeader
				? BitConverter.ToInt32(header, ServerConfig.PacketFlagsLength)
				: BitConverter.ToUInt16(header, ServerConfig.PacketFlagsLength);
			Opcode = IsBigHeader
				? BitConverter.ToUInt16(header
				,	ServerConfig.BigHeaderLength 
					- ServerConfig.OpcodeLength)
				: BitConverter.ToUInt16(header
				,	ServerConfig.HeaderLength 
					- ServerConfig.OpcodeLength);
		}

		public PacketHeader()
		{
		}

		#endregion
	}
}
