using Amicitia.IO.Binary;

namespace SA3D.Common.IO.ExeStructs
{
	internal struct OSRel : IBinarySerializable
	{
		public const uint StructSize = 8;

		/// <summary>
		/// byte offset from the previous entry
		/// </summary>
		public ushort Offset { get; set; }

		public byte Type { get; set; }

		public byte Section { get; set; }

		public uint Addend { get; set; }


		public void Read(BinaryObjectReader reader)
		{
			Offset = reader.ReadUInt16();
			Type = reader.ReadByte();
			Section = reader.ReadByte();
			Addend = reader.ReadUInt32();
		}

		public readonly void Write(BinaryObjectWriter writer)
		{
			writer.WriteUInt16(Offset);
			writer.WriteByte(Type);
			writer.WriteByte(Section);
			writer.WriteUInt32(Addend);
		}
	}
}
