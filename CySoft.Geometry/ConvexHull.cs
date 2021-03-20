/*
 * www.javagl.de - Geom - Geometry utilities
 *
 * Original Java version: Copyright (c) 2013-2015 Marco Hutter - http://www.javagl.de
 * C# port:               Copyright (c) 2021 Olivier Jacot-Descombes
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CySoft.Geometry.Comparers;
using CySoft.Geometry.Helpers;

namespace CySoft.Geometry
{

    /// <summary>
    /// Methods to compute the convex hull of a set of points
    /// </summary>
    public static class ConvexHull
    {
        /// <summary>
        /// Compute the list of points that form the convex hull of the given input points. If there are less than 4
        /// input points, a list containing the input points will be returned (regardless of whether these points are
        /// degenerate, i.e. even when they are on one line or at the same location)
        /// </summary>
        /// <param name="inputPoints">The input points</param>
        /// <returns>The convex hull points</returns>
        public static List<Vector2> Compute(IList<Vector2> inputPoints)
        {
            var points = new List<Vector2>(inputPoints);
            if (inputPoints.Count == 0) {
                return points;
            }

            // Compute the reference point: It sorts points by their y-coordinate.
            // If two points have the same y-coordinate, it sorts them by the x-coordinate.
            Vector2 referencePoint = points.Min(YXComparer.Instance);

            // Sort the points. The primary sorting criterion is the angle that the line from the reference point to
            // the point has to the x-axis. The secondary sorting criterion will be the YXComparer.
            var angleComparer = new AngleComparer(referencePoint);
            var comparer = new ComposedComparer<Vector2>(angleComparer, YXComparer.Instance);
            points.Sort(comparer);
            points = MakeAnglesUnique(points);
            if (inputPoints.Count <= 3) {
                return points;
            }
            return Scan(points);
        }

        /// <summary>
        /// Perform the actual Graham Scan on the given points
        /// </summary>
        /// <param name="points">The input points</param>
        /// <returns>The points of the convex hull</returns>
        private static List<Vector2> Scan(List<Vector2> points)
        {
            var result = new List<Vector2> { points[0], points[1] };
            for (int i = 2; i < points.Count; i++) {
                Vector2 p0 = result[^1];
                Vector2 p1 = result[^2];
                Vector2 p = points[i];
                int r = p.RelativeCCW(p0, p1);

                // A check for r>0 should be sufficient, but may cause problems for equal points.
                // So doing the conservative r>=0 check here.
                if (r >= 0) {
                    result.Add(p);
                } else {
                    result.RemoveAt(result.Count - 1);
                    i--;
                }
            }

            return result;
        }

        /// <summary>
        /// Given the sorted list of points, with point 0 being the reference point: Create a list containing the
        /// points that have a unique angle referring to the reference point. When two points have the same angle, then
        /// the point that is further away from the reference point will be kept.
        /// </summary>
        /// <param name="points">The input points</param>
        /// <returns>The points with unique angles</returns>
        private static List<Vector2> MakeAnglesUnique(List<Vector2> points)
        {
            const float Epsilon = 1e-8f;

            Vector2 referencePoint = points[0];
            var newPoints = new List<Vector2> { referencePoint };
            float previousAngle = 2 * MathF.PI;
            float previousDistanceSquared = Single.MaxValue;
            for (int i = 1; i < points.Count; i++) {
                Vector2 p = points[i];
                float angle = referencePoint.AngleToX(p);
                float distanceSquared = (referencePoint - p).LengthSquared();
                if (MathF.Abs(angle - previousAngle) > Epsilon) {
                    newPoints.Add(p);
                } else {
                    if (distanceSquared > previousDistanceSquared) {
                        newPoints[^1] = p;
                    }
                }

                previousAngle = angle;
                previousDistanceSquared = distanceSquared;
            }

            return newPoints;
        }
    }
}
