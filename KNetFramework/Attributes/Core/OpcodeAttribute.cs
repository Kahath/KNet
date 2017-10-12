/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Attributes.Base;
using KNetFramework.Enums;
using System;

namespace KNetFramework.Attributes.Core
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class OpcodeAttribute : Attribute, ICustomAttribute
	{
		#region Fields

		private ushort _opcode;
		private string _author;
		private int _version;
		private OpcodeTypes _type;

		#endregion

		#region Properties

		public ushort Opcode
		{
			get { return _opcode; }
		}

		public string Author
		{
			get { return _author; }
		}

		public int Version
		{
			get { return _version; }
		}

		public OpcodeTypes Type
		{
			get { return _type; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="KNetFramework.Constants.Attributes.OpcodeAttribute"/> type.
		/// </summary>
		/// <param name="opcode">Client packet opcode</param>
		/// <param name="author">Author of method</param>
		/// <param name="version">Version of method</param>
		/// <param name="type">Opcode type</param>
		public OpcodeAttribute(ushort opcode, string author, int version, OpcodeTypes type)
		{
			_opcode = opcode;
			_author = author;
			_version = version;
			_type = type;
		}

		public OpcodeAttribute()
		{

		}

		#endregion
	}
}
