/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Attributes.Base;
using ServerFramework.Enums;
using System;

namespace ServerFramework.Attributes.Core
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class OpcodeAttribute : Attribute, ICustomAttribute
	{
		#region Fields

		private ushort _opcode;
		private string _author;
		private int _version;
		private OpcodeType _type;

		#endregion

		#region Properties

		public ushort Opcode
		{
			get { return _opcode; }
			set { _opcode = value; }
		}

		public string Author
		{
			get { return _author; }
			set { _author = value; }
		}

		public int Version
		{
			get { return _version; }
			set { _version = value; }
		}

		public OpcodeType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ServerFramework.Constants.Attributes.OpcodeAttribute"/> type.
		/// </summary>
		/// <param name="opcode">Client packet opcode</param>
		/// <param name="author">Author of method</param>
		/// <param name="version">Version of method</param>
		/// <param name="type">Opcode type</param>
		public OpcodeAttribute(ushort opcode, string author, int version, OpcodeType type)
		{
			this.Opcode = opcode;
			this.Author = author;
			this.Version = version;
			this.Type = type;
		}

		public OpcodeAttribute()
		{

		}

		#endregion
	}
}
