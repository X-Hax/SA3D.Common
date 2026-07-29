using System;
using System.ComponentModel;
using System.Globalization;

namespace SA3D.Common.Converters
{
	/// <summary>
	/// A valueconverter for strings
	/// </summary>
	public sealed class StringConverter<T> : TypeConverter
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
			if(destinationType == typeof(string) && value is T t)
			{
				return ConvertTo(t, context?.PropertyDescriptor?.Name);
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Converts an object to a string
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <param name="debugName">Name by which to identify the value being converted</param>
		/// <returns></returns>
		public static string ConvertTo(T value, string? debugName)
		{
			try
			{
				return value?.ToString()
					?? throw new NullReferenceException("Conversion returned null");
			}
			catch(Exception exception)
			{
				throw new InvalidCastException($"Failed to cast {(string.IsNullOrWhiteSpace(debugName) ? "?" : debugName)} from an object to a string! Value: {value}", exception);
			}
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
		/// Converts a string to an object
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <param name="debugName">Name by which to identify the value being converted</param>
		/// <returns></returns>
		public static T ConvertFrom(string value, string? debugName)
		{
			try
			{
				return (T?)Activator.CreateInstance(typeof(T), value)
					?? throw new NullReferenceException("Conversion returned null");
			}
			catch(Exception exception)
			{
				throw new InvalidCastException($"Failed to cast {(string.IsNullOrWhiteSpace(debugName) ? "?" : debugName)} from a string to an object! Value: {value}", exception);
			}
		}
	}
}
