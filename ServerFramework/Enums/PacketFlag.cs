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
		BigPacket	= 0x40,
	};
}
