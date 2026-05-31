using SA3D.Common.Lookup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SA3D.Common
{
	/// <summary>
	/// Various general helper methods
	/// </summary>
	public static class CollectionExtensions
	{
		/// <summary>
		/// Returns a clone of an array where each field has been cloned too
		/// </summary>
		/// <param name="input">Array to clone</param>
		/// <returns></returns>
		public static T[] ContentClone<T>(this T[] input) where T : ICloneable
		{
			T[] result = new T[input.Length];

			for(int i = 0; i < result.Length; i++)
			{
				result[i] = (T)input[i].Clone();
			}

			return result;
		}

		/// <summary>
		/// Returns a clone of an array where each field has been cloned too
		/// </summary>
		/// <param name="input">Array to clone</param>
		/// <returns></returns>
		public static LabeledArray<T> ContentClone<T>(this LabeledArray<T> input) where T : ICloneable
		{
			LabeledArray<T> result = new(input.Label, input.Length);

			for(int i = 0; i < result.Length; i++)
			{
				result[i] = (T)input[i].Clone();
			}

			return result;
		}

		/// <summary>
		/// Returns the first key that is found to the given value. Throws an error if none is found
		/// </summary>
		/// <typeparam name="K">Type of the key</typeparam>
		/// <typeparam name="V">Type of the value</typeparam>
		/// <param name="dictionary">Dictionary to look through</param>
		/// <param name="value">Value to look for</param>
		/// <returns></returns>
		public static K FindKey<K, V>(this IDictionary<K, V> dictionary, V value) where V : notnull
		{
			return dictionary.First(x => x.Value.Equals(value)).Key;
		}

		/// <summary>
		/// Constructs an enumerable for iterating over the lines read (<see cref="StreamReader.ReadLine"/>) off a stream reader. Ends when reader returns null.
		/// </summary>
		/// <param name="reader">The reader to read lines off.</param>
		/// <returns>The enumerable.</returns>
		public static IEnumerable<string> StreamReaderAsLineEnumerable(this StreamReader reader)
		{
			while(reader.ReadLine() is string line)
			{
				yield return line;
			}

			yield break;
		}

#pragma warning disable CS8603 // We can manually ignore the possible null return this here
#pragma warning disable CS8619

		/// <summary>
		/// Linq Select but only returns non-null values.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> SelectIgnoringNull<TSource, TResult>(
			this IEnumerable<TSource> source,
			Func<TSource, TResult?> selector)
		{
			return source.Select(selector)
				.Where(x => x != null);
		}

		/// <summary>
		/// Linq SelectMany but only returns non-null values.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> SelectManyIgnoringNull<TSource, TResult>(
			this IEnumerable<TSource> source,
			Func<TSource, IEnumerable<TResult?>?> selector)
		{
			return source.Select(selector)
				.Where(e => e != null)
				.SelectMany(e => e)
				.Where(e => e != null);
		}

#pragma warning restore CS8619
#pragma warning restore CS8603
	}
}
