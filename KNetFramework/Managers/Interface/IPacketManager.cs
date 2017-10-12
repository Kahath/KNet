/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Network.Packets;
using System;
using System.Collections.Concurrent;

namespace KNetFramework.Managers.Interface
{
	public interface IPacketManager : IManager
	{
		#region Properties

		ConcurrentDictionary<ushort, OpcodeHandler> PacketHandlers { get; set; }
		int PacketHandlersCount { get; }

		#endregion

		#region Events

		event EventHandler BeforePacketInvoke;

		#endregion

		#region Methods

		#region InvokeHandler

		void InvokeHandler(Packet packet);

		#endregion

		#endregion
	}
}
