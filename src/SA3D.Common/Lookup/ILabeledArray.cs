using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SA3D.Common.Lookup
{
    /// <summary>
    /// Labeled array interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILabeledArray<T> : ILabel, ICloneable, IList<T>, IList, IReadOnlyList<T>
    {
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public new T this[int index] { get; }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Creates a shallow clone of the array.
        /// </summary>
        /// <returns>A shallow clone of the labeled array.</returns>
        public new ILabeledArray<T> Clone();

        /// <summary>
        /// Checks whether an object implements <see cref="ILabeledArray{T}"/>.
        /// </summary>
        /// <param name="value">object to check.</param>
        /// <returns>Whether the object implements <see cref="ILabeledArray{T}"/>.</returns>
        public static bool IsLabeledArray(object? value)
        {
            if(value == null)
            {
                return false;
            }

            return value.GetType().GetInterfaces().Any(x =>
                  x.IsGenericType &&
                  x.GetGenericTypeDefinition() == typeof(ILabeledArray<>));
        }
    }
}
