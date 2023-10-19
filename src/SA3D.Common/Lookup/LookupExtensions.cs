using System;
using System.Diagnostics.CodeAnalysis;

namespace SA3D.Common.Lookup
{
	/// <summary>
	/// Pointer dictionary extension methods
	/// </summary>
	public static class LookupExtensions
	{
		/// <summary>
		/// Returns a clone of a labeled array where each element has been cloned too.
		/// </summary>
		/// <param name="labeledArray">The array to clone.</param>
		/// <returns>The cloned array.</returns>
		public static LabeledArray<T> ContentClone<T>(this LabeledArray<T> labeledArray) where T : ICloneable
		{
			return new(labeledArray.Label, labeledArray.Array.ContentClone());
		}

		/// <summary>
		/// Returns a clone of a labeled array where each element has been cloned too. 
		/// <br/> If the labeled array is readonly, the content will not be cloned.
		/// <br/> Only works if labeledArray is a <see cref="LabeledArray{T}"/> or <see cref="LabeledReadOnlyArray{T}"/>.
		/// </summary>
		/// <param name="labeledArray">The array to clone.</param>
		/// <returns>The cloned array.</returns>
		/// <exception cref="ArgumentException"/>
		public static ILabeledArray<T> ContentClone<T>(this ILabeledArray<T> labeledArray) where T : ICloneable
		{
			if(labeledArray is LabeledReadOnlyArray<T> ro)
			{
				return ro.Clone();
			}
			else if(labeledArray is LabeledArray<T> la)
			{
				return la.ContentClone();
			}

			throw new ArgumentException("Input must be either a LabeledArray or a LabeledReadOnlyArray");
		}

		#region PointerDictionary Extensions

		/// <summary>
		/// Gets a labeled array interface object from a generic pointer dictionary.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="dict">The dictionary to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <returns>The labeled array if found, otherwise <see langword="null"/></returns>
		/// <exception cref="InvalidCastException"/>
		/// <exception cref="ArgumentNullException"/>
		public static ILabeledArray<T>? GetILabeledArray<T>(this PointerDictionary<object> dict, uint address)
		{
			if(dict.TryGetValue(address, out object? value))
			{
				if(value is not ILabeledArray<T> result)
				{
					throw new InvalidCastException($"The labelled array at {address:X8} is a {value.GetType()}");
				}

				return result;
			}

			return null;
		}

		/// <summary>
		/// Attempts to get a labeled array interface object from a generic pointer dictionary.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="dict">The dictionary to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <param name="result">When this method returns, contains the value associated with the specified address, if the address is found; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="PointerDictionary{T}"/> contains a value with the specified address, otherwise <see langword="false"/>.</returns>
		/// <exception cref="InvalidCastException"/>
		/// <exception cref="ArgumentNullException"/>
		public static bool TryGetILabeledArray<T>(this PointerDictionary<object> dict, uint address, [MaybeNullWhen(false)] out ILabeledArray<T> result)
		{
			result = dict.GetILabeledArray<T>(address);
			return result != null;
		}

		/// <summary>
		/// Gets a labeled array from a generic pointer dictionary.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="dict">The dictionary to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <returns>The labeled array if found, otherwise <see langword="null"/></returns>
		/// <exception cref="InvalidCastException"/>
		public static LabeledArray<T>? GetLabeledArray<T>(this PointerDictionary<object> dict, uint address)
		{
			ILabeledArray<T>? result = dict.GetILabeledArray<T>(address);
			if(result == null)
			{
				return null;
			}

			if(result is not LabeledArray<T> castResult)
			{
				throw new InvalidCastException("Collection at address {address:X8} is readonly!");
			}

			return castResult;
		}

		/// <summary>
		/// Attempts to get a labeled array from a generic pointer dictionary.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="dict">The dictionary to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <param name="result">When this method returns, contains the value associated with the specified address, if the address is found; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="PointerDictionary{T}"/> contains a value with the specified address, otherwise <see langword="false"/>.</returns>
		/// <exception cref="InvalidCastException"/>
		/// <exception cref="ArgumentNullException"/>
		public static bool TryGetLabeledArray<T>(this PointerDictionary<object> dict, uint address, [MaybeNullWhen(false)] out LabeledArray<T> result)
		{
			result = dict.GetLabeledArray<T>(address);
			return result != null;
		}

		/// <summary>
		/// Gets a labeled readonly array from a generic pointer dictionary.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="dict">The dictionary to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <returns>The labeled array if found, otherwise <see langword="null"/></returns>
		/// <exception cref="InvalidCastException"/>
		public static LabeledReadOnlyArray<T>? GetReadOnlyLabeledArray<T>(this PointerDictionary<object> dict, uint address)
		{
			ILabeledArray<T>? result = dict.GetILabeledArray<T>(address);
			if(result == null)
			{
				return null;
			}

			if(result is not LabeledReadOnlyArray<T> castResult)
			{
				return new(result);
			}

			return castResult;
		}

		/// <summary>
		/// Attempts to get a labeled read only array from a generic pointer dictionary.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="dict">The dictionary to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <param name="result">When this method returns, contains the value associated with the specified address, if the address is found; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="PointerDictionary{T}"/> contains a value with the specified address, otherwise <see langword="false"/>.</returns>
		/// <exception cref="InvalidCastException"/>
		/// <exception cref="ArgumentNullException"/>
		public static bool TryGetReadOnlyLabeledArray<T>(this PointerDictionary<object> dict, uint address, [MaybeNullWhen(false)] out LabeledReadOnlyArray<T> result)
		{
			result = dict.GetReadOnlyLabeledArray<T>(address);
			return result != null;
		}

		#endregion

		#region BaseLUT extensions

		/// <summary>
		/// Gets a labeled array interface object from a lookup table.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="lut">The lookup table to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <returns>The labeled array if found, otherwise <see langword="null"/></returns>
		/// <exception cref="InvalidCastException"/>
		/// <exception cref="ArgumentNullException"/>
		public static ILabeledArray<T>? GetILabeledArray<T>(this BaseLUT lut, uint address)
		{
			return lut.All.GetILabeledArray<T>(address);
		}

		/// <summary>
		/// Attempts to get a labeled array interface object from a lookup table.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="lut">The lookup table to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <param name="result">When this method returns, contains the value associated with the specified address, if the address is found; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="BaseLUT"/> contains a value with the specified address, otherwise <see langword="false"/>.</returns>
		/// <exception cref="InvalidCastException"/>
		/// <exception cref="ArgumentNullException"/>
		public static bool TryGetILabeledArray<T>(this BaseLUT lut, uint address, [MaybeNullWhen(false)] out ILabeledArray<T> result)
		{
			return lut.All.TryGetILabeledArray(address, out result);
		}

		/// <summary>
		/// Gets a labeled array from a lookup table.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="lut">The lookup table to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <returns>The labeled array if found, otherwise <see langword="null"/></returns>
		/// <exception cref="InvalidCastException"/>
		public static LabeledArray<T>? GetLabeledArray<T>(this BaseLUT lut, uint address)
		{
			return lut.All.GetLabeledArray<T>(address);
		}

		/// <summary>
		/// Attempts to get a labeled array from a lookup table.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="lut">The lookup table to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <param name="result">When this method returns, contains the value associated with the specified address, if the address is found; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="BaseLUT"/> contains a value with the specified address, otherwise <see langword="false"/>.</returns>
		/// <exception cref="InvalidCastException"/>
		/// <exception cref="ArgumentNullException"/>
		public static bool TryGetLabeledArray<T>(this BaseLUT lut, uint address, [MaybeNullWhen(false)] out LabeledArray<T> result)
		{
			return lut.All.TryGetLabeledArray(address, out result);
		}

		/// <summary>
		/// Gets a labeled readonly array from a lookup table.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="lut">The lookup table to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <returns>The labeled array if found, otherwise <see langword="null"/></returns>
		/// <exception cref="InvalidCastException"/>
		public static LabeledReadOnlyArray<T>? GetReadOnlyLabeledArray<T>(this BaseLUT lut, uint address)
		{
			return lut.All.GetReadOnlyLabeledArray<T>(address);
		}

		/// <summary>
		/// Attempts to get a labeled read only array from a lookup table.
		/// </summary>
		/// <typeparam name="T">The arrays element type.</typeparam>
		/// <param name="lut">The lookup table to get the value from.</param>
		/// <param name="address">The address to get the value of.</param>
		/// <param name="result">When this method returns, contains the value associated with the specified address, if the address is found; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the <see cref="BaseLUT"/> contains a value with the specified address, otherwise <see langword="false"/>.</returns>
		/// <exception cref="InvalidCastException"/>
		/// <exception cref="ArgumentNullException"/>
		public static bool TryGetReadOnlyLabeledArray<T>(this BaseLUT lut, uint address, [MaybeNullWhen(false)] out LabeledReadOnlyArray<T> result)
		{
			return lut.All.TryGetReadOnlyLabeledArray(address, out result);
		}

		#endregion
	}
}
