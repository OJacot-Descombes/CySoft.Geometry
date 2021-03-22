using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CySoft.Geometry.Helpers;

namespace CySoft.Geometry
{
    /// <summary>
    /// Methods that find oriented bounding rectangles from an unsorted set of points or from a convex hull.
    /// </summary>
    /// <remarks>Uses the rotating calipers algorithm.</remarks>
    public static class BoundingRectangles
    {
        /// <summary>
        /// Delegate telling whether a rectangle is better than a reference rectangle according to a user-defined
        /// criterion.
        /// </summary>
        /// <param name="candidate">The rectangle to be evaluated.</param>
        /// <param name="reference">A rectangle against which we evaluate the candidate.</param>
        /// <returns></returns>
        public delegate bool CompareRectPredicate(OrientedRectangle candidate, OrientedRectangle reference);

        /// <summary>
        /// Gets the best oriented bounding rectangle enclosing an arbitrary set of points according to a user-defined
        /// criterion.
        /// </summary>
        /// <remarks>The returned rectangle is always aligned with at least one edge of the convex hull of the points.
        /// For the minimum aera and minimum width this will return the optimal rectangle. For other criteria, the
        /// rectangle might be sub-optimal.<br/><br/>
        /// This calls <c>ConvexHull.Compute(points);</c></remarks>
        /// <param name="points">Arbitrary set of points.</param>
        /// <param name="isBetter">Delegate telling whether a rectangle is better than a reference rectangle.</param>
        /// <returns>The best oriented bounding rectangle.</returns>
        public static OrientedRectangle OptimalFromPoints(List<Vector2> points, CompareRectPredicate isBetter)
        {
            var convexHull = QuickHull.Compute(points);
            return OptimalFromConvexHull(convexHull, isBetter);
        }

        /// <summary>
        /// Gets the best oriented bounding rectangle enclosing a convex hull or polygon according to a user-defined
        /// criterion. You can call <c>ConvexHull.Compute(points);</c> to create a convex hull.
        /// </summary>
        /// <remarks>The returned rectangle is always aligned with at least one edge of the convex hull. For the
        /// minimum aera and minimum width this will return the optimal rectangle. For other criteria, the rectangle
        /// might be sub-optimal.</remarks>
        /// <param name="convexHull">A convex hull or polynome.</param>
        /// <param name="isBetter">Delegate telling whether a rectangle is better than a reference rectangle.</param>
        /// <returns>The best oriented bounding rectangle.</returns>
        public static OrientedRectangle OptimalFromConvexHull(IList<Vector2> convexHull, CompareRectPredicate isBetter)
        {
            if (convexHull.Count == 0) {
                return default;
            }

            var hullStatus = new HullStatus(convexHull);
            var caliperSet = new CalpierSet(hullStatus); // Creates initial axis-aligned calipers.
            caliperSet.RotateBySmallestTheta();

            var bestRectangle = caliperSet.AsOrientedRectangle();
            while (hullStatus.UndoneCount > 0) {
                caliperSet.RotateBySmallestTheta();
                var candidateRectangle = caliperSet.AsOrientedRectangle();
                if (isBetter(candidateRectangle, bestRectangle)) {
                    bestRectangle = candidateRectangle;
                }
            }

            return bestRectangle;
        }

        /// <summary>
        /// Returns all oriented bounded rectangles aligned with at least one edge of the convex hull of the points.
        /// </summary>
        /// <remarks>This calls <c>ConvexHull.Compute(points);</c></remarks>
        /// <param name="points">Arbitrary set of points.</param>
        /// <returns>List of oriented bounded rectangles.</returns>
        public static List<OrientedRectangle> AllFromPoints(IList<Vector2> points)
        {
            var convexHull = ConvexHull.Compute(points);
            return AllFromConvexHull(convexHull);
        }

        /// <summary>
        /// Returns all distinct oriented bounded rectangles aligned with at least one edge of the convex hull.
        /// </summary>
        /// <param name="convexHull">A convex hull or polynome.</param>
        /// <returns>List of oriented bounded rectangles.</returns>
        public static List<OrientedRectangle> AllFromConvexHull(IList<Vector2> convexHull)
        {
            var resultList = new List<OrientedRectangle>();
            if (convexHull.Count == 0) {
                return resultList;
            }

            var hullStatus = new HullStatus(convexHull);
            var caliperSet = new CalpierSet(hullStatus); // Creates initial axis-aligned calipers.
            caliperSet.RotateBySmallestTheta();

            OrientedRectangle rect = caliperSet.AsOrientedRectangle();
            resultList.Add(rect);
            while (hullStatus.UndoneCount > 0) {
                caliperSet.RotateBySmallestTheta();
                rect = caliperSet.AsOrientedRectangle();
                resultList.Add(rect);
            }

            return resultList;
        }

        private struct CalpierSet
        {
            private readonly HullStatus _hullStatus;

            public CalpierSet(HullStatus hullStatus)
            {
                _hullStatus = hullStatus;

                // Create initial axis-aligned calipers
                IList<Vector2> convexHull = hullStatus.ConvexHull;
                c0 = new Caliper(hullStatus, GetIndex(convexHull, Corner.UpperRight), Caliper.Deg90);
                c1 = new Caliper(hullStatus, GetIndex(convexHull, Corner.UpperLeft), Caliper.Deg180);
                c2 = new Caliper(hullStatus, GetIndex(convexHull, Corner.LowerLeft), Caliper.Deg270);
                c3 = new Caliper(hullStatus, GetIndex(convexHull, Corner.LowerRight), 0);
            }

            public Caliper c0, c1, c2, c3;

            public void RotateBySmallestTheta()
            {
                bool wasDone0, wasDone1, wasDone2, wasDone3;
                do {
                    double smallestTheta = GetSmallestTheta();
                    c0.RotateBy(smallestTheta, out wasDone0);
                    c1.RotateBy(smallestTheta, out wasDone1);
                    c2.RotateBy(smallestTheta, out wasDone2);
                    c3.RotateBy(smallestTheta, out wasDone3);
                } while (_hullStatus.UndoneCount > 0 && (wasDone0 || wasDone1 || wasDone2 || wasDone3));
            }

            public OrientedRectangle AsOrientedRectangle()
            {
                return new OrientedRectangle(
                    c3.IntersectWith(c0), c0.IntersectWith(c1), c1.IntersectWith(c2), c2.IntersectWith(c3));
            }

            private double GetSmallestTheta()
            {
                double theta0 = c0.AngleToNextPoint;
                double theta1 = c1.AngleToNextPoint;
                double theta2 = c2.AngleToNextPoint;
                double theta3 = c3.AngleToNextPoint;

                double theta = theta0;
                if (theta1 < theta) theta = theta1;
                if (theta2 < theta) theta = theta2;
                if (theta3 < theta) theta = theta3;
                return theta;
            }

            private static int GetIndex(IList<Vector2> convexHull, Corner corner)
            {
                Vector2 point = convexHull[0];
                int index = 0;
                for (int i = 1; i < convexHull.Count; i++) {
                    Vector2 temp = convexHull[i];
                    bool change = corner switch {
                        Corner.UpperRight => temp.X > point.X || temp.X == point.X && temp.Y > point.Y,
                        Corner.UpperLeft => temp.Y > point.Y || temp.Y == point.Y && temp.X < point.X,
                        Corner.LowerLeft => temp.X < point.X || temp.X == point.X && temp.Y < point.Y,
                        Corner.LowerRight => temp.Y < point.Y || temp.Y == point.Y && temp.X > point.X,
                        _ => false,
                    };
                    if (change) {
                        index = i;
                        point = temp;
                    }

                }
                return index;
            }
        }
    }
}
