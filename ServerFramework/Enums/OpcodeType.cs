/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;

namespace ServerFramework.Enums
{
	[Flags]
	public enum OpcodeType : byte
	{
		None			= 0x00,
		NotUsed			= 0x01,
		InDevelopment	= 0x02,
		Test			= 0x04,
		Stable			= 0x08,
		Release			= 0x10,
	};
}
