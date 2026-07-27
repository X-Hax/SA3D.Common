using Amicitia.IO.Binary;

namespace SA3D.Common.IO.ExeStructs
{
	internal struct OSSectionInfo : IBinarySerializable
	{
		public const uint StructSize = 8;

		public uint Offset { get; set; }
		public uint Size { get; set; }

		public void Read(BinaryObjectReader reader)
		{
			Offset = reader.ReadUInt32();
			Size = reader.ReadUInt32();
		}

		public readonly void Write(BinaryObjectWriter writer)
		{
			writer.WriteUInt32(Offset);
			writer.WriteUInt32(Size);
		}
	}
}
