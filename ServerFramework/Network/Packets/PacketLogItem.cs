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

using ServerFramework.Enums;
using ServerFramework.Network.Session;

namespace ServerFramework.Network.Packets
{
	internal class PacketLogItem
	{
		#region Fields

		private Client _client;
		private PacketHeader _packetHeader;
		private byte[] _packetMessage;
		private PacketLogType _packetLogType;

		#endregion

		#region Properties

		internal Client Client
		{
			get { return _client; }
		}

		internal PacketHeader PacketHeader
		{
			get { return _packetHeader; }
		}

		internal byte[] PacketMessage
		{
			get { return _packetMessage; }
		}

		internal PacketLogType PacketLogType
		{
			get { return _packetLogType; }
		}

		#endregion

		#region Constructors

		public PacketLogItem(Client client, PacketHeader header, byte[] message, PacketLogType logtype)
		{
			_client = client;
			_packetHeader = header;
			_packetMessage = message;
			_packetLogType = logtype;
		}

		#endregion
	}
}
