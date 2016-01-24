using System;

namespace ServerFramework.Exceptions
{
	[Serializable]
	public class DatabaseException : Exception
	{
		#region Constructors

		public DatabaseException(string message)
			: base(message)
		{

		}

		#endregion
	}
}
