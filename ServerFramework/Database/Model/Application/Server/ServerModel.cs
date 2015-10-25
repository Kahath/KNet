/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.Server
{
	[Table("Server", Schema="Application")]
	public class ServerModel : EntityBase
	{
		#region Properties

		[Key]
		public int ID { get; set; }
		public bool IsSuccessful { get; set; }

		#endregion
	}
}
