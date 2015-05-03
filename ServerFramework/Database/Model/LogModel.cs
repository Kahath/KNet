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

using ServerFramework.Database.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model
{
    [Table("Log", Schema="Application")]
    public class LogModel : EntityBase
    {
        #region Properties

        [Key]
        public int ID { get; set; }
        public string Message { get; set; }
        public int LogTypeID { get; set; }
        [ForeignKey("LogTypeID")]
        public LogTypeModel LogType { get; set; }

        #endregion
    }
}
