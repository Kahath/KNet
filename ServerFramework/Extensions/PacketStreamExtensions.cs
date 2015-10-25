/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace ServerFramework.Extensions
{
	public static class PacketStreamExtensions
	{
		#region Fields

		private static Dictionary<Type, Func<BinaryReader, int, object>> ReadFunctions
			= new Dictionary<Type, Func<BinaryReader, int, object>>()
		{
			{ typeof(bool),		(reader, count) => reader.ReadBoolean()		},
			{ typeof(sbyte),	(reader, count) => reader.ReadSByte()		},
			{ typeof(byte),		(reader, count) => reader.ReadByte()		},
			{ typeof(char),		(reader, count) => reader.ReadChar()		},
			{ typeof(short),	(reader, count) => reader.ReadInt16()		},
			{ typeof(ushort),	(reader, count) => reader.ReadUInt16()		},
			{ typeof(int),		(reader, count) => reader.ReadInt32()		},
			{ typeof(uint),		(reader, count) => reader.ReadUInt32()		},
			{ typeof(float),	(reader, count) => reader.ReadSingle()		},
			{ typeof(long),		(reader, count) => reader.ReadInt64()		},
			{ typeof(ulong),	(reader, count) => reader.ReadUInt64()		},
			{ typeof(double),	(reader, count) => reader.ReadDouble()		},
			{ typeof(byte[]),	(reader, count) => reader.ReadBytes(count)	},
			{ typeof(char[]),	(reader, count) => reader.ReadChars(count)	},
			{ typeof(decimal),	(reader, count) => reader.ReadDecimal()		},
			{ typeof(string),	(reader, count) => reader.ReadString()		},
		};

		private static Dictionary<Type, Action<BinaryWriter, object>> WriteActions
			= new Dictionary<Type, Action<BinaryWriter, object>>()
		{
			{ typeof(bool),		(writer, value) => writer.Write((bool)value)	},
			{ typeof(sbyte),	(writer, value) => writer.Write((sbyte)value)	},
			{ typeof(byte),		(writer, value) => writer.Write((byte)value)	},
			{ typeof(char),		(writer, value) => writer.Write((char)value)	},
			{ typeof(short),	(writer, value) => writer.Write((short)value)	},
			{ typeof(ushort),	(writer, value) => writer.Write((ushort)value)	},
			{ typeof(int),		(writer, value) => writer.Write((int)value)		},
			{ typeof(uint),		(writer, value) => writer.Write((uint)value)	},
			{ typeof(float),	(writer, value) => writer.Write((float)value)	},
			{ typeof(long),		(writer, value) => writer.Write((long)value)	},
			{ typeof(ulong),	(writer, value) => writer.Write((ulong)value)	},
			{ typeof(double),	(writer, value) => writer.Write((double)value)	},
			{ typeof(byte[]),	(writer, value) => writer.Write((byte[])value)	},
			{ typeof(char[]),	(writer, value) => writer.Write((char[])value)	},
			{ typeof(decimal),	(writer, value) => writer.Write((decimal)value)	},
			{ typeof(string),	(writer, value) => writer.Write((string)value)	},
		};

		#endregion

		#region Methods

		#region Read

		public static T Read<T>(this BinaryReader reader, int count)
		{
			Type type = typeof(T);
			object value = ReadFunctions[type](reader, count);

			return (T)Convert.ChangeType(value, type);
		}

		#endregion

		#region Write

		public static void Write<T>(this BinaryWriter writer, object value)
		{
			Type type = typeof(T);
			WriteActions[type](writer, value);
		}

		#endregion

		#region Skip

		public static void Skip(this Stream stream, int count)
		{
			stream.Position += count;
		}

		#endregion

		#endregion
	}
}
