using System;
using System.Collections.Generic;
using System.Numerics;

namespace CySoft.Geometry
{
    /// <summary>
    /// C# implementation of the QuickHull algorithm for finding the smallest polygon enclosing a set of points.<br/>
    /// Author: Olivier Jacot-Descombes.
    /// </summary>
    /// <remarks>Adaptation of the Java Script implementation https://github.com/claytongulick/quickhull<br/> Author:
    /// Clay Gulick.<br/><br/> Compared to 2 Graham Scan implementations I experimented with, this QuickHull
    /// implementation is much more stable. Graham Scan suffers from numerical problems due to floating point
    /// imprecision (probably due to the delicate relative angle calculations).</remarks>
    public static class QuickHull
    {
        public static List<Vector2> Compute(List<Vector2> points)
        {
            var hull = new List<Vector2>();
            if (points.Count <= 3) {
                //points.push(points[0]); //close the poly
                return points;
            }
            Line baseline = GetMinMaxPoints(points);
            AddSegments(hull, baseline, points);

            // Reverse line direction to get points on other side.
            AddSegments(hull, baseline.Reverse, points);

            //add the last point to make a closed loop
            //hull.push(hull[0]);
            return hull;
        }

        /// <summary>
        /// Return the min and max points in the set along the X axis
        /// </summary>
        /// <param name="points">An array of {x,y} objects</param>
        /// <returns>[ {x,y}, {x,y} ]</returns>
        private static Line GetMinMaxPoints(List<Vector2> points)
        {
            Vector2 minPoint = points[0];
            Vector2 maxPoint = points[0];

            for (int i = 1; i < points.Count; i++) {
                if (points[i].X < minPoint.X)
                    minPoint = points[i];
                if (points[i].X > maxPoint.X)
                    maxPoint = points[i];
            }

            return new Line(minPoint, maxPoint);
        }

        /// <summary>
        /// Calculates the distance of a point from a line
        /// </summary>
        /// <param name="point">Array [x,y]</param>
        /// <param name="line">rray of two points [ [x1,y1], [x2,y2] ]</param>
        /// <returns></returns>
        private static double DistanceFromLine(Vector2 point, Line line)
        {
            double vY = line.P1.Y - line.P0.Y;
            double vX = line.P0.X - line.P1.X;
            return (vX * (point.Y - line.P0.Y) + vY * (point.X - line.P0.X));
        }

        /// <summary>
        /// Determines the set of points that lay outside the line (positive), and the most distal point
        /// </summary>
        /// <param name="line"></param>
        /// <param name="points"></param>
        /// <returns>{points: [ [x1, y1], ... ], max: [x,y] ]</returns>
        private static (List<Vector2> points, Vector2 max) DistalPoints(Line line, List<Vector2> points)
        {
            var outer_points = new List<Vector2>();
            Vector2 distal_point = default;
            double max_distance = 0.0;

            for (int i = 0; i < points.Count; i++) {
                Vector2 point = points[i];
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

        /// <summary>
        /// Recursively adds hull segments
        /// </summary>
        /// <param name="line"></param>
        /// <param name="points"></param>
        private static void AddSegments(List<Vector2> hull, Line line, List<Vector2> points)
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

