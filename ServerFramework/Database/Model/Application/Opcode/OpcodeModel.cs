/*
 * This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
