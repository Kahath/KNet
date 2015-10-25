/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

namespace ServerFramework.Configuration.Base
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
