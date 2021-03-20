using System;
using System.Collections.Generic;
using System.Numerics;

namespace CySoft.Geometry.Helpers
{
    internal class HullStatus
    {
        private readonly bool[] _done;
        private int _undoneCount;

        public HullStatus(IList<Vector2> convexHull)
        {
            ConvexHull = convexHull;
            _done = new bool[convexHull.Count];
            _undoneCount = convexHull.Count;
        }

        public IList<Vector2> ConvexHull { get; }

        public int UndoneCount => _undoneCount;

        public void MarkAsDone(int hullIndex)
        {
            if (!_done[hullIndex]) {
                _done[hullIndex] = true;
                _undoneCount--;
            }
        }

        public bool IsDone(int hullIndex) => _done[hullIndex];
    }
}
