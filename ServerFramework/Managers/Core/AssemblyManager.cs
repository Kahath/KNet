/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Attributes.Base;
using ServerFramework.Attributes.Core;
using ServerFramework.Commands.Base;
using ServerFramework.Configuration.Helpers;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.Command;
using ServerFramework.Database.Model.Application.Opcode;
using ServerFramework.Database.Model.Application.Server;
using ServerFramework.Database.Repository;
using ServerFramework.Enums;
using ServerFramework.Events;
using ServerFramework.Extensions;
using ServerFramework.Managers.Base;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ServerFramework.Managers.Core
{
	public class AssemblyManager : ManagerBase<AssemblyManager>
	{
		#region Events

		public event AssemblyEventHandler OnType;
		public event AssemblyEventHandler OnMethod;

		#endregion

		#region Constructors

		AssemblyManager()
		{
			Init();
		}

		#endregion

		#region Methods

		#region Init

		protected override void Init()
		{
			string path = Path.Combine
				(
					Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
				,	ServerConfig.AssemblyPath
				);

			foreach (string dll in Directory.GetFiles(path, "*.dll"))
			{
				Load(dll);
			}

			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()
				.Where(x => x.CustomAttributes
					.Any(y => typeof(ICustomAttribute).IsAssignableFrom(y.AttributeType))))
			{
				ProcessCustomAssembly(a);
			}

			using(ApplicationContext context = new ApplicationContext())
			{
				ServerModel server = context.Servers.OrderByDescending(x => x.ID).First();

				context.Opcodes.RemoveRange(
					context.Opcodes.Where
					(x => 
						x.Active
						&& x.DateModified.HasValue ? x.DateModified.Value < server.DateCreated : x.DateCreated < server.DateCreated
					).ToList());

				context.Commands.RemoveRange(
					context.Commands.Where
					(x => 
						x.Active
						&& x.DateModified.HasValue ? x.DateModified.Value < server.DateCreated : x.DateCreated < server.DateCreated
					).ToList());

				context.SaveChanges();
			}
		}

		#endregion

		#region Load

		public void Load(string path)
		{
			Assembly assembly = null;

			try
			{
				assembly = Assembly.LoadFrom(path);
			}
			catch(FileNotFoundException e)
			{
				Manager.LogMgr.Log(LogType.Error, $"{e.ToString()}");
			}
			catch(FileLoadException e)
			{
				Manager.LogMgr.Log(LogType.Error, $"{e.ToString()}");
			}
			catch (ArgumentNullException e)
			{
				Manager.LogMgr.Log(LogType.Error, $"{e.ToString()}");
			}

			if(assembly != null)
			{
				ProcessCustomAssembly(assembly);
			}
		}

		#endregion

		#region ProcessCustomAssembly

		public void ProcessCustomAssembly(Assembly assembly)
		{
			using (ApplicationContext context = new ApplicationContext())
			{
				foreach (Type type in assembly.GetTypes())
				{
					OnCustomAssemblyType(assembly, type, context);

					foreach (MethodInfo method in type.GetAllMethods())
					{
						OnCustomAssemblyMethod(assembly, type, method, context);
					}
				}

				context.SaveChanges();
			}
		}

		#endregion

		#region GetType

		public Type GetType(string assemblyName, string typeName)
		{
			Type retVal = null;

			Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
				.FirstOrDefault(x => x.FullName == assemblyName);

			if (assembly != null)
			{
				retVal = assembly.GetType(typeName);
			}

			return retVal;
		}

		#endregion

		#region GetMethod

		public MethodInfo GetMethod(string assemblyName, string typeName
			, string methodName, params Type[] parameters)
		{
			MethodInfo retVal = null;

			Type type = GetType(assemblyName, typeName);

			if(type != null)
			{
				retVal = type.GetMethodByName(methodName, parameters);
			}

			return retVal;
		}

		#endregion

		#region OnCustomAssemblyType

		private void OnCustomAssemblyType(Assembly assembly, Type type, ApplicationContext context)
		{
			if (type.GetCustomAttributes<CommandAttribute>().Any())
			{
				CommandRepository CRepo = new CommandRepository(context);

				foreach (CommandAttribute attr in type.GetCustomAttributes<CommandAttribute>())
				{
					if (attr != null)
					{
						MethodInfo method = type.GetMethodByName("GetCommand");

						if (method != null)
						{
							CommandHandlerBase commandBase = InvokeConstructor(type) as CommandHandlerBase;

							if (commandBase != null)
							{
								CommandModel existingCommand = context.Commands
									.FirstOrDefault(x => x.Name == commandBase.Name && x.Active);

								if (existingCommand == null)
								{
									existingCommand = new CommandModel(commandBase);

									existingCommand.AssemblyName = assembly.FullName;
									existingCommand.TypeName = type.FullName;
									existingCommand.MethodName = method.Name;

									context.Commands.Add(existingCommand);
								}
								else
								{
									existingCommand.AssemblyName = assembly.FullName;
									existingCommand.TypeName = type.FullName;
									existingCommand.MethodName = method.Name;
									context.Entry(existingCommand).State = EntityState.Modified;
								}

								Command cmd = InvokeMethod<Command>(commandBase, method);

								if (cmd != null)
									CRepo.UpdateSubCommands(cmd, existingCommand);
							}
						}
					}
				}
			}

			if (OnType != null)
				OnType(this, new AssemblyEventArgs(assembly, type, null));
		}

		#endregion

		#region OnCustomAssemblyMethod

		private void OnCustomAssemblyMethod(Assembly assembly, Type type, MethodInfo method, ApplicationContext context)
		{
			foreach (OpcodeAttribute attr in method.GetCustomAttributes<OpcodeAttribute>())
			{
				if (attr != null)
				{
					OpcodeModel existingOpcode = context.Opcodes.FirstOrDefault
						(x =>
							x.Code == attr.Opcode
							&& x.TypeID == (int)attr.Type
							&& x.Version == attr.Version
							&& x.Active
						);

					if (existingOpcode == null)
					{
						OpcodeModel model = new OpcodeModel(attr);
						model.AssemblyName = assembly.FullName;
						model.TypeName = method.DeclaringType.FullName;
						model.MethodName = method.Name;

						context.Opcodes.Add(model);
					}
					else
					{
						existingOpcode.AssemblyName = assembly.FullName;
						existingOpcode.TypeName = type.FullName;
						existingOpcode.MethodName = method.Name;
						context.Entry(existingOpcode).State = EntityState.Modified;
					}
				}
			}

			if (OnMethod != null)
				OnMethod(this, new AssemblyEventArgs(assembly, type, method));
		}

		#endregion

		#region InvokeMethod

		public T InvokeMethod<T>(object obj, MethodInfo method, params object[] args)
		{
			T retVal = default(T);

			try
			{
				retVal = (T)method.Invoke(obj, args);
			}
			catch (TargetInvocationException)
			{
				Manager.LogMgr.Log
					(
						LogType.Error
					,	$"Error invoking method {method.Name} of type {typeof(T).FullName}"
					);
			}

			return retVal;
		}

		#endregion

		#region InvokeConstructor

		public object InvokeConstructor(Type type, params object[] args)
		{
			object retVal = null;

			Type[] types = args.Select(x => x.GetType()).ToArray();

			ConstructorInfo constructor = type.GetConstructor(types);

			if (constructor != null)
				retVal = constructor.Invoke(args);

			return retVal;
		}

		#endregion

		#endregion
	}
}
