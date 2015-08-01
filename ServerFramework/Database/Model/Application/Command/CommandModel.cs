﻿/*
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

using CCommand = ServerFramework.Constants.Entities.Console.Command;
using ServerFramework.Database.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.Command
{
	[Table("Command", Schema = "Application")]
	public class CommandModel : EntityBase
	{
		#region Properties

		[Key]
		public int ID							{ get; set; }
		public string AssemblyName				{ get; set; }
		public string TypeName					{ get; set; }
		public string MethodName				{ get; set; }

		[StringLength(50)]
		public string Name						{ get; set; }
		public string Description				{ get; set; }
		public int? CommandLevelID				{ get; set; }

		[ForeignKey("CommandLevelID")]
		public CommandLevelModel CommandLevel	{ get; set; }

		#endregion

		#region Constructors

		public CommandModel()
		{

		}

		public CommandModel(CCommand command)
		{
			Name = command.Name;
			Description = command.Description;
			CommandLevelID = (int)command.CommandLevel;
		}

		#endregion
	}
}
