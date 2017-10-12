/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using System;

namespace ServerFramework.Enums
{
	[Flags]
	public enum OpcodeTypes : byte
	{
		None			= 0x00,
		NotUsed			= 0x01,
		InDevelopment	= 0x02,
		Test			= 0x04,
		Stable			= 0x08,
		Release			= 0x10,
	};
}
