/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;

namespace ServerFramework.Enums
{
	[Flags]
	public enum LogType : byte
	{
		None		= 0x00,
		Normal		= 0x01,
		Init		= 0x02,
		Command		= 0x04,
		DB			= 0x08,
		Info		= 0x10,
		Warning		= 0x20,
		Error		= 0x40,
		Critical	= 0x80,
	};
}
