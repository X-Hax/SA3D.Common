using System.Collections.Generic;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Endian stack base handler class.
	/// </summary>
	public abstract class EndianStack
	{
		private readonly Stack<bool> _endianStack;

		/// <summary>
		/// Whether bytes should be read in big endian. 
		/// <br/> Set with <see cref="PushBigEndian(bool)"/> and free afterwards with <see cref="PopEndian"/>.
		/// </summary>
		public bool BigEndian { get; private set; }


		/// <summary>
		/// Creates a new endian stack.
		/// </summary>
		/// <param name="bigEndian">Whether to start with big endian.</param>
		protected EndianStack(bool bigEndian)
		{
			_endianStack = new();
			_endianStack.Push(bigEndian);
			BigEndian = bigEndian;
		}


		/// <summary>
		/// Sets an endian. Dont forget to free it afterwards as well using <see cref="PopEndian"/>.
		/// </summary>
		/// <param name="bigEndian">New bigendian mode.</param>
		public void PushBigEndian(bool bigEndian)
		{
			_endianStack.Push(bigEndian);
			BigEndian = bigEndian;
			OnEndianUpdate();
		}

		/// <summary>
		/// Pops the last endian set via <see cref="PushBigEndian(bool)"/>.
		/// </summary>
		public void PopEndian()
		{
			_endianStack.Pop();
			BigEndian = _endianStack.Peek();
			OnEndianUpdate();
		}

		/// <summary>
		/// Gets called when the endiannes updates.
		/// </summary>
		protected abstract void OnEndianUpdate();
	}
}
