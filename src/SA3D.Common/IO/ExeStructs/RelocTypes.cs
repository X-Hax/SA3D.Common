namespace SA3D.Common.IO.ExeStructs
{
    internal static class RelocTypes
    {
        /// <summary>
        /// C9h current offset += OSRel.offset
        /// </summary>
        public const byte R_DOLPHIN_NOP = 201;

        /// <summary>
        /// CAh current section = OSRel.section
        /// </summary>
        public const byte R_DOLPHIN_SECTION = 202;

        /// <summary>
        /// CBH
        /// </summary>
        public const byte R_DOLPHIN_END = 203;
    }
}
