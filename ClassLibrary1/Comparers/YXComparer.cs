using System;
using System.Collections.Generic;
using System.Numerics;

namespace CySoft.Geometry.Comparers
{
    /// <summary>
    /// Comparer that compares points colexicographically. That means that it compares the points by their
    /// y-coordinate, and, if these are equal, by their x-coordinate.    
    /// </summary>
    public class YXComparer : IComparer<Vector2>
    {
        #region Singleton Pattern

        public static readonly YXComparer Instance = new YXComparer();

        private YXComparer() { }

        #endregion

        public int Compare(Vector2 a, Vector2 b)
        {
            if (a.Y < b.Y) return -1;
            if (a.Y > b.Y) return +1;
            if (a.X < b.X) return -1;
            if (a.X > b.X) return +1;
            return 0;
        }
    }
}
