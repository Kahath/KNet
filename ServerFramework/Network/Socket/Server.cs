﻿/*
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

using DILibrary.Constants;
using DILibrary.DependencyInjection;
using ServerFramework.Configuration;
using System.Net.Sockets;

namespace ServerFramework.Network.Socket
{
	public class Server : Dependency<IServer>
	{
		#region Constructors

		public Server(SocketListenerSettings socketSettings = null)
			: base(ResolveTypes.Singleton, socketSettings)
		{

		}

		#endregion

		#region Events

		public event ServerEventHandler OnCloseClientSocket
		{
			add { instance.OnCloseClientSocket += value; }
			remove { instance.OnCloseClientSocket -= value; }
		}

		public event ServerEventHandler OnConnect
		{
			add { instance.OnConnect += value; }
			remove { instance.OnConnect -= value; }
		}

		#endregion

		#region Methods

		internal void Init()
		{
			instance.init();
		}

		internal void Send(SocketAsyncEventArgs e)
		{
			instance.Send(e);
		}

		#endregion

	}
}
