using Amicitia.IO.Binary;

namespace SA3D.Common.IO.ExeStructs
{
	internal struct OSModuleLink : IBinarySerializable
	{
		public const uint StructSize = 8;

		public uint Next { get; set; }
		public uint Prev { get; set; }


		public void Read(BinaryObjectReader reader)
		{
			Next = reader.ReadUInt32();
			Prev = reader.ReadUInt32();
		}

		public readonly void Write(BinaryObjectWriter writer)
		{
			writer.WriteUInt32(Next);
			writer.WriteUInt32(Prev);
		}
	}
}
