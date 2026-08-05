using Amicitia.IO.Binary;
using Amicitia.IO.Binary.Extensions;
using Amicitia.IO.Streams;
using System;
using System.IO;
using System.Reflection.PortableExecutable;

namespace SA3D.Common.IO
{
	/// <summary>
	/// File helper class
	/// </summary>
	public static class FileUtil
	{
		#region Check

		/// <summary>
		/// Checks whether a file can be deserialized as the given type
		/// </summary>
		/// <param name="filepath">Path to the file to read</param>
		public static bool CheckCanReadFile<T>(this string filepath) where T : IFileSerializable, new()
		{
			FileIOInfo info = new(filepath);
			return CheckCanReadFile<T>(ref info);
		}

		/// <summary>
		/// Checks whether a file can be deserialized as the given type
		/// </summary>
		/// <param name="fileInfo">Info which the file would be read with</param>
		public static bool CheckCanReadFile<T>(this ref FileIOInfo fileInfo) where T : IFileSerializable, new()
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenRead(fileInfo.Filepath);
			return CheckCanReadFile<T>(stream, ref fileInfo);
		}

		/// <summary>
		/// Checks whether a byte array can be deserialized as the given type
		/// </summary>
		/// <param name="data">Data to check</param>
		/// <param name="fileInfo">Info which the file would be read with</param>
		public static bool CheckCanReadFile<T>(this byte[] data, ref FileIOInfo fileInfo) where T : IFileSerializable, new()
		{
			using MemoryStream stream = new(data);
			return CheckCanReadFile<T>(stream, ref fileInfo);
		}

		/// <summary>
		/// Checks whether a stream can be deserialized as the given type
		/// </summary>
		/// <param name="stream">Stream to check</param>
		/// <param name="fileInfo">Info which the file would be read with</param>
		public static bool CheckCanReadFile<T>(this Stream stream, ref FileIOInfo fileInfo) where T : IFileSerializable, new()
		{
			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, Endianness.Little, fileInfo.Encoding, fileInfo.Filepath);

			if(fileInfo.OffsetOrigin != null)
			{
				reader.OffsetHandler.PushOffsetOrigin(fileInfo.OffsetOrigin.Value);
			}

			return reader.CheckCanReadFile<T>(ref fileInfo);
		}

		/// <summary>
		/// Checks whether the data at a binary readers current location can be read as <typeparamref name="T"/>
		/// </summary>
		/// <param name="reader">Reader to check</param>
		/// <param name="fileInfo">Info which the file would be read with</param>
		/// <returns></returns>
		public static bool CheckCanReadFile<T>(this BinaryObjectReader reader, ref FileIOInfo fileInfo) where T : IFileSerializable, new()
		{
			using SeekToken at = reader.At();
			return new T().CheckCanReadFile(reader, ref fileInfo);
		}


		/// <summary>
		/// Checks whether a file can be deserialized as the given type
		/// </summary>
		/// <param name="filepath">Path to the file to read</param>
		/// <param name="context">Context that will be read with</param>
		public static bool CheckCanReadFile<T, C>(this string filepath, C context) where T : IFileSerializable<C>, new()
		{
			FileIOInfo info = new(filepath);
			return CheckCanReadFile<T, C>(ref info, context);
		}

		/// <summary>
		/// Checks whether a file can be deserialized as the given type
		/// </summary>
		/// <param name="context">Context that will be read with</param>
		/// <param name="fileInfo">Info which the file would be read with</param>
		public static bool CheckCanReadFile<T, C>(this ref FileIOInfo fileInfo, C context) where T : IFileSerializable<C>, new()
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenRead(fileInfo.Filepath);
			return CheckCanReadFile<T, C>(stream, context, ref fileInfo);
		}

		/// <summary>
		/// Checks whether a byte array can be deserialized as the given type
		/// </summary>
		/// <param name="data">Data to check</param>
		/// <param name="context">Context that will be read with</param>
		/// <param name="fileInfo">Info which the file would be read with</param>
		public static bool CheckCanReadFile<T, C>(this byte[] data, C context, ref FileIOInfo fileInfo) where T : IFileSerializable<C>, new()
		{
			using MemoryStream stream = new(data);
			return CheckCanReadFile<T, C>(stream, context, ref fileInfo);
		}

		/// <summary>
		/// Checks whether a stream can be deserialized as the given type
		/// </summary>
		/// <param name="stream">Stream to check</param>
		/// <param name="context">Context that will be read with</param>
		/// <param name="fileInfo">Info which the file would be read with</param>
		public static bool CheckCanReadFile<T, C>(this Stream stream, C context, ref FileIOInfo fileInfo) where T : IFileSerializable<C>, new()
		{
			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, Endianness.Little, fileInfo.Encoding, fileInfo.Filepath);

			if(fileInfo.OffsetOrigin != null)
			{
				reader.OffsetHandler.PushOffsetOrigin(fileInfo.OffsetOrigin.Value);
			}

			return reader.CheckCanReadFile<T, C>(context, ref fileInfo);
		}

		/// <summary>
		/// Checks whether the data at a binary readers current location can be read as <typeparamref name="T"/>
		/// </summary>
		/// <param name="reader">Reader to check</param>
		/// <param name="context">Context that will be read with</param>
		/// <param name="fileInfo">Info which the file would be read with</param>
		/// <returns></returns>
		public static bool CheckCanReadFile<T, C>(this BinaryObjectReader reader, C context, ref FileIOInfo fileInfo) where T : IFileSerializable<C>, new()
		{
			using SeekToken at = reader.At();
			return new T().CheckCanReadFile(reader, context, ref fileInfo);
		}

		#endregion

		#region read

		/// <summary>
		/// Deserializes a file to the given type
		/// </summary>
		/// <param name="filepath">Path to the file that should be read</param>
		/// <returns>The file that was read.</returns>
		public static T ReadFile<T>(this string filepath) where T : IFileSerializable, new()
		{
			return ReadFile<T>(new FileIOInfo(filepath));
		}

		/// <summary>
		/// Deserializes a file to the given type
		/// </summary>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <returns>The file that was read.</returns>
		public static T ReadFile<T>(this FileIOInfo fileInfo) where T : IFileSerializable, new()
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenRead(fileInfo.Filepath);
			return ReadFile<T>(stream, fileInfo);
		}

		/// <summary>
		/// Deserialized a file off byte data
		/// </summary>
		/// <param name="data">Data to read.</param>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <returns>The model file that was read.</returns>
		public static T ReadFile<T>(this byte[] data, FileIOInfo fileInfo = default) where T : IFileSerializable, new()
		{
			using MemoryStream stream = new(data);
			return ReadFile<T>(stream, fileInfo);
		}

		/// <summary>
		/// Deserialized a file off a stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <returns></returns>
		public static T ReadFile<T>(this Stream stream, FileIOInfo fileInfo = default) where T : IFileSerializable, new()
		{
			if(!CheckCanReadFile<T>(stream, ref fileInfo))
			{
				throw new CannotReadFileException(typeof(T), $"Supplied data cannot be deserialized to the type {typeof(T).Name}!");
			}

			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, fileInfo.Endianness ?? Endianness.Little, fileInfo.Encoding, fileInfo.Filepath);
			using OffsetOriginToken? offsetOriginToken = fileInfo.OffsetOrigin != null ? reader.WithOffsetOrigin(fileInfo.OffsetOrigin.Value) : null;

			T result = new();
			result.ReadFile(reader, fileInfo);
			return result;
		}

		/// <summary>
		/// Deserializes file data
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <returns></returns>
		public static T ReadFile<T>(this BinaryObjectReader reader, FileIOInfo fileInfo = default) where T : IFileSerializable, new()
		{
			if(!CheckCanReadFile<T>(reader, ref fileInfo))
			{
				throw new CannotReadFileException(typeof(T), $"Supplied data cannot be deserialized to the type {typeof(T).Name}!");
			}

			using EndiannessToken? endiannessToken = fileInfo.Endianness != null ? reader.WithEndian(fileInfo.Endianness.Value) : null;
			using OffsetOriginToken? offsetOriginToken = fileInfo.OffsetOrigin != null ? reader.WithOffsetOrigin(fileInfo.OffsetOrigin.Value) : null;

			T result = new();
			result.ReadFile(reader, fileInfo);
			return result;
		}


		/// <summary>
		/// Deserializes a file to the given type
		/// </summary>
		/// <param name="filepath">Path to the file that should be read</param>
		/// <param name="context">Context to read with</param>
		/// <returns>The file that was read.</returns>
		public static T ReadFile<T, C>(this string filepath, C context) where T : IFileSerializable<C>, new()
		{
			return ReadFile<T, C>(new FileIOInfo(filepath), context);
		}

		/// <summary>
		/// Deserializes a file to the given type
		/// </summary>
		/// <param name="context">Context to read with</param>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <returns>The file that was read.</returns>
		public static T ReadFile<T, C>(this FileIOInfo fileInfo, C context) where T : IFileSerializable<C>, new()
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenRead(fileInfo.Filepath);
			return ReadFile<T, C>(stream, context, fileInfo);
		}

		/// <summary>
		/// Deserialized a file off byte data
		/// </summary>
		/// <param name="data">Data to read.</param>
		/// <param name="context">Context to read with</param>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <returns>The model file that was read.</returns>
		public static T ReadFile<T, C>(this byte[] data, C context, FileIOInfo fileInfo = default) where T : IFileSerializable<C>, new()
		{
			using MemoryStream stream = new(data);
			return ReadFile<T, C>(stream, context, fileInfo);
		}

		/// <summary>
		/// Deserialized a file off a stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="context">Context to read with</param>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <returns></returns>
		public static T ReadFile<T, C>(this Stream stream, C context, FileIOInfo fileInfo = default) where T : IFileSerializable<C>, new()
		{
			if(!CheckCanReadFile<T, C>(stream, context, ref fileInfo))
			{
				throw new CannotReadFileException(typeof(T), $"Supplied data cannot be deserialized to the type {typeof(T).Name}!");
			}

			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, fileInfo.Endianness ?? Endianness.Little, fileInfo.Encoding, fileInfo.Filepath);
			using OffsetOriginToken? offsetOriginToken = fileInfo.OffsetOrigin != null ? reader.WithOffsetOrigin(fileInfo.OffsetOrigin.Value) : null;

			T result = new();
			result.ReadFile(reader, context, fileInfo);
			return result;
		}

		/// <summary>
		/// Deserializes file data
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		/// <param name="context">Context to read with</param>
		/// <param name="fileInfo">Info with which the file should be read</param>
		/// <returns></returns>
		public static T ReadFile<T, C>(this BinaryObjectReader reader, C context, FileIOInfo fileInfo = default) where T : IFileSerializable<C>, new()
		{
			if(!CheckCanReadFile<T, C>(reader, context, ref fileInfo))
			{
				throw new CannotReadFileException(typeof(T), $"Supplied data cannot be deserialized to the type {typeof(T).Name}!");
			}

			using EndiannessToken? endiannessToken = fileInfo.Endianness != null ? reader.WithEndian(fileInfo.Endianness.Value) : null;
			using OffsetOriginToken? offsetOriginToken = fileInfo.OffsetOrigin != null ? reader.WithOffsetOrigin(fileInfo.OffsetOrigin.Value) : null;

			T result = new();
			result.ReadFile(reader, context, fileInfo);
			return result;
		}

		#endregion

		#region Write

		/// <summary>
		/// Serialize file data to a file.
		/// </summary>
		/// <param name="file">The file data to write</param>
		/// <param name="filepath">Path to the file to write to</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void WriteToFile<T>(this T file, string filepath) where T : IFileSerializable
		{
			WriteToFile(file, new FileIOInfo(filepath));
		}

		/// <summary>
		/// Serialize file data to a file.
		/// </summary>
		/// <param name="file">The file data to write</param>
		/// <param name="fileInfo">Information of the file to write to</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void WriteToFile<T>(this T file, FileIOInfo fileInfo) where T : IFileSerializable
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenWrite(fileInfo.Filepath);
			WriteToStream(file, stream, fileInfo);
		}

		/// <summary>
		/// Serialize file data to a byte array.
		/// </summary>
		/// <param name="file">The file data to write</param>
		/// <param name="fileInfo">Info for the file being written to</param>
		public static byte[] WriteToBytes<T>(this T file, FileIOInfo fileInfo = default) where T : IFileSerializable
		{
			using MemoryStream stream = new();
			WriteToStream(file, stream, fileInfo);
			return stream.ToArray();
		}

		/// <summary>
		/// Serialize file data to a stream.
		/// </summary>
		/// <param name="file">The file data to write</param>
		/// <param name="stream">The stream to write to</param>
		/// <param name="fileInfo">Info for the file being written to</param>
		public static void WriteToStream<T>(this T file, Stream stream, FileIOInfo fileInfo = default) where T : IFileSerializable
		{
			if(!file.CheckCanWriteFile(ref fileInfo))
			{
				throw new CannotWriteFileException(typeof(T), "The file cannot be serialized!");
			}

			using BinaryObjectWriter writer = new(stream, StreamOwnership.Retain, fileInfo.Endianness ?? Endianness.Little, fileInfo.Encoding, fileInfo.Filepath);
			using OffsetOriginToken? offsetOriginToken = fileInfo.OffsetOrigin != null ? writer.WithOffsetOrigin(fileInfo.OffsetOrigin.Value) : null;
			writer.OffsetFlushMode = OffsetFlushMode.Recursive;

			file.WriteFile(writer, fileInfo);
			writer.Flush();
		}

		/// <summary>
		/// Serialize file data to a writer.
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="file">The file data to write</param>
		/// <param name="fileInfo">Info for the file being written to</param>
		public static void WriteFile<T>(this BinaryObjectWriter writer, T file, FileIOInfo fileInfo = default) where T : IFileSerializable
		{
			file.WriteFileToWriter(writer, fileInfo);
		}

		/// <summary>
		/// Serialize file data to a writer.
		/// </summary>
		/// <param name="file">The file data to write</param>
		/// <param name="writer">The writer to write to</param>
		/// <param name="fileInfo">Info for the file being written to</param>
		public static void WriteFileToWriter<T>(this T file, BinaryObjectWriter writer, FileIOInfo fileInfo = default) where T : IFileSerializable
		{
			if(!file.CheckCanWriteFile(ref fileInfo))
			{
				throw new CannotWriteFileException(typeof(T), "The file cannot be serialized!");
			}

			using EndiannessToken? endiannessToken = fileInfo.Endianness != null ? writer.WithEndian(fileInfo.Endianness.Value) : null;
			using OffsetOriginToken? offsetOriginToken = fileInfo.OffsetOrigin != null ? writer.WithOffsetOrigin(fileInfo.OffsetOrigin.Value) : null;

			file.WriteFile(writer, fileInfo);
			writer.Flush();
		}


		/// <summary>
		/// Serialize file data to a file.
		/// </summary>
		/// <param name="file">The file data to write</param>
		/// <param name="context">Context to write with</param>
		/// <param name="filepath">Path to the file to write to</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void WriteToFile<T, C>(this T file, C context, string filepath) where T : IFileSerializable<C>
		{
			WriteToFile(file, context, new FileIOInfo(filepath));
		}

		/// <summary>
		/// Serialize file data to a file.
		/// </summary>
		/// <param name="file">The file data to write</param>
		/// <param name="context">Context to write with</param>
		/// <param name="fileInfo">Information of the file to write to</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void WriteToFile<T, C>(this T file, C context, FileIOInfo fileInfo) where T : IFileSerializable<C>
		{
			if(string.IsNullOrWhiteSpace(fileInfo.Filepath))
			{
				throw new ArgumentException("No filepath specified!");
			}

			using FileStream stream = File.OpenWrite(fileInfo.Filepath);
			WriteFileToStream(file, context, stream, fileInfo);
		}

		/// <summary>
		/// Serialize file data to a byte array.
		/// </summary>
		/// <param name="file">The file data to write</param>
		/// <param name="context">Context to write with</param>
		/// <param name="fileInfo">Info for the file being written to</param>
		public static byte[] WriteFileToBytes<T, C>(this T file, C context, FileIOInfo fileInfo = default) where T : IFileSerializable<C>
		{
			using MemoryStream stream = new();
			WriteFileToStream(file, context, stream, fileInfo);
			return stream.ToArray();
		}

		/// <summary>
		/// Serialize file data to a stream.
		/// </summary>
		/// <param name="file">The file data to write</param>
		/// <param name="context">Context to write with</param>
		/// <param name="fileInfo">Info for the file being written to</param>
		/// <param name="stream">The stream to write to</param>
		public static void WriteFileToStream<T, C>(this T file, C context, Stream stream, FileIOInfo fileInfo = default) where T : IFileSerializable<C>
		{
			if(!file.CheckCanWriteFile(context, ref fileInfo))
			{
				throw new CannotWriteFileException(typeof(T), "The file cannot be serialized!");
			}

			using BinaryObjectWriter writer = new(stream, StreamOwnership.Retain, fileInfo.Endianness ?? Endianness.Little, fileInfo.Encoding, fileInfo.Filepath);
			using OffsetOriginToken? offsetOriginToken = fileInfo.OffsetOrigin != null ? writer.WithOffsetOrigin(fileInfo.OffsetOrigin.Value) : null;
			writer.OffsetFlushMode = OffsetFlushMode.Recursive;

			file.WriteFile(writer, context, fileInfo);
			writer.Flush();
		}

		/// <summary>
		/// Serialize file data to a writer.
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="file">The file data to write</param>
		/// <param name="context">Context to write with</param>
		/// <param name="fileInfo">Info for the file being written to</param>
		public static void WriteFile<T, C>(this BinaryObjectWriter writer, T file, C context, FileIOInfo fileInfo = default) where T : IFileSerializable<C>
		{
			file.WriteFileToWriter(writer, context, fileInfo);
		}

		/// <summary>
		/// Serialize file data to a writer.
		/// </summary>
		/// <param name="file">The file data to write</param>
		/// <param name="writer">The writer to write to</param>
		/// <param name="context">Context to write with</param>
		/// <param name="fileInfo">Info for the file being written to</param>
		public static void WriteFileToWriter<T, C>(this T file, BinaryObjectWriter writer, C context, FileIOInfo fileInfo = default) where T : IFileSerializable<C>
		{
			if(!file.CheckCanWriteFile(context, ref fileInfo))
			{
				throw new CannotWriteFileException(typeof(T), "The file cannot be serialized!");
			}

			using EndiannessToken? endiannessToken = fileInfo.Endianness != null ? writer.WithEndian(fileInfo.Endianness.Value) : null;
			using OffsetOriginToken? offsetOriginToken = fileInfo.OffsetOrigin != null ? writer.WithOffsetOrigin(fileInfo.OffsetOrigin.Value) : null;

			file.WriteFile(writer, context, fileInfo);
			writer.Flush();
		}

		#endregion
	}
}
