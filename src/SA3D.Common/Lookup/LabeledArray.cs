using System;
using System.Collections;
using System.Collections.Generic;

namespace SA3D.Common.Lookup
{
    /// <summary>
    /// An array with a label.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LabeledArray<T> : ILabeledArray<T>
    {
        /// <inheritdoc/>
        public string Label { get; set; }

        /// <summary>
        /// The source array.
        /// </summary>
        public T[] Array { get; set; }

        /// <inheritdoc/>
        public bool IsFixedSize => Array.IsFixedSize;

        /// <inheritdoc/>
        public bool IsSynchronized => Array.IsSynchronized;

        /// <inheritdoc/>
        public object SyncRoot => Array.SyncRoot;

        /// <inheritdoc/>
        public bool IsReadOnly => Array.IsReadOnly;

        /// <inheritdoc/>
        public int Length
            => Array.Length;

        /// <inheritdoc/>
        public T this[int index]
        {
            get => Array[index];
            set => Array[index] = value;
        }


        #region Constructors

        /// <summary>
        /// Creates a new labeled array from a label and an array.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="array">The array.</param>
        public LabeledArray(string label, T[] array)
        {
            Label = label;
            Array = array;
        }

        /// <summary>
        /// Creates a new labeled array with an <see cref="string.Empty"/> label and an array.
        /// </summary>
        /// <param name="array">The array.</param>
        public LabeledArray(T[] array) : this(string.Empty, array) { }

        /// <summary>
        /// Creates a new labeled array with a label and a new array with specified size.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="size">The size of the array.</param>
        public LabeledArray(string label, int size) : this(label, new T[size]) { }

        /// <summary>
        /// Creates a new labeled array with an <see cref="string.Empty"/> label and a new array with specified size.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public LabeledArray(int size) : this(string.Empty, size) { }

        /// <summary>
        /// Creates a new labeled array with a label and a new array with specified size.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="size">The size of the array.</param>
        public LabeledArray(string label, uint size) : this(label, new T[size]) { }

        /// <summary>
        /// Creates a new labeled array with an <see cref="string.Empty"/> label and a new array with specified size.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public LabeledArray(uint size) : this(string.Empty, size) { }

        #endregion

        #region Hidden implementations 

        T IList<T>.this[int index]
        {
            get => ((IList<T>)Array)[index];
            set => ((IList<T>)Array)[index] = value;
        }

        int IList<T>.IndexOf(T item) => ((IList<T>)Array).IndexOf(item);
        void IList<T>.Insert(int index, T item) => ((IList<T>)Array).Insert(index, item);
        void IList<T>.RemoveAt(int index) => ((IList<T>)Array).RemoveAt(index);

        bool IList.IsReadOnly => Array.IsReadOnly;

        object? IList.this[int index]
        {
            get => ((IList)Array)[index];
            set => ((IList)Array)[index] = value;
        }

        int IList.Add(object? value) => ((IList)Array).Add(value);
        bool IList.Contains(object? value) => ((IList)Array).Contains(value);
        int IList.IndexOf(object? value) => ((IList)Array).IndexOf(value);
        void IList.Insert(int index, object? value) => ((IList)Array).Insert(index, value);
        void IList.Remove(object? value) => ((IList)Array).Remove(value);
        void IList.RemoveAt(int index) => ((IList)Array).RemoveAt(index);
        void IList.Clear() => ((IList)Array).Clear();


        int ICollection.Count => ((ICollection)Array).Count;
        int IReadOnlyCollection<T>.Count => ((IReadOnlyCollection<T>)Array).Count;
        int ICollection<T>.Count => ((ICollection<T>)Array).Count;
        bool ICollection<T>.IsReadOnly => ((ICollection<T>)Array).IsReadOnly;
        void ICollection<T>.Add(T item) => ((ICollection<T>)Array).Add(item);
        void ICollection<T>.Clear() => ((ICollection<T>)Array).Clear();
        bool ICollection<T>.Contains(T item) => ((ICollection<T>)Array).Contains(item);
        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)Array).CopyTo(array, arrayIndex);
        bool ICollection<T>.Remove(T item) => ((ICollection<T>)Array).Remove(item);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)Array).GetEnumerator();
        ILabeledArray<T> ILabeledArray<T>.Clone() => Clone();
        object ICloneable.Clone() => Clone();

        #endregion

        /// <inheritdoc/>
        public void CopyTo(Array array, int index) => Array.CopyTo(array, index);

        /// <inheritdoc/>
        public IEnumerator GetEnumerator() => Array.GetEnumerator();

        /// <inheritdoc/>
        public LabeledArray<T> Clone()
        {
            LabeledArray<T> result = new(Label, new T[Length]);
            for (int i = 0; i < Length; i++)
            {
                result[i] = this[i];
            }
            return result;
        }

        /// <inheritdoc/>
        public override string ToString()
            => $"{Label}: {Array}";
    }
}
