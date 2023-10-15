using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace SA3D.Common.Converters
{
    /// <summary>
    /// A valueconverter for <see cref="Vector3"/>
    /// </summary>
    public class Vector3Converter : ExpandableObjectConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
        {
            if (destinationType == typeof(Vector3))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is Vector3 v)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0:F6}, {1:F6}, {2:F6}", v.X, v.Y, v.Z);
            }
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
                string[] values = str.Split(',');
                return new Vector3(
                    float.Parse(values[0], CultureInfo.InvariantCulture),
                    float.Parse(values[1], CultureInfo.InvariantCulture),
                    float.Parse(values[2], CultureInfo.InvariantCulture));
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
