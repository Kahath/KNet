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
using System.IO;

namespace ServerFramework.Extensions
{
    public static class PacketStreamExtensions
    {
        #region Fields

        private static Dictionary<Type, Func<BinaryReader, int, object>> ReadFunctions = new Dictionary<Type, Func<BinaryReader, int, object>>()
        {
            { typeof(bool),     (reader, count) => reader.ReadBoolean()     },
            { typeof(sbyte),    (reader, count) => reader.ReadSByte()       },
            { typeof(byte),     (reader, count) => reader.ReadByte()        },
            { typeof(char),     (reader, count) => reader.ReadChar()        },
            { typeof(short),    (reader, count) => reader.ReadInt16()       },
            { typeof(ushort),   (reader, count) => reader.ReadUInt16()      },
            { typeof(int),      (reader, count) => reader.ReadInt32()       },
            { typeof(uint),     (reader, count) => reader.ReadUInt32()      },
            { typeof(float),    (reader, count) => reader.ReadSingle()      },
            { typeof(long),     (reader, count) => reader.ReadInt64()       },
            { typeof(ulong),    (reader, count) => reader.ReadUInt64()      },
            { typeof(double),   (reader, count) => reader.ReadDouble()      },
            { typeof(byte[]),   (reader, count) => reader.ReadBytes(count)  },
            { typeof(char[]),   (reader, count) => reader.ReadChars(count)  },
            { typeof(decimal),  (reader, count) => reader.ReadDecimal()     },
            { typeof(string),   (reader, count) => reader.ReadString()      },  
        };

        private static Dictionary<Type, Action<BinaryWriter, object>> WriteActions = new Dictionary<Type, Action<BinaryWriter, object>>()
        {
            { typeof(bool),     (writer, value) => writer.Write((bool)value)    },
            { typeof(sbyte),    (writer, value) => writer.Write((sbyte)value)   },
            { typeof(byte),     (writer, value) => writer.Write((byte)value)    },
            { typeof(char),     (writer, value) => writer.Write((char)value)    },
            { typeof(short),    (writer, value) => writer.Write((short)value)   },
            { typeof(ushort),   (writer, value) => writer.Write((ushort)value)  },
            { typeof(int),      (writer, value) => writer.Write((int)value)     },
            { typeof(uint),     (writer, value) => writer.Write((uint)value)    },
            { typeof(float),    (writer, value) => writer.Write((float)value)   },
            { typeof(long),     (writer, value) => writer.Write((long)value)    },
            { typeof(ulong),    (writer, value) => writer.Write((ulong)value)   },
            { typeof(double),   (writer, value) => writer.Write((double)value)  },
            { typeof(byte[]),   (writer, value) => writer.Write((byte[])value)  },
            { typeof(char[]),   (writer, value) => writer.Write((char[])value)  },
            { typeof(decimal),  (writer, value) => writer.Write((decimal)value) },
            { typeof(string),   (writer, value) => writer.Write((string)value)  },
        };

        #endregion

        #region Methods

        #region Read

        public static T Read<T>(this BinaryReader reader, int count)
        {
            Type type = typeof(T);
            object value = ReadFunctions[type](reader, count);

            return (T)Convert.ChangeType(value, typeof(T));
        }

        #endregion

        #region Write

        public static void Write<T>(this BinaryWriter writer, object value)
        {
            Type type = typeof(T);
            WriteActions[type](writer, value);
        }

        #endregion

        #endregion
    }
}
