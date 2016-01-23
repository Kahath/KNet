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
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ServerFramework.Database.Context
{
	public class ApplicationContext : DBContextBase
	{
		#region Fields

		private Dictionary<Type, DbSet> _dbSetMap;

		#endregion

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

		public override Dictionary<Type, DbSet> DbSetMap
		{
			get
			{
				if(_dbSetMap == null)
				{
					_dbSetMap = new Dictionary<Type, DbSet>()
					{
						{ typeof(CommandModel), Set(typeof(CommandModel)) }
					,   { typeof(CommandLevelModel), Set(typeof(CommandLevelModel)) }
					,   { typeof(CommandLogModel), Set(typeof(CommandLogModel)) }
					,   { typeof(PacketLogModel), Set(typeof(PacketLogModel)) }
					,   { typeof(PacketLogTypeModel), Set(typeof(PacketLogTypeModel)) }
					,   { typeof(LogModel), Set(typeof(LogModel)) }
					,   { typeof(OpcodeModel), Set(typeof(OpcodeModel)) }
					,   { typeof(OpcodeTypeModel), Set(typeof(OpcodeTypeModel)) }
					,   { typeof(ServerModel), Set(typeof(ServerModel)) }
					};
				}

				return _dbSetMap;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ServerFramework.Database.Context.ApplicationContext"/> type.
		/// </summary>
		public ApplicationContext()
			: base(ServerConfig.ConnectionString)
		{
		}

		#endregion
	}
}
