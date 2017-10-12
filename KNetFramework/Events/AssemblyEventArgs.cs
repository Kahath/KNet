/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using System;
using System.Reflection;

namespace KNetFramework.Events
{
	public class AssemblyEventArgs : EventArgs
	{
		#region Fields

		Assembly _assembly;
		Type _type;
		MethodInfo _method;

		#endregion

		#region Properties

		public Assembly Assembly
		{
			get { return _assembly; }
		}

		public Type Type
		{
			get { return _type; }
		}

		public MethodInfo Method
		{
			get { return _method; }
		}

		#endregion

		#region Constructors

		public AssemblyEventArgs(Assembly assembly, Type type, MethodInfo method)
		{
			_assembly = assembly;
			_type = type;
			_method = method;
		}

		#endregion
	}
}
