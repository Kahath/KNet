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

using ServerFramework.Configuration.Helpers;
using ServerFramework.Database.Base;
using ServerFramework.Database.Model.Application.Command;
using ServerFramework.Database.Model.Application.Log;
using ServerFramework.Database.Model.Application.Opcode;
using ServerFramework.Database.Model.Application.PacketLog;
using ServerFramework.Database.Model.Application.Server;
using System.Data.Entity;

namespace ServerFramework.Database.Context
{
	public class ApplicationContext : DBContextBase
	{
		#region Properties

		public DbSet<CommandModel> Commands				{ get; set; }
		public DbSet<CommandLevelModel> CommandLevels	{ get; set; }
		public DbSet<CommandLogModel> CommandLogs		{ get; set; }
		public DbSet<PacketLogModel> PacketLogs			{ get; set; }
		public DbSet<PacketLogTypeModel> PacketLogTypes	{ get; set; }
		public DbSet<LogModel> Logs						{ get; set; }
		public DbSet<LogTypeModel> LogTypes				{ get; set; }
		public DbSet<OpcodeModel> Opcodes				{ get; set; }
		public DbSet<OpcodeTypeModel> OpcodeTypes		{ get; set; }
		public DbSet<ServerModel> Servers				{ get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ServerFramework.Database.Context.ApplicationContex"/> type.
		/// </summary>
		public ApplicationContext()
			: base(ServerConfig.ConnectionString)
		{

		}

		#endregion
	}
}
