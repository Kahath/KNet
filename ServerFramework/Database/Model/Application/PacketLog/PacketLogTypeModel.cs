/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.PacketLog
{
	[Table("Packet.Log.Type", Schema = "Application")]
	public class PacketLogTypeModel : EntityBase<int>
	{
		#region Properties

		public string Name	{ get; set; }

		#endregion
	}
}
