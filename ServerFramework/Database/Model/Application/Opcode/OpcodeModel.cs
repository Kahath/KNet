/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Attributes.Core;
using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.Opcode
{
	[Table("Opcode", Schema="Application")]
	public class OpcodeModel : AssemblyEntityBase
	{
		#region Properties

		[Key]
		public int ID					{ get; set; }
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
