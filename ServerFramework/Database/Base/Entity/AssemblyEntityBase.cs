/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;

namespace ServerFramework.Database.Base.Entity
{
	public abstract class AssemblyEntityBase : EntityBase
	{
		#region Properties

		public string AssemblyName			{ get; set; }
		public string TypeName				{ get; set; }
		public string MethodName			{ get; set; }

		#endregion
	}
}
