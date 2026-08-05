using System;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Exception thrown when not able to read a file
	/// </summary>
	public class CannotReadFileException : InvalidOperationException
	{
		/// <summary>
		/// The type that attempted to read the file
		/// </summary>
		public Type ReadType { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CannotReadFileException"/> class.
		/// </summary>
		/// <param name="readType">The type that attempted to read the file</param>
		public CannotReadFileException(Type readType) : base()
		{
			ReadType = readType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CannotReadFileException"/> class with a specified error message.
		/// </summary>
		/// <param name="readType">The type that attempted to read the file</param>
		/// <param name="message">The message</param>
		public CannotReadFileException(Type readType, string? message) : base(message)
		{
			ReadType = readType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CannotReadFileException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="readType">The type that attempted to read the file</param>
		/// <param name="message">The message</param>
		/// <param name="innerException">The exception that is the cause of this exception</param>
		public CannotReadFileException(Type readType, string? message, Exception? innerException) : base(message, innerException)
		{
			ReadType = readType;
		}
	}
}
