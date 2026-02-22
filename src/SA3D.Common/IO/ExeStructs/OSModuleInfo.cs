using Amicitia.IO.Binary;

namespace SA3D.Common.IO.ExeStructs
{
	internal struct OSModuleInfo : IBinarySerializable
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


		public void Read(BinaryObjectReader reader)
		{
			ID = reader.ReadUInt32();
			Link = reader.Read<OSModuleLink>();
			NumSections = reader.ReadUInt32();
			SectionInfoOffset = reader.ReadUInt32();
			NameOffset = reader.ReadUInt32();
			NameSize = reader.ReadUInt32();
			Version = reader.ReadUInt32();
		}

		public readonly void Write(BinaryObjectWriter writer)
		{
			writer.WriteUInt32(ID);
			writer.WriteObject(Link);
			writer.WriteUInt32(NumSections);
			writer.WriteUInt32(SectionInfoOffset);
			writer.WriteUInt32(NameOffset);
			writer.WriteUInt32(NameSize);
			writer.WriteUInt32(Version);
		}
	}
}
