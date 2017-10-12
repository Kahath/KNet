/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using System;

namespace KNetFramework.Exceptions
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
