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
