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
			if(destinationType == typeof(string))
			{
				return true;
			}

			return base.CanConvertTo(context, destinationType);
		}

		/// <inheritdoc/>
		public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
		{
			if(destinationType == typeof(string) && value is float single)
			{
				return ConvertTo(single);
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Converts a radians value to a 16-bit BAMS string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ConvertTo(float value)
		{
			return ((ushort)MathHelper.RadToBAMS(value)).ToString("X");
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
				return ConvertFrom(str, context?.PropertyDescriptor?.Name);
			}

			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>
		/// Converts a 32-bit BAMS string value to radians
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <param name="debugName">Name by which to identify the value being converted</param>
		/// <returns></returns>
		public static float ConvertFrom(string value, string? debugName)
		{
			if(ushort.TryParse(value, NumberStyles.HexNumber, null, out ushort result))
			{
				return MathHelper.BAMSToRad(result);
			}

			throw new InvalidCastException($"Failed to cast {(string.IsNullOrWhiteSpace(debugName) ? "?" : debugName)} from BAMS16 to float! Value: {value}");
		}

		/// <inheritdoc/>
		public override bool IsValid(ITypeDescriptorContext? context, object? value)
		{
			if(value is ushort)
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
