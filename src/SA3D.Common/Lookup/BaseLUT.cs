using System;
using System.Collections.Generic;
using System.Diagnostics;

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
		public BaseLUT(Dictionary<uint, string> labels)
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
		protected abstract void AddEntry(uint address, object value);

		/// <summary>
		/// Gets an address, or executes a custom handler that adds and returns the address if it is not found.
		/// </summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <param name="value">Value to get/add the address of.</param>
		/// <param name="write">The custom handler to write data and return the corresponding address to add.</param>
		/// <returns>The address for the specified value.</returns>
		public uint GetAddAddress<T>(T? value, Func<T, uint> write) where T : class
		{
			if(value == null)
			{
				return 0;
			}

			if(!All.TryGetAddress(value, out uint result))
			{
				result = write(value);
				All.Add(result, value);

				if(value is ILabel label)
				{
					Labels.AddSafe(result, label.Label);
				}

				AddEntry(result, value);
			}

			return result;
		}

		private T GetAddValue<T>(uint address, string? genPrefix, Func<uint, T> read) where T : class
		{
			if(address == 0)
			{
				throw new ArgumentException("Address is 0!");
			}

			T result;
			if(!All.TryGetValue(address, out object? gottenValue))
			{
				result = read(address);

				All.Add(address, result);

				if(result is ILabel labelable)
				{
					if(Labels.TryGetValue(address, out string? label))
					{
						labelable.Label = label;
					}
					else if(genPrefix != null)
					{
						labelable.Label = Labels.GetGenerateValue(address, genPrefix);
						Labels.Add(address, labelable.Label);
					}
				}

				AddEntry(address, result);
			}
			else
			{
				result = (T)gottenValue;
			}

			return result;
		}


		/// <summary>
		/// Executes the read function and stores the resulting value in the LUT.
		/// <br/> The resulting ILabel value will receive the Label stored in <see cref="Labels"/> if found for the address; otherwise a label generated using the address and prefix
		/// </summary>
		/// <typeparam name="T">Type of the value to read/add.</typeparam>
		/// <param name="address">Address at which the value is located.</param>
		/// <param name="genPrefix">The prefix to use for generating the Label when not found in <see cref="Labels"/>.</param>
		/// <param name="read">Func for reading the label value at the specified address.</param>
		/// <returns>The result of the read func.</returns>
		public T GetAddLabeledValue<T>(uint address, string genPrefix, Func<uint, T> read) where T : class, ILabel
		{
			return GetAddValue(address, genPrefix, read);
		}

		/// <summary>
		/// Executes the read function and stores the resulting value in the LUT.
		/// <br/> The resulting ILabel value will receive the Label stored in <see cref="Labels"/> if found for the address; otherwise a label generated using the address and prefix
		/// </summary>
		/// <typeparam name="T">Type of the value to read/add.</typeparam>
		/// <param name="address">Address at which the value is located.</param>
		/// <param name="genPrefix">The prefix to use for generating the Label when not found in <see cref="Labels"/>.</param>
		/// <param name="read">Func for reading the label object at the specified address.</param>
		/// <returns>The result of the read func.</returns>
		public T GetAddLabeledValue<T>(uint address, string genPrefix, Func<T> read) where T : class, ILabel
		{
			return GetAddLabeledValue(address, genPrefix, (_) => read());
		}

		/// <summary>
		/// Executes the read function and stores the resulting value in the LUT.
		/// </summary>
		/// <typeparam name="T">Type of the value to read/add.</typeparam>
		/// <param name="address">Address at which the value is located.</param>
		/// <param name="read">Func for reading the value at the specified address.</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">Thrown when passing an ILabel type value</exception>
		public T GetAddValue<T>(uint address, Func<uint, T> read) where T : class
		{
			if(typeof(T).IsAssignableTo(typeof(ILabel)))
			{
				throw new InvalidOperationException("Please use GetAddLabeledValue for types that implement ILabel!");
			}

			return GetAddValue(address, null, read);
		}

		/// <summary>
		/// Executes the read function and stores the resulting value in the LUT.
		/// </summary>
		/// <typeparam name="T">Type of the value to read/add.</typeparam>
		/// <param name="address">Address at which the value is located.</param>
		/// <param name="read">Func for reading the value at the specified address.</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">Thrown when passing an ILabel type value</exception>
		public T GetAddValue<T>(uint address, Func<T> read) where T : class
		{
			return GetAddValue(address, null, (_) => read());
		}
	}
}
