using System;
using System.ComponentModel;
using System.Globalization;

namespace SA3D.Common.Converters
{
	/// <summary>
	/// A valueconverter for unsigned hexadecimal 16 bit numbers
	/// </summary>
	public class UInt16HexConverter : TypeConverter
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
			if(destinationType == typeof(string) && value is ushort integer)
			{
				return ConvertTo(integer);
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Converts an unsigned 16-bit integer to a hexadecimal string value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ConvertTo(ushort value)
		{
			return value.ToString("X");
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
		/// Converts a hexadecimal string value to an unsigned 16-bit integer
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <param name="debugName">Name by which to identify the value being converted</param>
		/// <returns></returns>
		public static ushort ConvertFrom(string value, string? debugName)
		{
			if(ushort.TryParse(value, NumberStyles.HexNumber, null, out ushort result))
			{
				return result;
			}

			throw new InvalidCastException($"Failed to cast {(string.IsNullOrWhiteSpace(debugName) ? "?" : debugName)} from hex to ushort! Value: {value}");
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
				return ushort.TryParse(str, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out _);
			}

			return base.IsValid(context, value);
		}
	}
}
