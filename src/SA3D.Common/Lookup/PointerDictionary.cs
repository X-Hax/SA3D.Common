using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SA3D.Common.Lookup
{
	/// <summary>
	/// A dictionary for mapping unique values to addresses.
	/// </summary>
	/// <typeparam name="T">Type of the value to map</typeparam>
	public class PointerDictionary<T> where T : notnull
	{
		private protected readonly Dictionary<uint, T> _fromAddr;
		private protected readonly Dictionary<T, uint> _toAddr;

		/// <summary>
		/// Creates a new pointer dictionary
		/// </summary>
		public PointerDictionary()
		{
			_fromAddr = new();
			_toAddr = new();
		}

		/// <summary>
		/// Gets the value mapped to the specified address.
		/// </summary>
		/// <param name="address">The address of the value to get.</param>
		/// <returns>The value if successful; <see langword="default"/> if unsuccessful.</returns>
		/// <exception cref="ArgumentNullException"/>
		public T? GetValue(uint address)
		{
			if(_fromAddr.TryGetValue(address, out T? value))
			{
				return value;
			}

			return default;
		}

		/// <summary>
		/// Gets the address mapped to the specifed value.
		/// </summary>
		/// <param name="value">The value of the address to get.</param>
		/// <returns>The address if successful; <see langword="null"/> if unsuccessful.</returns>
		/// <exception cref="ArgumentNullException"/>
		public uint? GetAddress(T value)
		{
			if(_toAddr.TryGetValue(value, out uint address))
			{
				return address;
			}

			return null;
		}

		/// <summary>
		/// Gets the address value to the specifed address.
		/// </summary>
		/// <param name="address">The address to get the value of.</param>
		/// <param name="result">When this method returns, contains the value associated with the specified address, if the address is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="PointerDictionary{T}"/> contains a value with the specified address, otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException"/>
		public bool TryGetValue(uint address, [MaybeNullWhen(false)] out T result)
		{
			return _fromAddr.TryGetValue(address, out result);
		}

		/// <summary>
		/// Gets the address to the specifed address.
		/// </summary>
		/// <param name="value">The value to get the address of.</param>
		/// <param name="result">When this method returns, contains the address associated with the specified value, if the value is found; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="PointerDictionary{T}"/> contains an address with the specified value, otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgumentNullException"/>
		public bool TryGetAddress(T value, [MaybeNullWhen(false)] out uint? result)
		{
			bool found = _toAddr.TryGetValue(value, out uint output);
			result = found ? output : null;
			return found;
		}

		/// <summary>
		/// Adds a new address/value pair to the dictionary.
		/// </summary>
		/// <param name="address">The address to add.</param>
		/// <param name="value">The value to add.</param>
		/// <exception cref="ArgumentException"/>
		public void Add(uint address, T value)
		{
			if(!_fromAddr.TryAdd(address, value))
			{
				throw new ArgumentException($"An item with the same address has already been added. Address: {address:X8}");
			}

			if(!_toAddr.TryAdd(value, address))
			{
				_fromAddr.Remove(address);
				throw new ArgumentException($"An item with the same value has already been added. Value: {value}");
			}
		}

		/// <summary>
		/// Attempts to add a new address/value pair to the dictionary.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="value"></param>
		/// <returns><see langword="true"/> if the address/value pair was successfully added to the dictionary; otherwise <see langword="false"/>.</returns>
		public bool TryAdd(uint address, T value)
		{
			if(!_fromAddr.TryAdd(address, value))
			{
				return false;
			}

			if(!_toAddr.TryAdd(value, address))
			{
				_fromAddr.Remove(address);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Get a copy of the dictionary mapping values to addresses.
		/// </summary>
		/// <returns></returns>
		public Dictionary<uint, T> GetDictFrom()
		{
			return new(_fromAddr);
		}

		/// <summary>
		/// Get a copy of the dictionary mapping addresses to values.
		/// </summary>
		/// <returns></returns>
		public Dictionary<T, uint> GetDictTo()
		{
			return new(_toAddr);
		}
	}
}
