using System;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Exception thrown when not able to Write a file
	/// </summary>
	public class CannotWriteFileException : InvalidOperationException
	{
		/// <summary>
		/// The type that attempted to Write the file
		/// </summary>
		public Type WrittenType { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CannotWriteFileException"/> class.
		/// </summary>
		/// <param name="writtenType">The type that attempted to write the file</param>
		public CannotWriteFileException(Type writtenType) : base()
		{
			WrittenType = writtenType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CannotWriteFileException"/> class with a specified error message.
		/// </summary>
		/// <param name="writtenType">The type that attempted to write the file</param>
		/// <param name="message">The message</param>
		public CannotWriteFileException(Type writtenType, string? message) : base(message)
		{
			WrittenType = writtenType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CannotWriteFileException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="writtenType">The type that attempted to write the file</param>
		/// <param name="message">The message</param>
		/// <param name="innerException">The exception that is the cause of this exception</param>
		public CannotWriteFileException(Type writtenType, string? message, Exception? innerException) : base(message, innerException)
		{
			WrittenType = writtenType;
		}
	}
}
