/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Network.Packets;
using System;

namespace ServerFramework.Managers.Interface
{
	public interface IPacketLogManager : IManager, IDisposable
	{
		#region Properties

		string Path { get; set; }

		#endregion

		#region Methods


		#region Log

		void Log(Packet packet);

		#endregion

		#endregion
	}
}
