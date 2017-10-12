/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Configuration.Helpers;
using KNetFramework.Database.Base.Context;
using KNetFramework.Database.Model.KNet.Command;
using KNetFramework.Database.Model.KNet.Log;
using KNetFramework.Database.Model.KNet.Opcode;
using KNetFramework.Database.Model.KNet.PacketLog;
using KNetFramework.Database.Model.KNet.Server;
using System.Data.Entity;

namespace KNetFramework.Database.Context
{
	public class KNetContext : DBContextBase
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
		/// Creates new instance of <see cref="KNetContext"/> type.
		/// </summary>
		public KNetContext()
			: base(KNetConfig.ConnectionString)
		{
		}

		#endregion
	}
}
