/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Database.Base.Context;
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
