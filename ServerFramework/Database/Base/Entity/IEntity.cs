/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;

namespace ServerFramework.Database.Base.Entity
{
	public interface IEntity
	{
		#region Properties

		bool Active					{ get; set; }
		DateTime DateCreated		{ get; set; }
		DateTime? DateModified		{ get; set; }
		DateTime? DateDeactivated	{ get; set; }

		#endregion
	}
}
