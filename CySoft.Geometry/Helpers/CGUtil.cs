using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CySoft.Geometry.Helpers
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
     *          in Java.
     * Developed by : Bart Kiers, bart@big-o.nl
     */

    /// <summary>
    /// A computational geometry (CG) utility class.
    /// </summary>
    /// <remarks>
    /// Author: Bart Kiers, bart@big-o.nl <br />
    /// Date: Mar 11, 2010
    /// </remarks>
    public static class CGUtil
    {
        ///// <summary>
        ///// Returns <c>true</c> if all <c>points</c> form a clock wise (right) rotation. If <c>points</c> are less than
        ///// 3 or if all <c>points</c> are collinear, <c>true</c> is also returned.
        ///// </summary>
        ///// <param name="points">the points to check.</param>
        ///// <returns><c>true</c> if all <c>points</c> form a clock wise (right) rotation.</returns>
        //public static bool AllClockWise(IList<RPoint2D> points)
        //{
        //    if (points.Count < 3 || AllCollinear(points)) {
        //        return true;
        //    }

        //    for (int i = 2; i < points.Count; i++) {
        //        RPoint2D a = points[i - 2];
        //        RPoint2D b = points[i - 1];
        //        RPoint2D c = points[i];
        //        if (CGUtil.FormsLeftTurn(a, b, c)) {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        ///// <summary>
        ///// Returns <c>true</c> if all <c>points</c> are collinear.
        ///// </summary>
        ///// <param name="points">the list of points to check.</param>
        ///// <returns><c>true</c> if all <c>points</c> are collinear.</returns>
        ///// <exception cref="IllegalArgumentException">if <c>points</c> contains less
        /////                                  than 3 points.</exception>
        //public static bool AllCollinear(IList<RPoint2D> points)
        //{
        //    if (points.Count < 3) {
        //        throw new ArgumentException("List must contain at least 3 points");
        //    }

        //    for (int i = 0; i < points.Count - 2; i++) {
        //        if (!CGUtil.Collinear(points[i], points[i + 1], points[i + 2])) {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        ///// <summary>
        ///// Returns <c>true</c> if all <c>points</c> form
        ///// a counter clock wise (left) rotation. If <c>points</c> are
        ///// less than 3 or if all <c>points</c> are collinear,
        ///// <c>true</c> is also returned.
        ///// </summary>
        ///// <param name="points">the points to check.</param>
        ///// <returns><c>true</c> if all <c>points</c> form
        /////         a counter clock wise (left) rotation.</returns>
        //public static bool AllCounterClockWise(IList<RPoint2D> points)
        //{
        //    return points.Count < 3 || AllCollinear(points) || !AllClockWise(points);
        //}

        ///// <summary>
        ///// Checks whether <c>a</c>, <c>b</c> and <c>c</c>
        ///// are collinear. Note that the order of the points does not matter:
        ///// as <c>collinear((1,1), (3,3), (5,5))</c>, so will
        ///// <c>collinear((1,1), (5,5), (3,3))</c> return <c>true</c>.
        ///// </summary>
        ///// <param name="a">the starting point.</param>
        ///// <param name="b">the middle point.</param>
        ///// <param name="c">the last point.</param>
        ///// <returns><c>true</c> iff the points are collinear.</returns>
        //public static bool Collinear(RPoint2D a, RPoint2D b, RPoint2D c)
        //{
        //    return CrossProduct(a, b, c).Equals(Rational.ZERO);
        //}

        ///// <summary>
        ///// <p>
        ///// Finds points in the <c>data</c> String. The <c>data</c>
        ///// String is first being split on it's white spaces after which each individual
        ///// token in the array is parsed into a {@link compgeom.Rational}.
        ///// </p>
        ///// <p>So for example, the following Strings will be properly parsed and
        ///// converted into a <c>List</c> of <c>RPoint2D</c>'s:</p>
        ///// <ul>
        ///// <li><c>"10 20 30 40 50 60"</c></li>
        ///// <li><c>"250.99 96/5 -252 11"</c></li>
        ///// </ul>
        ///// <p>But these will throw an exception:</p>
        ///// <ul>
        ///// <li><c>""</c> (an empty String)</li>
        ///// <li><c>"250.99 96/5 -252"</c> (uneven amount of numbers)</li>
        ///// </ul>
        ///// </summary>
        ///// <param name="data">the String containing the numbers.</param>
        ///// <returns>a list of points.</returns>
        ///// <exception cref="IllegalArgumentException">if <c>data</c> contains an uneven number of
        /////                                  {@link compgeom.Rational}'s, or no none at all.
        /////                                  Or if one of the rationals in <c>data</c> is
        /////                                  not properly formatted.</exception>
        ///// <remarks>@see compgeom.Rational#Rational(String)</remarks>
        //public static IList<RPoint2D> CreateRPoint2DList(string data)
        //{
        //    if (data.Matches("\\\\s*")) {
        //        return new List<RPoint2D>();
        //    }

        //    IList<Rational> rationals = new List<Rational>();
        //    IList<RPoint2D> points = new List<RPoint2D>();
        //    string tokens = data.Trim().Split("\\\\s++");
        //    foreach (string t in tokens) {
        //        BigInteger nd = RationalParser.Parse(t);
        //        rationals.Add(new Rational(nd[0], nd[1]));
        //    }

        //    if (rationals.IsEmpty()) {
        //        throw new ArgumentException("'data' does not contain any valid Rationals");
        //    }

        //    if (rationals.Count % 2 == 1) {
        //        throw new ArgumentException("'data' contains an uneven number of Rationals");
        //    }

        //    for (int i = 1; i < rationals.Count; i += 2) {
        //        points.Add(new RPoint2D(rationals[i - 1], rationals[i]));
        //    }

        //    return points;
        //}

        ///// <summary>
        ///// Helper method to create a <c>java.util.List</c> of
        ///// <c>RPoint2D</c>'s given an array of x- and y-coordinates.
        ///// </summary>
        ///// <param name="xs">the x-coordinates.</param>
        ///// <param name="ys">the y-coordinates.</param>
        ///// <returns>a <c>java.util.List</c> of <c>RPoint2D</c>'s
        /////         given an array of X- and y-coordinates.</returns>
        ///// <exception cref="IllegalArgumentException">if <c>xs</c> and <c>ys</c>
        /////                                  don't have the same size.</exception>
        //public static IList<RPoint2D> CreateRPoint2DList(int xs, int ys)
        //{
        //    if (xs.length != ys.length) {
        //        throw new ArgumentException("xs.length != ys.length");
        //    }

        //    IList<RPoint2D> points = new List<RPoint2D>();
        //    for (int i = 0; i < xs.length; i++) {
        //        points.Add(new RPoint2D(xs[i], ys[i]));
        //    }

        //    return points;
        //}

        ///// <summary>
        ///// <p>
        ///// Finds line segments in the <c>data</c> String. The <c>data</c>
        ///// String is first being split on it's white spaces after which each individual
        ///// token in the array is parsed into a {@link compgeom.Rational}.
        ///// </p>
        ///// <p>
        ///// So for example, the following Strings will be properly parsed and
        ///// converted into a <c>List</c> of <c>RLineSegment2D</c>'s:
        ///// </p>
        ///// <ul>
        ///// <li><c>"250 96 252 96 249 94 251 95 249 95 252 97"</c></li>
        ///// <li><c>"250 96 251 95"</c></li>
        ///// </ul>
        ///// <p>But these will throw an exception:</p>
        ///// <ul>
        ///// <li><c>""</c> (an empty String)</li>
        ///// <li><c>"250 9 252 6 251 5"</c> (6 values: not a multiple of 4)</li>
        ///// </ul>
        ///// </summary>
        ///// <param name="data">the String containing the numbers.</param>
        ///// <returns>a list of line segments.</returns>
        ///// <exception cref="IllegalArgumentException">if <c>data</c> contains a total number of
        /////                                  {@link compgeom.Rational}'s that is not a multiple of 4, or
        /////                                  none at all.
        /////                                  Or if one of the rationals in <c>data</c> is
        /////                                  not properly formatted.</exception>
        ///// <remarks>@see compgeom.Rational#Rational(String)</remarks>
        //public static IList<RLineSegment2D> CreateRLineSegment2DList(string data)
        //{
        //    if (data.Matches("\\\\s*")) {
        //        return new List<RLineSegment2D>();
        //    }

        //    IList<RLineSegment2D> segments = new List<RLineSegment2D>();
        //    IList<RPoint2D> points = CreateRPoint2DList(data);
        //    if (points.Count % 2 == 1) {
        //        throw new ArgumentException("'data' does not contain a multiple of 4 Rationals");
        //    }

        //    for (int i = 1; i < points.Count; i += 2) {
        //        segments.Add(new RLineSegment2D(points[i - 1], points[i]));
        //    }

        //    return segments;
        //}

        ///// <summary>
        ///// Creates a list of <c>RLineSegment2D</c>s based on four parallel arrays:
        ///// <pre>
        ///// segment[0] = point(xs1[0],ys1[0]) <-> point(xs2[0],ys2[0])
        ///// segment[1] = point(xs1[1],ys1[1]) <-> point(xs2[1],ys2[1])
        ///// ...
        ///// segment[n] = point(xs1[n],ys1[n]) <-> point(xs2[n],ys2[n])
        ///// </pre>
        ///// </summary>
        ///// <param name="xs1">the x coordinates from the first point.</param>
        ///// <param name="ys1">the y coordinates from the first point.</param>
        ///// <param name="xs2">the x coordinates from the second point.</param>
        ///// <param name="ys2">the y coordinates from the second point.</param>
        ///// <returns>a list of <c>RLineSegment2D</c>s based on four parallel arrays.</returns>
        ///// <exception cref="IllegalArgumentException">when the arrays <c>xs1</c>,
        /////                                  <c>ys1</c>, <c>xs2</c>
        /////                                  and <c>ys2</c> do not have
        /////                                  the same size.</exception>
        //public static IList<RLineSegment2D> CreateRLineSegment2DList(int xs1, int ys1, int xs2, int ys2)
        //{
        //    if (xs1.length != ys1.length || xs1.length != xs2.length || xs1.length != ys2.length) {
        //        throw new ArgumentException("all four array must have the same size");
        //    }

        //    IList<RLineSegment2D> segments = new List<RLineSegment2D>();
        //    IList<RPoint2D> pts1 = CreateRPoint2DList(xs1, ys1);
        //    IList<RPoint2D> pts2 = CreateRPoint2DList(xs2, ys2);
        //    for (int i = 0; i < pts1.Count; i++) {
        //        segments.Add(new RLineSegment2D(pts1[i], pts2[i]));
        //    }

        //    return segments;
        //}

        /// <summary>
        /// <p>Calculates the cross product of two vectors denoted by the three
        /// points <c>a</c>, <c>b</c> and <c>c</c>.</p>
        /// <p>In two dimensions, the cross product for <c>U = (U_x,U_y)</c>
        /// and <c>V = (V_x,V_y)</c> is:</p>
        /// <p>
        /// <pre>UxV
        /// == det(UV)
        /// == (U_x * V_y) - (U_y * V_x)
        /// == ((b.x-a.x)*(c.y-a.y)) - ((c.x-a.x)*(b.y-a.y))</pre>
        /// </p>
        /// <p>where <c>det(A)</c> is the determinant.</p>
        /// <p>See
        /// <a href="http://mathworld.wolfram.com/CrossProduct.html">http://mathworld.wolfram.com/CrossProduct.html</a>
        /// </p>
        /// </summary>
        /// <param name="a">the starting point.</param>
        /// <param name="b">the middle point.</param>
        /// <param name="c">the last point: it makes the turn to the left or right
        ///          (or is collinear with <c>a</c> and <c>b</c>).</param>
        /// <returns>the cross product of two vectors denoted by the three
        ///         points <c>a</c>, <c>b</c> and <c>c</c></returns>
        public static double CrossProduct(Vector2 a, Vector2 b, Vector2 c)
        {
            double ax = a.X, ay = a.Y;
            double bx = b.X, by = b.Y;
            double cx = c.X, cy = c.Y;
            return (bx - ax) * (cy - ay) - (cx - ax) * (by - ay);
        }

        /// <summary>
        /// Checks whether <c>a</c>, <c>b</c> and <c>c</c>
        /// form a left turn: where <c>a</c> 'walks' towards
        /// <c>c</c> through <c>b</c>.
        /// </summary>
        /// <param name="a">the starting point.</param>
        /// <param name="b">the middle point.</param>
        /// <param name="c">the last point.</param>
        /// <returns><c>true</c> iff the path <c>a->b->c</c>
        ///         forms a left turn</returns>
        public static bool FormsLeftTurn(Vector2 a, Vector2 b, Vector2 c)
        {
            return CrossProduct(a, b, c) > 0.0;
        }

        ///// <summary>
        ///// Checks whether <c>a</c>, <c>b</c> and <c>c</c>
        ///// form a right turn, where <c>a</c> 'walks' towards
        ///// <c>c</c> through <c>b</c>.
        ///// </summary>
        ///// <param name="a">the starting point.</param>
        ///// <param name="b">the middle point.</param>
        ///// <param name="c">the last point.</param>
        ///// <returns><c>true</c> iff the path <c>a->b->c</c>
        /////         forms a right turn</returns>
        //public static bool FormsRightTurn(RPoint2D a, RPoint2D b, RPoint2D c)
        //{
        //    return CrossProduct(a, b, c).IsLessThan(Rational.ZERO);
        //}

        ///// <summary>
        ///// <p>
        ///// Returns the index of the extremal point from a list of points
        ///// in <c>O(n)</c> time.
        ///// </p>
        ///// </summary>
        ///// <param name="points">the list of points to get the extreme point's index from.</param>
        ///// <param name="e">the {@link compgeom.util.Extremal}.</param>
        ///// <returns>the index of the extremal point from a given list of points.</returns>
        ///// <exception cref="IllegalArgumentException">if <c>points</c> is empty.</exception>
        //public static int GetExtremalIndex(IList<Vector2> points, Extremal e)
        //{
        //    if (points.Count == 0) {
        //        throw new ArgumentException("points must contain at least one element");
        //    }

        //    Vector2 extremalPoint = points[0];
        //    int extremalIndex = 0;
        //    for (int i = 1; i < points.Count; i++) {
        //        Vector2 p = points[i];
        //        if (e.MoreExtremeThan(p, extremalPoint)) {
        //            extremalPoint = p;
        //            extremalIndex = i;
        //        }
        //    }

        //    return extremalIndex;
        //}

        ///// <summary>
        ///// <p>
        ///// Returns the extremal point from a list of points in <c>O(n)</c> time .
        ///// </p>
        ///// </summary>
        ///// <param name="points">the list of point to get the extreme point from.</param>
        ///// <param name="e">the {@link compgeom.util.Extremal}.</param>
        ///// <returns>the extremal point from a given list of points.</returns>
        ///// <exception cref="IllegalArgumentException">if <c>points</c> is empty.</exception>
        //public static Vector2 GetExtremalPoint(ICollection<Vector2> points, Func<Vector2, Vector2, bool> isArg1MoreExtreme)
        //{
        //    Vector2 extreme = points[0];
        //    for (int i = 1; i < points.Count; i++) {
        //        if (isArg1MoreExtreme(points[i], extreme)) {

        //        }
        //    }
        //    return extreme;
        //}

        ///// <summary>
        ///// Returns a frequency table of all <c>K</c> objects in
        ///// the <c>keys</c> varargs parameter. For example, if
        ///// varargs looks like: <c>[a, b, a, a, b, c, a]</c>, this
        ///// method returns <c>{a:4, b:2, c:1}</c> (not particularly
        ///// in that order!).
        ///// </summary>
        ///// <param name="keys">the keys to build a frequency map of.</param>
        ///// <returns>a frequency table of all <c>K</c> objects in
        /////         the <c>keys</c> varargs parameter.</returns>
        //public static Map<K, int> GetFrequencyMap<K>(params K[] keys)
        //{
        //    Map<K, int> map = new LinkedHashMap<K, int>();
        //    foreach (K k in keys) {
        //        PutFrequencyMap(map, k);
        //    }

        //    return map;
        //}

        ///// <summary>
        ///// Returns a frequency table of all <c>K</c> objects in
        ///// the <c>keys</c> collection. For example, if the
        ///// collection looks like: <c>[a, b, a, a, b, c, a]</c>,
        ///// this method returns <c>{a:4, b:2, c:1}</c> (not
        ///// particularly in that order!).
        ///// </summary>
        ///// <param name="keys">the keys to build a frequency map of.</param>
        ///// <returns>a frequency table of all <c>K</c> objects in
        /////         the <c>keys</c> collection.</returns>
        //public static Map<K, int> GetFrequencyMap<K>(Collection<K> keys)
        //{
        //    Map<K, int> map = new LinkedHashMap<K, int>();
        //    foreach (K k in keys) {
        //        PutFrequencyMap(map, k);
        //    }

        //    return map;
        //}

        ///// <summary>
        ///// <p>
        ///// Checks if <c>p1</c> is more extreme than <c>p2</c>. Note
        ///// that this method returns <c>false</c> in case <c>p1</c>
        ///// equals <c>p2</c>.
        ///// </p>
        ///// <p>
        ///// This is a convenience method for:
        ///// {@link Extremal#moreExtremeThan(compgeom.RPoint2D, compgeom.RPoint2D)}
        ///// </p>
        ///// </summary>
        ///// <param name="p1">the first point.</param>
        ///// <param name="p2">the second point.</param>
        ///// <param name="e">the {@link Extremal}.</param>
        ///// <returns><c>true</c> iff <c>p1</c> is more extreme
        /////         than <c>p2</c> based on <c>e</c>.</returns>
        //public static bool MoreExtremeThan(RPoint2D p1, RPoint2D p2, Extremal e)
        //{
        //    return e.MoreExtremeThan(p1, p2);
        //}

        ///// <summary>
        ///// Puts a new <c>key</c> in a <c>Map</c> and increasing the
        ///// count of the frequency if <c>key</c> is already present. If
        ///// <c>key</c> is not present, the <c>Rational</c> value will
        ///// be initialized to 1.
        ///// </summary>
        ///// <param name="map">the map to store <c>key</c> in.</param>
        ///// <param name="key">the key to store in the map and count the frequency of.</param>
        //private static void PutFrequencyMap<K>(Map<K, int> map, K key)
        //{
        //    int count = map.Remove(key);
        //    if (count == null)
        //        count = 0;
        //    count++;
        //    map.Put(key, count);
        //}

        ///// <summary>
        ///// Returns the points from a given path but then without collinear points.
        ///// For example, from the path <c>(1,1) (1,3) (1,5) (3,6) (5,7)</c>
        ///// this methods returns a new list containing the points:
        ///// <c>(1,1) (1,5) (5,7)</c>.
        ///// </summary>
        ///// <param name="path">the list of points.</param>
        ///// <returns>the points from a given path but then without collinear points.</returns>
        //public static IList<RPoint2D> RemoveCollinear(IList<RPoint2D> path)
        //{
        //    if (path.Count < 3) {
        //        return new List<RPoint2D>(path);
        //    }

        //    IList<RPoint2D> cleaned = new List<RPoint2D>();
        //    RPoint2D start = path[0];
        //    cleaned.Add(start);
        //    Rational previousSlope = new RLine2D(start, path[1]).slope;
        //    for (int i = 1; i < path.Count - 1; i++) {
        //        Rational nextSlope = new RLine2D(path[i], path[i + 1]).slope;
        //        if (!nextSlope.Equals(previousSlope)) {
        //            previousSlope = nextSlope;
        //            cleaned.Add(path[i]);
        //        }
        //    }

        //    cleaned.Add(path[path.Count - 1]);
        //    return cleaned;
        //}

        ///// <summary>
        ///// Sorts the array of points <i>in place</i> based on a given
        ///// {@link Extremal} in <c>O(n*log(n))</c> time.
        ///// For example, sorting points using <c>sort(RPoint2D[])</c>,
        ///// the array will be sorted where the points with the <b>least y coordinate</b>
        ///// (LOWER) will be placed at the start of the array. In case of equal y
        ///// coordinates, the point with the <b>larger x coordinate</b> (RIGHT) is
        ///// placed before the other point.
        ///// </summary>
        ///// <param name="points">the array of points to sort <i>in place</i>.</param>
        ///// <param name="e">the {@link Extremal} to sort on.</param>
        //public static void Sort(RPoint2D points, Extremal e)
        //{
        //    Arrays.Sort(points, e.comparator);
        //}

        ///// <summary>
        ///// Sorts the list of points <i>in place</i> based on a given
        ///// {@link Extremal} in <c>O(n*log(n))</c> time.
        ///// For example, sorting points using <c>sort(List)</c>,
        ///// the List will be sorted where the points with the <b>least y coordinate</b>
        ///// (LOWER) will be placed at the start of the List. In case of equal y
        ///// coordinates, the point with the <b>larger x coordinate</b> (RIGHT) is
        ///// placed before the other point.
        ///// </summary>
        ///// <param name="points">the list of points to sort <i>in place</i>.</param>
        ///// <param name="e">the {@link Extremal} to sort on.</param>
        //public static void Sort(IList<RPoint2D> points, Extremal e)
        //{
        //    Collections.Sort(points, e.comparator);
        //}

        ///// <summary>
        ///// <p>
        ///// Sorts the collection of points based on a given {@link Extremal}
        ///// in <c>O(n*log(n))</c> time and returns
        ///// the sorted points as an array. For example, sorting points using
        ///// <c>sortAndGetList(List)</c>, the new List
        ///// being returned will be sorted where the points with the <b>least y
        ///// coordinate</b> (LOWER) will be placed at the start of the List. In case
        ///// of equal y coordinates, the point with the <b>larger x coordinate</b>
        ///// (RIGHT) is placed before the other point.
        ///// </p>
        ///// </summary>
        ///// <param name="points">the collection of points to sort.</param>
        ///// <param name="e">the {@link Extremal} to sort on.</param>
        ///// <returns>a new sorted List containing the points from the
        /////         collection <c>points</c>.</returns>
        //public static RPoint2D SortAndGetArray(Collection<RPoint2D> points, Extremal e)
        //{
        //    RPoint2D array = points.ToArray(new RPoint2D[points.Count]);
        //    Sort(array, e);
        //    return array;
        //}

        ///// <summary>
        ///// <p>
        ///// Sorts the collection of points based on a given {@link Extremal}
        ///// in <c>O(n*log(n))</c> time and returns
        ///// the sorted points as a new List. For example, sorting points using
        ///// <c>sortAndGetList(List)</c>, the new List
        ///// being returned will be sorted where the points with the <b>least y
        ///// coordinate</b> (LOWER) will be placed at the start of the List. In case
        ///// of equal y coordinates, the point with the <b>larger x coordinate</b>
        ///// (RIGHT) is placed before the other point.
        ///// </p>
        ///// </summary>
        ///// <param name="points">the collection of points to sort.</param>
        ///// <param name="e">the {@link Extremal} to sort on.</param>
        ///// <returns>a new sorted List containing the points from the
        /////         collection <c>points</c>.</returns>
        //public static IList<RPoint2D> SortAndGetList(Collection<RPoint2D> points, Extremal e)
        //{
        //    IList<RPoint2D> list = new List<RPoint2D>(points);
        //    Sort(list, e);
        //    return list;
        //}
    }
}
