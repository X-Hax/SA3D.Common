using Amicitia.IO.Binary;
using System.Numerics;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Read/Write extensions for built-in .NET data types
	/// </summary>
	public static class StructBinaryHelper
	{
		// use these to avoid AOT endian byte swapping issues

		/// <summary>
		/// Read a <see cref="Vector2"/> from a <see cref="BinaryValueReader"/>
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		public static Vector2 ReadVector2(this BinaryValueReader reader)
		{
			return new Vector2(
				reader.ReadSingle(),
				reader.ReadSingle()
			);
		}

		/// <summary>
		/// Write a <see cref="Vector2"/> value to a <see cref="BinaryValueWriter"/>
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="value">The value to write</param>
		public static void WriteVector2(this BinaryValueWriter writer, Vector2 value)
		{
			writer.WriteSingle(value.X);
			writer.WriteSingle(value.Y);
		}

		/// <summary>
		/// Read a <see cref="Vector2"/> array from a <see cref="BinaryValueReader"/>
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		/// <param name="count">The size of the array</param>
		public static Vector2[] ReadVector2Array(this BinaryValueReader reader, int count)
		{
			Vector2[] result = new Vector2[count];

			for (int i = 0; i < count; i++)
			{
				result[i] = reader.ReadVector2();
			}

			return result;
		}

		/// <summary>
		/// Write a <see cref="Vector2"/> array to a <see cref="BinaryValueWriter"/>
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="array">The array to write</param>
		public static void WriteVector2Array(this BinaryValueWriter writer, Vector2[] array)
		{
			foreach (Vector2 value in array)
			{
				writer.WriteVector2(value);
			}
		}

		/// <summary>
		/// Read a <see cref="Vector3"/> from a <see cref="BinaryValueReader"/>
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		public static Vector3 ReadVector3(this BinaryValueReader reader)
		{
			return new Vector3(
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle()
			);
		}

		/// <summary>
		/// Write a <see cref="Vector3"/> value to a <see cref="BinaryValueWriter"/>
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="value">The value to write</param>
		public static void WriteVector3(this BinaryValueWriter writer, Vector3 value)
		{
			writer.WriteSingle(value.X);
			writer.WriteSingle(value.Y);
			writer.WriteSingle(value.Z);
		}

		/// <summary>
		/// Read a <see cref="Vector2"/> array from a <see cref="BinaryValueReader"/>
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		/// <param name="count">The size of the array</param>
		public static Vector3[] ReadVector3Array(this BinaryValueReader reader, int count)
		{
			Vector3[] result = new Vector3[count];

			for(int i = 0; i < count; i++)
			{
				result[i] = reader.ReadVector3();
			}

			return result;
		}

		/// <summary>
		/// Write a <see cref="Vector3"/> array to a <see cref="BinaryValueWriter"/>
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="array">The array to write</param>
		public static void WriteVector3Array(this BinaryValueWriter writer, Vector3[] array)
		{
			foreach (Vector3 value in array)
			{
				writer.WriteVector3(value);
			}
		}

		/// <summary>
		/// Read a <see cref="Vector4"/> from a <see cref="BinaryValueReader"/>
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		public static Vector4 ReadVector4(this BinaryValueReader reader)
		{
			return new Vector4(
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle()
			);
		}

		/// <summary>
		/// Write a <see cref="Vector4"/> value to a <see cref="BinaryValueWriter"/>
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="value">The value to write</param>
		public static void WriteVector4(this BinaryValueWriter writer, Vector4 value)
		{
			writer.WriteSingle(value.X);
			writer.WriteSingle(value.Y);
			writer.WriteSingle(value.Z);
			writer.WriteSingle(value.W);
		}

		/// <summary>
		/// Read a <see cref="Quaternion"/> from a <see cref="BinaryValueReader"/>
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		public static Quaternion ReadQuaternion(this BinaryValueReader reader)
		{
			return new Quaternion(
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle()
			);
		}

		/// <summary>
		/// Write a <see cref="Quaternion"/> value to a <see cref="BinaryValueWriter"/>
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="value">The value to write</param>
		public static void WriteQuaternion(this BinaryValueWriter writer, Quaternion value)
		{
			writer.WriteSingle(value.X);
			writer.WriteSingle(value.Y);
			writer.WriteSingle(value.Z);
			writer.WriteSingle(value.W);
		}

		/// <summary>
		/// Read a <see cref="Matrix4x4"/> from a <see cref="BinaryValueReader"/>
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		public static Matrix4x4 ReadMatrix4x4(this BinaryValueReader reader)
		{
			return new Matrix4x4(
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle(),
				reader.ReadSingle()
			);
		}

		/// <summary>
		/// Write a <see cref="Matrix4x4"/> value to a <see cref="BinaryValueWriter"/>
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="value">The value to write</param>
		public static void WriteMatrix4x4(this BinaryValueWriter writer, Matrix4x4 value)
		{
			writer.WriteSingle(value.M11);
			writer.WriteSingle(value.M12);
			writer.WriteSingle(value.M13);
			writer.WriteSingle(value.M14);
			writer.WriteSingle(value.M21);
			writer.WriteSingle(value.M22);
			writer.WriteSingle(value.M23);
			writer.WriteSingle(value.M24);
			writer.WriteSingle(value.M31);
			writer.WriteSingle(value.M32);
			writer.WriteSingle(value.M33);
			writer.WriteSingle(value.M34);
			writer.WriteSingle(value.M41);
			writer.WriteSingle(value.M42);
			writer.WriteSingle(value.M43);
			writer.WriteSingle(value.M44);
		}
	}

}

