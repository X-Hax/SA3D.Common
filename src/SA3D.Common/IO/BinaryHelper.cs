using Amicitia.IO.Binary;
using Amicitia.IO.Streams;
using System.IO;

namespace SA3D.Common.IO
{
	/// <summary>
	/// General purpose read/write extensions
	/// </summary>
	public static class BinaryHelper
	{
		/// <summary>
		/// Seek from <see cref="SeekOrigin.Begin"/>
		/// </summary>
		/// <param name="reader">The reader to seek for</param>
		/// <param name="position">The position to seek to</param>
		public static void SeekOffset(this BinaryValueReader reader, long position)
		{
			reader.Seek(position, SeekOrigin.Begin);
		}

		/// <summary>
		/// Create a <see cref="SeekToken"/> to the current position
		/// </summary>
		/// <param name="reader">The reader to create the token for</param>
		/// <returns></returns>
		public static SeekToken At(this BinaryValueReader reader)
		{
			return reader.At(reader.Position, SeekOrigin.Begin);
		}

		/// <summary>
		/// Creates a new <see cref="EndiannessToken"/> for a <see cref="BinaryValueReader"/>
		/// </summary>
		/// <param name="reader">The reader to create the token for</param>
		/// <param name="endianness">The endianness to set</param>
		/// <returns></returns>
		public static EndiannessToken DisposableEndian(this BinaryValueReader reader, Endianness endianness)
		{
			return new(reader, endianness);
		}

		/// <summary>
		/// Seek from <see cref="SeekOrigin.Begin"/>
		/// </summary>
		/// <param name="writer">The writer to seek for</param>
		/// <param name="position">The position to seek to</param>
		public static void SeekOffset(this BinaryValueWriter writer, long position)
		{
			writer.Seek(position, SeekOrigin.Begin);
		}

		/// <summary>
		/// Create a <see cref="SeekToken"/> to the current position
		/// </summary>
		/// <param name="writer">The writer to create the token for</param>
		/// <returns></returns>
		public static SeekToken At(this BinaryValueWriter writer)
		{
			return writer.At(writer.Position, SeekOrigin.Begin);
		}

		/// <summary>
		/// Creates a new <see cref="EndiannessToken"/> for a <see cref="BinaryValueWriter"/>
		/// </summary>
		/// <param name="writer">The writer to create the token for</param>
		/// <param name="endianness">The endianness to set</param>
		/// <returns></returns>
		public static EndiannessToken DisposableEndian(this BinaryValueWriter writer, Endianness endianness)
		{
			return new(writer, endianness);
		}
	}
}
