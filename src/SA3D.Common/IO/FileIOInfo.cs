using Amicitia.IO.Binary;
using System.Text;

namespace SA3D.Common.IO
{
	/// <summary>
	/// File read/write info
	/// </summary>
	public struct FileIOInfo
	{
		/// <summary>
		/// Filepath to the file being read/written
		/// </summary>
		public string? Filepath { get; set; }

		/// <summary>
		/// Encoding with which strings in the file should be read/written
		/// </summary>
		public Encoding? Encoding { get; set; }

		/// <summary>
		/// Endiannes with which the file should be read/written
		/// </summary>
		public Endianness? Endianness { get; set; }

		/// <summary>
		/// Offset origin
		/// </summary>
		public long? OffsetOrigin { get; set; }


		/// <summary>
		/// Initializes a new instance of the <see cref="FileIOInfo"/> structure
		/// </summary>
		public FileIOInfo() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FileIOInfo"/> structure with a specified filepath
		/// </summary>
		/// <param name="filepath">Filepath to the file being read/written</param>
		public FileIOInfo(string? filepath)
		{
			Filepath = filepath;
		}
	}
}
