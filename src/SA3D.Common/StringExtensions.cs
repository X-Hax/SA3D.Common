using System;
using System.Globalization;
using System.Text;

namespace SA3D.Common
{
    /// <summary>
    /// String helper methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Global random generator for <see cref="GenerateIdentifier"/>
        /// </summary>
        private static readonly Random _rand = new();

        /// <summary>
        /// Converts a signed short to a C Hexadecimal constant.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>A C hexadecimal constant.</returns>
        public static string ToCHex(this short number)
        {
            if(number is > (-1) and < 10)
            {
                return number.ToString(NumberFormatInfo.InvariantInfo);
            }

            return $"0x{number:X}";
        }

        /// <summary>
        /// Converts an unsigned short to a C Hexadecimal constant.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>A C hexadecimal constant.</returns>
        public static string ToCHex(this ushort number)
        {
            if(number < 10)
            {
                return number.ToString(NumberFormatInfo.InvariantInfo);
            }

            return $"0x{number:X}u";
        }

        /// <summary>
        /// Converts a signed integer to a C Hexadecimal constant.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>A C hexadecimal constant.</returns>
        public static string ToCHex(this int number)
        {
            if(number is > (-1) and < 10)
            {
                return number.ToString(NumberFormatInfo.InvariantInfo);
            }

            return $"0x{number:X}";
        }

        /// <summary>
        /// Converts an unsigned integer to a C Hexadecimal constant.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>A C hexadecimal constant.</returns>
        public static string ToCHex(this uint number)
        {
            if(number < 10)
            {
                return number.ToString(NumberFormatInfo.InvariantInfo);
            }

            return $"0x{number:X}u";
        }

        /// <summary>
        /// Converts a signed long to a C Hexadecimal constant.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>A C hexadecimal constant.</returns>
        public static string ToCHex(this long number)
        {
            if(number is > (-1) and < 10)
            {
                return number.ToString(NumberFormatInfo.InvariantInfo);
            }

            return $"0x{number:X}";
        }

        /// <summary>
        /// Converts an unsigned long to a C Hexadecimal constant.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>A C hexadecimal constant.</returns>
        public static string ToCHex(this ulong number)
        {
            if(number < 10)
            {
                return number.ToString(NumberFormatInfo.InvariantInfo);
            }

            return $"0x{number:X}u";
        }

        /// <summary>
        /// Converts a string to an escaped C string.
        /// </summary>
        /// <param name="input">The text to convert</param>
        /// <returns>A C escaped string</returns>
        public static string ToC(this string? input)
        {
            if(input == null)
            {
                return "NULL";
            }

            Encoding enc = Encoding.GetEncoding(932);
            StringBuilder result = new("\"");
            foreach(char item in input)
            {
                switch(item)
                {
                    case '\0':
                        result.Append(@"\0");
                        break;
                    case '\a':
                        result.Append(@"\a");
                        break;
                    case '\b':
                        result.Append(@"\b");
                        break;
                    case '\f':
                        result.Append(@"\f");
                        break;
                    case '\n':
                        result.Append(@"\n");
                        break;
                    case '\r':
                        result.Append(@"\r");
                        break;
                    case '\t':
                        result.Append(@"\t");
                        break;
                    case '\v':
                        result.Append(@"\v");
                        break;
                    case '"':
                        result.Append(@"\""");
                        break;
                    case '\\':
                        result.Append(@"\\");
                        break;
                    default:
                        if(item < ' ')
                        {
                            result.AppendFormat(@"\{0}", Convert.ToString((short)item, 8).PadLeft(3, '0'));
                        }
                        else if(item > '\x7F')
                        {
                            foreach(byte b in enc.GetBytes(item.ToString()))
                            {
                                result.AppendFormat(@"\{0}", Convert.ToString(b, 8).PadLeft(3, '0'));
                            }
                        }
                        else
                        {
                            result.Append(item);
                        }

                        break;
                }
            }

            result.Append("\"");
            return result.ToString();
        }

        /// <summary>
        /// Converts a floating point number to a C constant.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>A C floating point constant.</returns>
        public static string ToC(this float number)
        {
            string result = number.ToLongString();
            if(result.Contains('.'))
            {
                result += "f";
            }

            return result;
        }

        /// <summary>
        /// Converts a floating point number to a long floating point expression.
        /// <br/> E.g.: 4.1231256e-10
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>The number as a long floating point expression.</returns>
        public static string ToLongString(this float number)
        {
            string str = number.ToString(NumberFormatInfo.InvariantInfo);
            // if string representation was collapsed from scientific notation, just return it: 
            if(!str.Contains('E') & !str.Contains('e'))
            {
                return str;
            }

            str = str.ToUpper();
            char decSeparator = '.';
            string[] exponentParts = str.Split('E');
            string[] decimalParts = exponentParts[0].Split(decSeparator);
            // fix missing decimal point: 
            if(decimalParts.Length == 1)
            {
                decimalParts = new[]
                {
                    exponentParts[0],
                    "0"
                };
            }

            int exponentValue = int.Parse(exponentParts[1]);
            string newNumber = decimalParts[0] + decimalParts[1];
            string result;
            if(exponentValue > 0)
            {
                result = newNumber + GetZeros(exponentValue - decimalParts[1].Length);
            }
            else
            {
                // negative exponent 
                result = string.Empty;
                if(newNumber.StartsWith("-"))
                {
                    result = "-";
                    newNumber = newNumber[1..];
                }

                result += "0" + decSeparator + GetZeros(exponentValue + decimalParts[0].Length) + newNumber;
                result = result.TrimEnd('0');
            }

            return result;
        }

        /// <summary>
        /// Returns a string containg N zeros.
        /// </summary>
        /// <param name="zeroCount">Number of zeros to return.</param>
        /// <returns>The zero string.</returns>
        public static string GetZeros(int zeroCount)
        {
            if(zeroCount < 0)
            {
                zeroCount = Math.Abs(zeroCount);

            }

            return new string('0', zeroCount);
        }

        /// <summary>
        /// Converts a string to a C variable identifier.
        /// </summary>
        /// <param name="input">String to convert.</param>
        /// <returns>The converted identifier.</returns>
        public static string MakeIdentifier(this string input)
        {
            StringBuilder result = new(input.Length + 1);
            foreach(char item in input)
            {
                if(char.IsAsciiLetter(item) && char.IsAsciiDigit(item))
                {
                    result.Append(item);
                }
            }

            if(result[0] >= '0' & result[0] <= '9')
            {
                result.Insert(0, '_');
            }

            return result.ToString();
        }

        /// <summary>
        /// Generates a unique, random hexadecimal identifier string consisting of the current Tick number and a random number between 0 and 65536.
        /// </summary>
        /// <returns>The unique identifier</returns>
        public static string GenerateIdentifier()
        {
            return DateTime.Now.Ticks.ToString("X") + _rand.Next(0, 65536).ToString("X4");
        }
    }
}
