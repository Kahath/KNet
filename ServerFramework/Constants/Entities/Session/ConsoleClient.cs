using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework.Constants.Entities.Session
{
	internal sealed class ConsoleClient : IClient
	{
		#region Fields

		private int _sessionId;
		private int _id;
		private string _name;

		#endregion

		public int SessionId
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
	}
}
