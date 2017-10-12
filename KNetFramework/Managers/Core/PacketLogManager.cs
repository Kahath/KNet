/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Managers.Base;
using KNetFramework.Managers.Interface;
using KNetFramework.Network.Packets;

namespace KNetFramework.Managers.Core
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
		/// <param name="packet">Instance of <see cref="KNetFramework.Network.Packets.Packet"/> type.</param>
		public void Log(Packet packet)
		{
			Instance.Log(packet);
		}

		#endregion

		#endregion
	}
}
