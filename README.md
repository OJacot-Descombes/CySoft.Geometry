# CySoft.Geometry
## C# geometry tools

### Convex Hull
QuickHull algorithm for finding the smallest polygon enclosing a set of points.
Author: Olivier Jacot-Descombes.

Based on the Java Script implementation https://github.com/claytongulick/quickhull by Clay Gulick.

Compared to two Graham Scan implementations I experimented with, this QuickHull implementation is much more stable. Graham Scan suffers from numerical problems due to floating point imprecision (probably due to the delicate relative angle calculations).

The Quick hull algorithm has an expected runtime of **O(n log(n))**.
See also: [Quickhull (Wikipedia)](https://en.wikipedia.org/wiki/Quickhull)

Although the point coordinates are given as `float`, we do all calculations in `double` precision.

### Bounding Rectangles
Methods that find oriented bounding rectangles from an unsorted set of points or from a convex hull. Uses the **rotating calipers** algorithm.

You can find the best bounding rectangle through a delegate specifying the desired criteria like smallest aera, smallest width, smallest side ratio, etc.

#### Copyright © 2021, Olivier Jacot-Descombes
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
