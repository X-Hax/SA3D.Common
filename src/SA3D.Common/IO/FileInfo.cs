using Amicitia.IO.Binary;
using System.Text;

namespace SA3D.Common.IO
{
	/// <summary>
	/// File read/write info
	/// </summary>
	public struct FileInfo
	{
		/// <summary>
		/// Filepath to the file being read
		/// </summary>
		public string? Filepath { get; set; }

		/// <summary>
		/// Encoding with which strings in the file should be read
		/// </summary>
		public Encoding? Encoding { get; set; }

		/// <summary>
		/// Endiannes with which the file should be read
		/// </summary>
		public Endianness Endiannes { get; set; }
	}
}
