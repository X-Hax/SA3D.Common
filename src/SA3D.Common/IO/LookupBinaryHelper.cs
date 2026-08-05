using Amicitia.IO.Binary;
using SA3D.Common.Lookup;
using System;
using System.Linq;


namespace SA3D.Common.IO
{
	/// <summary>
	/// 
	/// </summary>
	public static class LookupBinaryHelper
	{
		/// <summary>
		/// LUT object read delegate
		/// </summary>
		/// <typeparam name="T">Type of the object to read</typeparam>
		/// <param name="reader">The reader to read from</param>
		/// <param name="lutObject">the output object to read to</param>
		public delegate void ReadLUTObject<T>(BinaryObjectReader reader, T lutObject) where T : class;

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="lut"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="read"></param>
		/// <param name="create"></param>
		/// <returns></returns>
		public static T ReadLUTItem<T>(this BinaryObjectReader reader, OffsetLUT lut, string? labelPrefix, ReadLUTObject<T> read, Func<T> create) where T : class
		{
			long offset = reader.GetPositionOffset();

			if(!lut.TryGetValue(offset, out T? result))
			{
				result = create();

				if(result is ILabel labelable)
				{
					labelable.Label = lut.Labels.GetGenerateValue(offset, labelPrefix ?? labelable.LabelPrefix);
				}

				lut.AddTryLabel(offset, result);

				read(reader, result);
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="lut"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="read"></param>
		/// <param name="create"></param>
		/// <returns></returns>
		public static T? ReadLUTItemAtOffset<T>(this BinaryObjectReader reader, long offset, OffsetLUT lut, string? labelPrefix, ReadLUTObject<T> read, Func<T> create) where T : class
		{
			long resolvedOffset = reader.OffsetHandler.ResolveOffset(offset);

			if(resolvedOffset == -1)
			{
				return null;
			}

			using(reader.At(resolvedOffset, System.IO.SeekOrigin.Begin))
			{
				return reader.ReadLUTItem(lut, labelPrefix, read, create);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="lut"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="read"></param>
		/// <returns></returns>
		public static T ReadLUTItem<T>(this BinaryObjectReader reader, OffsetLUT lut, string? labelPrefix, ReadLUTObject<T> read) where T : class, new()
		{
			return reader.ReadLUTItem(lut, labelPrefix, read, () => new());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="lut"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="read"></param>
		/// <returns></returns>
		public static T? ReadLUTItemAtOffset<T>(this BinaryObjectReader reader, long offset, OffsetLUT lut, string? labelPrefix, ReadLUTObject<T> read) where T : class, new()
		{
			return reader.ReadLUTItemAtOffset(offset, lut, labelPrefix, read, () => new());
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="lut"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="read"></param>
		/// <returns></returns>
		public static LabeledArray<T> ReadLUTLabeledArray<T>(this BinaryObjectReader reader, int count, OffsetLUT lut, string labelPrefix, ReadLUTObject<T[]> read)
		{
			long offset = reader.GetPositionOffset();
			if(count == 0)
			{
				return new(lut.Labels.GetGenerateValue(offset, labelPrefix), count);
			}

			if(!lut.TryGetValue(offset, out LabeledArray<T>? result))
			{
				result = new(lut.Labels.GetGenerateValue(offset, labelPrefix), count);
				lut.AddTryLabel(offset, result);
				read(reader, result.Array);
			}
			else if(result.Length < count)
			{
				result.Array = new T[count];
				read(reader, result.Array);
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="offset"></param>
		/// <param name="lut"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="read"></param>
		/// <returns></returns>
		public static LabeledArray<T>? ReadLUTLabeledArrayAtOffset<T>(this BinaryObjectReader reader, long offset, int count, OffsetLUT lut, string labelPrefix, ReadLUTObject<T[]> read)
		{
			long resolvedOffset = reader.OffsetHandler.ResolveOffset(offset);

			if(resolvedOffset == -1)
			{
				return null;
			}

			using(reader.At(resolvedOffset, System.IO.SeekOrigin.Begin))
			{
				return reader.ReadLUTLabeledArray<T>(count, lut, labelPrefix, read);
			}
		}



		#region Read unmanaged array

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T> ReadLabeledArray<T>(this BinaryObjectReader reader, int count, string labelPrefix, OffsetLUT lut) where T : unmanaged
		{
			return reader.ReadLUTLabeledArray<T>(count, lut, labelPrefix, (r, dst) => r.ReadArray(dst.Length, dst));
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
		public static LabeledArray<T>? ReadLabeledArrayAtOffset<T>(this BinaryObjectReader reader, long offset, int count, string labelPrefix, OffsetLUT lut) where T : unmanaged
		{
			return reader.ReadLUTLabeledArrayAtOffset<T>(offset, count, lut, labelPrefix, (r, dst) => r.ReadArray(dst.Length, dst));
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
		public static LabeledArray<T>? ReadLabeledArrayOffset<T>(this BinaryObjectReader reader, int count, string labelPrefix, OffsetLUT lut) where T : unmanaged
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadLabeledArrayAtOffset<T>(offset, count, labelPrefix, lut)!;
		}

		#endregion

		#region Read Object

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T ReadObject<T>(this BinaryObjectReader reader, OffsetLUT lut) where T : class, IBinarySerializable, new()
		{
			return reader.ReadLUTItem<T>(lut, null, (r, dst) => r.ReadObject(ref dst));
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
		public static T ReadObject<T, TContext>(this BinaryObjectReader reader, TContext context, OffsetLUT lut) where T : class, IBinarySerializable<TContext>, new()
		{
			return reader.ReadLUTItem<T>(lut, null, (r, dst) => r.ReadObject(ref dst, context));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="create"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T ReadObject<T>(this BinaryObjectReader reader, ReadLUTObject<T> read, Func<T> create, OffsetLUT lut) where T : class
		{
			return reader.ReadLUTItem(lut, null, read, create);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T ReadObject<T>(this BinaryObjectReader reader, ReadLUTObject<T> read, OffsetLUT lut) where T : class, new()
		{
			return reader.ReadLUTItem(lut, null, read);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T? ReadObjectAtOffset<T>(this BinaryObjectReader reader, long offset, OffsetLUT lut) where T : class, IBinarySerializable, new()
		{
			return reader.ReadLUTItemAtOffset<T>(offset, lut, null, (r, dst) => r.ReadObject(ref dst));
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
		public static T? ReadObjectAtOffset<T, TContext>(this BinaryObjectReader reader, long offset, TContext context, OffsetLUT lut) where T : class, IBinarySerializable<TContext>, new()
		{
			return reader.ReadLUTItemAtOffset<T>(offset, lut, null, (r, dst) => r.ReadObject(ref dst, context));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="create"></param>
		/// <param name="offset"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T? ReadObjectAtOffset<T>(this BinaryObjectReader reader, ReadLUTObject<T> read, Func<T> create, long offset, OffsetLUT lut) where T : class
		{
			return reader.ReadLUTItemAtOffset(offset, lut, null, read, create);
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
		public static T? ReadObjectAtOffset<T>(this BinaryObjectReader reader, ReadLUTObject<T> read, long offset, OffsetLUT lut) where T : class, new()
		{
			return reader.ReadLUTItemAtOffset(offset, lut, null, read);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T? ReadObjectOffset<T>(this BinaryObjectReader reader, OffsetLUT lut) where T : class, IBinarySerializable, new()
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
		public static T? ReadObjectOffset<T, TContext>(this BinaryObjectReader reader, TContext context, OffsetLUT lut) where T : class, IBinarySerializable<TContext>, new()
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
		/// <param name="create"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T? ReadObjectOffset<T>(this BinaryObjectReader reader, ReadLUTObject<T> read, Func<T> create, OffsetLUT lut) where T : class
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadObjectAtOffset(read, create, offset, lut);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="read"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static T? ReadObjectOffset<T>(this BinaryObjectReader reader, ReadLUTObject<T> read, OffsetLUT lut) where T : class, new()
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadObjectAtOffset(read, offset, lut);
		}

		#endregion

		#region Read Object Array

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <param name="count"></param>
		/// <param name="labelPrefix"></param>
		/// <param name="lut"></param>
		/// <returns></returns>
		public static LabeledArray<T> ReadLabeledObjectArray<T>(this BinaryObjectReader reader, int count, string labelPrefix, OffsetLUT lut) where T : IBinarySerializable, new()
		{
			return reader.ReadLUTLabeledArray<T>(count, lut, labelPrefix, (r, dst) => r.ReadObjectArray(dst));
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
		public static LabeledArray<T> ReadLabeledObjectArray<T, TContext>(this BinaryObjectReader reader, int count, string labelPrefix, TContext context, OffsetLUT lut) where T : IBinarySerializable<TContext>, new()
		{
			return reader.ReadLUTLabeledArray<T>(count, lut, labelPrefix, (r, dst) => r.ReadObjectArray(context, dst));
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
		public static LabeledArray<T> ReadLabeledObjectArray<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, int count, string labelPrefix, OffsetLUT lut)
		{
			return reader.ReadLUTLabeledArray<T>(count, lut, labelPrefix, (r, dst) => r.ReadObjectArray(read, dst));
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
		public static LabeledArray<T>? ReadLabeledObjectArrayAtOffset<T>(this BinaryObjectReader reader, long offset, int count, string labelPrefix, OffsetLUT lut) where T : IBinarySerializable, new()
		{
			return reader.ReadLUTLabeledArrayAtOffset<T>(offset, count, lut, labelPrefix, (r, dst) => r.ReadObjectArray(dst));
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
		public static LabeledArray<T>? ReadLabeledObjectArrayAtOffset<T, TContext>(this BinaryObjectReader reader, long offset, int count, string labelPrefix, TContext context, OffsetLUT lut) where T : IBinarySerializable<TContext>, new()
		{
			return reader.ReadLUTLabeledArrayAtOffset<T>(offset, count, lut, labelPrefix, (r, dst) => r.ReadObjectArray(context, dst));
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
		public static LabeledArray<T>? ReadLabeledObjectArrayAtOffset<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, long offset, int count, string labelPrefix, OffsetLUT lut)
		{
			return reader.ReadLUTLabeledArrayAtOffset<T>(offset, count, lut, labelPrefix, (r, dst) => r.ReadObjectArray(read, dst));
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
		public static LabeledArray<T>? ReadLabeledObjectArrayOffset<T>(this BinaryObjectReader reader, int count, string labelPrefix, OffsetLUT lut) where T : IBinarySerializable, new()
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
		public static LabeledArray<T>? ReadLabeledObjectArrayOffset<T, TContext>(this BinaryObjectReader reader, int count, string labelPrefix, TContext context, OffsetLUT lut) where T : IBinarySerializable<TContext>, new()
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
		public static LabeledArray<T>? ReadLabeledObjectArrayOffset<T>(this BinaryObjectReader reader, Func<BinaryObjectReader, T> read, int count, string labelPrefix, OffsetLUT lut)
		{
			long offset = reader.ReadOffsetValue();
			return reader.ReadLabeledObjectArrayAtOffset(read, offset, count, labelPrefix, lut)!;
		}

		#endregion

		#region Write

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="writer"></param>
		/// <param name="value"></param>
		/// <param name="lut"></param>
		public static void WriteObject<T>(this BinaryObjectWriter writer, T value, OffsetLUT lut) where T : class, IBinarySerializable
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
		public static void WriteObject<T, TContext>(this BinaryObjectWriter writer, T value, TContext context, OffsetLUT lut) where T : class, IBinarySerializable<TContext>
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
		/// <param name="write"></param>
		/// <param name="lut"></param>
		public static void WriteObject<T>(this BinaryObjectWriter writer, T value, Action<BinaryObjectWriter, T> write, OffsetLUT lut) where T : class
		{
			lut.AddForWriter(writer, value);
			write(writer, value);
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
		public static void WriteObjectOffset<T>(this BinaryObjectWriter writer, T? value, OffsetLUT lut, int alignment = 0, int priority = 0) where T : class, IBinarySerializable
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
		public static void WriteObjectOffset<T, TContext>(this BinaryObjectWriter writer, T? value, TContext context, OffsetLUT lut, int alignment = 0, int priority = 0) where T : class, IBinarySerializable<TContext>
		{
			writer.WriteOffset(value, () => writer.WriteObject(value!, context, lut), alignment, priority);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="writer"></param>
		/// <param name="value"></param>
		/// <param name="write"></param>
		/// <param name="lut"></param>
		/// <param name="alignment"></param>
		/// <param name="priority"></param>
		public static void WriteObjectOffset<T>(this BinaryObjectWriter writer, T? value, Action<BinaryObjectWriter, T> write, OffsetLUT lut, int alignment = 0, int priority = 0) where T : class
		{
			writer.WriteOffset(value, () => writer.WriteObject(value!, write, lut), alignment, priority);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="writer"></param>
		/// <param name="items"></param>
		/// <param name="lut"></param>
		public static void WriteArray<T>(this BinaryObjectWriter writer, LabeledArray<T> items, OffsetLUT lut) where T : unmanaged
		{
			lut.AddForWriter(writer, items);
			writer.WriteArray(items.ToArray());
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
		public static void WriteArrayOffset<T>(this BinaryObjectWriter writer, LabeledArray<T>? items, OffsetLUT lut, int alignment = 0, int priority = 0) where T : unmanaged
		{
			writer.WriteOffset(items, () => writer.WriteArray(items!, lut), alignment, priority);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="writer"></param>
		/// <param name="items"></param>
		/// <param name="lut"></param>
		public static void WriteObjectArray<T>(this BinaryObjectWriter writer, LabeledArray<T> items, OffsetLUT lut) where T : IBinarySerializable
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
		public static void WriteObjectArray<T, TContext>(this BinaryObjectWriter writer, LabeledArray<T> items, TContext context, OffsetLUT lut) where T : IBinarySerializable<TContext>
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
		public static void WriteObjectArray<T>(this BinaryObjectWriter writer, Action<BinaryObjectWriter, T> write, LabeledArray<T> items, OffsetLUT lut)
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
		public static void WriteObjectArrayOffset<T>(this BinaryObjectWriter writer, LabeledArray<T>? items, OffsetLUT lut, int alignment = 0, int priority = 0) where T : IBinarySerializable
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
		public static void WriteObjectArrayOffset<T, TContext>(this BinaryObjectWriter writer, LabeledArray<T>? items, TContext context, OffsetLUT lut, int alignment = 0, int priority = 0) where T : IBinarySerializable<TContext>
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
		public static void WriteObjectArrayOffset<T>(this BinaryObjectWriter writer, Action<BinaryObjectWriter, T> write, LabeledArray<T>? items, OffsetLUT lut, int alignment = 0, int priority = 0)
		{
			writer.WriteOffset(items, () => writer.WriteObjectArray(write, items!, lut), alignment, priority);
		}

		#endregion
	}
}
