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

using ServerFramework.Constants.Attributes;
using ServerFramework.Constants.Entities.Console;
using ServerFramework.Constants.Events;
using ServerFramework.Constants.Misc;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.Command;
using ServerFramework.Database.Model.Application.Opcode;
using ServerFramework.Database.Model.Application.Server;
using ServerFramework.Managers.Base;
using System;
using System.Collections.Generic;
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

		internal override void Init()
		{
			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies().
				Where(x => x.CustomAttributes.Any(y => typeof(ICustomAttribute).IsAssignableFrom(y.AttributeType))))
			{
				HandleCustomAssemblyTypes(a);
			}

			using(ApplicationContext context = new ApplicationContext())
			{
				ServerModel server = context.Servers.OrderByDescending(x => x.ID).First();

				context.Opcodes.Where(x => x.DateModified.HasValue ? x.DateModified.Value < server.DateCreated : false).ToList().ForEach(x => x.Active = false);
				context.Commands.Where(x => x.DateModified.HasValue ? x.DateModified.Value < server.DateCreated : false).ToList().ForEach(x => x.Active = false);

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
				Manager.LogMgr.Log(LogType.Error, "{0}", e.ToString());
			}
			catch(FileLoadException e)
			{
				Manager.LogMgr.Log(LogType.Error, "{0}", e.ToString());
			}
			catch (ArgumentNullException e)
			{
				Manager.LogMgr.Log(LogType.Error, "{0}", e.ToString());
			}

			if(assembly != null)
			{
				HandleCustomAssemblyTypes(assembly);
			}
		}

		#endregion

		#region HandleAssemblyTypes

		public void HandleCustomAssemblyTypes(Assembly assembly)
		{
			using (ApplicationContext context = new ApplicationContext())
			{
				foreach (Type type in assembly.GetTypes())
				{
					OnCustomAssemblyType(assembly, type, context);

					foreach (MethodInfo method in GetMethods(type))
					{
						OnCustomAssemblyMethod(assembly, type, method, context);
					}

					context.SaveChanges();
				}
			}
		}

		#endregion

		#region GetMethods

		public IList<MethodInfo> GetMethods(Type type)
		{
			List<MethodInfo> retVal = new List<MethodInfo>();

			retVal.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.Static));
			retVal.AddRange(type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static));

			retVal.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.Instance));
			retVal.AddRange(type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance));

			return retVal;
		}

		#endregion

		#region GetMethod

		public MethodInfo GetMethod(string assemblyName, string typeName, string methodName)
		{
			MethodInfo retVal = null;

			Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == assemblyName);

			if (assembly != null)
			{
				Type type = assembly.GetType(typeName);
				if(type != null)
				{
					retVal = GetMethod(type, methodName);
				}
			}

			return retVal;
		}

		public MethodInfo GetMethod(Type type, string name)
		{
			MethodInfo retVal;

			retVal = GetMethods(type).FirstOrDefault(x => x.Name == name);

			return retVal;
		}

		#endregion

		#region OnAssemblyType

		private void OnCustomAssemblyType(Assembly assembly, Type type, ApplicationContext context)
		{
			foreach (CommandAttribute attr in type.GetCustomAttributes<CommandAttribute>())
			{
				if (attr != null)
				{
					MethodInfo method = GetMethod(type, "GetCommand");

					if (method != null)
					{
						Command c = InvokeStaticMethod<Command>(method);
						CommandModel existingCommand = context.Commands.FirstOrDefault(x => x.Name == c.Name && x.Active);

						if(c != null)
						{
							if (existingCommand == null)
							{
								CommandModel command = new CommandModel(c);
								command.AssemblyName = assembly.FullName;
								command.TypeName = type.FullName;
								command.MethodName = method.Name;

								context.Commands.Add(command);
							}
							else
							{
								existingCommand.AssemblyName = assembly.FullName;
								existingCommand.TypeName = type.FullName;
								existingCommand.MethodName = method.Name;
								context.Entry(existingCommand).State = EntityState.Modified;
							}
						}

					}
				}
			}

			if (OnType != null)
				OnType(this, new AssemblyEventArgs(assembly, type, null));
		}

		#endregion

		#region OnAssemblyMethod

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

		#region InvokeStaticMethod

		public T InvokeStaticMethod<T>(string assemblyName, string typeName, string methodName, params object[] args)
		{
			T retVal = default(T);

			MethodInfo method = GetMethod(assemblyName, typeName, methodName);

			retVal = InvokeStaticMethod<T>(method, args);

			return retVal;
		}

		public T InvokeStaticMethod<T>(MethodInfo method, params object[] args)
		{
			T retVal = default(T);

			try
			{
				retVal = (T)method.Invoke(null, args);
			}
			catch (TargetInvocationException)
			{
				Manager.LogMgr.Log
					(
						LogType.Error
					,	"Error invoking method {0} of type {1}"
					,	method.Name
					,	typeof(T).FullName
					);
			}

			return retVal;
		}

		#endregion

		#endregion
	}
}
