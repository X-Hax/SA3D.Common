using Amicitia.IO.Binary;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Base file interface
	/// </summary>
	public interface IFileSerializable : IBinarySerializable<FileContext>
	{
		/// <summary>
		/// Check whether the data behind a reader can be read as the file
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public bool Check(BinaryObjectReader reader);
	}

	/// <summary>
	/// Base file interface (with a context)
	/// </summary>
	public interface IFileSerializable<T> : IBinarySerializable<FileContext<T>>, IFileSerializable where T : unmanaged
	{
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
