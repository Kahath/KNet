/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using System;

namespace ServerFramework.Enums
{
	[Flags]
	public enum PacketFlags : byte
	{
		None		= 0x00,
		Log			= 0x20,
		BigPacket	= 0x40,
	};
}
