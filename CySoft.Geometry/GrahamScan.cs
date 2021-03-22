using System;
using System.Collections.Generic;
using System.Numerics;
using CySoft.Geometry.Comparers;
using CySoft.Geometry.Helpers;

namespace CySoft.Geometry
{
    /*
     * Copyright (c) 2010, Bart Kiers
     * Copyright (c) 2021 Olivier Jacot-Descombes, ported to C#
     *
     * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
     * documentation files (the "Software"), to deal in the Software without restriction, including without limitation
     * the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
     * to permit persons to whom the Software is furnished to do so, subject to the following conditions:
     *
     * The above copyright notice and this permission notice shall be included in all copies or substantial portions of
     * the Software.
     *
     * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
     * THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
     * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
     * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
     * DEALINGS IN THE SOFTWARE.
     *
     * Project: CompGeom; a computational geometry library using arbitrary-precision arithmetic where possible, written
     *          in Java. Developed by : Bart Kiers, bart@big-o.nl
     */

    /// <summary>
    /// <p> A class that finds the <a href="http://mathworld.wolfram.com/ConvexHull.html">convex hull</a> of a set of 2
    /// dimensional points using the <a href="http://en.wikipedia.org/wiki/Graham_scan">Graham scan</a> algorithm which
    /// finds the convex hull in <c>O(n * log(n))</c> time where <c>n</c> is the size of the set of points.
    /// </p>
    /// <p> For a detailed explanation, see: O'Rourke, J. (1998), "Section 3.5. Graham's Algorithm", Computational
    /// Geometry in C (2nd ed.), Cambridge University Press, pages 72-86. </p>
    /// <p> Author: Bart Kiers, bart@big-o.nl
    /// <br /> Date: Mar 11, 2010 </p>
    /// </summary>
    public static class GrahamScan
    {
        /// <summary>
        /// Returns the convex hull of the list of points in <c>O(N*log(N))</c> time. Note that the returned list is a
        /// counter-clockwise 'closed' walk: the first and last points are the same.
        /// </summary>
        /// <param name="polygon">The points.</param>
        /// <returns>The convex hull of the set of points as a counter-clockwise, 'closed' walk.</returns>
        public static List<Vector2> GetConvexHull(IEnumerable<Vector2> points)
        {
            List<Vector2> sorted = GetSortedFromLowestY(points);
            RemoveCollinearPoints(sorted);
            if (sorted.Count <= 3) {
                return sorted;
            }
            var stack = new Stack<Vector2>();
            stack.Push(sorted[0]);
            stack.Push(sorted[1]);
            stack.Push(sorted[2]);
            for (int i = 3; i < sorted.Count; i++) {
                Vector2 head = sorted[i];
                Vector2 middle = stack.Pop();
                Vector2 tail = stack.Pop();
                if (CGUtil.FormsLeftTurn(tail, middle, head)) {
                    stack.Push(tail);
                    stack.Push(middle);
                    stack.Push(head);
                } else {
                    stack.Push(tail);
                    i--;
                }
            }

            // Close the 'walk'
            stack.Push(sorted[0]);
            return new List<Vector2>(stack);
        }

        /// <summary>
        /// Returns the set of points sorted in increasing order of the angle around the point with the lowest y
        /// coordinate.
        /// </summary>
        /// <param name="points">the set of points.</param>
        /// <returns>Returns the set of points sorted in increasing order of the degrees around the point with the
        /// lowest y coordinate.</returns>
        private static List<Vector2> GetSortedFromLowestY(IEnumerable<Vector2> points)
        {
            Vector2 lowestY = points.Min(YXComparer.Instance);
            var sorted = new List<Vector2>(points);
            sorted.Sort(CompareAngle);
            return sorted;


            int CompareAngle(Vector2 pA, Vector2 pB)
            {
                var x1 = pA - lowestY;
                var x2 = pB - lowestY;
                return -Math.Sign(x1.X * x2.Y - x2.X * x1.Y);
                //-----------------------

                double slopeA = pA.Y == lowestY.Y ? 0.0 : Slope(lowestY, pA);
                double slopeB = pB.Y == lowestY.Y ? 0.0 : Slope(lowestY, pB);
                //if (pA == lowestY)
                //    return -1;
                //if (pB == lowestY)
                //    return 1;
                if (slopeA == slopeB) {
                    double distanceAToLowestY = DistanceXY(pA, lowestY);
                    double distanceBToLowestY = DistanceXY(pB, lowestY);
                    return distanceAToLowestY.CompareTo(distanceBToLowestY);
                }

                if (slopeA > 0.0 && slopeB < 0.0) {
                    return -1;
                } else if (slopeA < 0.0 && slopeB > 0.0) {
                    return 1;
                } else {
                    return slopeA.CompareTo(slopeB);
                }
            }

            static double DistanceXY(Vector2 a, Vector2 b)
            {
                return Math.Abs((double)a.X - b.X) + Math.Abs((double)a.Y - b.Y);
            }
        }

        /// <summary>
        /// Removes all collinear points in the list of sorted points. If a straight line can be drawn through the
        /// points A, B, C, D then B and C are removed from the list.
        /// </summary>
        /// <param name="sorted">the list of sorted points.</param>
        private static void RemoveCollinearPoints(List<Vector2> sorted)
        {
            Vector2 lowestY = sorted[0];
            for (int i = sorted.Count - 1; i > 1; i--) {
                double slopeB = Slope(lowestY, sorted[i]);
                double slopeA = Slope(lowestY, sorted[i - 1]);
                if (slopeB == slopeA) {
                    sorted.RemoveAt(i - 1);
                }
            }
        }

        private static double Slope(Vector2 p1, Vector2 p2)
        {
            if (p1.X == p2.X) {
                return Double.PositiveInfinity;
            } else {
                return ((double)p2.Y - p1.Y) / ((double)p2.X - p1.X);
            }
        }
    }
}
