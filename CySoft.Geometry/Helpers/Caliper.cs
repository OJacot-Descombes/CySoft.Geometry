using System;
using System.Collections.Generic;
using System.Numerics;

namespace CySoft.Geometry.Helpers
{
    internal class Caliper
    {
        const double SIGMA = 1e-6f;
        public const double Deg90 = MathF.PI / 2, Deg180 = MathF.PI, Deg270 = 1.5f * MathF.PI, Deg360 = 2 * MathF.PI;

        readonly IList<Vector2> _convexHull;
        readonly bool[] _visited;
        private int _pointIndex;
        private double _orientation;

        // Calculated values depending on _pointIndex and _orientation.
        private double _angleToNextPoint;
        private Vector2 _vertex;

        public int PointIndex => _pointIndex;

        public Caliper(IList<Vector2> convexHull, bool[] visited, int pointIndex, double orientation)
        {
            _convexHull = convexHull;
            _visited = visited;
            _pointIndex = pointIndex;
            _orientation = orientation;
            Recalc();
        }

        public double Orientation => _orientation;
        public double AngleToNextPoint => _angleToNextPoint;

        private void Recalc()
        {
            _vertex = _convexHull[_pointIndex];
            Vector2 nextVertex = _convexHull[(_pointIndex + 1) % _convexHull.Count];

            Vector2 delta = nextVertex - _vertex;
            double orientationOfNextEdge = Math.Atan2(delta.Y, delta.X);
            if (orientationOfNextEdge < 0) {
                orientationOfNextEdge += Deg360;
            }

            _angleToNextPoint = orientationOfNextEdge - _orientation;
            if (_angleToNextPoint < 0) {
                _angleToNextPoint += Deg360;
            }
        }

        public Vector2 IntersectWith(Caliper other)
        {
            Vector2 s1 = _vertex;
            (double X, double Y) e1 = (s1.X + Math.Cos(_orientation), s1.Y + Math.Sin(_orientation));
            Vector2 s2 = other._vertex;
            (double X, double Y) e2 = (s2.X + Math.Cos(other._orientation), s2.Y + Math.Sin(other._orientation));

            double a1 = e1.Y - s1.Y;
            double b1 = s1.X - e1.X;
            double c1 = a1 * s1.X + b1 * s1.Y;

            double a2 = e2.Y - s2.Y;
            double b2 = s2.X - e2.X;
            double c2 = a2 * s2.X + b2 * s2.Y;

            double delta = a1 * b2 - a2 * b1; // The calipers cannot be parallel, therefore delta will be ≠ 0.

            return new Vector2((float)((b2 * c1 - b1 * c2) / delta), (float)((a1 * c2 - a2 * c1) / delta));
        }

        public void RotateBy(double angle)
        {
            if (Math.Abs(_angleToNextPoint - angle) < SIGMA) {
                _visited[_pointIndex] = true;
                _pointIndex = (_pointIndex + 1) % _convexHull.Count;
            }

            _orientation = (_orientation + angle) % Deg360;
            Recalc();
        }

        public override string ToString() =>
            $"Caliper {{ hull[{_pointIndex}] = {_vertex}, angle = {_orientation}, {_orientation * 180 / MathF.PI}° }}";
    }
}
