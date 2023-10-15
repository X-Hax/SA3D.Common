namespace SA3D.Common.IO.ExeStructs
{
    internal struct OSModuleLink
    {
        public const uint StructSize = 8;

        public uint Next { get; set; }
        public uint Prev { get; set; }

        public OSModuleLink(uint next, uint prev)
        {
            this.Next = next;
            this.Prev = prev;
        }

        public static OSModuleLink Read(EndianStackReader data, uint address)
        {
            return new(
                data.ReadUInt(address),
                data.ReadUInt(address + 4));
        }
    }
}
