using Amicitia.IO.Binary;
using SA3D.Common.Lookup;
using System;
using System.Collections.Generic;

#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters

namespace SA3D.Common.IO
{
	/// <summary>
	/// 
	/// </summary>
	public static class LookupBinaryHelper
	{
		private static T? ReadLUTItemAtOffset<T>(this BinaryObjectReader reader, long offset, BaseLUT lut, string? labelPrefix, Func<T> read) where T : class
		{
			long resolvedOffset = reader.OffsetHandler.ResolveOffset(offset);

			if(resolvedOffset == -1)
			{
				return null;
			}

			if(lut.TryGetValue(offset, out T? result))
			{
				return result;
			}

			using(reader.At(resolvedOffset, System.IO.SeekOrigin.Begin))
			{
				result = read();
			}

			if(result is ILabel labelable)
			{
				labelable.Label = lut.Labels.GetGenerateValue(offset, labelPrefix ?? labelable.LabelPrefix);
			}

			lut.AddTryLabel(offset, result);

			return result;
		}

		#region Read unmanaged array

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[]? ReadArrayAtOffset<T>(this BinaryObjectReader reader, long offset, int count, BaseLUT lut) where T : unmanaged
		{
			return ReadLUTItemAtOffset(reader, offset, lut, null, () => reader.ReadArray<T>(count));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[]? ReadArrayOffset<T>(this BinaryObjectReader reader, int count, BaseLUT lut) where T : unmanaged
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadArrayAtOffset<T>(offset, count, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[] ReadArray<T>(this BinaryObjectReader reader, int count, BaseLUT lut) where T : unmanaged
		{
			long pointer = reader.GetPointerPosition();
			return reader.ReadArrayAtOffset<T>(pointer, count, lut)!;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T>? ReadLabeledArrayAtOffset<T>(this BinaryObjectReader reader, long offset, int count, string labelPrefix, BaseLUT lut) where T : unmanaged
		{
			return ReadLUTItemAtOffset(reader, offset, lut, labelPrefix, () =>
			{
				LabeledArray<T> result = new(count);
				reader.ReadArray(count, result.Array);
				return result;
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T>? ReadLabeledArrayOffset<T>(this BinaryObjectReader reader, int count, string labelPrefix, BaseLUT lut) where T : unmanaged
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadLabeledArrayAtOffset<T>(offset, count, labelPrefix, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T> ReadLabeledArray<T>(this BinaryObjectReader reader, int count, string labelPrefix, BaseLUT lut) where T : unmanaged
		{
			long pointer = reader.GetPointerPosition();
			return reader.ReadLabeledArrayAtOffset<T>(pointer, count, labelPrefix, lut)!;
		}

		#endregion

		#region Read object

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T? ReadObjectAtOffset<T>(this BinaryObjectReader reader, long offset, BaseLUT lut) where T : class, IBinarySerializable, new()
		{
			return ReadLUTItemAtOffset(reader, offset, lut, null, reader.ReadObject<T>);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T? ReadObjectAtOffset<T, TContext>(this BinaryObjectReader reader, long offset, TContext context, BaseLUT lut) where T : class, IBinarySerializable<TContext>, new()
		{
			return ReadLUTItemAtOffset(reader, offset, lut, null, () => reader.ReadObject<T, TContext>(context));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="offset"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T? ReadObjectAtOffset<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, long offset, BaseLUT lut) where T : class
		{
			return ReadLUTItemAtOffset(reader, offset, lut, null, () => read(reader));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T? ReadObjectOffset<T>(this BinaryObjectReader reader, BaseLUT lut) where T : class, IBinarySerializable, new()
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadObjectAtOffset<T>(offset, lut);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="reader"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T? ReadObjectOffset<T, TContext>(this BinaryObjectReader reader, TContext context, BaseLUT lut) where T : class, IBinarySerializable<TContext>, new()
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadObjectAtOffset<T, TContext>(offset, context, lut);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T? ReadObjectOffset<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, BaseLUT lut) where T : class
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadObjectAtOffset(read, offset, lut);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T ReadObject<T>(this BinaryObjectReader reader, BaseLUT lut) where T : class, IBinarySerializable, new()
		{
			long pointer = reader.GetPointerPosition();
			return reader.ReadObjectAtOffset<T>(pointer, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="reader"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T ReadObject<T, TContext>(this BinaryObjectReader reader, TContext context, BaseLUT lut) where T : class, IBinarySerializable<TContext>, new()
		{
			long pointer = reader.GetPointerPosition();
			return reader.ReadObjectAtOffset<T, TContext>(pointer, context, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T ReadObject<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, BaseLUT lut) where T : class
		{
			long pointer = reader.GetPointerPosition();
			return reader.ReadObjectAtOffset(read, pointer, lut)!;
		}

		#endregion


		#region Read Object Array

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[]? ReadObjectArrayAtOffset<T>(this BinaryObjectReader reader, long offset, int count, BaseLUT lut) where T : IBinarySerializable, new()
		{
			return ReadLUTItemAtOffset(reader, offset, lut, null, () => reader.ReadObjectArray<T>(count));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[]? ReadObjectArrayAtOffset<T, TContext>(this BinaryObjectReader reader, long offset, int count, TContext context, BaseLUT lut) where T : IBinarySerializable<TContext>, new()
		{
			return ReadLUTItemAtOffset(reader, offset, lut, null, () => reader.ReadObjectArray<T, TContext>(count, context));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[]? ReadObjectArrayAtOffset<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, long offset, int count, BaseLUT lut)
		{
			return ReadLUTItemAtOffset(reader, offset, lut, null, () => reader.ReadObjectArray(read, count));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[]? ReadObjectArrayOffset<T>(this BinaryObjectReader reader, int count, BaseLUT lut) where T : IBinarySerializable, new()
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadObjectArrayAtOffset<T>(offset, count, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[]? ReadObjectArrayOffset<T, TContext>(this BinaryObjectReader reader, int count, TContext context, BaseLUT lut) where T : IBinarySerializable<TContext>, new()
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadObjectArrayAtOffset<T, TContext>(offset, count, context, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="count"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[]? ReadObjectArrayOffset<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, int count, BaseLUT lut)
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadObjectArrayAtOffset(read, offset, count, lut)!;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[] ReadObjectArray<T>(this BinaryObjectReader reader, int count, BaseLUT lut) where T : IBinarySerializable, new()
		{
			long pointer = reader.GetPointerPosition();
			return reader.ReadObjectArrayAtOffset<T>(pointer, count, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[] ReadObjectArray<T, TContext>(this BinaryObjectReader reader, int count, TContext context, BaseLUT lut) where T : IBinarySerializable<TContext>, new()
		{
			long pointer = reader.GetPointerPosition();
			return reader.ReadObjectArrayAtOffset<T, TContext>(pointer, count, context, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="count"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T[] ReadObjectArray<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, int count, BaseLUT lut)
		{
			long pointer = reader.GetPointerPosition();
			return reader.ReadObjectArrayAtOffset(read, pointer, count, lut)!;
		}

		#endregion

		#region Read Labeled Object Array

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T>? ReadLabeledObjectArrayAtOffset<T>(this BinaryObjectReader reader, long offset, int count, string labelPrefix, BaseLUT lut) where T : IBinarySerializable, new()
		{
			return ReadLUTItemAtOffset(reader, offset, lut, labelPrefix, () =>
			{
				LabeledArray<T> result = new(count);
				reader.ReadObjectArray(result.Array);
				return result;
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T>? ReadLabeledObjectArrayAtOffset<T, TContext>(this BinaryObjectReader reader, long offset, int count, string labelPrefix, TContext context, BaseLUT lut) where T : IBinarySerializable<TContext>, new()
		{
			return ReadLUTItemAtOffset(reader, offset, lut, labelPrefix, () =>
			{
				LabeledArray<T> result = new(count);
				reader.ReadObjectArray(context, result.Array);
				return result;
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T>? ReadLabeledObjectArrayAtOffset<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, long offset, int count, string labelPrefix, BaseLUT lut)
		{
			return ReadLUTItemAtOffset(reader, offset, lut, labelPrefix, () =>
			{
				LabeledArray<T> result = new(count);
				reader.ReadObjectArray(read, result.Array);
				return result;
			});
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T>? ReadLabeledObjectArrayOffset<T>(this BinaryObjectReader reader, int count, string labelPrefix, BaseLUT lut) where T : IBinarySerializable, new()
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadLabeledObjectArrayAtOffset<T>(offset, count, labelPrefix, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T>? ReadLabeledObjectArrayOffset<T, TContext>(this BinaryObjectReader reader, int count, string labelPrefix, TContext context, BaseLUT lut) where T : IBinarySerializable<TContext>, new()
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadLabeledObjectArrayAtOffset<T, TContext>(offset, count, labelPrefix, context, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T>? ReadLabeledObjectArrayOffset<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, int count, string labelPrefix, BaseLUT lut)
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadLabeledObjectArrayAtOffset(read, offset, count, labelPrefix, lut)!;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T> ReadLabeledObjectArray<T>(this BinaryObjectReader reader, int count, string labelPrefix, BaseLUT lut) where T : IBinarySerializable, new()
		{
			long pointer = reader.GetPointerPosition();
			return reader.ReadLabeledObjectArrayAtOffset<T>(pointer, count, labelPrefix, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T> ReadLabeledObjectArray<T, TContext>(this BinaryObjectReader reader, int count, string labelPrefix, TContext context, BaseLUT lut) where T : IBinarySerializable<TContext>, new()
		{
			long pointer = reader.GetPointerPosition();
			return reader.ReadLabeledObjectArrayAtOffset<T, TContext>(pointer, count, labelPrefix, context, lut)!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T> ReadLabeledObjectArray<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, int count, string labelPrefix, BaseLUT lut)
		{
			long pointer = reader.GetPointerPosition();
			return reader.ReadLabeledObjectArrayAtOffset(read, pointer, count, labelPrefix, lut)!;
		}

		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="writer"></param>
		/// <param name="value"></param>
		/// <param name="lut"></param>
		public static void WriteObject<T>(this BinaryObjectWriter writer, T value, BaseLUT lut) where T : class, IBinarySerializable
		{
			lut.AddForWriter(writer, value);
			writer.WriteObject(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="writer"></param>
		/// <param name="value"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		public static void WriteObject<T, TContext>(this BinaryObjectWriter writer, T value, TContext context, BaseLUT lut) where T : class, IBinarySerializable<TContext>
		{
			lut.AddForWriter(writer, value);
			writer.WriteObject(value, context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="writer"></param>
		/// <param name="value"></param>
		/// <param name="lut"></param>
		/// <param name="alignment"></param>
		/// <param name="priority"></param>
		public static void WriteObjectOffset<T>(this BinaryObjectWriter writer, T? value, BaseLUT lut, int alignment = 0, int priority = 0) where T : class, IBinarySerializable
		{
			writer.WriteOffset(value, () => writer.WriteObject(value!, lut), alignment, priority);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="writer"></param>
		/// <param name="value"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		/// <param name="alignment"></param>
		/// <param name="priority"></param>
		public static void WriteObjectOffset<T, TContext>(this BinaryObjectWriter writer, T? value, TContext context, BaseLUT lut, int alignment = 0, int priority = 0) where T : class, IBinarySerializable<TContext>
		{
			writer.WriteOffset(value, () => writer.WriteObject(value!, context, lut), alignment, priority);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="writer"></param>
		/// <param name="items"></param>
		/// <param name="lut"></param>
		public static void WriteObjectArray<T>(this BinaryObjectWriter writer, IEnumerable<T> items, BaseLUT lut) where T : class, IBinarySerializable
		{
			lut.AddForWriter(writer, items);
			foreach(T item in items)
			{
				writer.WriteObject(item);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="writer"></param>
		/// <param name="items"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		public static void WriteObjectArray<T, TContext>(this BinaryObjectWriter writer, IEnumerable<T> items, TContext context, BaseLUT lut) where T : class, IBinarySerializable<TContext>
		{
			lut.AddForWriter(writer, items);
			foreach(T item in items)
			{
				writer.WriteObject(item, context);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="writer"></param>
		/// <param name="write"></param>
		/// <param name="items"></param>
		/// <param name="lut"></param>
		public static void WriteObjectArray<T>(this BinaryObjectWriter writer, Action<BinaryObjectWriter, T> write, IEnumerable<T> items, BaseLUT lut)
		{
			lut.AddForWriter(writer, items);
			foreach(T item in items)
			{
				write(writer, item);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="writer"></param>
		/// <param name="items"></param>
		/// <param name="lut"></param>
		/// <param name="alignment"></param>
		/// <param name="priority"></param>
		public static void WriteObjectArrayOffset<T>(this BinaryObjectWriter writer, LabeledArray<T>? items, BaseLUT lut, int alignment = 0, int priority = 0) where T : class, IBinarySerializable
		{
			writer.WriteOffset(items, () => writer.WriteObjectArray(items!, lut), alignment, priority);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="writer"></param>
		/// <param name="items"></param>
		/// <param name="context"></param>
		/// <param name="lut"></param>
		/// <param name="alignment"></param>
		/// <param name="priority"></param>
		public static void WriteObjectArrayOffset<T, TContext>(this BinaryObjectWriter writer, LabeledArray<T>? items, TContext context, BaseLUT lut, int alignment = 0, int priority = 0) where T : class, IBinarySerializable<TContext>
		{
			writer.WriteOffset(items, () => writer.WriteObjectArray(items!, context, lut), alignment, priority);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="writer"></param>
		/// <param name="write"></param>
		/// <param name="items"></param>
		/// <param name="lut"></param>
		/// <param name="alignment"></param>
		/// <param name="priority"></param>
		public static void WriteObjectArrayOffset<T>(this BinaryObjectWriter writer, Action<BinaryObjectWriter, T> write, LabeledArray<T>? items, BaseLUT lut, int alignment = 0, int priority = 0)
		{
			writer.WriteOffset(items, () => writer.WriteObjectArray(write, items!, lut), alignment, priority);
		}
	}
}
