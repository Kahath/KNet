/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Managers.Base;
using KNetFramework.Managers.Interface;
using System;
using System.Reflection;

namespace KNetFramework.Managers.Core
{
	public class AssemblyManager : ManagerBase<AssemblyManager, IAssemblyManager>
	{
		#region Events

		public event AssemblyEventHandler OnType
		{
			add { Instance.OnType += value; }
			remove { Instance.OnType -= value; }
		}

		public event AssemblyEventHandler OnMethod
		{
			add { Instance.OnMethod += value; }
			remove { Instance.OnMethod -= value; }
		}

		#endregion

		#region Methods

		#region Load

		public void Load(string path)
		{
			Instance.Load(path);
		}

		#endregion

		#region ProcessCustomAssembly

		public void ProcessCustomAssembly(Assembly assembly)
		{
			Instance.ProcessCustomAssembly(assembly);
		}

		#endregion

		#region GetType

		public Type GetType(string assemblyName, string typeName)
		{
			return Instance.GetType(assemblyName, typeName);
		}

		#endregion

		#region GetMethod

		public MethodInfo GetMethod(string assemblyName, string typeName, string methodName, params Type[] parameters)
		{
			return Instance.GetMethod(assemblyName, typeName, methodName, parameters);
		}

		#endregion

		#region InvokeMethod

		public T InvokeMethod<T>(object obj, MethodInfo method, params object[] args)
			where T : class
		{
			return Instance.InvokeMethod<T>(obj, method, args);
		}

		#endregion

		#region InvokeConstructor

		public object InvokeConstructor(Type type, params object[] args)
		{
			return Instance.InvokeConstructor(type, args);
		}

		#endregion

		#endregion
	}
}
