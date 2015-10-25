/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

namespace ServerFramework.Network.Session
{
	public interface IClient
	{
		#region Properties

		int SessionId
		{
			get;
			set;
		}

		int ID
		{
			get;
			set;
		}

		string Name
		{
			get;
			set;
		}

		#endregion
	}
}
