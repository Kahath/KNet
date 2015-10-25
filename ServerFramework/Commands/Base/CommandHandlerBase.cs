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

using ServerFramework.Attributes.Core;
using ServerFramework.Enums;
using System;
using System.Linq;

namespace ServerFramework.Commands.Base
{
	public abstract class CommandHandlerBase
	{
		#region Fields

		private CommandAttribute _attribute;

		#endregion

		#region Properties

		private CommandAttribute Attribute
		{
			get
			{
				if(_attribute == null)
					_attribute = GetType().GetCustomAttributes(typeof(CommandAttribute), true).FirstOrDefault() as CommandAttribute;

				return _attribute;
			}
		}

		public string Name
		{
			get
			{
				string retVal = String.Empty;

				if (Attribute != null)
					retVal = Attribute.Name;

				return retVal;
			}
		}

		public CommandLevel Level
		{
			get
			{
				CommandLevel retVal = CommandLevel.Ten;

				if (Attribute != null)
					retVal = Attribute.CommandLevel;

				return retVal;
			}
		}

		public string Description
		{
			get
			{
				string retVal = String.Empty;

				if (Attribute != null)
					retVal = Attribute.Description;

				return retVal;
			}
		}

		#endregion

		#region Methods

		#region GetCommand

		protected abstract Command GetCommand();

		#endregion

		#endregion
	}
}
