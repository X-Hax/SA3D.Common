using System;
using System.ComponentModel;
using System.Globalization;

namespace SA3D.Common.Converters
{
    /// <summary>
    /// A valueconverter for 16 bit BAMS angles
    /// </summary>
    public class BAMS16Converter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is float single)
                return ((ushort)MathHelper.RadToBAMS(single)).ToString("X");
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc/>
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string str)
            {
                ushort bams = ushort.Parse(str, NumberStyles.HexNumber);
                return MathHelper.BAMSToRad(bams);
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc/>
        public override bool IsValid(ITypeDescriptorContext? context, object? value)
        {
            if (value is ushort)
                return true;
            if (value is string str)
                return uint.TryParse(str, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out _);
            return base.IsValid(context, value);
        }
    }
}
