/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;

namespace ServerFramework.Enums
{
	[Flags]
	public enum PacketFlag : byte
	{
		None		= 0x00,
		Log			= 0x20,
		Unicode		= 0x40,
		BigPacket	= 0x80,
	};
}
