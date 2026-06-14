using Amicitia.IO.Binary;
using System;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Token to temporarily change the offset binary format of a <see cref="BinaryObjectReader"/> or <see cref="BinaryObjectWriter"/>
	/// </summary>
	public readonly struct OffsetBinaryFormatToken : IDisposable
	{
		private readonly BinaryObjectReader? _reader;
		private readonly BinaryObjectWriter? _writer;

		private readonly OffsetBinaryFormat _format;
		private readonly OffsetBinaryFormat _previousFormat;

		/// <summary>
		/// Creates a new token for a <see cref="BinaryObjectReader"/>
		/// </summary>
		/// <param name="reader">The reader to create the token for</param>
		/// <param name="format">The format to set</param>
		public OffsetBinaryFormatToken(BinaryObjectReader reader, OffsetBinaryFormat format)
		{
			_reader = reader;
			_format = format;
			_previousFormat = reader.OffsetBinaryFormat;
			reader.OffsetBinaryFormat = format;
		}

		/// <summary>
		/// Creates a new token for a <see cref="BinaryObjectWriter"/>
		/// </summary>
		/// <param name="writer">The writer to create the token for</param>
		/// <param name="format">The format to set</param>
		public OffsetBinaryFormatToken(BinaryObjectWriter writer, OffsetBinaryFormat format)
		{
			_writer = writer;
			_format = format;
			_previousFormat = writer.OffsetBinaryFormat;
			writer.OffsetBinaryFormat = format;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_reader?.OffsetBinaryFormat = _previousFormat;
			_writer?.OffsetBinaryFormat = _previousFormat;
		}

		/// <summary>
		/// Retrieve the format of the token
		/// </summary>
		/// <param name="token">The token to receive the format of</param>
		public static explicit operator OffsetBinaryFormat(OffsetBinaryFormatToken token)
		{
			return token._format;
		}
	}
}
