using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SA3D.Common.Lookup
{
    /// <summary>
    /// A read only array with a label.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LabeledReadOnlyArray<T> : ILabeledArray<T>
    {
        /// <inheritdoc/>
        public string Label { get; set; }

        /// <inheritdoc/>
        public ReadOnlyCollection<T> Array { get; }

        /// <inheritdoc/>
        public int Length => Array.Count;

        /// <inheritdoc/>
        public T this[int index] => Array[index];

        #region Constructors

        /// <summary>
        /// Creates a new labeled read only array with a label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="array">The collection.</param>
        public LabeledReadOnlyArray(string label, ReadOnlyCollection<T> array)
        {
            Label = label;
            Array = array;
        }

        /// <summary>
        /// Creates a new labeled read only array with an <see cref="string.Empty"/> label.
        /// </summary>
        /// <param name="array">The collection.</param>
        public LabeledReadOnlyArray(ReadOnlyCollection<T> array) : this(string.Empty, array) { }

        /// <summary>
        /// Creates a new labeled read only array with a label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="list">The collection.</param>
        public LabeledReadOnlyArray(string label, IList<T> list) : this(label, new(list)) { }

        /// <summary>
        /// Creates a new labeled read only array with an <see cref="string.Empty"/> label.
        /// </summary>
        /// <param name="list">The collection.</param>
        public LabeledReadOnlyArray(IList<T> list) : this(string.Empty, list) { }

        #endregion

        #region Hidden implementations

        object? IList.this[int index]
        {
            get => ((IList)Array)[index];
            set => ((IList)Array)[index] = value;
        }

        bool IList.IsFixedSize => ((IList)Array).IsFixedSize;
        bool IList.IsReadOnly => ((IList)Array).IsReadOnly;

        int IList.Add(object? value)
        {
            return ((IList)Array).Add(value);
        }

        void IList.Clear()
        {
            ((IList)Array).Clear();
        }

        bool IList.Contains(object? value)
        {
            return ((IList)Array).Contains(value);
        }

        int IList.IndexOf(object? value)
        {
            return ((IList)Array).IndexOf(value);
        }

        void IList.Insert(int index, object? value)
        {
            ((IList)Array).Insert(index, value);
        }

        void IList.Remove(object? value)
        {
            ((IList)Array).Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            ((IList)Array).RemoveAt(index);
        }

        T IList<T>.this[int index]
        {
            get => ((IList<T>)Array)[index];
            set => ((IList<T>)Array)[index] = value;
        }

        int IList<T>.IndexOf(T item)
        {
            return ((IList<T>)Array).IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            ((IList<T>)Array).Insert(index, item);
        }

        void IList<T>.RemoveAt(int index)
        {
            ((IList<T>)Array).RemoveAt(index);
        }

        T IReadOnlyList<T>.this[int index] => ((IReadOnlyList<T>)Array)[index];

        int ICollection.Count => ((ICollection)Array).Count;
        bool ICollection.IsSynchronized => ((ICollection)Array).IsSynchronized;
        object ICollection.SyncRoot => ((ICollection)Array).SyncRoot;

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)Array).CopyTo(array, index);
        }

        int ICollection<T>.Count => ((ICollection<T>)Array).Count;
        bool ICollection<T>.IsReadOnly => ((ICollection<T>)Array).IsReadOnly;

        void ICollection<T>.Add(T item)
        {
            ((ICollection<T>)Array).Add(item);
        }

        void ICollection<T>.Clear()
        {
            ((ICollection<T>)Array).Clear();
        }

        bool ICollection<T>.Contains(T item)
        {
            return ((ICollection<T>)Array).Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)Array).CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            return ((ICollection<T>)Array).Remove(item);
        }

        int IReadOnlyCollection<T>.Count => ((IReadOnlyCollection<T>)Array).Count;

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)Array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Array).GetEnumerator();
        }

        ILabeledArray<T> ILabeledArray<T>.Clone()
        {
            return Clone();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        /// <inheritdoc/>
        public LabeledReadOnlyArray<T> Clone()
        {
            return new(Label, Array);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Label}: {Array}";
        }
    }
}
