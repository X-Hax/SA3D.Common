using Amicitia.IO;
using Amicitia.IO.Binary;
using Amicitia.IO.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SA3D.Common.IO
{
	/// <summary>
	/// General purpose read/write extensions
	/// </summary>
	public static class BinaryHelper
	{
		/// <summary>
		/// Create a new <see cref="ReadNullReferenceException"/> for the reader
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="structName"></param>
		/// <param name="fieldname"></param>
		/// <returns></returns>
		public static ReadNullReferenceException ReadNullReference(this BinaryValueReader reader, string structName, string fieldname)
		{
			return new(reader, structName, fieldname);
		}

		/// <summary>
		/// Create a new <see cref="ReadNullReferenceException"/> for the reader
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="structName"></param>
		/// <param name="fieldname"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		public static ReadNullReferenceException ReadNullReference(this BinaryValueReader reader, string structName, string fieldname, long offset)
		{
			return new(reader, structName, fieldname, offset);
		}

		#region Offsets

		/// <summary>
		/// Gets the current offset position in the stream
		/// </summary>
		/// <param name="reader">The reader to get the position offset from</param>
		public static long GetPositionOffset(this BinaryObjectReader reader)
		{
			return reader.OffsetHandler.CalculateOffset(reader.Position);
		}

		/// <summary>
		/// Gets the current offset position in the stream
		/// </summary>
		/// <param name="writer">The writer to get the position offset from</param>
		public static long GetPositionOffset(this BinaryObjectWriter writer)
		{
			return writer.OffsetHandler.CalculateOffset(writer.Position);
		}

		/// <summary>
		/// Aligns the writer from a specific position
		/// </summary>
		/// <param name="writer">The writer to align</param>
		/// <param name="origin">The origin from which to align</param>
		/// <param name="alignment">The alignment</param>
		public static void Align(this BinaryObjectWriter writer, int alignment, long origin)
		{
			long currentSize = writer.Position - origin;
			writer.WriteCollection(Enumerable.Repeat((byte)0, (int)(AlignmentHelper.Align(currentSize, alignment) - currentSize)));
		}

		#endregion

		#region BAMS

		/// <summary>
		/// Writes a radians angle as a 16 bit BAMS value
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="radians">The radians value to write</param>
		public static void WriteBAMS16(this BinaryValueWriter writer, float radians)
		{
			writer.WriteInt16((short)MathHelper.RadToBAMS(radians));
		}

		/// <summary>
		/// Writes a radians angle as a 32 bit BAMS value
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="radians">The radians value to write</param>
		public static void WriteBAMS32(this BinaryValueWriter writer, float radians)
		{
			writer.WriteInt32(MathHelper.RadToBAMS(radians));
		}

		/// <summary>
		/// Reads a 16 bit BAMS value as a radians angle
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		/// <returns></returns>
		public static float ReadBAMS16(this BinaryValueReader reader)
		{
			return MathHelper.BAMSToRad(reader.ReadInt16());
		}

		/// <summary>
		/// Reads a 32 bit BAMS value as a radians angle
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		/// <returns></returns>
		public static float ReadBAMS32(this BinaryValueReader reader)
		{
			return MathHelper.BAMSToRad(reader.ReadInt32());
		}

		#endregion

		#region Seek / Endian / Binary Offset Format

		/// <summary>
		/// Seek from <see cref="SeekOrigin.Begin"/>
		/// </summary>
		/// <param name="reader">The reader to seek for</param>
		/// <param name="position">The position to seek to</param>
		public static void SeekPosition(this BinaryValueReader reader, long position)
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
		public static EndiannessToken WithEndian(this BinaryValueReader reader, Endianness endianness)
		{
			return new(reader, endianness);
		}

		/// <summary>
		/// Creates a new <see cref="OffsetBinaryFormatToken"/> for a <see cref="BinaryObjectReader"/>
		/// </summary>
		/// <param name="reader">The reader to create the token for</param>
		/// <param name="format">The format to set</param>
		/// <returns></returns>
		public static OffsetBinaryFormatToken WithBinaryOffsetFormat(this BinaryObjectReader reader, OffsetBinaryFormat format)
		{
			return new(reader, format);
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
		public static EndiannessToken WithEndian(this BinaryValueWriter writer, Endianness endianness)
		{
			return new(writer, endianness);
		}

		/// <summary>
		/// Creates a new <see cref="OffsetBinaryFormatToken"/> for a <see cref="BinaryObjectReader"/>
		/// </summary>
		/// <param name="writer">The writer to create the token for</param>
		/// <param name="format">The format to set</param>
		/// <returns></returns>
		public static OffsetBinaryFormatToken WithBinaryOffsetFormat(this BinaryObjectWriter writer, OffsetBinaryFormat format)
		{
			return new(writer, format);
		}


		/// <summary>
		/// Peeks the 2 bytes at the current position. Returns the endianness in which it would be the smaller value
		/// </summary>
		/// <returns></returns>
		public static Endianness CheckEndianness16(this BinaryValueReader reader)
		{
			ushort little, big;

			using(reader.WithEndian(Endianness.Little))
			{
				using(reader.At())
				{
					little = reader.ReadUInt16();
				}
			}

			using(reader.WithEndian(Endianness.Big))
			{
				using(reader.At())
				{
					big = reader.ReadUInt16();
				}
			}

			return little < big ? Endianness.Little : Endianness.Big;
		}

		/// <summary>
		/// Peeks the 4 bytes at the current position. Returns the endianness in which it would be the smaller value
		/// </summary>
		/// <returns></returns>
		public static Endianness CheckEndianness32(this BinaryValueReader reader, long offset = 0, SeekOrigin origin = SeekOrigin.Begin)
		{
			uint little, big;

			using(reader.WithEndian(Endianness.Little))
			{
				using(reader.At(offset, origin))
				{
					little = reader.ReadUInt32();
				}
			}

			using(reader.WithEndian(Endianness.Big))
			{
				using(reader.At(offset, origin))
				{
					big = reader.ReadUInt32();
				}
			}

			return little < big ? Endianness.Little : Endianness.Big;
		}

		#endregion

		#region Basic Reads

		/// <summary>
		/// Reads a string at the the offset of the current position
		/// </summary>
		/// <param name="reader">The reader to read the string from</param>
		/// <param name="offset">Offset to read a string at</param>
		/// <param name="format">The format to read the string in</param>
		/// <param name="fixedLength">The length of the string, if <see cref="StringBinaryFormat.FixedLength"/> is used</param>
		/// <returns></returns>
		public static string? ReadStringAtOffset(this BinaryObjectReader reader, long offset, StringBinaryFormat format = StringBinaryFormat.NullTerminated, int fixedLength = -1)
		{
			if(offset == 0)
			{
				return null;
			}

			using SeekToken token = reader.AtOffset(offset);
			return reader.ReadString(format, fixedLength);
		}

		/// <summary>
		/// Reads a string at a specific offset. Returns an empty string instead of null
		/// </summary>
		/// <param name="reader">The reader to read the string from</param>
		/// <param name="offset">Offset to read a string at</param>
		/// <param name="format">The format to read the string in</param>
		/// <param name="fixedLength">The length of the string, if <see cref="StringBinaryFormat.FixedLength"/> is used</param>
		/// <returns></returns>
		public static string ReadStringAtOffsetOrEmpty(this BinaryObjectReader reader, long offset, StringBinaryFormat format = StringBinaryFormat.NullTerminated, int fixedLength = -1)
		{
			return reader.ReadStringAtOffset(offset, format, fixedLength) ?? string.Empty;
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
			return reader.ReadStringAtOffset(reader.ReadOffsetValue(), format, fixedLength);
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

		#endregion

		#region Basic Writes

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
		public static void WriteOffset(this BinaryObjectWriter writer, object? instance, Action action, int alignment = 0, int priority = 0)
		{
			if(instance == null)
			{
				writer.WriteOffsetValue(writer.OffsetHandler.NullOffset);
				return;
			}

			writer.WriteOffset(alignment, instance, instance, (w, o) => action(), priority);
		}

		#endregion

		#region Read Object Array

		internal static void ReadObjectArray<T>(this BinaryObjectReader reader, T[] output) where T : IBinarySerializable, new()
		{
			for(int i = 0; i < output.Length; i++)
			{
				output[i] = reader.ReadObject<T>();
			}
		}

		internal static void ReadObjectArray<T, TContext>(this BinaryObjectReader reader, TContext context, T[] output) where T : IBinarySerializable<TContext>, new()
		{
			for(int i = 0; i < output.Length; i++)
			{
				output[i] = reader.ReadObject<T, TContext>(context);
			}
		}

		internal static void ReadObjectArray<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, T[] output)
		{
			for(int i = 0; i < output.Length; i++)
			{
				output[i] = read(reader);
			}
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
			reader.ReadObjectArray(result);
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
		public static T[] ReadObjectArray<T, TContext>(this BinaryObjectReader reader, int count, TContext context) where T : IBinarySerializable<TContext>, new()
		{
			T[] result = new T[count];
			reader.ReadObjectArray(context, result);
			return result;
		}

		/// <summary>
		/// Reads an array of objects at the current location
		/// </summary>
		/// <typeparam name="T">Object type to read</typeparam>
		/// <param name="reader">Reader to read from</param>
		/// <param name="read">The read callback</param>
		/// <param name="count">Number of items in the array to read</param>
		public static T[] ReadObjectArray<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, int count)
		{
			T[] result = new T[count];
			reader.ReadObjectArray(read, result);
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
			if(count == 0)
			{
				return [];
			}

			T[] result = new T[count];
			reader.ReadAtOffset(offset, () => reader.ReadObjectArray(result));
			return result;
		}

		/// <summary>
		/// Reads an array of objects at a specific offset
		/// </summary>
		/// <typeparam name="T">Object type to read</typeparam>
		/// <typeparam name="TContext">Reader context type</typeparam>
		/// <param name="reader">Reader to read from</param>
		/// <param name="offset">The offset to read at</param>
		/// <param name="count">Number of items in the array to read</param>
		/// <param name="context">Reader context to use</param>
		public static T[] ReadObjectArrayAtOffset<T, TContext>(this BinaryObjectReader reader, long offset, int count, TContext context) where T : IBinarySerializable<TContext>, new()
		{
			if(count == 0)
			{
				return [];
			}

			T[] result = new T[count];
			reader.ReadAtOffset(offset, () => reader.ReadObjectArray(context, result));
			return result;
		}

		/// <summary>
		/// Reads an array of objects at a specific offset 
		/// </summary>
		/// <typeparam name="T">Object type to read</typeparam>
		/// <param name="reader">Reader to read from</param>
		/// <param name="read">The read callback</param>
		/// <param name="offset">The offset to read at</param>
		/// <param name="count">Number of items in the array to read</param>
		public static T[] ReadObjectArrayAtOffset<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, long offset, int count)
		{
			if(count == 0)
			{
				return [];
			}

			T[] result = new T[count];
			reader.ReadAtOffset(offset, () => reader.ReadObjectArray(read, result));
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
			return reader.ReadObjectArrayAtOffset<T>(reader.ReadOffsetValue(), count);
		}

		/// <summary>
		/// Reads an array of objects at the offset stored at the current position
		/// </summary>
		/// <typeparam name="T">Object type to read</typeparam>
		/// <typeparam name="TContext">Reader context type</typeparam>
		/// <param name="reader">Reader to read from</param>
		/// <param name="count">Number of items in the array to read</param>
		/// <param name="context">Reader context to use</param>
		public static T[] ReadObjectArrayOffset<T, TContext>(this BinaryObjectReader reader, int count, TContext context) where T : IBinarySerializable<TContext>, new()
		{
			return reader.ReadObjectArrayAtOffset<T, TContext>(reader.ReadOffsetValue(), count, context);
		}

		/// <summary>
		/// Reads an array of objects at the offset stored at the current position
		/// </summary>
		/// <typeparam name="T">Object type to read</typeparam>
		/// <param name="reader">Reader to read from</param>
		/// <param name="read">The read callback</param>
		/// <param name="count">Number of items in the array to read</param>
		public static T[] ReadObjectArrayOffset<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, int count)
		{
			return reader.ReadObjectArrayAtOffset(read, reader.ReadOffsetValue(), count);
		}

		#endregion

		#region Write Object Array

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
		public static void WriteObjectArray<T, TContext>(this BinaryObjectWriter writer, IEnumerable<T> items, TContext context) where T : IBinarySerializable<TContext>
		{
			foreach(T item in items)
			{
				writer.WriteObject(item, context);
			}
		}

		/// <summary>
		/// Writes a collection of objects to a <see cref="BinaryObjectWriter"/> as an array
		/// </summary>
		/// <typeparam name="T">Type of the object to write</typeparam>
		/// <param name="writer">Writer to write to</param>
		/// <param name="write">The write callback</param>
		/// <param name="items">Items to write</param>
		public static void WriteObjectArray<T>(this BinaryObjectWriter writer, Action<BinaryObjectWriter, T> write, IEnumerable<T> items)
		{
			foreach(T item in items)
			{
				write(writer, item);
			}
		}


		/// <summary>
		/// Writes the offset to a collection of objects to a <see cref="BinaryObjectWriter"/> as an array
		/// </summary>
		/// <typeparam name="T">Type of the object to write</typeparam>
		/// <param name="writer">Writer to write to</param>
		/// <param name="items">Items to write</param>
		public static void WriteObjectArrayOffset<T>(this BinaryObjectWriter writer, IEnumerable<T>? items) where T : IBinarySerializable
		{
			writer.WriteOffset(items, () => writer.WriteObjectArray(items!));
		}

		/// <summary>
		/// Writes a collection of objects to a <see cref="BinaryObjectWriter"/> as an array
		/// </summary>
		/// <typeparam name="T">Type of the object to write</typeparam>
		/// <typeparam name="TContext">Type of the writer context</typeparam>
		/// <param name="writer">Writer to write to</param>
		/// <param name="context">Writer context to use</param>
		/// <param name="items">Items to write</param>
		public static void WriteObjectArrayOffset<T, TContext>(this BinaryObjectWriter writer, IEnumerable<T>? items, TContext context) where T : IBinarySerializable<TContext>
		{
			writer.WriteOffset(items, () => writer.WriteObjectArray(items!, context));
		}

		/// <summary>
		/// Writes the offset to a collection of objects to a <see cref="BinaryObjectWriter"/> as an array
		/// </summary>
		/// <typeparam name="T">Type of the object to write</typeparam>
		/// <param name="writer">Writer to write to</param>
		/// <param name="write">The write callback</param>
		/// <param name="items">Items to write</param>
		public static void WriteObjectArrayOffset<T>(this BinaryObjectWriter writer, Action<BinaryObjectWriter, T> write, IEnumerable<T>? items)
		{
			writer.WriteOffset(items, () => writer.WriteObjectArray(write, items!));
		}

		#endregion
	}
}
