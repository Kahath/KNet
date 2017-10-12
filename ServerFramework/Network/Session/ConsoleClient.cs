/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

namespace ServerFramework.Network.Session
{
	internal sealed class ConsoleClient : IClient
	{
		#region Fields

		private int _sessionId;
		private int _id;
		private string _name;

		#endregion

		#region Properties

		public int SessionID
		{
			get { return _sessionId; }
			set { _sessionId = value; }
		}

		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		#endregion
	}
}
