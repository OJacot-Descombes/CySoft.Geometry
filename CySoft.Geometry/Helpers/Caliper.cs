using System;
using System.Collections.Generic;
using System.Numerics;

namespace CySoft.Geometry.Helpers
{
    internal class Caliper
    {
        const double SIGMA = 1E-6f;
        public const double Deg90 = MathF.PI / 2, Deg180 = MathF.PI, Deg270 = 1.5f * MathF.PI, Deg360 = 2 * MathF.PI;

        readonly IList<Vector2> _convexHull;
        private readonly HullStatus _hullStatus;
        private int _pointIndex;
        private double _orientation;

        // Calculated values depending on _pointIndex and _orientation.
        private double _constant, _slope, _angleToNextPoint;
        private Vector2 _vertex, _nextVertex;

        public int PointIndex => _pointIndex;

        public Caliper(HullStatus hullStatus, int pointIndex, double orientation)
        {
            _hullStatus = hullStatus;
            _convexHull = hullStatus.ConvexHull;
            _pointIndex = pointIndex;
            _orientation = orientation;
            Recalc();
        }

        public double Orientation => _orientation;
        public double AngleToNextPoint => _angleToNextPoint;

        private void Recalc()
        {
            _vertex = _convexHull[_pointIndex];
            _nextVertex = _convexHull[(_pointIndex + 1) % _convexHull.Count];
            _slope = Math.Tan(_orientation);
            _constant = _vertex.Y - _slope * _vertex.X;

            Vector2 delta = _nextVertex - _vertex;
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
            double x, y;
            if (IsVertical()) {
                x = _vertex.X;
                y = other._vertex.Y;
            } else if (IsHorizontal()) {
                x = other._vertex.X;
                y = _vertex.Y;
            } else {
                // The x-intercept with the other caliper: x = ((c2 - c1) / (m1 - m2))
                // The y-intercept with the other caliper, given 'x': (m*x) + c
                x = (other._constant - _constant) / (_slope - other._slope);
                y = _slope * x + _constant;
            }

            return new Vector2((float)x, (float)y);
        }

        bool IsHorizontal()
        {
            return Math.Abs(_orientation) < SIGMA || Math.Abs(_orientation - Deg180) < SIGMA;
        }

        bool IsVertical()
        {
            return Math.Abs(_orientation - Deg90) < SIGMA || Math.Abs(_orientation - Deg270) < SIGMA;
        }

        public void RotateBy(double angle, out bool wasDone)
        {
            wasDone = false;
            if (Math.Abs(_angleToNextPoint - angle) < 1e-6f) {
                wasDone = _hullStatus.IsDone(_pointIndex);
                _hullStatus.MarkAsDone(_pointIndex);
                _pointIndex = (_pointIndex + 1) % _convexHull.Count;
            }

            _orientation = (_orientation + angle) % Deg360;
            Recalc();
        }

        public override string ToString() => $"Caliper {{ hull[{_pointIndex}] = {_vertex}, angle = {_orientation}, {_orientation * 180 / MathF.PI}° }}";
    }
}
