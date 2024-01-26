using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SA3D.Common.Lookup
{
	/// <summary>
	/// A dictionary for mapping labels to addresses.
	/// </summary>
	public partial class LabelDictionary : PointerDictionary<string>
	{
		[GeneratedRegex("(?![A-Za-z_]).")]
		private static partial Regex IllegalCharactersCheck();

		/// <summary>
		/// Creates a new empty label dictionary.
		/// </summary>
		public LabelDictionary() : base() { }

		/// <summary>
		/// Creates a new label dictionary and copies labels from a pre-existing dictionary.
		/// </summary>
		public LabelDictionary(Dictionary<uint, string> labels) : base()
		{
			foreach(KeyValuePair<uint, string> label in labels)
			{
				Add(label.Key, label.Value);
			}
		}

		/// <summary>
		/// Adds a new label to the dictionary. If the name is already taken, it gets preceded by a number to keep it unique.
		/// </summary>
		/// <param name="address">The address to add.</param>
		/// <param name="label">The label to add.</param>
		/// <returns>The label as it was added.</returns>
		/// <exception cref="ArgumentException"/>
		public string AddSafe(uint address, string label)
		{
			label = IllegalCharactersCheck().Replace(label, "_");

			if(_toAddr.ContainsKey(label))
			{
				int append = 1;
				while(_toAddr.ContainsKey($"{label}_{append}"))
				{
					append++;
				}

				label = $"{label}_{append}";
			}

			Add(address, label);
			return label;
		}

		/// <summary>
		/// Attempts to get the label for the specified address. If none is found, it will return a hexadecimal representation of the address prefixed with the specified prefix.
		/// </summary>
		/// <param name="address">The address to get the label for.</param>
		/// <param name="prefix">The prefix to add when no label is found.</param>
		/// <returns>The found or generated label.</returns>
		public string GetGenerateValue(uint address, string prefix)
		{
			return TryGetValue(address, out string? result)
				? result
				: $"{prefix}{address:X8}";
		}

	}
}
