using System;
using System.Collections.Generic;
using System.Numerics;

namespace CySoft.Geometry.Helpers
{
    public static class Extensions
    {
        public static T Min<T>(this IEnumerable<T> source, IComparer<T> comparer)
        {
            using var sourceIterator = source.GetEnumerator();
            if (!sourceIterator.MoveNext()) {
                throw new InvalidOperationException("Sequence contains no elements");
            }
            T min = sourceIterator.Current;
            while (sourceIterator.MoveNext()) {
                T candidate = sourceIterator.Current;
                if (comparer.Compare(candidate, min) < 0) {
                    min = candidate;
                }
            }
            return min;
        }

        /// <summary>
        /// Computes the angle, in radians, that the line from p0 (this) to p1 has to the x axis
        /// </summary>
        /// <param name="p0">The start point of the line</param>
        /// <param name="p1">The end point of the line</param>
        /// <returns>The angle, in radians, that the line has to the x-axis</returns>
        public static float AngleToX(this Vector2 p0, Vector2 p1)
        {
            float dx = p1.X - p0.X;
            float dy = p1.Y - p0.Y;
            return MathF.Atan2(dy, dx);
        }

        /// <summary>
        /// Normalize the given angle, which is given in radians, to the range [0, 2PI]
        /// </summary>
        /// <param name="angle">The angle</param>
        /// <returns>The normalized angle</returns>
        public static float NormalizeAngle(this float angle)
        {
            const float TwoPI = MathF.PI + MathF.PI;

            return (angle + TwoPI) % TwoPI;
        }

        // Ported and refactored from the original Line2D.RelativeCCW implementation on
        // https://raw.githubusercontent.com/JetBrains/jdk8u_jdk/master/src/share/classes/java/awt/geom/Line2D.java
        // Copyright (c) 1997, 2013, Oracle and/or its affiliates. All rights reserved.

        /// <summary>
        /// Returns an indicator of where the specified point <c>p</c> lies with respect to the line segment from
        /// <c>p1</c> to <c>p2</c>. The return value can be either 1, -1, or 0 and indicates in which direction the
        /// specified line must pivot around its first end point, <c>p1</c>, in order to point at the specified point
        /// <c>p</c>.<br/><br/>
        /// A return value of 1 indicates that the line segment must turn in the direction that takes the positive X
        /// axis towards the negative Y axis. In the default coordinate system used by Java 2D, this direction is
        /// counterclockwise.<br/><br/>
        /// A return value of -1 indicates that the line segment must turn in the direction that takes the positive X
        /// axis towards the positive Y axis. In the default coordinate system, this direction is clockwise.<br/><br/>
        /// A return value of 0 indicates that the point lies exactly on the line segment. Note that an indicator value
        /// of 0 is rare and not useful for determining collinearity because of floating point rounding
        /// issues.<br/><br/>
        /// If the point is colinear with the line segment, but not between the end points, then the value will be -1
        /// if the point lies "beyond <c>p1</c>" or 1 if the point lies "beyond <c>p2</c>".
        /// </summary>
        /// <param name="p">The coordinate of the specified point to be compared with the specified line segment</param>
        /// <param name="p1">The coordinate of the start point of the specified line segment</param>
        /// <param name="p2">The coordinate of the end point of the specified line segment</param>
        /// <returns>An integer that indicates the position of the third specified coordinates with respect to the line
        /// segment formed by the first two specified coordinates.</returns>
        public static int RelativeCCW(this Vector2 p, Vector2 p1, Vector2 p2)
        {
            p2 -= p1;
            p -= p1;
            float px = p.X; float py = p.Y;
            float x2 = p2.X; float y2 = p2.Y;

            float ccw = px * y2 - py * x2;
            if (ccw == 0) {
                // The point is colinear, classify based on which side of the segment the point falls on. We can
                // calculate a relative value using the projection of px,py onto the segment - a negative value
                // indicates the point projects outside of the segment in the direction of the particular endpoint used
                // as the origin for the projection.
                ccw = px * x2 + py * y2;
                if (ccw > 0) {
                    // Reverse the projection to be relative to the original (x2,y2), x2 and y2 are simply negated. px
                    // and py need to have (x2 - x1) or (y2 - y1) subtracted from them (based on the original values)
                    // Since we really want to get a positive answer when the point is "beyond (x2,y2)", then we want
                    // to calculate the inverse anyway - thus we leave x2 & y2 negated.
                    px -= x2; py -= y2;
                    ccw = px * x2 + py * y2;
                    if (ccw < 0) {
                        ccw = 0;
                    }
                }
            }

            return (ccw < 0) ? -1 : ((ccw > 0) ? 1 : 0);
        }
    }
}
