using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Streams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace SA3D.Common.IO
{
    /// <summary>
    /// Allows for interchangeably writing little and big endian to a stream. The changed endian gets stored on a stack and can be popped later.
    /// </summary>
    [DebuggerNonUserCode]
    public class EndianStackWriter
    {
        #region Private Fields

        private readonly Stack<bool> _endianStack;
        private readonly byte[] _endianWriterBuffer;
        private IEndianWriter _endianWriter;
        private readonly BigEndianWriter _bigEndianWriter;
        private readonly LittleEndianWriter _littleEndianWriter;

        #endregion

        #region Properties

        /// <summary>
        /// Stream the writer is writing to.
        /// </summary>
        public Stream Stream { get; }

        /// <summary>
        /// Position the stream is currently at.
        /// </summary>
        public uint Position => (uint)Stream.Position;

        /// <summary>
        /// Imagebase (pointer offset) to use for <see cref="PointerPosition"/>.
        /// </summary>
        public uint ImageBase { get; set; }

        /// <summary>
        /// Gets the <see cref="Position"/> with <see cref="ImageBase"/>.
        /// </summary>
        public uint PointerPosition => Position + ImageBase;

        /// <summary>
        /// Whether bytes should be read in big endian. Set with <see cref="PushBigEndian(bool)"/> and freed afterwards with <see cref="PopEndian"/>.
        /// </summary>
        public bool BigEndian { get; private set; }

        #endregion

        /// <summary>
        /// Creates a new endian stack writer.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="imageBase">The pointer offset to use.</param>
        /// <param name="bigEndian">Whether to start with big endian.</param>
        public unsafe EndianStackWriter(Stream stream, uint imageBase = 0, bool bigEndian = false)
        {
            Stream = stream;
            ImageBase = imageBase;

            _endianStack = new();
            _endianWriterBuffer = new byte[8];

            fixed (byte* source = _endianWriterBuffer)
            {
                _bigEndianWriter = new(source);
                _littleEndianWriter = new(source);
            }

            PushBigEndian(bigEndian);
        }

        #region Methods

        /// <summary>
        /// Sets the endian. Has to be freed afterwards using <see cref="PopEndian"/>.
        /// </summary>
        /// <param name="bigEndian">New bigendian mode.</param>
        [MemberNotNull(nameof(_endianWriter))]
        public void PushBigEndian(bool bigEndian)
        {
            _endianStack.Push(bigEndian);
            BigEndian = bigEndian;
            _endianWriter = BigEndian ? _bigEndianWriter : _littleEndianWriter;
        }

        /// <summary>
        /// Pops the last endian set via <see cref="PushBigEndian(bool)"/>
        /// </summary>
        [MemberNotNull(nameof(_endianWriter))]
        public void PopEndian()
        {
            BigEndian = _endianStack.Pop();
            _endianWriter = BigEndian ? _bigEndianWriter : _littleEndianWriter;
        }


        /// <summary>
        /// Sets the stream position to the beginning of the stream.
        /// </summary>
        public void SeekStart()
        {
            Stream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Sets the stream position to the end of the stream.
        /// </summary>
        public void SeekEnd()
        {
            Stream.Seek(0, SeekOrigin.End);
        }

        /// <summary>
        /// Sets the stream position.
        /// </summary>
        public void Seek(uint offset, SeekOrigin origin)
        {
            Stream.Seek(offset, origin);
        }


        /// <summary>
        /// Writes number of empty bytes
        /// </summary>
        /// <param name="length">Number of empty bytes to write</param>
        public virtual void WriteEmpty(uint length)
        {
            Stream.Write(new byte[length]);
        }

        /// <summary>
        /// Writes a byte to the stream.
        /// </summary>
        /// <param name="data">The byte to write.</param>
        public virtual void WriteByte(byte data)
        {
            Stream.WriteByte(data);
        }

        /// <summary>
        /// Writes a signed byte to the stream.
        /// </summary>
        /// <param name="data">The signed byte to write.</param>
        public virtual void WriteSByte(sbyte data)
        {
            Stream.WriteByte(unchecked((byte)data));
        }

        /// <summary>
        /// Writes multiple bytes to the stream.
        /// </summary>
        /// <param name="data">The bytes to write.</param>
        public virtual void Write(ReadOnlySpan<byte> data)
        {
            Stream.Write(data);
        }

        /// <summary>
        /// Writes a signed short to the stream.
        /// </summary>
        /// <param name="data">The signed short to write.</param>
        public virtual void WriteShort(short data)
        {
            _endianWriter.WriteAtOffset(data, 0);
            Stream.Write(_endianWriterBuffer, 0, 2);
        }

        /// <summary>
        /// Writes an unsigned short to the stream.
        /// </summary>
        /// <param name="data">The unsigned short to write.</param>
        public virtual void WriteUShort(ushort data)
        {
            _endianWriter.WriteAtOffset(data, 0);
            Stream.Write(_endianWriterBuffer, 0, 2);
        }

        /// <summary>
        /// Writes a signed integer to the stream.
        /// </summary>
        /// <param name="data">The signed integer to write.</param>
        public virtual void WriteInt(int data)
        {
            _endianWriter.WriteAtOffset(data, 0);
            Stream.Write(_endianWriterBuffer, 0, 4);
        }

        /// <summary>
        /// Writes an unsigned integer to the stream.
        /// </summary>
        /// <param name="data">The unsigned integer to write.</param>
        public virtual void WriteUInt(uint data)
        {
            _endianWriter.WriteAtOffset(data, 0);
            Stream.Write(_endianWriterBuffer, 0, 4);
        }

        /// <summary>
        /// Writes a signed long to the stream.
        /// </summary>
        /// <param name="data">The signed long to write.</param>
        public virtual void WriteLong(long data)
        {
            _endianWriter.WriteAtOffset(data, 0);
            Stream.Write(_endianWriterBuffer, 0, 8);
        }

        /// <summary>
        /// Writes an unsigned long to the stream.
        /// </summary>
        /// <param name="data">The unsigned long to write.</param>
        public virtual void WriteULong(ulong data)
        {
            _endianWriter.WriteAtOffset(data, 0);
            Stream.Write(_endianWriterBuffer, 0, 8);
        }

        /// <summary>
        /// Writes a float to the stream.
        /// </summary>
        /// <param name="data">The float to write.</param>
        public virtual void WriteFloat(float data)
        {
            _endianWriter.WriteAtOffset(data, 0);
            Stream.Write(_endianWriterBuffer, 0, 4);
        }

        /// <summary>
        /// Writes a double to the stream.
        /// </summary>
        /// <param name="data">The double to write.</param>
        public virtual void WriteDouble(double data)
        {
            _endianWriter.WriteAtOffset(data, 0);
            Stream.Write(_endianWriterBuffer, 0, 8);
        }


        /// <summary>
        /// Writes a string of any encoding to the stream.
        /// </summary>
        /// <param name="data">The string to write.</param>
        /// <param name="encoding">Encoding to use for writing the string.</param>
        public void WriteString(string data, Encoding encoding)
        {
            Write(encoding.GetBytes(data));
        }

        /// <summary>
        /// Writes an ASCII string to the stream.
        /// </summary>
        /// <param name="data">The string to write.</param>
        public void WriteString(string data)
        {
            WriteString(data, Encoding.ASCII);
        }

        /// <summary>
        /// Writes a string of any encoding to the stream and pads/trims it to fit a specific length exactly.
        /// </summary>
        /// <param name="data">The string to write.</param>
        /// <param name="length">How many bytes should be written.</param>
        /// <param name="encoding">The encoding to use.</param>
        public void WriteString(string data, int length, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(data);

            if (bytes.Length > length)
            {
                Array.Resize(ref bytes, length);
            }

            Write(bytes);

            if (bytes.Length < length)
            {
                WriteEmpty((uint)(length - bytes.Length));
            }
        }

        /// <summary>
        /// Writes an ASCII string to the stream and pads/trims it to fit a specific length exactly.
        /// </summary>
        /// <param name="data">The string to write.</param>
        /// <param name="length">How many bytes should be written.</param>
        public void WriteString(string data, int length)
        {
            WriteString(data, length, Encoding.ASCII);
        }

        /// <summary>
        /// Writes a string of any encoding to the stream and ends it with a 0 byte.
        /// </summary>
        /// <param name="data">The string to write.</param>
        /// <param name="encoding">The encoding to use.</param>
        public void WriteStringNullterminated(string data, Encoding encoding)
        {
            WriteString(data, encoding);
            WriteByte(0);
        }

        /// <summary>
        /// Writes an ASCII string to the stream and ends it with a 0 byte.
        /// </summary>
        /// <param name="data">The string to write.</param>
        public void WriteStringNullterminated(string data)
        {
            WriteStringNullterminated(data, Encoding.ASCII);
        }

        /// <summary>
        /// Writes 0 bytes to the stream until the length of the stream is a multiple of the size to align with.
        /// </summary>
        /// <param name="size">Size to align with.</param>
        /// <param name="offset">Offset (to the current position) from which to start measuring the length.</param>
        public void Align(uint size, uint offset = 0)
        {
            uint pos = Position + offset;
            if (pos % size == 0)
                return;
            size -= pos % size;
            WriteEmpty(size);
        }

        #endregion
    }
}
