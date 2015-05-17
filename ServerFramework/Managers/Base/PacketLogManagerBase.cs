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

using ServerFramework.Database.Model.Application.PacketLog;
using ServerFramework.Network.Packets;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ServerFramework.Managers.Base
{
	public abstract class PacketLogManagerBase<T> : ManagerBase<T> where T : class
	{
		#region Fields

		private string _path;
		private List<PacketLogModel> _packetLog;
		private BlockingCollection<Packet> _packetLogQueue
			= new BlockingCollection<Packet>();

		#endregion

		#region Properties

		internal string Path
		{
			get { return _path; }
			set { _path = value; }
		}

		protected BlockingCollection<Packet> PacketLogQueue
		{
			get { return _packetLogQueue; }
			set { _packetLogQueue = value; }
		}

		protected List<PacketLogModel> PacketLog
		{
            get
            {
                if (_packetLog == null)
                    _packetLog = new List<PacketLogModel>();

                return _packetLog;
            }
		}

		#endregion

		#region Methods

		protected abstract void LogPacket(Packet packet);
		internal abstract void Log(Packet packet);

		#endregion
	}
}
