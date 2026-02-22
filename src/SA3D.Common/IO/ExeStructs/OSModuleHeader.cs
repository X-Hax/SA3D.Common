using Amicitia.IO.Binary;

namespace SA3D.Common.IO.ExeStructs
{
	internal struct OSModuleHeader : IBinarySerializable
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

		public void Read(BinaryObjectReader reader)
		{
			Info = reader.ReadObject<OSModuleInfo>();
			BssSize = reader.ReadUInt32();
			RelOffset = reader.ReadUInt32();
			ImpOffset = reader.ReadUInt32();
			ImpSize = reader.ReadUInt32();
			PrologSection = reader.ReadByte();
			EpilogSection = reader.ReadByte();
			UnresolvedSection = reader.ReadByte();
			Padding0 = reader.ReadByte();
			Prolog = reader.ReadUInt32();
			Epilog = reader.ReadUInt32();
			Unresolved = reader.ReadUInt32();
			Align = reader.ReadUInt32();
			BssAlign = reader.ReadUInt32();
		}

		public readonly void Write(BinaryObjectWriter writer)
		{
			writer.WriteObject(Info);
			writer.WriteUInt32(BssSize);
			writer.WriteUInt32(RelOffset);
			writer.WriteUInt32(ImpOffset);
			writer.WriteUInt32(ImpSize);
			writer.WriteByte(PrologSection);
			writer.WriteByte(EpilogSection);
			writer.WriteByte(UnresolvedSection);
			writer.WriteByte(Padding0);
			writer.WriteUInt32(Prolog);
			writer.WriteUInt32(Epilog);
			writer.WriteUInt32(Unresolved);
			writer.WriteUInt32(Align);
			writer.WriteUInt32(BssAlign);
		}
	}
}
