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

using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.PacketLog
{
	[Table("Packet.Log", Schema = "Application")]
	public class PacketLogModel : EntityBase
	{
		#region Properties

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID							{ get; set; }
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
