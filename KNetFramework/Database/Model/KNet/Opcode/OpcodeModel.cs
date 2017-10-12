/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Attributes.Core;
using KNetFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KNetFramework.Database.Model.KNet.Opcode
{
	[Table("Opcode", Schema="KNet")]
	public class OpcodeModel : AssemblyEntityBase<int>
	{
		#region Properties

		public int Code					{ get; set; }
		public int TypeID				{ get; set; }
		public int Version				{ get; set; }
		public string Author			{ get; set; }

		[ForeignKey("TypeID")]
		public OpcodeTypeModel Type		{ get; set; }

		#endregion

		#region Constructors

		public OpcodeModel()
		{

		}

		public OpcodeModel(OpcodeAttribute opcode)
		{
			Code = opcode.Opcode;
			TypeID = (int)opcode.Type;
			Version = opcode.Version;
			Author = opcode.Author;
		}

		#endregion
	}
}
