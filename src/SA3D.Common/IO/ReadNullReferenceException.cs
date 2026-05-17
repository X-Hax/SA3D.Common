using Amicitia.IO.Binary;
using System;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Exception for when a read resulted in null when it should not
	/// </summary>
	public class ReadNullReferenceException : NullReferenceException
	{
		/// <summary>
		/// The reader that threw the error
		/// </summary>
		public BinaryValueReader Reader { get; }

		/// <summary>
		/// The struct that tried to read the field
		/// </summary>
		public string StructName { get; }

		/// <summary>
		/// The field that was attempted to be read
		/// </summary>
		public string FieldName { get; }

		/// <summary>
		/// The offset at which the field was attempted to be read ends
		/// </summary>
		public long Offset { get; }

		/// <summary>
		/// Creates a new exception
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="structName"></param>
		/// <param name="fieldName"></param>
		public ReadNullReferenceException(BinaryValueReader reader, string structName, string fieldName)
			: base($"The field \"{structName}.{fieldName}\" at {reader.Position:X8} yielded a null reference")
		{
			Reader = reader;
			StructName = structName;
			FieldName = fieldName;
			Offset = reader.Position;
		}

		/// <summary>
		/// Creates a new exception
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="structName"></param>
		/// <param name="fieldName"></param>
		/// <param name="offset"></param>
		public ReadNullReferenceException(BinaryValueReader reader, string structName, string fieldName, long offset)
			: base($"THe field \"{structName}.{fieldName}\" at {offset:X8} yielded a null reference")
		{
			Reader = reader;
			StructName = structName;
			FieldName = fieldName;
			Offset = offset;
		}
	}
}
