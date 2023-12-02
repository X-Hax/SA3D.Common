using System;
using System.Collections.Generic;
using System.Linq;

namespace SA3D.Common
{
	/// <summary>
	/// A handler for defining 1 dimensional regions (like to keep track of value types in an array).
	/// </summary>
	/// <typeparam name="T">Marker Datatype</typeparam>
	public class RegionMarker<T> where T : unmanaged, IEquatable<T>
	{
		private readonly SortedDictionary<uint, T> _regions;

		/// <summary>
		/// Creates a new region marker with a default region value.
		/// </summary>
		/// <param name="defaultValue">Default value for all unmarked regions</param>
		public RegionMarker(T defaultValue)
		{
			_regions = new() { { 0, defaultValue } };
		}

		/// <summary>
		/// Creates a new region marker.
		/// </summary>
		public RegionMarker() : this(default) { }

		/// <summary>
		/// Marks a region with a value.
		/// </summary>
		/// <param name="from">Start of the region (inclusive).</param>
		/// <param name="to">End of the region (exclusive).</param>
		/// <param name="value">Value to mark the region as.</param>
		public void MarkRegion(uint from, uint to, T value)
		{
			T endValue = _regions.Last(x => x.Key < to).Value;

			foreach(uint marker in _regions.Keys.ToArray())
			{
				if(marker < from)
				{
					continue;
				}

				if(marker >= to)
				{
					break;
				}

				_regions.Remove(marker);
			}

			if(from == 0 || !_regions.Last(x => x.Key < from).Value.Equals(value))
			{
				_regions.Add(from, value);
			}

			if(_regions.ContainsKey(to))
			{
				if(_regions[to].Equals(value))
				{
					_regions.Remove(to);
				}
			}
			else if(!value.Equals(endValue))
			{
				_regions.Add(to, endValue);
			}
		}

		/// <summary>
		/// Used to check whether a region contains a specific value.
		/// </summary>
		/// <param name="from">Start of region to check (inclusive).</param>
		/// <param name="to">End of region to check (exclusive).</param>
		/// <param name="value">Value to check for.</param>
		/// <returns></returns>
		public bool HasValue(uint from, uint to, T value)
		{
			List<KeyValuePair<uint, T>> relevantMarkers = _regions.Where(x => x.Key >= from && x.Key < to).ToList();

			if(from > 0 && (relevantMarkers.Count == 0 || relevantMarkers[0].Key != from))
			{
				relevantMarkers.Add(_regions.Last(x => x.Key < from));
			}

			return relevantMarkers.Any(x => x.Value.Equals(value));
		}
	}
}
