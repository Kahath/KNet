/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;

namespace ServerFramework.Enums
{
	[Flags]
	public enum PacketLogType : byte
	{
		None	= 0x00,
		CMSG	= 0x01,
		SMSG	= 0x02,
	};
}
