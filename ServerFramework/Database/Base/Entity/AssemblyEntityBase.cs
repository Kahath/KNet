/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;

namespace ServerFramework.Database.Base.Entity
{
	public abstract class AssemblyEntityBase : IEntity
	{
		#region Properties

		public bool Active					{ get; set; }
		public DateTime DateCreated			{ get; set; }
		public DateTime? DateModified		{ get; set; }
		public DateTime? DateDeactivated	{ get; set; }
		public string AssemblyName			{ get; set; }
		public string TypeName				{ get; set; }
		public string MethodName			{ get; set; }

		#endregion

		#region Constructors

		public AssemblyEntityBase()
		{
			Active = true;
		}

		#endregion
	}
}
