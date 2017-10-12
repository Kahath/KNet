/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Network.Packets;
using System;

namespace KNetFramework.Managers.Interface
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
