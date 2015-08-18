//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Data.Entity.Infrastructure.MappingViews;

[assembly: DbMappingViewCacheTypeAttribute(
	typeof(ServerFramework.Database.Context.ApplicationContext),
	typeof(ServerFramework.Database.PrecompiledViews.ViewsForBaseEntitySets5c1201b70f268ecbd9bb141aa15d90cf9f5b6999f735f7ac28caf3c6a61e8663))]

namespace ServerFramework.Database.PrecompiledViews
{
	using System;
	using System.CodeDom.Compiler;
	using System.Data.Entity.Core.Metadata.Edm;

	/// <summary>
	/// Implements a mapping view cache.
	/// </summary>
	[GeneratedCode("Entity Framework Power Tools", "0.9.0.0")]
	internal sealed class ViewsForBaseEntitySets5c1201b70f268ecbd9bb141aa15d90cf9f5b6999f735f7ac28caf3c6a61e8663 : DbMappingViewCache
	{
		/// <summary>
		/// Gets a hash value computed over the mapping closure.
		/// </summary>
		public override string MappingHashValue
		{
			get { return "5c1201b70f268ecbd9bb141aa15d90cf9f5b6999f735f7ac28caf3c6a61e8663"; }
		}

		/// <summary>
		/// Gets a view corresponding to the specified extent.
		/// </summary>
		/// <param name="extent">The extent.</param>
		/// <returns>The mapping view, or null if the extent is not associated with a mapping view.</returns>
		public override DbMappingView GetView(EntitySetBase extent)
		{
			if (extent == null)
			{
				throw new ArgumentNullException("extent");
			}

			var extentName = extent.EntityContainer.Name + "." + extent.Name;

			if (extentName == "CodeFirstDatabase.CommandLevelModel")
			{
				return GetView0();
			}

			if (extentName == "CodeFirstDatabase.CommandModel")
			{
				return GetView1();
			}

			if (extentName == "ApplicationContext.CommandLevels")
			{
				return GetView2();
			}

			if (extentName == "ApplicationContext.Commands")
			{
				return GetView3();
			}

			if (extentName == "CodeFirstDatabase.CommandLogModel")
			{
				return GetView4();
			}

			if (extentName == "ApplicationContext.CommandLogs")
			{
				return GetView5();
			}

			if (extentName == "CodeFirstDatabase.LogModel")
			{
				return GetView6();
			}

			if (extentName == "CodeFirstDatabase.LogTypeModel")
			{
				return GetView7();
			}

			if (extentName == "ApplicationContext.Logs")
			{
				return GetView8();
			}

			if (extentName == "ApplicationContext.LogTypes")
			{
				return GetView9();
			}

			if (extentName == "CodeFirstDatabase.OpcodeModel")
			{
				return GetView10();
			}

			if (extentName == "CodeFirstDatabase.OpcodeTypeModel")
			{
				return GetView11();
			}

			if (extentName == "ApplicationContext.Opcodes")
			{
				return GetView12();
			}

			if (extentName == "ApplicationContext.OpcodeTypes")
			{
				return GetView13();
			}

			if (extentName == "CodeFirstDatabase.PacketLogModel")
			{
				return GetView14();
			}

			if (extentName == "CodeFirstDatabase.PacketLogTypeModel")
			{
				return GetView15();
			}

			if (extentName == "ApplicationContext.PacketLogs")
			{
				return GetView16();
			}

			if (extentName == "ApplicationContext.PacketLogTypes")
			{
				return GetView17();
			}

			if (extentName == "CodeFirstDatabase.ServerModel")
			{
				return GetView18();
			}

			if (extentName == "ApplicationContext.Servers")
			{
				return GetView19();
			}

			return null;
		}

		/// <summary>
		/// Gets the view for CodeFirstDatabase.CommandLevelModel.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView0()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing CommandLevelModel
        [CodeFirstDatabaseSchema.CommandLevelModel](T1.CommandLevelModel_ID, T1.CommandLevelModel_Name, T1.CommandLevelModel_Active, T1.CommandLevelModel_DateCreated, T1.CommandLevelModel_DateModified, T1.CommandLevelModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS CommandLevelModel_ID, 
            T.Name AS CommandLevelModel_Name, 
            T.Active AS CommandLevelModel_Active, 
            T.DateCreated AS CommandLevelModel_DateCreated, 
            T.DateModified AS CommandLevelModel_DateModified, 
            T.DateDeactivated AS CommandLevelModel_DateDeactivated, 
            True AS _from0
        FROM ApplicationContext.CommandLevels AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for CodeFirstDatabase.CommandModel.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView1()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing CommandModel
        [CodeFirstDatabaseSchema.CommandModel](T1.CommandModel_ID, T1.CommandModel_Name, T1.CommandModel_Description, T1.CommandModel_CommandLevelID, T1.CommandModel_ParentID, T1.CommandModel_Active, T1.CommandModel_DateCreated, T1.CommandModel_DateModified, T1.CommandModel_DateDeactivated, T1.CommandModel_AssemblyName, T1.CommandModel_TypeName, T1.CommandModel_MethodName)
    FROM (
        SELECT 
            T.ID AS CommandModel_ID, 
            T.Name AS CommandModel_Name, 
            T.Description AS CommandModel_Description, 
            T.CommandLevelID AS CommandModel_CommandLevelID, 
            T.ParentID AS CommandModel_ParentID, 
            T.Active AS CommandModel_Active, 
            T.DateCreated AS CommandModel_DateCreated, 
            T.DateModified AS CommandModel_DateModified, 
            T.DateDeactivated AS CommandModel_DateDeactivated, 
            T.AssemblyName AS CommandModel_AssemblyName, 
            T.TypeName AS CommandModel_TypeName, 
            T.MethodName AS CommandModel_MethodName, 
            True AS _from0
        FROM ApplicationContext.Commands AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for ApplicationContext.CommandLevels.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView2()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing CommandLevels
        [ServerFramework.Database.Context.CommandLevelModel](T1.CommandLevelModel_ID, T1.CommandLevelModel_Name, T1.CommandLevelModel_Active, T1.CommandLevelModel_DateCreated, T1.CommandLevelModel_DateModified, T1.CommandLevelModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS CommandLevelModel_ID, 
            T.Name AS CommandLevelModel_Name, 
            T.Active AS CommandLevelModel_Active, 
            T.DateCreated AS CommandLevelModel_DateCreated, 
            T.DateModified AS CommandLevelModel_DateModified, 
            T.DateDeactivated AS CommandLevelModel_DateDeactivated, 
            True AS _from0
        FROM CodeFirstDatabase.CommandLevelModel AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for ApplicationContext.Commands.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView3()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing Commands
        [ServerFramework.Database.Context.CommandModel](T1.CommandModel_ID, T1.CommandModel_Name, T1.CommandModel_Description, T1.CommandModel_CommandLevelID, T1.CommandModel_ParentID, T1.CommandModel_Active, T1.CommandModel_DateCreated, T1.CommandModel_DateModified, T1.CommandModel_DateDeactivated, T1.CommandModel_AssemblyName, T1.CommandModel_TypeName, T1.CommandModel_MethodName)
    FROM (
        SELECT 
            T.ID AS CommandModel_ID, 
            T.Name AS CommandModel_Name, 
            T.Description AS CommandModel_Description, 
            T.CommandLevelID AS CommandModel_CommandLevelID, 
            T.ParentID AS CommandModel_ParentID, 
            T.Active AS CommandModel_Active, 
            T.DateCreated AS CommandModel_DateCreated, 
            T.DateModified AS CommandModel_DateModified, 
            T.DateDeactivated AS CommandModel_DateDeactivated, 
            T.AssemblyName AS CommandModel_AssemblyName, 
            T.TypeName AS CommandModel_TypeName, 
            T.MethodName AS CommandModel_MethodName, 
            True AS _from0
        FROM CodeFirstDatabase.CommandModel AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for CodeFirstDatabase.CommandLogModel.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView4()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing CommandLogModel
        [CodeFirstDatabaseSchema.CommandLogModel](T1.CommandLogModel_ID, T1.CommandLogModel_UserID, T1.CommandLogModel_UserName, T1.CommandLogModel_Command, T1.CommandLogModel_Active, T1.CommandLogModel_DateCreated, T1.CommandLogModel_DateModified, T1.CommandLogModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS CommandLogModel_ID, 
            T.UserID AS CommandLogModel_UserID, 
            T.UserName AS CommandLogModel_UserName, 
            T.Command AS CommandLogModel_Command, 
            T.Active AS CommandLogModel_Active, 
            T.DateCreated AS CommandLogModel_DateCreated, 
            T.DateModified AS CommandLogModel_DateModified, 
            T.DateDeactivated AS CommandLogModel_DateDeactivated, 
            True AS _from0
        FROM ApplicationContext.CommandLogs AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for ApplicationContext.CommandLogs.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView5()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing CommandLogs
        [ServerFramework.Database.Context.CommandLogModel](T1.CommandLogModel_ID, T1.CommandLogModel_UserID, T1.CommandLogModel_UserName, T1.CommandLogModel_Command, T1.CommandLogModel_Active, T1.CommandLogModel_DateCreated, T1.CommandLogModel_DateModified, T1.CommandLogModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS CommandLogModel_ID, 
            T.UserID AS CommandLogModel_UserID, 
            T.UserName AS CommandLogModel_UserName, 
            T.Command AS CommandLogModel_Command, 
            T.Active AS CommandLogModel_Active, 
            T.DateCreated AS CommandLogModel_DateCreated, 
            T.DateModified AS CommandLogModel_DateModified, 
            T.DateDeactivated AS CommandLogModel_DateDeactivated, 
            True AS _from0
        FROM CodeFirstDatabase.CommandLogModel AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for CodeFirstDatabase.LogModel.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView6()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing LogModel
        [CodeFirstDatabaseSchema.LogModel](T1.LogModel_ID, T1.LogModel_Message, T1.LogModel_LogTypeID, T1.LogModel_Active, T1.LogModel_DateCreated, T1.LogModel_DateModified, T1.LogModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS LogModel_ID, 
            T.Message AS LogModel_Message, 
            T.LogTypeID AS LogModel_LogTypeID, 
            T.Active AS LogModel_Active, 
            T.DateCreated AS LogModel_DateCreated, 
            T.DateModified AS LogModel_DateModified, 
            T.DateDeactivated AS LogModel_DateDeactivated, 
            True AS _from0
        FROM ApplicationContext.Logs AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for CodeFirstDatabase.LogTypeModel.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView7()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing LogTypeModel
        [CodeFirstDatabaseSchema.LogTypeModel](T1.LogTypeModel_ID, T1.LogTypeModel_Name, T1.LogTypeModel_Active, T1.LogTypeModel_DateCreated, T1.LogTypeModel_DateModified, T1.LogTypeModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS LogTypeModel_ID, 
            T.Name AS LogTypeModel_Name, 
            T.Active AS LogTypeModel_Active, 
            T.DateCreated AS LogTypeModel_DateCreated, 
            T.DateModified AS LogTypeModel_DateModified, 
            T.DateDeactivated AS LogTypeModel_DateDeactivated, 
            True AS _from0
        FROM ApplicationContext.LogTypes AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for ApplicationContext.Logs.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView8()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing Logs
        [ServerFramework.Database.Context.LogModel](T1.LogModel_ID, T1.LogModel_Message, T1.LogModel_LogTypeID, T1.LogModel_Active, T1.LogModel_DateCreated, T1.LogModel_DateModified, T1.LogModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS LogModel_ID, 
            T.Message AS LogModel_Message, 
            T.LogTypeID AS LogModel_LogTypeID, 
            T.Active AS LogModel_Active, 
            T.DateCreated AS LogModel_DateCreated, 
            T.DateModified AS LogModel_DateModified, 
            T.DateDeactivated AS LogModel_DateDeactivated, 
            True AS _from0
        FROM CodeFirstDatabase.LogModel AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for ApplicationContext.LogTypes.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView9()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing LogTypes
        [ServerFramework.Database.Context.LogTypeModel](T1.LogTypeModel_ID, T1.LogTypeModel_Name, T1.LogTypeModel_Active, T1.LogTypeModel_DateCreated, T1.LogTypeModel_DateModified, T1.LogTypeModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS LogTypeModel_ID, 
            T.Name AS LogTypeModel_Name, 
            T.Active AS LogTypeModel_Active, 
            T.DateCreated AS LogTypeModel_DateCreated, 
            T.DateModified AS LogTypeModel_DateModified, 
            T.DateDeactivated AS LogTypeModel_DateDeactivated, 
            True AS _from0
        FROM CodeFirstDatabase.LogTypeModel AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for CodeFirstDatabase.OpcodeModel.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView10()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing OpcodeModel
        [CodeFirstDatabaseSchema.OpcodeModel](T1.OpcodeModel_ID, T1.OpcodeModel_Code, T1.OpcodeModel_TypeID, T1.OpcodeModel_Version, T1.OpcodeModel_Author, T1.OpcodeModel_Active, T1.OpcodeModel_DateCreated, T1.OpcodeModel_DateModified, T1.OpcodeModel_DateDeactivated, T1.OpcodeModel_AssemblyName, T1.OpcodeModel_TypeName, T1.OpcodeModel_MethodName)
    FROM (
        SELECT 
            T.ID AS OpcodeModel_ID, 
            T.Code AS OpcodeModel_Code, 
            T.TypeID AS OpcodeModel_TypeID, 
            T.Version AS OpcodeModel_Version, 
            T.Author AS OpcodeModel_Author, 
            T.Active AS OpcodeModel_Active, 
            T.DateCreated AS OpcodeModel_DateCreated, 
            T.DateModified AS OpcodeModel_DateModified, 
            T.DateDeactivated AS OpcodeModel_DateDeactivated, 
            T.AssemblyName AS OpcodeModel_AssemblyName, 
            T.TypeName AS OpcodeModel_TypeName, 
            T.MethodName AS OpcodeModel_MethodName, 
            True AS _from0
        FROM ApplicationContext.Opcodes AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for CodeFirstDatabase.OpcodeTypeModel.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView11()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing OpcodeTypeModel
        [CodeFirstDatabaseSchema.OpcodeTypeModel](T1.OpcodeTypeModel_ID, T1.OpcodeTypeModel_Name, T1.OpcodeTypeModel_Active, T1.OpcodeTypeModel_DateCreated, T1.OpcodeTypeModel_DateModified, T1.OpcodeTypeModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS OpcodeTypeModel_ID, 
            T.Name AS OpcodeTypeModel_Name, 
            T.Active AS OpcodeTypeModel_Active, 
            T.DateCreated AS OpcodeTypeModel_DateCreated, 
            T.DateModified AS OpcodeTypeModel_DateModified, 
            T.DateDeactivated AS OpcodeTypeModel_DateDeactivated, 
            True AS _from0
        FROM ApplicationContext.OpcodeTypes AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for ApplicationContext.Opcodes.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView12()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing Opcodes
        [ServerFramework.Database.Context.OpcodeModel](T1.OpcodeModel_ID, T1.OpcodeModel_Code, T1.OpcodeModel_TypeID, T1.OpcodeModel_Version, T1.OpcodeModel_Author, T1.OpcodeModel_Active, T1.OpcodeModel_DateCreated, T1.OpcodeModel_DateModified, T1.OpcodeModel_DateDeactivated, T1.OpcodeModel_AssemblyName, T1.OpcodeModel_TypeName, T1.OpcodeModel_MethodName)
    FROM (
        SELECT 
            T.ID AS OpcodeModel_ID, 
            T.Code AS OpcodeModel_Code, 
            T.TypeID AS OpcodeModel_TypeID, 
            T.Version AS OpcodeModel_Version, 
            T.Author AS OpcodeModel_Author, 
            T.Active AS OpcodeModel_Active, 
            T.DateCreated AS OpcodeModel_DateCreated, 
            T.DateModified AS OpcodeModel_DateModified, 
            T.DateDeactivated AS OpcodeModel_DateDeactivated, 
            T.AssemblyName AS OpcodeModel_AssemblyName, 
            T.TypeName AS OpcodeModel_TypeName, 
            T.MethodName AS OpcodeModel_MethodName, 
            True AS _from0
        FROM CodeFirstDatabase.OpcodeModel AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for ApplicationContext.OpcodeTypes.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView13()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing OpcodeTypes
        [ServerFramework.Database.Context.OpcodeTypeModel](T1.OpcodeTypeModel_ID, T1.OpcodeTypeModel_Name, T1.OpcodeTypeModel_Active, T1.OpcodeTypeModel_DateCreated, T1.OpcodeTypeModel_DateModified, T1.OpcodeTypeModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS OpcodeTypeModel_ID, 
            T.Name AS OpcodeTypeModel_Name, 
            T.Active AS OpcodeTypeModel_Active, 
            T.DateCreated AS OpcodeTypeModel_DateCreated, 
            T.DateModified AS OpcodeTypeModel_DateModified, 
            T.DateDeactivated AS OpcodeTypeModel_DateDeactivated, 
            True AS _from0
        FROM CodeFirstDatabase.OpcodeTypeModel AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for CodeFirstDatabase.PacketLogModel.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView14()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing PacketLogModel
        [CodeFirstDatabaseSchema.PacketLogModel](T1.PacketLogModel_ID, T1.PacketLogModel_IP, T1.PacketLogModel_ClientID, T1.PacketLogModel_Size, T1.PacketLogModel_PacketLogTypeID, T1.PacketLogModel_Opcode, T1.PacketLogModel_Message, T1.PacketLogModel_Active, T1.PacketLogModel_DateCreated, T1.PacketLogModel_DateModified, T1.PacketLogModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS PacketLogModel_ID, 
            T.IP AS PacketLogModel_IP, 
            T.ClientID AS PacketLogModel_ClientID, 
            T.Size AS PacketLogModel_Size, 
            T.PacketLogTypeID AS PacketLogModel_PacketLogTypeID, 
            T.Opcode AS PacketLogModel_Opcode, 
            T.Message AS PacketLogModel_Message, 
            T.Active AS PacketLogModel_Active, 
            T.DateCreated AS PacketLogModel_DateCreated, 
            T.DateModified AS PacketLogModel_DateModified, 
            T.DateDeactivated AS PacketLogModel_DateDeactivated, 
            True AS _from0
        FROM ApplicationContext.PacketLogs AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for CodeFirstDatabase.PacketLogTypeModel.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView15()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing PacketLogTypeModel
        [CodeFirstDatabaseSchema.PacketLogTypeModel](T1.PacketLogTypeModel_ID, T1.PacketLogTypeModel_Name, T1.PacketLogTypeModel_Active, T1.PacketLogTypeModel_DateCreated, T1.PacketLogTypeModel_DateModified, T1.PacketLogTypeModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS PacketLogTypeModel_ID, 
            T.Name AS PacketLogTypeModel_Name, 
            T.Active AS PacketLogTypeModel_Active, 
            T.DateCreated AS PacketLogTypeModel_DateCreated, 
            T.DateModified AS PacketLogTypeModel_DateModified, 
            T.DateDeactivated AS PacketLogTypeModel_DateDeactivated, 
            True AS _from0
        FROM ApplicationContext.PacketLogTypes AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for ApplicationContext.PacketLogs.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView16()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing PacketLogs
        [ServerFramework.Database.Context.PacketLogModel](T1.PacketLogModel_ID, T1.PacketLogModel_IP, T1.PacketLogModel_ClientID, T1.PacketLogModel_Size, T1.PacketLogModel_PacketLogTypeID, T1.PacketLogModel_Opcode, T1.PacketLogModel_Message, T1.PacketLogModel_Active, T1.PacketLogModel_DateCreated, T1.PacketLogModel_DateModified, T1.PacketLogModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS PacketLogModel_ID, 
            T.IP AS PacketLogModel_IP, 
            T.ClientID AS PacketLogModel_ClientID, 
            T.Size AS PacketLogModel_Size, 
            T.PacketLogTypeID AS PacketLogModel_PacketLogTypeID, 
            T.Opcode AS PacketLogModel_Opcode, 
            T.Message AS PacketLogModel_Message, 
            T.Active AS PacketLogModel_Active, 
            T.DateCreated AS PacketLogModel_DateCreated, 
            T.DateModified AS PacketLogModel_DateModified, 
            T.DateDeactivated AS PacketLogModel_DateDeactivated, 
            True AS _from0
        FROM CodeFirstDatabase.PacketLogModel AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for ApplicationContext.PacketLogTypes.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView17()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing PacketLogTypes
        [ServerFramework.Database.Context.PacketLogTypeModel](T1.PacketLogTypeModel_ID, T1.PacketLogTypeModel_Name, T1.PacketLogTypeModel_Active, T1.PacketLogTypeModel_DateCreated, T1.PacketLogTypeModel_DateModified, T1.PacketLogTypeModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS PacketLogTypeModel_ID, 
            T.Name AS PacketLogTypeModel_Name, 
            T.Active AS PacketLogTypeModel_Active, 
            T.DateCreated AS PacketLogTypeModel_DateCreated, 
            T.DateModified AS PacketLogTypeModel_DateModified, 
            T.DateDeactivated AS PacketLogTypeModel_DateDeactivated, 
            True AS _from0
        FROM CodeFirstDatabase.PacketLogTypeModel AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for CodeFirstDatabase.ServerModel.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView18()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing ServerModel
        [CodeFirstDatabaseSchema.ServerModel](T1.ServerModel_ID, T1.ServerModel_IsSuccessful, T1.ServerModel_Active, T1.ServerModel_DateCreated, T1.ServerModel_DateModified, T1.ServerModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS ServerModel_ID, 
            T.IsSuccessful AS ServerModel_IsSuccessful, 
            T.Active AS ServerModel_Active, 
            T.DateCreated AS ServerModel_DateCreated, 
            T.DateModified AS ServerModel_DateModified, 
            T.DateDeactivated AS ServerModel_DateDeactivated, 
            True AS _from0
        FROM ApplicationContext.Servers AS T
    ) AS T1");
		}

		/// <summary>
		/// Gets the view for ApplicationContext.Servers.
		/// </summary>
		/// <returns>The mapping view.</returns>
		private static DbMappingView GetView19()
		{
			return new DbMappingView(@"
    SELECT VALUE -- Constructing Servers
        [ServerFramework.Database.Context.ServerModel](T1.ServerModel_ID, T1.ServerModel_IsSuccessful, T1.ServerModel_Active, T1.ServerModel_DateCreated, T1.ServerModel_DateModified, T1.ServerModel_DateDeactivated)
    FROM (
        SELECT 
            T.ID AS ServerModel_ID, 
            T.IsSuccessful AS ServerModel_IsSuccessful, 
            T.Active AS ServerModel_Active, 
            T.DateCreated AS ServerModel_DateCreated, 
            T.DateModified AS ServerModel_DateModified, 
            T.DateDeactivated AS ServerModel_DateDeactivated, 
            True AS _from0
        FROM CodeFirstDatabase.ServerModel AS T
    ) AS T1");
		}
	}
}
