using System;
using System.Collections.Generic;
using System.Linq;

namespace SA3D.Common
{
    /// <summary>
    /// A distinct index to value mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DistinctMap<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Collection of unique values
        /// </summary>
        public IList<T> Values { get; }

        /// <summary>
        /// Values as an array
        /// </summary>
        public T[] ValueArray
        {
            get
            {
                if (Values is T[] array)
                    return array;
                return Values.ToArray();
            }
        }

        /// <summary>
        /// Index to value mapping. Null if values are already distinct
        /// </summary>
        public int[]? Map { get; }

        /// <summary>
        /// Returns an indexes remapped index.
        /// </summary>
        /// <param name="absoluteIndex">Absolute index</param>
        /// <returns>Mapped index</returns>
        public int this[int absoluteIndex]
            => Map == null ? absoluteIndex : Map[absoluteIndex];

        /// <summary>
        /// Returns an indexes remapped index.
        /// </summary>
        /// <param name="absoluteIndex">Absolute index</param>
        /// <returns>Mapped index</returns>
        public uint this[uint absoluteIndex]
            => Map == null ? absoluteIndex : (uint)Map[absoluteIndex];

        /// <summary>
        /// Returns an indexes remapped index.
        /// </summary>
        /// <param name="absoluteIndex">Absolute index</param>
        /// <returns>Mapped index</returns>
        public short this[short absoluteIndex]
            => Map == null ? absoluteIndex : (short)Map[absoluteIndex];

        /// <summary>
        /// Returns an indexes remapped index.
        /// </summary>
        /// <param name="absoluteIndex">Absolute index</param>
        /// <returns>Mapped index</returns>
        public ushort this[ushort absoluteIndex]
            => Map == null ? absoluteIndex : (ushort)Map[absoluteIndex];

        /// <summary>
        /// Creates a new distinct map
        /// </summary>
        /// <param name="values">Distinct values</param>
        /// <param name="map">Index mapping</param>
        public DistinctMap(IList<T> values, int[]? map)
        {
            Values = values;
            Map = map;
        }

        /// <summary>
        /// Gets a value from an original absolute index
        /// </summary>
        /// <param name="absoluteIndex"></param>
        /// <returns></returns>
        public T GetValue(int absoluteIndex)
        {
            return Values[this[absoluteIndex]];
        }

        /// <summary>
        /// Tries to create a distinct mapping for a collection of values.
        /// </summary>
        /// <param name="collection">The collection to create a mapping for.</param>
        /// <param name="map">Resulting mapping.</param>
        /// <returns>Whether not all values were distinct.</returns>
        public static bool TryCreateDistinctMap(IList<T> collection, out DistinctMap<T> map)
        {
            map = new(collection, null);

            int[] resultMap = new int[collection.Count];
            T[] resultDistinct = new T[collection.Count];
            int distinctCount = 0;

            int i = 0;
            foreach (T c in collection)
            {

                for (int j = 0; j < distinctCount; j++)
                {
                    if (resultDistinct[j].Equals(c))
                    {
                        resultMap[i] = j;
                        goto found;
                    }
                }

                resultMap[i] = distinctCount;
                resultDistinct[distinctCount] = c;
                distinctCount++;

            found:
                ;
                i++;
            }

            if (distinctCount == resultMap.Length)
                return false;

            T[] distinct = new T[distinctCount];
            Array.Copy(resultDistinct, distinct, distinctCount);
            map = new(distinct, resultMap);

            return true;
        }

        /// <summary>
        /// Creates a distinct map for a collection.
        /// </summary>
        /// <param name="collection">Collection to create the distinct map for</param>
        /// <returns>Created mapping.</returns>
        public static DistinctMap<T> CreateDistinctMap(IList<T> collection)
        {
            collection.TryCreateDistinctMap(out DistinctMap<T> result);
            return result;
        }
    }


    /// <summary>
    /// Extensions variants for the create methods of <see cref="DistinctMap{T}"/>
    /// </summary>
    public static class DistinctMapExtensions
    {
        /// <summary>
        /// Tries to create a distinct mapping for a collection of values.
        /// </summary>
        /// <param name="collection">The collection to create a mapping for.</param>
        /// <param name="map">Resulting mapping.</param>
        /// <returns>Whether not all values were distinct.</returns>
        public static bool TryCreateDistinctMap<T>(this IList<T> collection, out DistinctMap<T> map) where T : IEquatable<T>
        {
            return DistinctMap<T>.TryCreateDistinctMap(collection, out map);
        }

        /// <summary>
        /// Creates a distinct map for a collection.
        /// </summary>
        /// <param name="collection">Collection to create the distinct map for</param>
        /// <returns>Created mapping.</returns>
        public static DistinctMap<T> CreateDistinctMap<T>(this IList<T> collection) where T : IEquatable<T>
        {
            return DistinctMap<T>.CreateDistinctMap(collection);
        }
    }
}
