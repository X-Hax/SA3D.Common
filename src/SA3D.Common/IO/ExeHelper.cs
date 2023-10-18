using SA3D.Common.IO.ExeStructs;
using System;

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

            EndianStackReader exeData = new(file);

            if(exeData.ReadUShort(0) != 0x5A4D)
            {
                return false;
            }

            uint ptr = exeData.ReadUInt(0x3c);
            if(exeData.ReadUInt(ptr) != 0x4550) //PE\0\0
            {
                return false;
            }

            ushort numsects = exeData.ReadUShort(ptr + 6);
            imageBase = exeData.ReadUInt(ptr + 0x34);
            result = new byte[exeData.ReadUInt(ptr + 0x50)];
            exeData.ReadBytes(0, result, 0, exeData.ReadInt(ptr + 0x54));

            ptr += 0xF8;
            for(int i = 0; i < numsects; i++)
            {
                exeData.ReadBytes(
                    exeData.ReadUInt(ptr + SectionOffsets.FAddr),
                    result,
                    exeData.ReadUInt(ptr + SectionOffsets.VAddr),
                    exeData.ReadInt(ptr + SectionOffsets.FSize)
                    );

                ptr += SectionOffsets.Size;
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
            EndianStackReader data = new(file);

            OSModuleHeader header = OSModuleHeader.Read(data, 0);

            OSSectionInfo[] sections = new OSSectionInfo[header.Info.NumSections];
            for(uint i = 0; i < header.Info.NumSections; i++)
            {
                sections[i] = OSSectionInfo.Read(data, header.Info.SectionInfoOffset + (i * 8));
            }

            OSImportInfo[] imports = new OSImportInfo[header.ImpSize / 8];
            for(uint i = 0; i < imports.Length; i++)
            {
                imports[i] = OSImportInfo.Read(data, header.ImpOffset + (i * 8));
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

            OSRel rel = OSRel.Read(data, reladdr);
            uint dataaddr = 0;

            unchecked
            {
                while(rel.Type != RelocTypes.R_DOLPHIN_END)
                {
                    dataaddr += rel.Offset;
                    uint sectionbase = (uint)(sections[rel.Section].Offset & ~1);
                    uint? newPointer = null;
                    switch(rel.Type)
                    {
                        case 0x01:
                            newPointer = rel.Addend + sectionbase;
                            break;
                        case 0x02:
                            newPointer = (data.ReadUInt(dataaddr) & 0xFC000003) | ((rel.Addend + sectionbase) & 0x3FFFFFC);
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
                            newPointer = (data.ReadUInt(dataaddr) & 0xFC000003) | ((rel.Addend + sectionbase - dataaddr) & 0x3FFFFFC);
                            break;
                        case 0x00:
                        case RelocTypes.R_DOLPHIN_NOP:
                        case RelocTypes.R_DOLPHIN_END:
                            break;
                        case RelocTypes.R_DOLPHIN_SECTION:
                            dataaddr = sectionbase;
                            break;
                        default:
                            throw new NotImplementedException($"REL type \"{rel.Type}\" not supported");
                    }

                    if(newPointer != null)
                    {
                        BitConverter.GetBytes(newPointer.Value + imageBase).CopyTo(file, dataaddr);
                    }

                    reladdr += 8;
                    rel = OSRel.Read(data, reladdr);
                }
            }
        }

    }
}
