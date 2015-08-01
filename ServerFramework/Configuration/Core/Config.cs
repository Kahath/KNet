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

using DILibrary.Constants;
using DILibrary.DependencyInjection;
using ServerFramework.Configuration.Base;

namespace ServerFramework.Configuration.Core
{
	/// <summary>
	/// Provides configuration dependency injection.
	/// </summary>
	public class Config : Dependency<IConfig>
	{
		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ServerFramework.Configuration.Config"/> type.
		/// </summary>
		/// <param name="path">Path to configuration file.</param>
		public Config(string path)
			: base(ResolveType.Transient, path)
		{

		}

		#endregion

		#region Methods

		#region Read

		/// <summary>
		/// Reads generic value from configuration.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="config">Name of configuration.</param>
		/// <param name="hex">Is value written as hexadecimal value.</param>
		/// <returns>Configuration value of generic data type.</returns>
		public T Read<T>(string config, bool hex = false)
		{
			return Instance.Read<T>(config, hex);
		}

		#endregion

		#endregion
	}
}
