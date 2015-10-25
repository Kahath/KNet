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

namespace ServerFramework.Network.Session
{
	internal sealed class ConsoleClient : IClient
	{
		#region Fields

		private int _sessionId;
		private int _id;
		private string _name;

		#endregion

		#region Properties

		public int SessionId
		{
			get { return _sessionId; }
			set { _sessionId = value; }
		}

		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		#endregion
	}
}
