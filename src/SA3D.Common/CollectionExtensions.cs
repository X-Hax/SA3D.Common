using System;
using System.Collections.Generic;
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
