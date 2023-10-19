namespace SA3D.Common.IO.ExeStructs
{
	internal struct OSModuleInfo
	{
		public const uint StructSize = 0x20;

		/// <summary>
		/// unique identifier for the module
		/// </summary>
		public uint ID { get; set; }

		/// <summary>
		/// doubly linked list of modules
		/// </summary>
		public OSModuleLink Link { get; set; }

		/// <summary>
		/// # of sections
		/// </summary>
		public uint NumSections { get; set; }

		/// <summary>
		/// offset to section info table
		/// </summary>
		public uint SectionInfoOffset { get; set; }

		/// <summary>
		/// offset to module name
		/// </summary>
		public uint NameOffset { get; set; }

		/// <summary>
		/// size of module name
		/// </summary>
		public uint NameSize { get; set; }

		/// <summary>
		/// version number
		/// </summary>
		public uint Version { get; set; }

		public OSModuleInfo(uint id, OSModuleLink link, uint numSections, uint sectionInfoOffset, uint nameOffset, uint nameSize, uint version)
		{
			ID = id;
			Link = link;
			NumSections = numSections;
			SectionInfoOffset = sectionInfoOffset;
			NameOffset = nameOffset;
			NameSize = nameSize;
			Version = version;
		}

		public static OSModuleInfo Read(EndianStackReader data, uint address)
		{
			return new(
				data.ReadUInt(address),
				OSModuleLink.Read(data, address + 4),
				data.ReadUInt(address + 0xC),
				data.ReadUInt(address + 0x10),
				data.ReadUInt(address + 0x14),
				data.ReadUInt(address + 0x18),
				data.ReadUInt(address + 0x1C));
		}
	}
}
