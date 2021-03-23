using System;
using System.Collections.Generic;
using System.Numerics;

namespace CySoft.Geometry
{
    /// <summary>
    /// C# implementation of the QuickHull algorithm for finding the smallest polygon enclosing a set of points.<br/>
    /// Author: Olivier Jacot-Descombes.
    /// </summary>
    /// <remarks>
    /// Adaptation of the Java Script implementation https://github.com/claytongulick/quickhull. <br/>
    /// Author: Clay Gulick.<br/>
    /// <br/>
    /// Compared to two Graham Scan implementations I experimented with, this QuickHull implementation is much more
    /// stable. Graham Scan suffers from numerical problems due to floating point imprecision (probably due to the
    /// delicate relative angle calculations).<br/>
    /// <br/>
    /// The Quick hull algorithm has an expected runtime of <c>O(n log(n))</c>.
    /// <seealso cref="https://en.wikipedia.org/wiki/Quickhull">Quickhull (Wikipedia)</seealso><br/>
    /// <br/>
    /// Although the point coordinates are given as <c>float</c>, we do all calculations in <c>double</c> precision.
    /// </remarks>
    public static class QuickHull
    {
        /// <summary>
        /// Computes the convex hull of the points given a finite set of 2D points.
        /// </summary>
        /// <remarks>
        /// The hull is a convex polygon returned with counterclockwise-ordered vertices in a right-handed coordinate
        /// system (y-axis pointing upwards) and clockwise-ordered vertices in a left-handed coordinate system (y-axis
        /// pointing downwards, as is the case for screen coordinates).
        /// </remarks>
        /// <param name="points">Collection of 2D points in an arbitrary order.</param>
        /// <returns>Convex hull</returns>
        public static List<Vector2> Compute(ICollection<Vector2> points)
        {
            var hull = new List<Vector2>();
            if (points.Count < 3) { // We still compute for 3 points to fix any wrong hull orientation.
                return new List<Vector2>(points);

            }
            Line baseline = GetMinMaxPoints(points);
            AddSegments(hull, baseline, points);

            // Reverse line direction to get points on other side.
            AddSegments(hull, baseline.Reverse, points);

            return hull;
        }

        // Return the min and max points in the set along the X axis
        private static Line GetMinMaxPoints(ICollection<Vector2> points)
        {
            Vector2 minPoint = default;
            Vector2 maxPoint = default;
            var enumerator = points.GetEnumerator();
            if (enumerator.MoveNext()) {
                minPoint = enumerator.Current;
                maxPoint = enumerator.Current;

                while (enumerator.MoveNext()) {
                    Vector2 point = enumerator.Current;
                    float x = point.X;
                    float y = point.Y;
                    if (x < minPoint.X || x == minPoint.X && y < minPoint.Y) {
                        minPoint = point;
                    }
                    if (x > maxPoint.X || x == maxPoint.X && y > maxPoint.Y) {
                        maxPoint = point;
                    }
                }
            }

            return new Line(minPoint, maxPoint);
        }

        // Calculates the distance of a point from a line.
        private static double DistanceFromLine(Vector2 point, Line line)
        {
            double vY = (double)line.P1.Y - line.P0.Y;
            double vX = (double)line.P0.X - line.P1.X;
            return (vX * ((double)point.Y - line.P0.Y) + vY * ((double)point.X - line.P0.X));
        }

        // Determines the set of points that lay outside the line (positive), and the most distal point.
        private static (List<Vector2> points, Vector2 max) DistalPoints(Line line, ICollection<Vector2> points)
        {
            var outer_points = new List<Vector2>();
            Vector2 distal_point = default;
            double max_distance = 0.0;

            foreach (Vector2 point in points) {
                double distance = DistanceFromLine(point, line);

                if (distance > 0.0) {
                    outer_points.Add(point);
                    if (distance > max_distance) {
                        distal_point = point;
                        max_distance = distance;
                    }
                }
            }

            return (outer_points, distal_point);
        }

        // Recursively adds hull segments.
        private static void AddSegments(List<Vector2> hull, Line line, ICollection<Vector2> points)
        {
            var distal = DistalPoints(line, points);
            if (distal.points.Count == 0) {
                hull.Add(line.P0);
                return;
            }
            AddSegments(hull, new Line(line.P0, distal.max), distal.points);
            AddSegments(hull, new Line(distal.max, line.P1), distal.points);
        }

        private struct Line
        {
            public Line(Vector2 p0, Vector2 p1)
            {
                P0 = p0;
                P1 = p1;
            }

            public Vector2 P0, P1;

            public Line Reverse => new Line(P1, P0);
        }
    }
}

