/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;
using System.Reflection;

namespace ServerFramework.Events
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
			set { _assembly = value; }
		}

		public Type Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public MethodInfo Method
		{
			get { return _method; }
			set { _method = value; }
		}

		#endregion

		#region Constructors

		public AssemblyEventArgs(Assembly assembly, Type type, MethodInfo method)
		{
			Assembly = assembly;
			Type = type;
			Method = method;
		}

		#endregion
	}
}
