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
		/// <param name="filepath">The path to the file to check.</param>
		public static bool CheckFile<T>(string filepath) where T : IFileSerializable, new()
		{
			using FileStream stream = System.IO.File.OpenRead(filepath);
			return CheckStream<T>(stream);
		}

		/// <summary>
		/// Checks whether data is formatted as a file.
		/// </summary>
		/// <param name="data">The data to check.</param>
		public static bool CheckBytes<T>(byte[] data) where T : IFileSerializable, new()
		{
			using MemoryStream stream = new(data);
			return CheckStream<T>(stream);
		}

		/// <summary>
		/// Checks whether a stream is formatted as a model file
		/// </summary>
		/// <param name="stream">The stream to check</param>
		/// <returns></returns>
		public static bool CheckStream<T>(Stream stream) where T : IFileSerializable, new()
		{
			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, Endianness.Little);
			return reader.Check<T>();
		}

		/// <summary>
		/// Checks whether the data at a binary readers current location can be read as <typeparamref name="T"/>
		/// </summary>
		/// <param name="reader">Reader to check</param>
		/// <returns></returns>
		public static bool Check<T>(this BinaryObjectReader reader) where T : IFileSerializable, new()
		{
			using SeekToken at = reader.At();
			return new T().Check(reader);
		}


		/// <summary>
		/// Reads a file.
		/// </summary>
		/// <param name="filepath">The path to the file that should be read.</param>
		/// <returns>The file that was read.</returns>
		public static T ReadFromFile<T>(string filepath) where T : IFileSerializable, new()
		{
			using FileStream stream = System.IO.File.OpenRead(filepath);
			return ReadFromStream<T>(stream, filepath);
		}

		/// <summary>
		/// Reads a model file off byte data.
		/// </summary>
		/// <param name="data">Data to read.</param>
		/// <param name="filepath">Path to the file being read</param>
		/// <returns>The model file that was read.</returns>
		public static T ReadFromBytes<T>(byte[] data, string? filepath) where T : IFileSerializable, new()
		{
			using MemoryStream stream = new(data);
			return ReadFromStream<T>(stream, filepath);
		}

		/// <summary>
		/// Read a model file off a stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="filepath">Path to the file being read</param>
		/// <returns></returns>
		public static T ReadFromStream<T>(Stream stream, string? filepath) where T : IFileSerializable, new()
		{
			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, Endianness.Little);
			return reader.ReadObject<T, FileContext>(new(filepath));
		}


		/// <summary>
		/// Reads a file.
		/// </summary>
		/// <param name="filepath">The path to the file that should be read.</param>
		/// <param name="context">IO Context to use</param>
		/// <returns>The file that was read.</returns>
		public static T ReadFromFile<T, C>(string filepath, C context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			using FileStream stream = System.IO.File.OpenRead(filepath);
			return ReadFromStream<T, C>(stream, filepath, context);
		}

		/// <summary>
		/// Reads a model file off byte data.
		/// </summary>
		/// <param name="data">Data to read.</param>
		/// <param name="filepath">Path to the file being read</param>
		/// <param name="context">IO Context to use</param>
		/// <returns>The model file that was read.</returns>
		public static T ReadFromBytes<T, C>(byte[] data, string? filepath, C context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			using MemoryStream stream = new(data);
			return ReadFromStream<T, C>(stream, filepath, context);
		}

		/// <summary>
		/// Read a model file off a stream
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="filepath">Path to the file being read</param>
		/// <param name="context">IO Context to use</param>
		/// <returns></returns>
		public static T ReadFromStream<T, C>(Stream stream, string? filepath, C context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			using BinaryObjectReader reader = new(stream, StreamOwnership.Retain, Endianness.Little, fileName: filepath);
			return reader.ReadObject<T, FileContext<C>>(new(filepath, context));
		}


		/// <summary>
		/// Write the file to a file.
		/// </summary>
		/// <param name="file">The file to write</param>
		/// <param name="filepath">Path to the file to write to.</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void WriteToFile<T>(this T file, string filepath) where T : IFileSerializable, new()
		{
			using FileStream stream = System.IO.File.OpenWrite(filepath);
			WriteToStream(file, filepath, stream);
		}

		/// <summary>
		/// Writes the file to a byte array.
		/// </summary>
		/// <param name="file">The file to write</param>
		/// <param name="filepath">Path to the file being read</param>
		/// <returns>The written byte data.</returns>
		public static byte[] WriteToBytes<T>(this T file, string? filepath) where T : IFileSerializable, new()
		{
			using MemoryStream stream = new();
			WriteToStream(file, filepath, stream);
			return stream.ToArray();
		}

		/// <summary>
		/// Writes the model file to a byte array.
		/// </summary>
		/// <param name="file">The file to write</param>
		/// <param name="stream">The stream to write to</param>
		/// <param name="filepath">Path to the file being read</param>
		public static void WriteToStream<T>(this T file, string? filepath, Stream stream) where T : IFileSerializable, new()
		{
			using BinaryObjectWriter writer = new(stream, StreamOwnership.Retain, Endianness.Little, fileName: filepath);
			writer.OffsetFlushMode = OffsetFlushMode.Recursive;
			writer.WriteObject(file, new FileContext(filepath));
		}


		/// <summary>
		/// Write the file to a file.
		/// </summary>
		/// <param name="file">The file to write</param>
		/// <param name="context">IO Context to use</param>
		/// <param name="filepath">Path to the file to write to.</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void WriteToFile<T, C>(this T file, string filepath, C context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			using FileStream stream = System.IO.File.OpenWrite(filepath);
			WriteToStream(file, filepath, context, stream);
		}

		/// <summary>
		/// Writes the file to a byte array.
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		/// <param name="file">The file to write</param>
		/// <param name="filepath">Path to the file being read</param>
		/// <param name="context">IO Context to use</param>
		/// <returns>The written byte data.</returns>
		public static byte[] WriteToBytes<T, C>(this T file, string? filepath, C context) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			using MemoryStream stream = new();
			WriteToStream(file, filepath, context, stream);
			return stream.ToArray();
		}

		/// <summary>
		/// Writes the model file to a byte array.
		/// </summary>
		/// <param name="file">The file to write</param>
		/// <param name="filepath">Path to the file being read</param>
		/// <param name="context">IO Context to use</param>
		/// <param name="stream">The stream to write to</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void WriteToStream<T, C>(this T file, string? filepath, C context, Stream stream) where T : IFileSerializable<C>, new() where C : unmanaged
		{
			using BinaryObjectWriter writer = new(stream, StreamOwnership.Retain, Endianness.Little);
			writer.OffsetFlushMode = OffsetFlushMode.Recursive;
			writer.WriteObject(file, new FileContext<C>(filepath, context));
		}
	}
}
