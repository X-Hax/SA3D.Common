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
	public abstract class BaseLUT
	{
		/// <summary>
		/// Labels for the added objects
		/// </summary>
		public LabelDictionary Labels { get; }

		/// <summary>
		/// All objects in this LUT
		/// </summary>
		public PointerDictionary<object> All { get; }

		/// <summary>
		/// Creates a LUT with preexisting labels.
		/// </summary>
		/// <param name="labels">Preexisting labels.</param>
		public BaseLUT(Dictionary<long, string> labels)
		{
			Labels = new(labels);
			All = new();
		}

		/// <summary>
		/// Creates a new empty LUT.
		/// </summary>
		public BaseLUT()
		{
			Labels = new();
			All = new();
		}

		/// <summary>
		/// Custom handler for adding a new address/value pair to the lookup table.
		/// </summary>
		/// <param name="address">The address to add.</param>
		/// <param name="value">The value to add.</param>
		protected abstract void AddEntry(long address, object value);

		/// <summary>
		/// Adds a new address-value pair to the LUT. Tries to add the label
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="address"></param>
		/// <param name="value"></param>
		public void AddTryLabel<T>(long address, T value) where T : class
		{
			All.Add(address, value);

			if(value is ILabel label)
			{
				Labels.TryAdd(address, label.Label);
			}

			AddEntry(address, value);
		}

		/// <summary>
		/// Adds a new address-value pair to the LUT. Adds a safe label
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="address"></param>
		/// <param name="value"></param>
		public void AddSafeLabel<T>(long address, T value) where T : class
		{
			All.Add(address, value);

			if(value is ILabel label)
			{
				Labels.AddSafe(address, label.Label);
			}

			AddEntry(address, value);
		}

		/// <summary>
		/// Tries to get the typed value assigned to a given address
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="address"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		public bool TryGetValue<T>(long address, [NotNullWhen(true)] out T? value) where T : class
		{
			if(All.TryGetValue(address, out object? genValue))
			{
				value = genValue as T ?? throw new InvalidCastException($"Stored value is of type \"{genValue!.GetType()}\" and not {typeof(T)}");
				return true;
			}

			value = null;
			return false;
		}

	}
}
