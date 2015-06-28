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
using System;

namespace ServerFramework.Network.Packets
{
	public class PacketHeader
	{
		#region Fields

		private int _size;
		private ushort _opcode;
		private bool _isBigHeader;

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

		public bool IsBigHeader
		{
			get { return _isBigHeader; }
			set { _isBigHeader = value; }
		}

		#endregion

		#region Constructors

		public PacketHeader(byte[] header, bool isBigHeader)
		{
			IsBigHeader = isBigHeader;

			Size = isBigHeader 
				? BitConverter.ToInt32(header, 0) & Int32.MaxValue
				: BitConverter.ToInt16(header, 0);
			Opcode = isBigHeader
				? BitConverter.ToUInt16(header, ServerConfig.BigHeaderLength - ServerConfig.OpcodeLength)
				: BitConverter.ToUInt16(header, ServerConfig.HeaderLength - ServerConfig.OpcodeLength);
		}

		public PacketHeader()
		{
			IsBigHeader = false;
		}

		#endregion
	}
}
