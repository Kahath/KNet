/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

namespace ServerFramework.Network.Session
{
	public interface IClient
	{
		#region Properties

		int SessionID
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
