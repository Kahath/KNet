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

using ServerFramework.Attributes.Base;
using ServerFramework.Enums;
using System;

namespace ServerFramework.Attributes.Core
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
	public sealed class CommandAttribute : Attribute, ICustomAttribute
	{
		#region Fields

		private string _name;
		private CommandLevel _commandLevel;
		private string _description;

		#endregion

		#region Properties

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public CommandLevel CommandLevel
		{
			get { return _commandLevel; }
			set { _commandLevel = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		#endregion

		#region Constructors

		public CommandAttribute()
		{

		}

		public CommandAttribute(string name, CommandLevel commandLevel, string description)
		{
			Name = name;
			CommandLevel = commandLevel;
			Description = description;
		}

		#endregion
	}
}
