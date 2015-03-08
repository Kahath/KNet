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

using ServerFramework.Network.Packets;
using System;
using System.Collections.Generic;

namespace ServerFramework.Managers.Base
{
	public abstract class PacketManagerBase<T> : ManagerBase<T> where T : class
	{
		#region Fields

		private Dictionary<ushort, PacketHandler> _packetHandlers
			= new Dictionary<ushort, PacketHandler>();

		#endregion

		#region Properties

		internal Dictionary<ushort, PacketHandler> PacketHandlers
		{
			get { return _packetHandlers; }
			set { _packetHandlers = value; }
		}

		public int PacketHandlersCount
		{
			get { return _packetHandlers.Count; }
		}

		#endregion

		#region Events

		public event PacketManagerInvokeEventHandler BeforePacketInvoke;

		#endregion

		#region Methods

		internal abstract void InvokeHandler(Packet packet);

		public void BeforePacketInvokeEvent(Packet packet)
		{
			if (BeforePacketInvoke != null)
				BeforePacketInvoke(packet, new EventArgs());
		}

		#endregion
	}
}
