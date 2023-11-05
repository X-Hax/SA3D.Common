using Reloaded.Memory.Streams;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SA3D.Common.IO
{
	/// <summary>
	/// Allows for interchangeably reading little and big endian from an array. The changed endian gets stored on a stack and can be popped later.
	/// </summary>
	[DebuggerNonUserCode]
	public class EndianStackReader : EndianStack, IDisposable
	{
		#region Private fields

		private readonly byte[] _source;
		private readonly GCHandle _sourceHandle;
		private readonly BigEndianReader _bigEndianReader;
		private readonly LittleEndianReader _littleEndianReader;
		private bool _disposed;

		#endregion

		#region Properties

		/// <summary>
		/// Readonly access to the Source
		/// </summary>
		public ReadOnlySpan<byte> Source => _source;

		/// <summary>
		/// Imagebase (pointer offset) to use when reading pointers with <see cref="ReadPointer(uint)"/> or <see cref="TryReadPointer(uint, out uint)"/>
		/// </summary>
		public uint ImageBase { get; set; }

		/// <summary>
		/// A 32-bit integer that represents the number of bytes in the source.
		/// </summary>
		public int Length => _source.Length;

		/// <summary>
		/// A 64-bit integer that represents the number of bytes in the source.
		/// </summary>
		public long LongLength => _source.LongLength;

		#endregion

		/// <summary>
		/// Creates a new endian stack reader.
		/// </summary>
		/// <param name="source">Byte source that should be read from.</param>
		/// <param name="imageBase">The pointer offset to use.</param>
		/// <param name="bigEndian">Whether to start with big endian.</param>
		public unsafe EndianStackReader(byte[] source, uint imageBase = 0, bool bigEndian = false) : base(bigEndian)
		{
			_source = source;
			_sourceHandle = GCHandle.Alloc(_source, GCHandleType.Pinned);
			ImageBase = imageBase;

			_bigEndianReader = new((byte*)_sourceHandle.AddrOfPinnedObject());
			_littleEndianReader = new((byte*)_sourceHandle.AddrOfPinnedObject());
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			if(_disposed)
			{
				return;
			}

			_disposed = true;
			_sourceHandle.Free();
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Finalizer for freeing up the GC handle.
		/// </summary>
		~EndianStackReader() => Dispose();


		#region Methods

		/// <summary>
		/// Returns the byte at a specific index from the source.
		/// </summary>
		/// <param name="index">Index from which to read the byte</param>
		/// <returns>The byte at the index.</returns>
		public virtual byte this[uint index] => _source[index];

		/// <summary>
		/// Reads a set of bytes at a given address from the source.
		/// </summary>
		/// <param name="address">The address at which to read the bytes.</param>
		/// <param name="length">The number of bytes to read.</param>
		/// <returns>The bytes that were read.</returns>
		public virtual byte[] ReadBytes(uint address, int length)
		{
			return _source[(int)address..(int)(address + length)];
		}

		/// <summary>
		/// Reads a set of bytes at a given addres from the source into a destination array.
		/// </summary>
		/// <param name="sourceAddress">The address at which to read the bytes.</param>
		/// <param name="destination">The array to read the bytes into.</param>
		/// <param name="destinationAddress">The address in the destination array to which to write the bytes to.</param>
		/// <param name="length">The number of bytes to read.</param>
		public virtual void ReadBytes(uint sourceAddress, byte[] destination, uint destinationAddress, int length)
		{
			Array.Copy(_source, sourceAddress, destination, destinationAddress, length);
		}

		/// <summary>
		/// Reads an unsigned byte at a given address from the source.
		/// </summary>
		/// <param name="address">The address at which to read the unsigned byte.</param>
		/// <returns>The unsigned byte that was read.</returns>
		public virtual byte ReadByte(uint address)
		{
			return _source[address];
		}

		/// <summary>
		/// Reads a signed byte at a given address from the source.
		/// </summary>
		/// <param name="address">The address at which to read the signed byte.</param>
		/// <returns>The signed byte read.</returns>
		public virtual sbyte ReadSByte(uint address)
		{
			return unchecked((sbyte)_source[address]);
		}

		/// <summary>
		/// Reads a signed short at a given address from the source.
		/// </summary>
		/// <param name="address">The address at which to read the unsigned short.</param>
		/// <returns>The unsigned short that was read.</returns>
		public virtual short ReadShort(uint address)
		{
			if(BigEndian)
			{
				return _bigEndianReader.ReadShortAtOffset((int)address);
			}
			else
			{
				return _littleEndianReader.ReadShortAtOffset((int)address);
			}
		}

		/// <summary>
		/// Reads an unsigned short at a given address from the source.
		/// </summary>
		/// <param name="address">The address at which to read the unsigned short.</param>
		/// <returns>The unsigned short that was read.</returns>
		public virtual ushort ReadUShort(uint address)
		{
			if(BigEndian)
			{
				return _bigEndianReader.ReadUShortAtOffset((int)address);
			}
			else
			{
				return _littleEndianReader.ReadUShortAtOffset((int)address);
			}
		}

		/// <summary>
		/// Reads a signed integer at a given address from the source.
		/// </summary>
		/// <param name="address">The address at which to read the signed integer.</param>
		/// <returns>The signed integer that was read.</returns>
		public virtual int ReadInt(uint address)
		{
			if(BigEndian)
			{
				return _bigEndianReader.ReadIntAtOffset((int)address);
			}
			else
			{
				return _littleEndianReader.ReadIntAtOffset((int)address);
			}
		}

		/// <summary>
		/// Reads an unsigned integer at a given address from the source.
		/// </summary>
		/// <param name="address">The address at which to read the unsigned integer.</param>
		/// <returns>The unsigned integer that was read.</returns>
		public virtual uint ReadUInt(uint address)
		{
			if(BigEndian)
			{
				return _bigEndianReader.ReadUIntAtOffset((int)address);
			}
			else
			{
				return _littleEndianReader.ReadUIntAtOffset((int)address);
			}
		}

		/// <summary>
		/// Reads a signed long at a given address from the source.
		/// </summary>
		/// <param name="address">The address at which to read the signed long.</param>
		/// <returns>The signed long that was read.</returns>
		public virtual long ReadLong(uint address)
		{
			if(BigEndian)
			{
				return _bigEndianReader.ReadLongAtOffset((int)address);
			}
			else
			{
				return _littleEndianReader.ReadLongAtOffset((int)address);
			}
		}

		/// <summary>
		/// Reads an unsigned long at a given address from the source.
		/// </summary>
		/// <param name="address">The address at which to read the unsigned long.</param>
		/// <returns>The unsigned long that was read.</returns>
		public virtual ulong ReadULong(uint address)
		{
			if(BigEndian)
			{
				return _bigEndianReader.ReadULongAtOffset((int)address);
			}
			else
			{
				return _littleEndianReader.ReadULongAtOffset((int)address);
			}
		}

		/// <summary>
		/// Reads a float at a given address from the source.
		/// </summary>
		/// <param name="address">The address at which to read the float.</param>
		/// <returns>The float that was read.</returns>
		public virtual float ReadFloat(uint address)
		{
			if(BigEndian)
			{
				return _bigEndianReader.ReadFloatAtOffset((int)address);
			}
			else
			{
				return _littleEndianReader.ReadFloatAtOffset((int)address);
			}
		}

		/// <summary>
		/// Reads a double at a given address from the source.
		/// </summary>
		/// <param name="address">The address at which to read the double.</param>
		/// <returns>The double that was read.</returns>
		public virtual double ReadDouble(uint address)
		{
			if(BigEndian)
			{
				return _bigEndianReader.ReadDoubleAtOffset((int)address);
			}
			else
			{
				return _littleEndianReader.ReadDoubleAtOffset((int)address);
			}
		}


		/// <summary>
		/// Reads an unsigned integer from the source and subtracts the <see cref="ImageBase"/>.
		/// <br/> 0 Remains 0.
		/// </summary>
		/// <param name="address">The address at which to read the pointer.</param>
		/// <returns>The pointer that was read.</returns>
		public uint ReadPointer(uint address)
		{
			uint tmp = ReadUInt(address);
			return tmp == 0 ? 0 : tmp - ImageBase;
		}

		/// <summary>
		/// Attempts to read a pointer from the source. 
		/// <br/>Returns False if the unsigned integer is 0. 
		/// <br/>If not 0, subtracts the <see cref="ImageBase"/> and returns true.
		/// </summary>
		/// <param name="address">The address at which to read the pointer.</param>
		/// <param name="pointer">The resulting pointer.</param>
		/// <returns>Whether the pointer is 0.</returns>
		public bool TryReadPointer(uint address, out uint pointer)
		{
			pointer = ReadUInt(address);
			if(pointer != 0)
			{
				pointer -= ImageBase;
				return true;
			}

			return false;
		}


		/// <summary>
		/// Reads a string of any encoding at a given address from the source.
		/// </summary>
		/// <param name="address">The Address at which to read the string.</param>
		/// <param name="encoding">Encoding to read the string with.</param>
		/// <param name="count">Length of the string to read.</param>
		/// <returns>The string that was read.</returns>
		public virtual string ReadString(uint address, Encoding encoding, uint count)
		{
			if(count == 0)
			{
				return "";
			}

			return encoding.GetString(_source, (int)address, (int)count);
		}

		/// <summary>
		/// Reads an ASCII string at a given address from the source.
		/// </summary>
		/// <param name="address">The Address at which to read the string.</param>
		/// <param name="count">Length of the string to read.</param>
		/// <returns>The string that was read.</returns>
		public string ReadString(uint address, uint count)
		{
			return ReadString(address, Encoding.UTF8, count);
		}

		/// <summary>
		/// Reads a null terminated string of any encoding at a given address from the source.
		/// </summary>
		/// <param name="address">The Address at which to read the string.</param>
		/// <param name="encoding">Encoding to read the string with.</param>
		/// <param name="byteLength">Number of bytes read (excluding the null terminator)</param>
		/// <returns>The string that was read.</returns>
		public string ReadNullterminatedString(uint address, Encoding encoding, out uint byteLength)
		{
			byteLength = 0;
			while(_source[address + byteLength] != 0)
			{
				byteLength++;
			}

			return ReadString(address, encoding, byteLength);
		}

		/// <summary>
		/// Reads a null terminated string of any encoding at a given address from the source.
		/// </summary>
		/// <param name="address">The Address at which to read the string.</param>
		/// <param name="encoding">Encoding to read the string with.</param>
		/// <returns>The string that was read.</returns>
		public string ReadNullterminatedString(uint address, Encoding encoding)
		{
			return ReadNullterminatedString(address, encoding, out _);
		}

		/// <summary>
		/// Reads a null-terminated ASCII terminated string at a given address from the source.
		/// </summary>
		/// <param name="address">The Address at which to read the string.</param>
		/// <param name="byteLength">Number of bytes read (excluding the null terminator)</param>
		/// <returns>The string that was read.</returns>
		public string ReadNullterminatedString(uint address, out uint byteLength)
		{
			return ReadNullterminatedString(address, Encoding.UTF8, out byteLength);
		}

		/// <summary>
		/// Reads a null-terminated ASCII terminated string at a given address from the source.
		/// </summary>
		/// <param name="address">The Address at which to read the string.</param>
		/// <returns>The string that was read.</returns>
		public string ReadNullterminatedString(uint address)
		{
			return ReadNullterminatedString(address, out _);
		}

		/// <summary>
		/// Reads a null terminated string of any encoding at a given address from the source, but stops reading after a set number of bytes.
		/// </summary>
		/// <param name="address">The Address at which to read the string.</param>
		/// <param name="encoding">Encoding to read the string with.</param>
		/// <param name="limit">Maximum number of bytes to read.</param>
		/// <param name="byteLength">Number of bytes read (excluding the null terminator)</param>
		/// <returns>The string that was read.</returns>
		public string ReadStringLimited(uint address, Encoding encoding, int limit, out uint byteLength)
		{
			byteLength = 0;
			while(byteLength <= limit && _source[address + byteLength] != 0)
			{
				byteLength++;
			}

			return ReadString(address, encoding, byteLength);
		}

		/// <summary>
		/// Reads a null terminated ASCII string at a given address from the source, but stops reading after a set number of bytes.
		/// </summary>
		/// <param name="address">The Address at which to read the string.</param>
		/// <param name="limit">Maximum number of bytes to read.</param>
		/// <param name="byteLength">Number of bytes read (excluding the null terminator)</param>
		/// <returns>The string that was read.</returns>
		public string ReadStringLimited(uint address, int limit, out uint byteLength)
		{
			return ReadStringLimited(address, Encoding.UTF8, limit, out byteLength);
		}


		/// <summary>
		/// Forms a read only slice out of the source at a specified address for a specified length.
		/// </summary>
		/// <param name="address">Address at which to start the slice.</param>
		/// <param name="length">Number of bytes to create the slice of.</param>
		/// <returns>The created slice.</returns>
		public virtual ReadOnlySpan<byte> Slice(int address, int length)
		{
			return Source.Slice(address, length);
		}

		/// <summary>
		/// Forms a read only slice out of the source at a specified address for a specified length.
		/// </summary>
		/// <param name="address">Address at which to start the slice.</param>
		/// <param name="length">Number of bytes to create the slice of.</param>
		/// <returns>The created slice.</returns>
		public ReadOnlySpan<byte> Slice(uint address, int length)
		{
			return Slice((int)address, length);
		}

		/// <summary>
		/// Forms a read only slice out of the source at a specified address until the end of the source.
		/// </summary>
		/// <param name="address">Address at which to start the slice.</param>
		/// <returns>The created slice.</returns>
		public ReadOnlySpan<byte> Slice(int address)
		{
			return Slice(address, Length - address);
		}

		/// <summary>
		/// Forms a read only slice out of the source at a specified address until the end of the source.
		/// </summary>
		/// <param name="address">Address at which to start the slice.</param>
		/// <returns>The created slice.</returns>
		public ReadOnlySpan<byte> Slice(uint address)
		{
			return Slice((int)address);
		}


		/// <summary>
		/// Reads 16 bits at a given address in Big and little endian and returns whether the little endian valua is bigger than its big endian equivalent.
		/// </summary>
		/// <param name="address">The address at which to check for endianness.</param>
		/// <returns>Whether the little endian value is greater than the big endian value.</returns>
		public bool CheckBigEndian16(uint address)
		{
			PushBigEndian(false);
			uint little = ReadUShort(address);
			PopEndian();

			PushBigEndian(true);
			uint big = ReadUShort(address);
			PopEndian();

			return little > big;
		}

		/// <summary>
		/// Reads 32 bits at a given address in Big and little endian and returns whether the little endian valua is bigger than its big endian equivalent.
		/// </summary>
		/// <param name="address">The address at which to check for endianness.</param>
		/// <returns>Whether the little endian value is greater than the big endian value.</returns>
		public bool CheckBigEndian32(uint address)
		{
			PushBigEndian(false);
			uint little = ReadUInt(address);
			PopEndian();

			PushBigEndian(true);
			uint big = ReadUInt(address);
			PopEndian();

			return little > big;
		}

		#endregion
	}
}
