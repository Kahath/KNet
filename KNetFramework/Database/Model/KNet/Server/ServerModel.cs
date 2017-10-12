/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KNetFramework.Database.Model.KNet.Server
{
	[Table("Server", Schema="KNet")]
	public class ServerModel : EntityBase<int>
	{
		#region Properties

		public bool IsSuccessful { get; set; }

		#endregion
	}
}
