using System;
using System.Collections.Generic;
using System.Numerics;
using CySoft.Geometry;

namespace WorkbenchConsole
{
    class Program
    {
        static void Main()
        {
            var points4 = new[] { new Vector2(1, 0), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) };
            var points3 = new[] { new Vector2(1, 0), new Vector2(0, 0), new Vector2(1, 1) };

            var hull4 = QuickHull.Compute(points4);
            var hull3 = QuickHull.Compute(points3);

            PrintHull(hull4);
            Console.WriteLine();
            PrintHull(hull3);

            static void PrintHull(IEnumerable<Vector2> hull)
            {
                foreach (Vector2 p in hull) {
                    Console.WriteLine(p);
                }
            }
        }
    }
}
