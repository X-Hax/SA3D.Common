using Amicitia.IO.Binary;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Base file interface
	/// </summary>
	public interface IFileSerializable : IBinarySerializable
	{
		/// <summary>
		/// Check whether the data behind a reader can be read as the file, and adjusts missing information in <paramref name="fileInfo"/> accordingly
		/// </summary>
		/// <param name="reader">Reader to check</param>
		/// <param name="fileInfo">Info that the file will be read with</param>
		/// <returns></returns>
		public bool CheckCanReadFile(BinaryObjectReader reader, ref FileIOInfo fileInfo)
		{
			return true;
		}

		/// <summary>
		/// Reads object information in a file context
		/// </summary>
		/// <param name="fileReader">The reader to read from</param>
		/// <param name="fileInfo">The file context being read with</param>
		public void ReadFile(BinaryObjectReader fileReader, FileIOInfo fileInfo)
		{
			IFileSerializable dst = this;
			fileReader.ReadObject(ref dst);
		}

		/// <summary>
		/// Checks whether the data can read to a file, and adjusts missing information in <paramref name="fileInfo"/> accordingly
		/// </summary>
		/// <param name="fileInfo">Info that the file will be written with</param>
		/// <returns></returns>
		public bool CheckCanWriteFile(ref FileIOInfo fileInfo)
		{
			return true;
		}

		/// <summary>
		/// Writes object information in a file context
		/// </summary>
		/// <param name="fileWriter">The writer to write to</param>
		/// <param name="fileInfo">The file context to write with</param>
		public void WriteFile(BinaryObjectWriter fileWriter, FileIOInfo fileInfo)
		{
			fileWriter.WriteObject(this);
		}
	}

	/// <summary>
	/// Base file interface (with a context)
	/// </summary>
	public interface IFileSerializable<T> : IBinarySerializable<T>
	{
		/// <summary>
		/// Check whether the data behind a reader can be read as the file, and adjusts missing information in <paramref name="fileInfo"/> accordingly
		/// </summary>
		/// <param name="reader">Reader to check</param>
		/// <param name="context">Context to be read with</param>
		/// <param name="fileInfo">Info that the file will be read with</param>
		/// <returns></returns>
		public bool CheckCanReadFile(BinaryObjectReader reader, T context, ref FileIOInfo fileInfo)
		{
			return true;
		}

		/// <summary>
		/// Reads object information in a file context
		/// </summary>
		/// <param name="fileReader">The reader to read from</param>
		/// <param name="context">Context to read with</param>
		/// <param name="fileInfo">The file context being read with</param>
		public void ReadFile(BinaryObjectReader fileReader, T context, FileIOInfo fileInfo)
		{
			IFileSerializable<T> dst = this;
			fileReader.ReadObject(ref dst, context);
		}

		/// <summary>
		/// Checks whether the data can read to a file, and adjusts missing information in <paramref name="fileInfo"/> accordingly
		/// </summary>
		/// <param name="context">Context to be written with</param>
		/// <param name="fileInfo">Info that the file will be written with</param>
		/// <returns></returns>
		public bool CheckCanWriteFile(T context, ref FileIOInfo fileInfo)
		{
			return true;
		}

		/// <summary>
		/// Writes object information in a file context
		/// </summary>
		/// <param name="fileWriter">The writer to write to</param>
		/// <param name="context">Context to write with</param>
		/// <param name="fileInfo">The file context to write with</param>
		public void WriteFile(BinaryObjectWriter fileWriter, T context, FileIOInfo fileInfo)
		{
			fileWriter.WriteObject(this, context);
		}
	}
}
