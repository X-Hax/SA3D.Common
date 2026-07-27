using System;

namespace SA3D.Common.Ascii
{
	/// <summary>
	/// Ascii writer block token
	/// </summary>
	public readonly struct AsciiWriterBlockToken : IDisposable
	{
		private readonly AsciiWriter _writer;
		private readonly string? _prefix;
		private readonly int _endNewlineCount;

		internal AsciiWriterBlockToken(AsciiWriter writer, string? prefix = null, int endNewlineCount = 2)
		{
			_writer = writer;
			_prefix = prefix;
			_endNewlineCount = endNewlineCount;

			_writer.WriteLine($"{_prefix ?? string.Empty}START");
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_writer.WriteLine($"{_prefix ?? string.Empty}END", _endNewlineCount);
		}
	}
}
