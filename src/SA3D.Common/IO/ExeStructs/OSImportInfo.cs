using Amicitia.IO.Binary;

namespace SA3D.Common.IO.ExeStructs
{
	internal struct OSImportInfo : IBinarySerializable
	{
		public const uint StructSize = 8;

		public uint ID { get; set; }
		public uint Offset { get; set; }


		public void Read(BinaryObjectReader reader)
		{
			ID = reader.ReadUInt32();
			Offset = reader.ReadUInt32();
		}

		public readonly void Write(BinaryObjectWriter writer)
		{
			writer.WriteUInt32(ID);
			writer.WriteUInt32(Offset);
		}
	}
}
