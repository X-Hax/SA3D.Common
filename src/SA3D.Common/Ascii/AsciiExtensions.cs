using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace SA3D.Common.Ascii
{
	/// <summary>
	/// Ascii utility methods
	/// </summary>
	public static class AsciiExtensions
	{
		/// <summary>
		/// Converts a floating point number to an ascii value
		/// </summary>
		/// <param name="value">The value to convert</param>
		public static string ToAscii(this float value)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0:F6}F", value).PadLeft(10);
		}

		/// <summary>
		/// Converts a 2-component vector to an ascii value (without parantheses)
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns></returns>
		public static string ToAscii(this Vector2 value)
		{
			return $"{value.X.ToAscii()}, {value.Y.ToAscii()}";
		}

		/// <summary>
		/// Converts a 3-component vector to an ascii value (without parantheses)
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns></returns>
		public static string ToAscii(this Vector3 value)
		{
			return $"{value.X.ToAscii()}, {value.Y.ToAscii()}, {value.Z.ToAscii()}";
		}

		/// <summary>
		/// Converts a floating point number to an ascii BAMS value
		/// </summary>
		/// <param name="value">The value to convert</param>
		public static string ToAsciiDegrees(this float value)
		{
			return MathHelper.RadToDeg(value).ToAscii();
		}

		/// <summary>
		/// Converts a 3-component vector to an ascii BAMS value (without parantheses)
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns></returns>
		public static string ToAsciiDegrees(this Vector3 value)
		{
			return $"{value.X.ToAsciiDegrees()}, {value.Y.ToAsciiDegrees()}, {value.Z.ToAsciiDegrees()}";
		}

		/// <summary>
		/// Converts an unsigned integer to a hexadecimal value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToAsciiHex(this uint value)
		{
			return $"0x{value:x8}";
		}

		/// <summary>
		/// Converts a float value to an ascii hexadecimal representation
		/// </summary>
		/// <param name="value">Thje value to convert</param>
		/// <returns></returns>
		public static unsafe string ToAsciiHex(this float value)
		{
			return ToAsciiHex(*(uint*)&value);
		}

		/// <summary>
		/// Converts a 2-component vector to an ascii hexadecimal representation (without parantheses)
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns></returns>
		public static string ToAsciiHex(this Vector2 value)
		{
			return $"{value.X.ToAsciiHex()}, {value.Y.ToAsciiHex()}";
		}

		/// <summary>
		/// Converts a 3-component vector to an ascii hexadecimal representation (without parantheses)
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns></returns>
		public static string ToAsciiHex(this Vector3 value)
		{
			return $"{value.X.ToAsciiHex()}, {value.Y.ToAsciiHex()}, {value.Z.ToAsciiHex()}";
		}

		/// <summary>
		/// Converts a flag value to an ascii value
		/// </summary>
		/// <typeparam name="T">Type of the flag</typeparam>
		/// <param name="lut">Lookup table for the individual string-flag values</param>
		/// <param name="value">The value to convert</param>
		/// <returns></returns>
		public static string ToAscii<T>(this T value, IDictionary<string, T> lut) where T : struct, Enum
		{
			string result = string.Empty;
			foreach(KeyValuePair<string, T> item in lut)
			{
				if(value.HasFlag(item.Value))
				{
					if(result.Length > 0)
					{
						result += '|';
					}

					result += item.Key;
				}
			}

			if(result == string.Empty)
			{
				result = "0x0";
			}

			return result;
		}

	}
}
