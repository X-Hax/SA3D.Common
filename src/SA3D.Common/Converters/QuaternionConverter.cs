using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace SA3D.Common.Converters
{
	/// <summary>
	/// A valueconverter for <see cref="Quaternion"/>
	/// </summary>
	public sealed class QuaternionConverter : ExpandableObjectConverter
	{
		/// <inheritdoc/>
		public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
		{
			if(destinationType == typeof(Quaternion))
			{
				return true;
			}

			return base.CanConvertTo(context, destinationType);
		}

		/// <inheritdoc/>
		public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
		{
			if(destinationType == typeof(string) && value is Quaternion v)
			{
				return ConvertTo(v);
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Converts a quaternion to a string
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns></returns>
		public static string ConvertTo(Quaternion value)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0:F6}, {1:F6}, {2:F6}, {3:F6}", value.X, value.Y, value.Z, value.W);
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
		/// Converts a string to a quaternion
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <param name="debugName">Name by which to identify the value being converted</param>
		/// <returns></returns>
		public static Quaternion ConvertFrom(string value, string? debugName)
		{
			try
			{
				string[] values = value.Split(',');
				if(values.Length != 4)
				{
					throw new InvalidOperationException($"Value split in {values.Length}; Expected 4!");
				}

				return new Quaternion(
					float.Parse(values[0], CultureInfo.InvariantCulture),
					float.Parse(values[1], CultureInfo.InvariantCulture),
					float.Parse(values[2], CultureInfo.InvariantCulture),
					float.Parse(values[3], CultureInfo.InvariantCulture)
				);
			}
			catch(Exception exception)
			{
				throw new InvalidCastException($"Failed to cast {(string.IsNullOrWhiteSpace(debugName) ? "?" : debugName)} from a string to a 4D vector! Value: {value}", exception);
			}
		}
	}
}
