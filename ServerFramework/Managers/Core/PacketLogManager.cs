/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Managers.Base;
using ServerFramework.Managers.Interface;
using ServerFramework.Network.Packets;

namespace ServerFramework.Managers.Core
{
	public class PacketLogManager : ManagerBase<PacketLogManager, IPacketLogManager>
	{
		#region Properties

		internal string Path
		{
			get { return Instance.Path; }
			set { Instance.Path = value; }
		}

		#endregion

		#region Methods

		#region Log

		/// <summary>
		/// Adds packet to queue for logging.
		/// </summary>
		/// <param name="packet">Instance of <see cref="ServerFramework.Network.Packets.Packet"/> type.</param>
		public void Log(Packet packet)
		{
			Instance.Log(packet);
		}

		#endregion

		#endregion
	}
}
