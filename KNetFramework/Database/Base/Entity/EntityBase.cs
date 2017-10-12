/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KNetFramework.Database.Base.Entity
{
	public abstract class EntityBase<T> : IEntity<T>
		where T : struct
	{
		#region Properties
		
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public T ID { get; set; }
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
