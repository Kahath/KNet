/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using System;

namespace KNetFramework.Database.Base.Entity
{
	public interface IEntity<T> : IEntity
		where T : struct
	{
		#region Properties

		T ID { get; set; }

		#endregion
	}

	public interface IEntity
	{
		#region Properties

		bool Active { get; set; }
		DateTime DateCreated { get; set; }
		DateTime? DateModified { get; set; }
		DateTime? DateDeactivated { get; set; }

		#endregion
	}
}
