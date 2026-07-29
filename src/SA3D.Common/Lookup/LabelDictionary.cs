using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SA3D.Common.Lookup
{
	/// <summary>
	/// A dictionary for mapping labels to offsets.
	/// </summary>
	public sealed partial class LabelDictionary : OffsetDictionary<string>
	{
		[GeneratedRegex("(?![0-9A-Za-z_]).")]
		private static partial Regex IllegalCharactersCheck();

		/// <summary>
		/// Creates a new empty label dictionary.
		/// </summary>
		public LabelDictionary() : base() { }

		/// <summary>
		/// Creates a new label dictionary and copies labels from a pre-existing dictionary.
		/// </summary>
		public LabelDictionary(Dictionary<long, string> labels) : base()
		{
			foreach(KeyValuePair<long, string> label in labels)
			{
				Add(label.Key, label.Value);
			}
		}

		/// <summary>
		/// Adds a new label to the dictionary. If the name is already taken, it gets preceded by a number to keep it unique.
		/// </summary>
		/// <param name="offset">The offset to add.</param>
		/// <param name="label">The label to add.</param>
		/// <returns>The label as it was added.</returns>
		/// <exception cref="ArgumentException"/>
		public string AddSafe(long offset, string label)
		{
			label = IllegalCharactersCheck().Replace(label, "_");

			if(_toOffset.ContainsKey(label))
			{
				int append = 1;
				while(_toOffset.ContainsKey($"{label}_{append}"))
				{
					append++;
				}

				label = $"{label}_{append}";
			}

			Add(offset, label);
			return label;
		}

		/// <summary>
		/// Returns either the found offset, or assembled a custom label for the offset
		/// </summary>
		/// <param name="offset">The lookup offset</param>
		/// <param name="prefix">The prefix to use when creating a custom label</param>
		/// <returns></returns>
		public string GetSafe(long offset, string prefix)
		{
			if(!TryGetValue(offset, out string? result))
			{
				result = prefix + offset.ToString("X8");
			}

			return result;
		}

		/// <summary>
		/// Attempts to get the label for the specified offset. If none is found, it will return a hexadecimal representation of the offset prefixed with the specified prefix.
		/// </summary>
		/// <param name="offset">The offset to get the label for.</param>
		/// <param name="prefix">The prefix to add when no label is found.</param>
		/// <returns>The found or generated label.</returns>
		public string GetGenerateValue(long offset, string prefix)
		{
			return TryGetValue(offset, out string? result)
				? result
				: $"{prefix}{offset:X8}";
		}

	}
}
