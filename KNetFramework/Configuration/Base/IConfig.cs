﻿/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

namespace KNetFramework.Configuration.Base
{
	public interface IConfig
	{
		#region Methods

		#region Read

		T Read<T>(string config, bool hex = false);

		#endregion

		#endregion
	}
}
