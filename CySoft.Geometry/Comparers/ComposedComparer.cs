using System;
using System.Collections.Generic;

namespace CySoft.Geometry.Comparers
{
    /// <summary>
    /// A comparer that combines several comparers. They are applied in the given order until one of them returns a
    /// non-0 result. Then this result is returned. Otherwise, 0 is returned.
    /// </summary>
    /// <typeparam name="T">The type of the values to be comapred</typeparam>
    public class ComposedComparer<T> : IComparer<T>
    {
        private readonly IComparer<T>[] _comparers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparers">The comparers to be combined</param>
        public ComposedComparer(params IComparer<T>[] comparers)
        {
            _comparers = comparers;
        }

        public int Compare(T a, T b)
        {
            for (var i = 0; i < _comparers.Length; i++) {
                int result = _comparers[i].Compare(a, b);
                if (result != 0) {
                    return result;
                }
            }
            return 0;
        }
    }
}
