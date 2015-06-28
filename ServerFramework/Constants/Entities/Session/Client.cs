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
using ServerFramework.Network.Packets;
using ServerFramework.Network.Socket;
using System;
using System.Net;

namespace ServerFramework.Constants.Entities.Session
{
	public sealed class Client
	{
		#region Fields

		private SocketExtended _socketExtended;
		private IClient _clientToken;
		private CommandLevel _userLevel;

		#endregion

		#region Properties

		internal SocketExtended SocketExtended
		{
			get { return _socketExtended; }
			set { _socketExtended = value; }
		}

		public IClient Token
		{
			get { return _clientToken; }
			set { _clientToken = value; }
		}

		public string IP
		{
			get { return SocketExtended.RemoteEndPoint.Address.ToString(); }
		}

		public int Port
		{
			get { return SocketExtended.RemoteEndPoint.Port; }
		}

		public int SessionID
		{
			get { return SocketExtended.ReceiverToken.SessionId; }
		}

		public bool IsConsole
		{
			get { return Token != null ? Token is ConsoleClient : false; }
		}

		public CommandLevel UserLevel
		{
			get { return _userLevel; }
			set { _userLevel = value; }
		}

		#endregion

		#region Constructors

		internal Client()
		{

		}

		internal Client(SocketExtended socketExtended)
		{
			SocketExtended = socketExtended;
		}

		#endregion

		#region Events

		public event PacketEventHandler BeforePacketSend;

		#endregion

		#region Methods

		#region Send

		public void Send(Packet packet)
		{
			if (BeforePacketSend != null)
				BeforePacketSend(packet, new EventArgs());

			SocketExtended.SendResetEvent.WaitOne();
			UserToken token = SocketExtended.SenderToken;
			token.Packet = packet;
			token.Finish();
			token.Packet.SessionId = token.SessionId;

			KahathFramework.Server.Send(SocketExtended.Sender);
		}

		#endregion

		#endregion
	}
}
