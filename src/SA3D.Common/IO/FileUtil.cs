using Amicitia.IO.Binary;
using Amicitia.IO.Streams;
using System;
using System.IO;

namespace SA3D.Common.IO
{
	/// <summary>
	/// File helper class
	/// </summary>
	public static class FileUtil
	{
		/// <summary>
		/// Checks whether a file is formatted as a file.
		/// </summary>
		/// <param name="fileInfo">Info with which the file should be checked</param>
		public static bool CheckFile<T>(FileInfo fileInfo) where T : IFileSerializable, new()
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenRead(fileInfo.Filepath);
			return CheckStream<T>(stream, fileInfo);
		}

		/// <summary>
		/// Checks whether data is formatted as a file.
		/// </summary>
		/// <param name="data">The data to check.</param>
		/// <param name="fileInfo">Info with which the file should be checked</param>
		public static bool CheckBytes<T>(byte[] data, FileInfo fileInfo) where T : IFileSerializable, new()
		{
			using MemoryStream stream = new(data);
			return CheckStream<T>(stream, fileInfo);
		}

		/// <summary>
		/// Checks whether a stream is formatted as a model file
		/// </summary>
		/// <param name="stream">The stream to check</param>
		/// <param name="fileInfo">Info with which the file should be checked</param>
		/// <returns></returns>
		public static bool CheckStream<T>(Stream stream, FileInfo fileInfo) where T : IFileSerializable, new()
		{
			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, fileInfo.Endiannes ?? new T().DefaultFileEndianness, fileInfo.Encoding, fileInfo.Filepath);
			return reader.Check<T>(new(fileInfo.Filepath));
		}

		/// <summary>
		/// Checks whether the data at a binary readers current location can be read as <typeparamref name="T"/>
		/// </summary>
		/// <param name="reader">Reader to check</param>
		/// <param name="context">File context with which to check the data</param>
		/// <returns></returns>
		public static bool Check<T>(this BinaryObjectReader reader, FileContext context) where T : IFileSerializable, new()
		{
			using SeekToken at = reader.At();
			return new T().Check(reader, context);
		}


		/// <summary>
		/// Checks whether a file is formatted as a file.
		/// </summary>
		/// <param name="fileInfo">Info with which the file should be checked</param>
		/// <param name="context">Context with which to check the data</param>
		public static bool CheckFile<T, C>(FileInfo fileInfo, C context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenRead(fileInfo.Filepath);
			return CheckStream<T, C>(stream, fileInfo, context);
		}

		/// <summary>
		/// Checks whether data is formatted as a file.
		/// </summary>
		/// <param name="data">The data to check.</param>
		/// <param name="fileInfo">Info with which the file should be checked</param>
		/// <param name="context">Context with which to check the data</param>
		public static bool CheckBytes<T, C>(byte[] data, FileInfo fileInfo, C context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			using MemoryStream stream = new(data);
			return CheckStream<T, C>(stream, fileInfo, context);
		}

		/// <summary>
		/// Checks whether a stream is formatted as a model file
		/// </summary>
		/// <param name="stream">The stream to check</param>
		/// <param name="fileInfo">Info with which the file should be checked</param>
		/// <param name="context">Context with which to check the data</param>
		/// <returns></returns>
		public static bool CheckStream<T, C>(Stream stream, FileInfo fileInfo, C context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, fileInfo.Endiannes ?? new T().DefaultFileEndianness, fileInfo.Encoding, fileInfo.Filepath);
			return reader.Check<T, C>(new(fileInfo.Filepath, context));
		}

		/// <summary>
		/// Checks whether the data at a binary readers current location can be read as <typeparamref name="T"/>
		/// </summary>
		/// <param name="reader">Reader to check</param>
		/// <param name="context">File context with which to check the data</param>
		/// <returns></returns>
		public static bool Check<T, C>(this BinaryObjectReader reader, FileContext<C> context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			using SeekToken at = reader.At();
			return new T().Check(reader, context);
		}


		/// <summary>
		/// Reads a file.
		/// </summary>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <returns>The file that was read.</returns>
		public static T ReadFromFile<T>(FileInfo fileInfo) where T : IFileSerializable, new()
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenRead(fileInfo.Filepath);
			return ReadFromStream<T>(stream, fileInfo);
		}

		/// <summary>
		/// Reads a model file off byte data.
		/// </summary>
		/// <param name="data">Data to read.</param>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <returns>The model file that was read.</returns>
		public static T ReadFromBytes<T>(byte[] data, FileInfo fileInfo) where T : IFileSerializable, new()
		{
			using MemoryStream stream = new(data);
			return ReadFromStream<T>(stream, fileInfo);
		}

		/// <summary>
		/// Read a model file off a stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <returns></returns>
		public static T ReadFromStream<T>(Stream stream, FileInfo fileInfo) where T : IFileSerializable, new()
		{
			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, fileInfo.Endiannes ?? new T().DefaultFileEndianness, fileInfo.Encoding, fileInfo.Filepath);
			return reader.ReadObject<T, FileContext>(new(fileInfo.Filepath));
		}


		/// <summary>
		/// Reads a file.
		/// </summary>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <param name="context">IO Context to use</param>
		/// <returns>The file that was read.</returns>
		public static T ReadFromFile<T, C>(FileInfo fileInfo, C context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenRead(fileInfo.Filepath);
			return ReadFromStream<T, C>(stream, fileInfo, context);
		}

		/// <summary>
		/// Reads a model file off byte data.
		/// </summary>
		/// <param name="data">Data to read.</param>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <param name="context">IO Context to use</param>
		/// <returns>The model file that was read.</returns>
		public static T ReadFromBytes<T, C>(byte[] data, FileInfo fileInfo, C context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			using MemoryStream stream = new(data);
			return ReadFromStream<T, C>(stream, fileInfo, context);
		}

		/// <summary>
		/// Read a model file off a stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <param name="context">IO Context to use</param>
		/// <returns></returns>
		public static T ReadFromStream<T, C>(Stream stream, FileInfo fileInfo, C context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, fileInfo.Endiannes ?? new T().DefaultFileEndianness, fileInfo.Encoding, fileInfo.Filepath);
			return reader.ReadObject<T, FileContext<C>>(new(fileInfo.Filepath, context));
		}


		/// <summary>
		/// Write the file to a file.
		/// </summary>
		/// <param name="file">The file to write</param>
		/// <param name="fileInfo">Path to the file to write to.</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void WriteToFile<T>(this T file, FileInfo fileInfo) where T : IFileSerializable
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenWrite(fileInfo.Filepath);
			WriteToStream(file, fileInfo, stream);
		}

		/// <summary>
		/// Writes the file to a byte array.
		/// </summary>
		/// <param name="file">The file to write</param>
		/// <param name="fileInfo">Info with which the file should be written</param>
		/// <returns>The written byte data.</returns>
		public static byte[] WriteToBytes<T>(this T file, FileInfo fileInfo) where T : IFileSerializable
		{
			using MemoryStream stream = new();
			WriteToStream(file, fileInfo, stream);
			return stream.ToArray();
		}

		/// <summary>
		/// Writes the model file to a byte array.
		/// </summary>
		/// <param name="file">The file to write</param>
		/// <param name="stream">The stream to write to</param>
		/// <param name="fileInfo">Info with which the file should be written</param>
		public static void WriteToStream<T>(this T file, FileInfo fileInfo, Stream stream) where T : IFileSerializable
		{
			using BinaryObjectWriter writer = new(stream, StreamOwnership.Retain, fileInfo.Endiannes ?? file.DefaultFileEndianness, fileInfo.Encoding, fileInfo.Filepath);
			writer.OffsetFlushMode = OffsetFlushMode.Recursive;
			writer.WriteObject(file, new FileContext(fileInfo.Filepath));
		}


		/// <summary>
		/// Write the file to a file.
		/// </summary>
		/// <param name="file">The file to write</param>
		/// <param name="context">IO Context to use</param>
		/// <param name="fileInfo">Path to the file to write to.</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void WriteToFile<T, C>(this T file, FileInfo fileInfo, C context) where T : IFileSerializable<C> where C : unmanaged
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenWrite(fileInfo.Filepath);
			WriteToStream(file, fileInfo, context, stream);
		}

		/// <summary>
		/// Writes the file to a byte array.
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		/// <param name="file">The file to write</param>
		/// <param name="fileInfo">Info with which the file should be written</param>
		/// <param name="context">IO Context to use</param>
		/// <returns>The written byte data.</returns>
		public static byte[] WriteToBytes<T, C>(this T file, FileInfo fileInfo, C context) where T : IFileSerializable<C> where C : unmanaged
		{
			using MemoryStream stream = new();
			WriteToStream(file, fileInfo, context, stream);
			return stream.ToArray();
		}

		/// <summary>
		/// Writes the model file to a byte array.
		/// </summary>
		/// <param name="file">The file to write</param>
		/// <param name="fileInfo">Info with which the file should be written</param>
		/// <param name="context">IO Context to use</param>
		/// <param name="stream">The stream to write to</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void WriteToStream<T, C>(this T file, FileInfo fileInfo, C context, Stream stream) where T : IFileSerializable<C> where C : unmanaged
		{
			using BinaryObjectWriter writer = new(stream, StreamOwnership.Retain, fileInfo.Endiannes ?? file.DefaultFileEndianness, fileInfo.Encoding, fileInfo.Filepath);
			writer.OffsetFlushMode = OffsetFlushMode.Recursive;
			writer.WriteObject(file, new FileContext<C>(fileInfo.Filepath, context));
		}
	}
}
