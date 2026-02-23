using Amicitia.IO.Binary;
using Amicitia.IO.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace SA3D.Common.IO
{
	/// <summary>
	/// General purpose read/write extensions
	/// </summary>
	public static class BinaryHelper
	{
		/// <summary>
		/// Seek from <see cref="SeekOrigin.Begin"/>
		/// </summary>
		/// <param name="reader">The reader to seek for</param>
		/// <param name="position">The position to seek to</param>
		public static void SeekOffset(this BinaryValueReader reader, long position)
		{
			reader.Seek(position, SeekOrigin.Begin);
		}

		/// <summary>
		/// Create a <see cref="SeekToken"/> to the current position
		/// </summary>
		/// <param name="reader">The reader to create the token for</param>
		/// <returns></returns>
		public static SeekToken At(this BinaryValueReader reader)
		{
			return reader.At(reader.Position, SeekOrigin.Begin);
		}

		/// <summary>
		/// Creates a new <see cref="EndiannessToken"/> for a <see cref="BinaryValueReader"/>
		/// </summary>
		/// <param name="reader">The reader to create the token for</param>
		/// <param name="endianness">The endianness to set</param>
		/// <returns></returns>
		public static EndiannessToken DisposableEndian(this BinaryValueReader reader, Endianness endianness)
		{
			return new(reader, endianness);
		}

		/// <summary>
		/// Seek from <see cref="SeekOrigin.Begin"/>
		/// </summary>
		/// <param name="writer">The writer to seek for</param>
		/// <param name="position">The position to seek to</param>
		public static void SeekOffset(this BinaryValueWriter writer, long position)
		{
			writer.Seek(position, SeekOrigin.Begin);
		}

		/// <summary>
		/// Create a <see cref="SeekToken"/> to the current position
		/// </summary>
		/// <param name="writer">The writer to create the token for</param>
		/// <returns></returns>
		public static SeekToken At(this BinaryValueWriter writer)
		{
			return writer.At(writer.Position, SeekOrigin.Begin);
		}

		/// <summary>
		/// Creates a new <see cref="EndiannessToken"/> for a <see cref="BinaryValueWriter"/>
		/// </summary>
		/// <param name="writer">The writer to create the token for</param>
		/// <param name="endianness">The endianness to set</param>
		/// <returns></returns>
		public static EndiannessToken DisposableEndian(this BinaryValueWriter writer, Endianness endianness)
		{
			return new(writer, endianness);
		}


		/// <summary>
		/// Write an offset value
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="value">The value to write</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteOffsetValue(this BinaryObjectWriter writer, long value)
		{
			if(writer.OffsetBinaryFormat == OffsetBinaryFormat.U32)
			{
				writer.WriteUInt32((uint)value);
			}
			else
			{
				writer.WriteInt64(value);
			}
		}

		/// <summary>
		/// Write an offset
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="instance">The instance to associate with the offset</param>
		/// <param name="action">The write action</param>
		/// <param name="alignment">The byte alignment to apply after writing</param>
		/// <param name="priority">Writing priotiy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteOffset(this BinaryObjectWriter writer, object instance, Action action, int alignment = 0, int priority = 0)
		{
			writer.WriteOffset(alignment, instance, instance, (w, o) => action(), priority);
		}

		/// <summary>
		/// Reads a string at the the offset of the current position
		/// </summary>
		/// <param name="reader">The reader to read the string from</param>
		/// <param name="format">The format to read the string in</param>
		/// <param name="fixedLength">The length of the string, if <see cref="StringBinaryFormat.FixedLength"/> is used</param>
		/// <returns></returns>
		public static string? ReadStringOffset(this BinaryObjectReader reader, StringBinaryFormat format = StringBinaryFormat.NullTerminated, int fixedLength = -1)
		{
			long offset = reader.ReadOffsetValue();
			if(offset == 0)
			{
				return null;
			}

			using SeekToken token = reader.AtOffset(offset);
			return reader.ReadString(format, fixedLength);
		}

		/// <summary>
		/// Reads a string at the the offset of the current position. Returns an empty string instead of null
		/// </summary>
		/// <param name="reader">The reader to read the string from</param>
		/// <param name="format">The format to read the string in</param>
		/// <param name="fixedLength">The length of the string, if <see cref="StringBinaryFormat.FixedLength"/> is used</param>
		/// <returns></returns>
		public static string ReadStringOffsetOrEmpty(this BinaryObjectReader reader, StringBinaryFormat format = StringBinaryFormat.NullTerminated, int fixedLength = -1)
		{
			return reader.ReadStringOffset(format, fixedLength) ?? string.Empty;
		}


		/// <summary>
		/// Reads an array of objects at the current location
		/// </summary>
		/// <typeparam name="T">Object type to read</typeparam>
		/// <param name="reader">Reader to read from</param>
		/// <param name="count">Number of items in the array to read</param>
		public static T[] ReadObjectArray<T>(this BinaryObjectReader reader, int count) where T : IBinarySerializable, new()
		{
			T[] result = new T[count];
			for(int i = 0; i < count; i++)
			{
				result[i] = reader.ReadObject<T>();
			}

			return result;
		}

		/// <summary>
		/// Reads an array of objects at the current location
		/// </summary>
		/// <typeparam name="T">Object type to read</typeparam>
		/// <typeparam name="TContext">Reader context type</typeparam>
		/// <param name="reader">Reader to read from</param>
		/// <param name="context">Reader context to use</param>
		/// <param name="count">Number of items in the array to read</param>
		public static T[] ReadObjectArray<T, TContext>(this BinaryObjectReader reader, TContext context, int count) where T : IBinarySerializable<TContext>, new()
		{
			T[] result = new T[count];
			for(int i = 0; i < count; i++)
			{
				result[i] = reader.ReadObject<T, TContext>(context);
			}

			return result;
		}

		/// <summary>
		/// Reads an array of objects at the offset stored at the current position
		/// </summary>
		/// <typeparam name="T">Object type to read</typeparam>
		/// <param name="reader">Reader to read from</param>
		/// <param name="count">Number of items in the array to read</param>
		public static T[] ReadObjectArrayOffset<T>(this BinaryObjectReader reader, int count) where T : IBinarySerializable, new()
		{
			T[] result = new T[count];

			reader.ReadOffset(() =>
			{
				for(int i = 0; i < count; i++)
				{
					result[i] = reader.ReadObject<T>();
				}
			});

			return result;
		}

		/// <summary>
		/// Reads an array of objects at the offset stored at the current position
		/// </summary>
		/// <typeparam name="T">Object type to read</typeparam>
		/// <typeparam name="TContext">Reader context type</typeparam>
		/// <param name="reader">Reader to read from</param>
		/// <param name="context">Reader context to use</param>
		/// <param name="count">Number of items in the array to read</param>
		public static T[] ReadObjectArrayOffset<T, TContext>(this BinaryObjectReader reader, TContext context, int count) where T : IBinarySerializable<TContext>, new()
		{
			T[] result = new T[count];

			reader.ReadOffset(() =>
			{
				for(int i = 0; i < count; i++)
				{
					result[i] = reader.ReadObject<T, TContext>(context);
				}
			});

			return result;
		}

		/// <summary>
		/// Reads an array of objects at a specific offset 
		/// </summary>
		/// <typeparam name="T">Object type to read</typeparam>
		/// <param name="reader">Reader to read from</param>
		/// <param name="offset">The offset to read at</param>
		/// <param name="count">Number of items in the array to read</param>
		public static T[] ReadObjectArrayAtOffset<T>(this BinaryObjectReader reader, long offset, int count) where T : IBinarySerializable, new()
		{
			T[] result = new T[count];

			reader.ReadAtOffset(offset, () =>
			{
				for(int i = 0; i < count; i++)
				{
					result[i] = reader.ReadObject<T>();
				}
			});

			return result;
		}

		/// <summary>
		/// Reads an array of objects at a specific offset
		/// </summary>
		/// <typeparam name="T">Object type to read</typeparam>
		/// <typeparam name="TContext">Reader context type</typeparam>
		/// <param name="reader">Reader to read from</param>
		/// <param name="context">Reader context to use</param>
		/// <param name="offset">The offset to read at</param>
		/// <param name="count">Number of items in the array to read</param>
		public static T[] ReadObjectArrayAtOffset<T, TContext>(this BinaryObjectReader reader, TContext context, long offset, int count) where T : IBinarySerializable<TContext>, new()
		{
			T[] result = new T[count];

			reader.ReadAtOffset(offset, () =>
			{
				for(int i = 0; i < count; i++)
				{
					result[i] = reader.ReadObject<T, TContext>(context);
				}
			});

			return result;
		}


		/// <summary>
		/// Writes a collection of objects to a <see cref="BinaryObjectWriter"/> as an array
		/// </summary>
		/// <typeparam name="T">Type of the object to write</typeparam>
		/// <param name="writer">Writer to write to</param>
		/// <param name="items">Items to write</param>
		public static void WriteObjectArray<T>(this BinaryObjectWriter writer, IEnumerable<T> items) where T : IBinarySerializable
		{
			foreach(T item in items)
			{
				writer.WriteObject(item);
			}
		}

		/// <summary>
		/// Writes a collection of objects to a <see cref="BinaryObjectWriter"/> as an array
		/// </summary>
		/// <typeparam name="T">Type of the object to write</typeparam>
		/// <typeparam name="TContext">Type of the writer context</typeparam>
		/// <param name="writer">Writer to write to</param>
		/// <param name="context">Writer context to use</param>
		/// <param name="items">Items to write</param>
		public static void WriteObjectArray<T, TContext>(this BinaryObjectWriter writer, TContext context, IEnumerable<T> items) where T : IBinarySerializable<TContext>
		{
			foreach(T item in items)
			{
				writer.WriteObject(item, context);
			}
		}

		/// <summary>
		/// Writes the offset to a collection of objects to a <see cref="BinaryObjectWriter"/> as an array
		/// </summary>
		/// <typeparam name="T">Type of the object to write</typeparam>
		/// <param name="writer">Writer to write to</param>
		/// <param name="items">Items to write</param>
		public static void WriteObjectArrayOffset<T>(this BinaryObjectWriter writer, IEnumerable<T> items) where T : IBinarySerializable
		{
			writer.WriteOffset(() =>
			{
				foreach(T item in items)
				{
					writer.WriteObject(item);
				}
			});
		}

		/// <summary>
		/// Writes a collection of objects to a <see cref="BinaryObjectWriter"/> as an array
		/// </summary>
		/// <typeparam name="T">Type of the object to write</typeparam>
		/// <typeparam name="TContext">Type of the writer context</typeparam>
		/// <param name="writer">Writer to write to</param>
		/// <param name="context">Writer context to use</param>
		/// <param name="items">Items to write</param>
		public static void WriteObjectArrayOffset<T, TContext>(this BinaryObjectWriter writer, TContext context, IEnumerable<T> items) where T : IBinarySerializable<TContext>
		{
			writer.WriteOffset(() =>
			{
				foreach(T item in items)
				{
					writer.WriteObject(item, context);
				}
			});
		}
	}
}
