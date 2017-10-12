/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KNetFramework.Database.Model.KNet.Log
{
	[Table("Log.Type", Schema = "KNet")]
	public class LogTypeModel : EntityBase<int>
	{
		#region Properties

		public string Name	{ get; set; }

		#endregion
	}
}
