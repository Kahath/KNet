/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KNetFramework.Extensions
{
	public static class TypeExtensions
	{
		#region Methods

		#region GetAllMethods

		public static IList<MethodInfo> GetAllMethods(this Type type)
		{
			List<MethodInfo> retVal = new List<MethodInfo>();

			retVal.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.Static));
			retVal.AddRange(type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static));

			retVal.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.Instance));
			retVal.AddRange(type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance));

			return retVal;
		}

		#endregion

		#region GetMethodByName

		public static MethodInfo GetMethodByName(this Type type, string name, params Type[] parameters)
		{
			MethodInfo retVal;

			retVal = type.GetAllMethods()
				.FirstOrDefault
				(x =>
					x.Name == name
					&& x.GetParameters()
						.Select(y => y.ParameterType)
						.SequenceEqual(parameters)
				);

			return retVal;
		}

		#endregion

		#endregion
	}
}
