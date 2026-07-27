using Amicitia.IO.Binary;
using Amicitia.IO.Streams;
using SA3D.Common.IO.ExeStructs;
using System;
using System.IO;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Helper methods for reading data off an exe file
	/// </summary>
	public static class ExeHelper
	{
		/// <summary>
		/// Expands sections in the exe to match RAM offsets.
		/// </summary>
		/// <param name="file">The exe file data to set up.</param>
		/// <param name="result">Resulting byte array.</param>
		/// <param name="imageBase">Extracted image base.</param>
		/// <returns>An endian reader for the setup exe data.</returns>
		public static bool SetupEXE(byte[] file, out byte[]? result, out uint imageBase)
		{
			result = null;
			imageBase = default;

			using MemoryStream stream = new(file);
			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, Endianness.Little);

			if(reader.ReadUInt16() != 0x5A4D)
			{
				return false;
			}

			reader.SeekPosition(0x3C);
			uint ptr = reader.ReadUInt32();

			reader.SeekPosition(ptr);
			if(reader.ReadUInt32() != 0x4550) //PE\0\0
			{
				return false;
			}

			reader.SeekPosition(ptr + 6);
			ushort numsects = reader.ReadUInt16();

			reader.SeekPosition(ptr + 0x34);
			imageBase = reader.ReadUInt32();

			reader.SeekPosition(ptr + 0x50);
			result = new byte[reader.ReadUInt32()];
			reader.ReadArray(reader.ReadInt32(), result);

			reader.SeekPosition(ptr + 0xF8);

			for(int i = 0; i < numsects; i++)
			{
				reader.Skip(0xC);
				uint vAddr = reader.ReadUInt32();
				int fSize = reader.ReadInt32();
				int fAddr = reader.ReadInt32();
				reader.Skip(0x10);

				using(reader.AtOffset(vAddr))
				{
					reader.ReadArray(fSize, result.AsSpan(fAddr));
				}
			}

			return true;
		}

		/// <summary>
		/// Restores pointers to REL (Wii/GC binary library) files.
		/// </summary>
		/// <param name="file">The Exe file data to fix the REL Pointers of</param>
		/// <param name="imageBase">Imagebase of the </param>
		/// <exception cref="NotImplementedException"/>
		public static void FixRELPointers(byte[] file, uint imageBase = 0)
		{
			using MemoryStream stream = new(file);
			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, Endianness.Little);

			OSModuleHeader header = reader.ReadObject<OSModuleHeader>();

			OSSectionInfo[] sections = new OSSectionInfo[header.Info.NumSections];
			reader.SeekPosition(header.Info.SectionInfoOffset);
			for(uint i = 0; i < header.Info.NumSections; i++)
			{
				sections[i] = reader.ReadObject<OSSectionInfo>();
			}

			OSImportInfo[] imports = new OSImportInfo[header.ImpSize / 8];
			reader.SeekPosition(header.ImpOffset);
			for(uint i = 0; i < imports.Length; i++)
			{
				imports[i] = reader.ReadObject<OSImportInfo>();
			}

			uint reladdr = 0;
			for(int i = 0; i < imports.Length; i++)
			{
				if(imports[i].ID == header.Info.ID)
				{
					reladdr = imports[i].Offset;
					break;
				}
			}

			reader.SeekPosition(reladdr);
			OSRel rel = reader.ReadObject<OSRel>();
			reladdr = (uint)reader.Position;

			unchecked
			{
				reader.SeekPosition(0);
				while(rel.Type != RelocTypes.R_DOLPHIN_END)
				{
					reader.Seek(rel.Offset, SeekOrigin.Current);
					uint sectionbase = (uint)(sections[rel.Section].Offset & ~1);
					uint? newPointer = null;
					switch(rel.Type)
					{
						case 0x01:
							newPointer = rel.Addend + sectionbase;
							break;
						case 0x02:
							using(reader.At())
							{
								newPointer = (reader.ReadUInt32() & 0xFC000003) | ((rel.Addend + sectionbase) & 0x3FFFFFC);
							}

							break;
						case 0x03:
						case 0x04:
							newPointer = (ushort)(rel.Addend + sectionbase);
							break;
						case 0x05:
							newPointer = (ushort)((rel.Addend + sectionbase) >> 16);
							break;
						case 0x06:
							newPointer = (ushort)(((rel.Addend + sectionbase) >> 16) + (((rel.Addend + sectionbase) & 0x8000) == 0x8000 ? 1 : 0));
							break;
						case 0x0A:
							using(SeekToken token = reader.At())
							{
								newPointer = (reader.ReadUInt32() & 0xFC000003) | ((rel.Addend + sectionbase - (uint)(long)token) & 0x3FFFFFC);
							}

							break;
						case 0x00:
						case RelocTypes.R_DOLPHIN_NOP:
						case RelocTypes.R_DOLPHIN_END:
							break;
						case RelocTypes.R_DOLPHIN_SECTION:
							reader.SeekPosition(sectionbase);
							break;
						default:
							throw new NotImplementedException($"REL type \"{rel.Type}\" not supported");
					}

					if(newPointer != null)
					{
						BitConverter.GetBytes(newPointer.Value + imageBase).CopyTo(file, reader.Position);
					}

					using(reader.AtOffset(reladdr))
					{
						rel = reader.ReadObject<OSRel>();
						reladdr = (uint)reader.Position;
					}
				}
			}
		}

	}
}
