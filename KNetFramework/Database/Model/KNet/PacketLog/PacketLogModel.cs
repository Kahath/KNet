/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KNetFramework.Database.Model.KNet.PacketLog
{
	[Table("Packet.Log", Schema = "KNet")]
	public class PacketLogModel : EntityBase<int>
	{
		#region Properties

		public string IP						{ get; set; }
		public int? ClientID					{ get; set; }
		public int? Size						{ get; set; }
		public int PacketLogTypeID				{ get; set; }
		public int? Opcode						{ get; set; }
		public string Message					{ get; set; }

		[ForeignKey("PacketLogTypeID")]
		public PacketLogTypeModel PacketLogType { get; set; }

		#endregion
	}
}
