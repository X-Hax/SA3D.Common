using SA3D.Common.JsonConverters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SA3D.Common.Lookup
{
	/// <summary>
	/// An array with a label.
	/// </summary>
	/// <typeparam name="T"></typeparam>

	[JsonConverter(typeof(LabeledArrayJsonConverterFactory))]
	public class LabeledArray<T> : ILabel, IList, IList<T>, ICloneable
	{
		private const string _labelPrefix = "array_";

		/// <inheritdoc/>
		public string LabelPrefix { get; } = _labelPrefix;

		/// <inheritdoc/>
		public string Label { get; set; }

		/// <summary>
		/// The source array.
		/// </summary>
		public T[] Array { get; set; }


		/// <inheritdoc/>
		public T this[int index]
		{
			get => Array[index];
			set => Array[index] = value;
		}

		/// <inheritdoc/>
		public int Length => Array.Length;

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
		/// Creates a new labeled array with a generated label and an array.
		/// </summary>
		/// <param name="array">The array.</param>
		public LabeledArray(T[] array) : this(_labelPrefix.GenerateIdentifier(), array) { }

		/// <summary>
		/// Creates a new labeled array with a label and a new array with specified size.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="size">The size of the array.</param>
		public LabeledArray(string label, int size) : this(label, new T[size]) { }

		/// <summary>
		/// Creates a new labeled array with a generated label and a new array with specified size.
		/// </summary>
		/// <param name="size">The size of the array.</param>
		public LabeledArray(int size) : this(new T[size]) { }

		/// <summary>
		/// Creates a new labeled array with a label and a new array with specified size.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="size">The size of the array.</param>
		public LabeledArray(string label, uint size) : this(label, new T[size]) { }

		/// <summary>
		/// Creates a new labeled array with a generated label and a new array with specified size.
		/// </summary>
		/// <param name="size">The size of the array.</param>
		public LabeledArray(uint size) : this(new T[size]) { }

		#endregion

		#region Enumerable & Enumerable<T>

		/// <inheritdoc/>
		public IEnumerator GetEnumerator()
		{
			return Array.GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return ((IEnumerable<T>)Array).GetEnumerator();
		}

		#endregion

		#region ICollection

		private ICollection Collection => Array;

		int ICollection.Count => Collection.Count;

		bool ICollection.IsSynchronized => Collection.IsSynchronized;

		object ICollection.SyncRoot => Collection.SyncRoot;

		/// <inheritdoc/>
		public void CopyTo(Array array, int index)
		{
			Array.CopyTo(array, index);
		}

		#endregion

		#region ICollection<T>

		private ICollection<T> CollectionG => Array;


		void ICollection<T>.Add(T item)
		{
			CollectionG.Add(item);
		}

		void ICollection<T>.Clear()
		{
			CollectionG.Clear();
		}

		bool ICollection<T>.Contains(T item)
		{
			return CollectionG.Contains(item);
		}

		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			CollectionG.CopyTo(array, arrayIndex);
		}

		bool ICollection<T>.Remove(T item)
		{
			return CollectionG.Remove(item);
		}

		#endregion

		#region IList

		private IList List => Array;


		bool IList.IsFixedSize => List.IsFixedSize;

		bool IList.IsReadOnly => List.IsReadOnly;


		object? IList.this[int index]
		{
			get => ((IList)Array)[index];
			set => ((IList)Array)[index] = value;
		}

		int IList.Add(object? value)
		{
			return List.Add(value);
		}

		void IList.Clear()
		{
			List.Clear();
		}

		bool IList.Contains(object? value)
		{
			return List.Contains(value);
		}

		int IList.IndexOf(object? value)
		{
			return List.IndexOf(value);
		}

		void IList.Insert(int index, object? value)
		{
			List.Insert(index, value);
		}

		void IList.Remove(object? value)
		{
			List.Remove(value);
		}

		void IList.RemoveAt(int index)
		{
			List.RemoveAt(index);
		}

		#endregion

		#region IList<T>

		private IList<T> ListG => Array;


		int ICollection<T>.Count => ListG.Count;

		bool ICollection<T>.IsReadOnly => ListG.IsReadOnly;

		T IList<T>.this[int index]
		{
			get => ListG[index];
			set => ListG[index] = value;
		}


		int IList<T>.IndexOf(T item)
		{
			return ListG.IndexOf(item);
		}

		void IList<T>.Insert(int index, T item)
		{
			ListG.Insert(index, item);
		}

		void IList<T>.RemoveAt(int index)
		{
			ListG.RemoveAt(index);
		}

		#endregion

		/// <inheritdoc/>
		public override string ToString()
		{
			return Label;
		}

		/// <inheritdoc/>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Creates a shallow copy of <see cref="Array"/> with the same <see cref="Label"/>
		/// </summary>
		/// <returns></returns>
		public LabeledArray<T> Clone()
		{
			return new LabeledArray<T>(Label, (T[])Array.Clone());
		}
	}
}