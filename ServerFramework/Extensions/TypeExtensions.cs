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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServerFramework.Extensions
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
