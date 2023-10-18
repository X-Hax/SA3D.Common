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
        private readonly SortedDictionary<uint, T> _regions = new() { { 0, default } };
        private readonly HashSet<uint> _toRemoveBuffer = new();

        /// <summary>
        /// Marks a region with a value.
        /// </summary>
        /// <param name="from">Start of the region.</param>
        /// <param name="to">End of the region.</param>
        /// <param name="value">Value to mark the region as.</param>
        public void MarkRegion(uint from, uint to, T value)
        {
            T before = value;
            T after = default;
            uint nextIndex = 0;
            T next = default;

            foreach(KeyValuePair<uint, T> item in _regions)
            {
                if(item.Key < from)
                {
                    before = item.Value;
                }
                else if(item.Key < to)
                {
                    _toRemoveBuffer.Add(item.Key);
                    after = item.Value;
                }
                else
                {
                    next = item.Value;
                    nextIndex = item.Key;
                    break;
                }
            }

            foreach(uint key in _toRemoveBuffer)
            {
                _regions.Remove(key);
            }

            if(from == 0 || !before.Equals(value))
            {
                _regions.Add(from, value);
            }

            if(_regions.TryGetValue(to, out T currentAfter))
            {
                if(currentAfter.Equals(value))
                {
                    _regions.Remove(to);
                }
            }
            else
            {
                T newEndValue = value;
                if(!after.Equals(value))
                {
                    newEndValue = after;
                    _regions.Add(to, after);
                }

                if(nextIndex != 0 && newEndValue.Equals(next))
                {
                    _regions.Remove(nextIndex);
                }
            }

            _toRemoveBuffer.Clear();
        }

        /// <summary>
        /// Used to check whether a region contains a specific value.
        /// </summary>
        /// <param name="from">Start of region to check.</param>
        /// <param name="to">End of region to check.</param>
        /// <param name="value">Value to check.</param>
        /// <returns></returns>
        public bool HasValue(uint from, uint to, T value)
        {
            return _regions.Any(x => x.Key >= from && x.Key < to && x.Value.Equals(value));
        }
    }
}
