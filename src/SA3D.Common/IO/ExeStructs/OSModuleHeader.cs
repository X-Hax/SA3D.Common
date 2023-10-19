namespace SA3D.Common.IO.ExeStructs
{
	internal struct OSModuleHeader
	{
		public const uint StructSize = 0x48;

		/// <summary>
		/// CAUTION: info must be the 1st member
		/// </summary>
		public OSModuleInfo Info { get; set; }

		#region OS_MODULE_VERSION == 1

		/// <summary>
		/// total size of bss sections in bytes
		/// </summary>
		public uint BssSize { get; set; }

		public uint RelOffset { get; set; }

		public uint ImpOffset { get; set; }

		/// <summary>
		/// size in bytes
		/// </summary>
		public uint ImpSize { get; set; }

		/// <summary>
		/// section # for prolog function
		/// </summary>
		public byte PrologSection { get; set; }

		/// <summary>
		/// section # for epilog function
		/// </summary>
		public byte EpilogSection { get; set; }

		/// <summary>
		///  section # for unresolved function
		/// </summary>
		public byte UnresolvedSection { get; set; }

		public byte Padding0 { get; set; }

		/// <summary>
		/// prolog function offset
		/// </summary>
		public uint Prolog { get; set; }

		/// <summary>
		///  epilog function offset
		/// </summary>
		public uint Epilog { get; set; }

		/// <summary>
		///  unresolved function offset
		/// </summary>
		public uint Unresolved { get; set; }

		#endregion

		#region OS_MODULE_VERSION == 2

		/// <summary>
		/// module alignment constraint
		/// </summary>
		public uint Align { get; set; }

		/// <summary>
		/// bss alignment constraint
		/// </summary>
		public uint BssAlign { get; set; }

		#endregion

		public OSModuleHeader(
			OSModuleInfo info,
			uint bssSize,
			uint relOffset,
			uint impOffset,
			uint impSize,
			byte prologSection,
			byte epilogSection,
			byte unresolvedSection,
			byte padding0,
			uint prolog,
			uint epilog,
			uint unresolved,
			uint align,
			uint bssAlign)
		{
			Info = info;
			BssSize = bssSize;
			RelOffset = relOffset;
			ImpOffset = impOffset;
			ImpSize = impSize;
			PrologSection = prologSection;
			EpilogSection = epilogSection;
			UnresolvedSection = unresolvedSection;
			Padding0 = padding0;
			Prolog = prolog;
			Epilog = epilog;
			Unresolved = unresolved;
			Align = align;
			BssAlign = bssAlign;
		}

		public static OSModuleHeader Read(EndianStackReader data, uint address)
		{
			return new(
				OSModuleInfo.Read(data, address),
				data.ReadUInt(address + 0x20),
				data.ReadUInt(address + 0x24),
				data.ReadUInt(address + 0x28),
				data.ReadUInt(address + 0x2C),
				data[address + 0x30],
				data[address + 0x31],
				data[address + 0x32],
				data[address + 0x33],
				data.ReadUInt(address + 0x34),
				data.ReadUInt(address + 0x38),
				data.ReadUInt(address + 0x3C),
				data.ReadUInt(address + 0x40),
				data.ReadUInt(address + 0x44));
		}

	}
}
