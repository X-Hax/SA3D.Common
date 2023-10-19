namespace SA3D.Common.IO.ExeStructs
{
	internal struct OSSectionInfo
	{
		public const uint StructSize = 8;

		public uint Offset { get; set; }
		public uint Size { get; set; }

		public OSSectionInfo(uint offset, uint size)
		{
			Offset = offset;
			Size = size;
		}

		public static OSSectionInfo Read(EndianStackReader data, uint address)
		{
			return new(
				data.ReadUInt(address),
				data.ReadUInt(address + 4));
		}
	}
}
