namespace SA3D.Common.IO.ExeStructs
{
	internal struct OSImportInfo
	{
		public const uint StructSize = 8;

		public uint ID { get; set; }
		public uint Offset { get; set; }

		public OSImportInfo(uint id, uint offset)
		{
			ID = id;
			Offset = offset;
		}

		public static OSImportInfo Read(EndianStackReader data, uint address)
		{
			return new(
				data.ReadUInt(address),
				data.ReadUInt(address + 4));
		}
	}
}
