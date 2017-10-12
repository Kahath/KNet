/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.Server
{
	[Table("Server", Schema="Application")]
	public class ServerModel : EntityBase<int>
	{
		#region Properties

		public bool IsSuccessful { get; set; }

		#endregion
	}
}
