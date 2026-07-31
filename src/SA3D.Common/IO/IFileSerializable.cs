using Amicitia.IO.Binary;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Base file interface
	/// </summary>
	public interface IFileSerializable : IBinarySerializable<FileContext>
	{
		/// <summary>
		/// Default endianness to use when writing the file
		/// </summary>
		public Endianness DefaultFileEndianness => Endianness.Little;

		/// <summary>
		/// Check whether the data behind a reader can be read as the file
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="context">file context to check with</param>
		/// <returns></returns>
		public bool Check(BinaryObjectReader reader, FileContext context);
	}

	/// <summary>
	/// Base file interface (with a context)
	/// </summary>
	public interface IFileSerializable<T> : IBinarySerializable<FileContext<T>>, IFileSerializable where T : unmanaged
	{
		/// <summary>
		/// Check whether the data behind a reader can be read as the file
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="context">file context to check with</param>
		/// <returns></returns>
		public bool Check(BinaryObjectReader reader, FileContext<T> context);

		bool IFileSerializable.Check(BinaryObjectReader reader, FileContext context)
		{
			return Check(reader, new FileContext<T>()
			{
				Filepath = context.Filepath
			});
		}


		void IBinarySerializable.Read(BinaryObjectReader reader)
		{
			Read(reader, default(FileContext<T>));
		}

		void IBinarySerializable.Write(BinaryObjectWriter writer)
		{
			Write(writer, default(FileContext<T>));
		}


		void IBinarySerializable<FileContext>.Read(BinaryObjectReader reader, FileContext context)
		{
			Read(reader, new FileContext<T>()
			{
				Filepath = context.Filepath
			});
		}

		void IBinarySerializable<FileContext>.Write(BinaryObjectWriter writer, FileContext context)
		{
			Write(writer, new FileContext<T>()
			{
				Filepath = context.Filepath
			});
		}
	}
}
