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

namespace ServerFramework.Configuration.Helpers
{
	public class ConfigurationHelper
	{
		public const string Key = "name";
		public const string Value = "value";

		public const string Path = "ServerConfig.xml";
		public const string BindIPKey = "bindip";
		public const string BindPortKey = "bindport";
		public const string LogLevelKey = "loglevel";
		public const string PacketLogLevelKey = "packetloglevel";
		public const string OpcodeAllowLevelKey = "opcodeallowlevel";
		public const string PacketLogSizeKey = "packetlogsize";
		public const string BufferSIzeKey = "buffersize";
		public const string MaxConnectionsKey = "maxconnections";
		public const string MaxSimultaneousAcceptOpsKey = "maxsimultaneousacceptops";
		public const string BacklogKey = "backlog";
		public const string DBHostKey = "dbhost";
		public const string DBPortKey = "dbport";
		public const string DBUserKey = "dbuser";
		public const string DBPassKey = "dbpass";
		public const string DBNameKey = "dbname";
	}
}
