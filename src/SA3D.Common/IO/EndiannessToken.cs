using Amicitia.IO.Binary;
using System;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Token to temporarily change the endianness of a <see cref="BinaryValueReader"/> or <see cref="BinaryValueWriter"/>
	/// </summary>
	public readonly struct EndiannessToken : IDisposable
	{
		private readonly BinaryValueReader? _reader;
		private readonly BinaryValueWriter? _writer;

		private readonly Endianness _endianness;
		private readonly Endianness _previousEndianness;

		/// <summary>
		/// Creates a new token for a <see cref="BinaryValueReader"/>
		/// </summary>
		/// <param name="reader">The reader to create the token for</param>
		/// <param name="endianness">The endianness to set</param>
		public EndiannessToken(BinaryValueReader reader, Endianness endianness)
		{
			_reader = reader;
			_endianness = endianness;
			_previousEndianness = reader.Endianness;
			reader.Endianness = _endianness;
		}

		/// <summary>
		/// Creates a new token for a <see cref="BinaryValueWriter"/>
		/// </summary>
		/// <param name="writer">The writer to create the token for</param>
		/// <param name="endianness">The endianness to set</param>
		public EndiannessToken(BinaryValueWriter writer, Endianness endianness)
		{
			_writer = writer;
			_endianness = endianness;
			_previousEndianness = writer.Endianness;
			writer.Endianness = _endianness;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_reader?.Endianness = _previousEndianness;
			_writer?.Endianness = _previousEndianness;
		}

		/// <summary>
		/// Retrieve the endianness of the token
		/// </summary>
		/// <param name="token">The token to receive the endianness of</param>
		public static explicit operator Endianness(EndiannessToken token)
		{
			return token._endianness;
		}
	}
}
