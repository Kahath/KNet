/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

namespace KNetFramework.Database.Base.Entity
{
	public abstract class AssemblyEntityBase<T> : EntityBase<T>
		where T : struct
	{
		#region Properties

		public string AssemblyName			{ get; set; }
		public string TypeName				{ get; set; }
		public string MethodName			{ get; set; }

		#endregion
	}
}
