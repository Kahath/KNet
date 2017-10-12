/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Managers.Base;
using KNetFramework.Managers.Interface;
using KNetFramework.Network.Packets;
using System;
using System.Collections.Concurrent;

namespace KNetFramework.Managers.Core
{
	public class PacketManager : ManagerBase<PacketManager, IPacketManager>
	{
		#region Properties

		internal ConcurrentDictionary<ushort, OpcodeHandler> PacketHandlers
		{
			get { return Instance.PacketHandlers; }
			set { Instance.PacketHandlers = value; }
		}

		public int PacketHandlersCount
		{
			get { return Instance.PacketHandlersCount; }
		}

		#endregion

		#region Events

		public event EventHandler BeforePacketInvoke
		{
			add { Instance.BeforePacketInvoke += value; }
			remove { Instance.BeforePacketInvoke -= value; }
		}

		#endregion

		#region Methods

		#region InvokeHandler

		/// <summary>
		/// Invokes packet script.
		/// </summary>
		/// <param name="packet">Instance of <see cref="Packet"/> type.</param>
		internal void InvokeHandler(Packet packet)
		{
			Instance.InvokeHandler(packet);
		}

		#endregion

		#endregion
	}
}
