/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using System;
using System.Reflection;

namespace KNetFramework.Managers.Interface
{
	public interface IAssemblyManager : IManager
	{
		#region Events

		event AssemblyEventHandler OnType;
		event AssemblyEventHandler OnMethod;

		#endregion

		#region Methods

		void Load(string path);
		void ProcessCustomAssembly(Assembly assembly);
		Type GetType(string assemblyName, string typeName);
		MethodInfo GetMethod(string assemblyName, string typeName, string methodName, params Type[] parameters);
		T InvokeMethod<T>(object obj, MethodInfo method, params object[] args) where T : class;
		object InvokeConstructor(Type type, params object[] args);

		#endregion
	}
}
