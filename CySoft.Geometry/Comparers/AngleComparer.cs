using System;
using System.Collections.Generic;
using System.Numerics;
using CySoft.Geometry.Helpers;

namespace CySoft.Geometry.Comparers
{
    /// <summary>
    /// Comparer that compares points by the angle that the line between the center and the point has to the x-axis.
    /// </summary>
    public class AngleComparer : IComparer<Vector2>
    {
        private readonly Vector2 _center;

        public AngleComparer(Vector2 center)
        {
            _center = center;
        }

        public int Compare(Vector2 p0, Vector2 p1)
        {
            float angle0 = _center.AngleToX(p0).NormalizeAngle();
            float angle1 = _center.AngleToX(p1).NormalizeAngle();

            return  angle0.CompareTo(angle1);
        }
    }
}
