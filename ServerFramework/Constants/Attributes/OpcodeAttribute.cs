using ServerFramework.Constants.Misc;
using System;

namespace ServerFramework.Constants.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
    public sealed class OpcodeAttribute : Attribute
    {
        public ushort Opcode;
        public string Author;
        public double Version;
        public OpcodeType Type;

        /// <summary>
        /// Attribute used for reading opcodes.
        /// </summary>
        /// <param name="opcode">Client packet opcode</param>
        /// <param name="author">Author of method</param>
        /// <param name="version">Version of method</param>
        /// <param name="description">Brief description</param>
        public OpcodeAttribute(ushort opcode, string author, double version, OpcodeType type)
        {
            this.Opcode = opcode;
            this.Author = author;
            this.Version = version;
            this.Type = type;
        }
    }
}
