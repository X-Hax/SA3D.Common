using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SA3D.Common.Lookup
{
	/// <summary>
	/// A dictionary for mapping unique objects to unique offsets.
	/// </summary>
	/// <typeparam name="T">Type of the objects to map</typeparam>
	public class OffsetDictionary<T> where T : notnull
	{
		private protected readonly Dictionary<long, T> _fromOffset;
		private protected readonly Dictionary<T, long> _toOffset;

		/// <summary>
		/// Number of entries in the dictionary
		/// </summary>
		public int Count => _fromOffset.Count;

		/// <summary>
		/// Creates a new offset dictionary
		/// </summary>
		public OffsetDictionary()
		{
			_fromOffset = [];
			_toOffset = [];
		}

		/// <summary>
		/// Gets the value mapped to the specified offset.
		/// </summary>
		/// <param name="offset">The offset of the value to get.</param>
		/// <returns>The value if successful; <see langword="default"/> if unsuccessful.</returns>
		/// <exception cref="ArgumentNullException"/>
		public T? GetValue(long offset)
		{
			if(_fromOffset.TryGetValue(offset, out T? value))
			{
				return value;
			}

			return default;
		}

		/// <summary>
		/// Gets the offset mapped to the specifed value.
		/// </summary>
		/// <param name="value">The value of the offset to get.</param>
		/// <returns>The offset if successful; <see langword="null"/> if unsuccessful.</returns>
		/// <exception cref="ArgumentNullException"/>
		public long? GetOffset(T value)
		{
			if(_toOffset.TryGetValue(value, out long offset))
			{
				return offset;
			}

			return null;
		}

		/// <summary>
		/// Gets the offset value to the specifed offset.
		/// </summary>
		/// <param name="offset">The offset to get the value of.</param>
		/// <param name="result">When this method returns, contains the value associated with the specified offset, if the offset is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="OffsetDictionary{T}"/> contains a value with the specified offset, otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException"/>
		public bool TryGetValue(long offset, [MaybeNullWhen(false)] out T result)
		{
			return _fromOffset.TryGetValue(offset, out result);
		}

		/// <summary>
		/// Gets the offset to the specifed offset.
		/// </summary>
		/// <param name="value">The value to get the offset of.</param>
		/// <param name="result">When this method returns, contains the offset associated with the specified value, if the value is found; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="OffsetDictionary{T}"/> contains an offset with the specified value, otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException"/>
		public bool TryGetOffset(T value, [MaybeNullWhen(false)] out long result)
		{
			return _toOffset.TryGetValue(value, out result);
		}

		/// <summary>
		/// Adds a new offset/value pair to the dictionary.
		/// </summary>
		/// <param name="offset">The offset to add.</param>
		/// <param name="value">The value to add.</param>
		/// <exception cref="ArgumentException"/>
		public void Add(long offset, T value)
		{
			if(!_fromOffset.TryAdd(offset, value))
			{
				throw new ArgumentException($"An item with the same offset has already been added. Offset: {offset:X8}");
			}

			if(!_toOffset.TryAdd(value, offset))
			{
				_fromOffset.Remove(offset);
				throw new ArgumentException($"An item with the same value has already been added. Value: {value}");
			}
		}

		/// <summary>
		/// Attempts to add a new offset/value pair to the dictionary.
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="value"></param>
		/// <returns><see langword="true"/> if the offset/value pair was successfully added to the dictionary; otherwise <see langword="false"/>.</returns>
		public bool TryAdd(long offset, T value)
		{
			if(!_fromOffset.TryAdd(offset, value))
			{
				return false;
			}

			if(!_toOffset.TryAdd(value, offset))
			{
				_fromOffset.Remove(offset);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Get a copy of the dictionary mapping values to offsets.
		/// </summary>
		/// <returns></returns>
		public Dictionary<long, T> GetDictFrom()
		{
			return new(_fromOffset);
		}

		/// <summary>
		/// Get a copy of the dictionary mapping offsets to values.
		/// </summary>
		/// <returns></returns>
		public Dictionary<T, long> GetDictTo()
		{
			return new(_toOffset);
		}
	}
}
