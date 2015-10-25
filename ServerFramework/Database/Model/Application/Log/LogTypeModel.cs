/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.Log
{
	[Table("Log.Type", Schema = "Application")]
	public class LogTypeModel : EntityBase
	{
		#region Properties

		[Key]
		public int ID		{ get; set; }
		public string Name	{ get; set; }

		#endregion
	}
}
