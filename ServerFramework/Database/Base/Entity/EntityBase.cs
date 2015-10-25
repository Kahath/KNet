/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;

namespace ServerFramework.Database.Base.Entity
{
	public abstract class EntityBase : IEntity
	{
		#region Properties

		public bool Active					{ get; set; }
		public DateTime DateCreated			{ get; set; }
		public DateTime? DateModified		{ get; set; }
		public DateTime? DateDeactivated	{ get; set; }

		#endregion

		#region Constructors

		public EntityBase()
		{
			Active = true;
		}

		#endregion
	}
}
