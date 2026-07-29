using Amicitia.IO.Binary;
using SA3D.Common.IO;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SA3D.Common.Lookup
{
	/// <summary>
	/// Offset dictionary extension methods
	/// </summary>
	public static class LookupExtensions
	{
		/// <summary>
		/// Utility function for checking whether a dictionary is null. Needed for reliable Amicitia.IO serialization
		/// </summary>
		/// <param name="offsetDictionary">The dictionary to check for null</param>
		/// <exception cref="NullReferenceException"></exception>
		public static void NullReferenceCheck<T>([NotNull] this OffsetDictionary<T>? offsetDictionary) where T : notnull
		{
			if(offsetDictionary == null)
			{
				throw new NullReferenceException("No offset lookup dictionary!");
			}
		}

		/// <summary>
		/// Utility function for checking whether a lut is null. Needed for reliable Amicitia.IO serialization
		/// </summary>
		/// <param name="lut">The lookup table to check for null</param>
		/// <exception cref="NullReferenceException"></exception>
		public static void NullReferenceCheck([NotNull] this OffsetLUT? lut)
		{
			if(lut == null)
			{
				throw new NullReferenceException("No lookup table!");
			}
		}

		/// <summary>
		/// Adds a value to the LUT at the current writing position of a <see cref="BinaryObjectWriter"/>
		/// </summary>
		/// <param name="lut">Lookup table to add the value to</param>
		/// <param name="writer">The writer from which to get the offset</param>
		/// <param name="value">The value to add</param>
		public static void AddForWriter<T>(this OffsetLUT lut, BinaryObjectWriter writer, T value) where T : class
		{
			if(value is IList list && list.Count == 0)
			{
				return;
			}

			lut.AddSafeLabel(writer.GetPositionOffset(), value);
		}

		/// <summary>
		/// returns null if <paramref name="array"/> is null or has a length of 0, otherwise returns <paramref name="array"/> again.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns></returns>
		public static LabeledArray<T>? EmptyNull<T>(this LabeledArray<T>? array)
		{
			return array?.Length > 0 ? array : null;
		}
	}
}
