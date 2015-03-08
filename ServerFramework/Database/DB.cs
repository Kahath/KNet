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

using ServerFramework.Database.Providers;

namespace ServerFramework.Database
{
    internal static class DB
	{
		#region Fields

		private static ApplicationProvider _application;

		#endregion

		#region Properties

		internal static ApplicationProvider Application
		{
			get
			{
				if (_application == null)
					_application = new ApplicationProvider();

				return _application;
			}
		}

		#endregion
    }
}
