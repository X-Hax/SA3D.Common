namespace SA3D.Common.IO.ExeStructs
{
	internal struct OSRel
	{
		public const uint StructSize = 8;

		/// <summary>
		/// byte offset from the previous entry
		/// </summary>
		public ushort Offset { get; set; }

		public byte Type { get; set; }

		public byte Section { get; set; }

		public uint Addend { get; set; }

		public OSRel(ushort offset, byte type, byte section, uint addend)
		{
			Offset = offset;
			Type = type;
			Section = section;
			Addend = addend;
		}

		public static OSRel Read(EndianStackReader data, uint address)
		{
			return new(
				data.ReadUShort(address),
				data[address + 2],
				data[address + 3],
				data.ReadUInt(address + 4));
		}
	}
}
