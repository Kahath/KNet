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

		public Server(SocketListenerSettings socketSettings)
			: base(ResolveType.Singleton, socketSettings)
		{

		}

		#endregion

		#region Events

		public event ServerEventHandler CloseClientSocket
		{
			add { Instance.CloseClientSocket += value; }
			remove { Instance.CloseClientSocket -= value; }
		}

		public event ServerEventHandler Connect
		{
			add { Instance.Connect += value; }
			remove { Instance.Connect -= value; }
		}

		#endregion

		#region Methods

		#region Init

		internal void Init()
		{
			Instance.Init();
		}

		#endregion

		#region Send

		internal void Send(SocketAsyncEventArgs e)
		{
			Instance.Send(e);
		}

		#endregion

		#endregion
	}
}
