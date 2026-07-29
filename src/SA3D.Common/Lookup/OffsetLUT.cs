using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SA3D.Common.Lookup
{
	/// <summary>
	/// Base lookuptable (LUT) for various uses.
	/// </summary>
	[DebuggerNonUserCode]
	public class OffsetLUT
	{
		/// <summary>
		/// Labels for the added objects
		/// </summary>
		public LabelDictionary Labels { get; }

		/// <summary>
		/// All objects in this LUT
		/// </summary>
		public OffsetDictionary<object> All { get; }


		/// <summary>
		/// Creates a LUT with preexisting labels.
		/// </summary>
		/// <param name="labels">Preexisting labels.</param>
		public OffsetLUT(Dictionary<long, string> labels)
		{
			Labels = new(labels);
			All = new();
		}

		/// <summary>
		/// Creates a new empty LUT.
		/// </summary>
		public OffsetLUT()
		{
			Labels = new();
			All = new();
		}

		/// <summary>
		/// Custom handler for adding a new offset/value pair to the lookup table.
		/// </summary>
		/// <param name="offset">The offset to add.</param>
		/// <param name="value">The value to add.</param>
		protected virtual void OnAddEntry(long offset, object value)
		{

		}

		/// <summary>
		/// Adds a new offset-value pair to the LUT. Tries to add the label
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="offset"></param>
		/// <param name="value"></param>
		public void AddTryLabel<T>(long offset, T value) where T : class
		{
			All.Add(offset, value);

			if(value is ILabel label)
			{
				Labels.TryAdd(offset, label.Label);
			}

			OnAddEntry(offset, value);
		}

		/// <summary>
		/// Adds a new offset-value pair to the LUT. Adds a safe label
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="offset"></param>
		/// <param name="value"></param>
		public void AddSafeLabel<T>(long offset, T value) where T : class
		{
			All.Add(offset, value);

			if(value is ILabel label)
			{
				Labels.AddSafe(offset, label.Label);
			}

			OnAddEntry(offset, value);
		}

		/// <summary>
		/// Tries to get the typed value assigned to a given offset
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="offset"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		public bool TryGetValue<T>(long offset, [NotNullWhen(true)] out T? value) where T : class
		{
			if(All.TryGetValue(offset, out object? genValue))
			{
				value = genValue as T ?? throw new InvalidCastException($"Stored value is of type \"{genValue!.GetType()}\" and not {typeof(T)}");
				return true;
			}

			value = null;
			return false;
		}

	}
}
