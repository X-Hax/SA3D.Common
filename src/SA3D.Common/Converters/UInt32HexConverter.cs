using System;
using System.ComponentModel;
using System.Globalization;

namespace SA3D.Common.Converters
{
    /// <summary>
    /// A valueconverter for unsigned hexadecimal 32 bit numbers
    /// </summary>
    public class UInt32HexConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            if(destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if(destinationType == typeof(string) && value is uint integer)
            {
                return integer.ToString("X");
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if(sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc/>
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if(value is string str)
            {
                return uint.Parse(str, NumberStyles.HexNumber);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc/>
        public override bool IsValid(ITypeDescriptorContext? context, object? value)
        {
            if(value is uint)
            {
                return true;
            }

            if(value is string str)
            {
                return uint.TryParse(str, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out _);
            }

            return base.IsValid(context, value);
        }
    }
}
