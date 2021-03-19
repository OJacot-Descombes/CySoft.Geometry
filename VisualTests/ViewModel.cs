using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using CySoft.Geometry;

namespace VisualTests
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly Random _random = new();
        private static readonly Font _font = new("Segoe UI", 8);

        private int _numberOfPoints = 10;
        private byte _pointSetIndex = 255;
        private Vector2[] _points;
        private List<Vector2> _convexHull;
        private Size _originalCanvasSize;

        private ResultKind _resultKind = ResultKind.Optimum;
        public ResultKind ResultKind
        {
            get { return _resultKind; }
            set {
                if (value != _resultKind) {
                    _resultKind = value;
                    OnPropertyChanged(nameof(ResultKind));
                }
            }
        }

        private PointSet _pointSet = PointSet.Random;
        public PointSet PointSet
        {
            get { return _pointSet; }
            set {
                if (value != _pointSet) {
                    _pointSet = value;
                    OnPropertyChanged(nameof(PointSet));
                    OnPropertyChanged(nameof(EnableNumberOfPoints));
                }
            }
        }

        public bool EnableNumberOfPoints
        {
            get { return _pointSet is PointSet.Random or PointSet.RandomInRaster; }
        }

        public int NumberOfPoints
        {
            get { return _numberOfPoints; }
            set {
                if (value != _numberOfPoints) {
                    _numberOfPoints = value;
                    OnPropertyChanged();
                }
            }
        }

        public void DrawNext(Control canvas, bool doGeneratePoints = true)
        {
            const int margin = 80;

            if (doGeneratePoints) {
                GeneratePoints(canvas, margin);
            }

            Graphics g = InitializeGraphics(canvas);
            using var hullPen = new Pen(Color.Blue, 1.8f);
            using var pointPen = new Pen(Color.Red, 1.8f);

            for (int i = 0; i < _convexHull.Count; i++) {
                g.DrawLine(hullPen,
                    _convexHull[i].ToPointF(),
                    _convexHull[(i + 1) % _convexHull.Count].ToPointF());
            }
            foreach (Vector2 p in _points) {
                g.DrawCircle(pointPen, p, 2.5f);
            }
            foreach (Vector2 p in _convexHull) {
                g.DrawCircle(hullPen, p, 2.5f);
            }

            if (ResultKind == ResultKind.All) {
                var rectangles = BoundingRectangles.AllFromConvexHull(_convexHull);
                Color c0 = Color.Green;
                Color c1 = Color.Red;
                for (var i = 0; i < rectangles.Count; i++) {
                    float f0 = (float)i / (rectangles.Count - 1);
                    float f1 = 1f - (float)i / (rectangles.Count - 1);
                    int R = (int)(c0.R * f0 + c1.R * f1);
                    int G = (int)(c0.G * f0 + c1.G * f1);
                    int B = (int)(c0.B * f0 + c1.B * f1);
                    using var pen = new Pen(Color.FromArgb(128, R, G, B));
                    DrawOrientedRectangle(g, pen, rectangles[i]);
                }
                g.DrawString($"Rectangles = {rectangles.Count:n0}, Hull points = {_convexHull.Count}", _font, Brushes.LightCoral, 5, 5);
            } else {
                OrientedRectangle minWidthRectangle = BoundingRectangles.OptimalFromConvexHull(
                    _convexHull, (a, b) => a.MinSideSquared < b.MinSideSquared);
                DrawOrientedRectangle(g, Pens.LightCoral, minWidthRectangle);
                g.DrawString($"min width w={minWidthRectangle.Width:n1}, r={minWidthRectangle.Ratio:n4}, a={minWidthRectangle.Area:n0}", _font, Brushes.LightCoral, 5, 5);

                OrientedRectangle minRatioRectangle = BoundingRectangles.OptimalFromConvexHull(
                    _convexHull, (a, b) => a.RatioSquared < b.RatioSquared);
                DrawOrientedRectangle(g, Pens.SkyBlue, minRatioRectangle);
                g.DrawString($"min ratio  w={minRatioRectangle.Width:n1}, r={minRatioRectangle.Ratio:n4}, a={minRatioRectangle.Area:n0}", _font, Brushes.SkyBlue, 5, 20);

                OrientedRectangle minAreaRectangle = BoundingRectangles.OptimalFromConvexHull(
                    _convexHull, (a, b) => a.AreaSquared < b.AreaSquared);
                DrawOrientedRectangle(g, Pens.YellowGreen, minAreaRectangle);
                g.DrawString($"min area   w={minAreaRectangle.Width:n1}, r={minAreaRectangle.Ratio:n4}, a={minAreaRectangle.Area:n0}", _font, Brushes.YellowGreen, 5, 35);
            }

            static void DrawOrientedRectangle(Graphics g, Pen pen, OrientedRectangle r)
            {
                g.DrawLine(pen, r.V0.ToPointF(), r.V1.ToPointF());
                g.DrawLine(pen, r.V1.ToPointF(), r.V2.ToPointF());
                g.DrawLine(pen, r.V2.ToPointF(), r.V3.ToPointF());
                g.DrawLine(pen, r.V3.ToPointF(), r.V0.ToPointF());
            }
        }

        private Graphics InitializeGraphics(Control canvas)
        {
            var g = canvas.CreateGraphics();
            g.Clear(canvas.BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float scaleFactor = MathF.Min(
                (float)canvas.Width / _originalCanvasSize.Width,
                (float)canvas.Height / _originalCanvasSize.Height);
            if (scaleFactor > 0) {
                g.ScaleTransform(scaleFactor, scaleFactor);
            }
            return g;
        }

        private void GeneratePoints(Control canvas, int margin)
        {
            switch (PointSet) {
                case PointSet.AlignmentTest:
                    GetAlignmentTestPoints(canvas, margin);
                    break;
                case PointSet.Random:
                    GetRandomPoints(canvas, margin);
                    break;
                case PointSet.RandomInRaster:
                    GetRandomPoints(canvas, margin, 40);
                    break;
                case PointSet.SpecificPolynome:
                    GetSpecificPolynome(canvas, margin);
                    break;
                default:
                    break;
            }


            _convexHull = ConvexHull.Compute(_points);
            _originalCanvasSize = canvas.Size;

            void GetAlignmentTestPoints(Control canvas, int margin)
            {
                const int size = 200;

                var bits = new BitArray(new byte[] { _pointSetIndex });
                margin *= 2;

                _points = new Vector2[4];
                _points[0] = new Vector2(0 + margin + Shift(0), 0 + margin + Shift(1));
                _points[1] = new Vector2(size + margin + Shift(2), 0 + margin + Shift(3));
                _points[2] = new Vector2(size + margin + Shift(4), size + margin + Shift(5));
                _points[3] = new Vector2(0 + margin + Shift(6), size + margin + Shift(7));
                _pointSetIndex++;

                int Shift(int bitIndex)
                {
                    return bits.Get(bitIndex) ? 40 : 0;
                }
            }

            void GetRandomPoints(Control canvas, int margin, int rasterSize = 1)
            {
                int w = canvas.Width - 2 * margin;
                int h = canvas.Height - 2 * margin;
                float rx = 0.5f * w;
                float ry = 0.5f * h;

                _points = new Vector2[_numberOfPoints];

                // Get random points in ellipse
                for (int i = 0; i < _numberOfPoints; i++) {
                    float x, y;
                    do {
                        x = _random.Next(0, w);
                        y = _random.Next(0, h);
                    } while ((x - rx).Squared() / rx.Squared() + (y - ry).Squared() / ry.Squared() > 1f);
                    x = (int)x / rasterSize * rasterSize;
                    y = (int)y / rasterSize * rasterSize;
                    _points[i] = new Vector2(x + margin, y + margin);
                }
            }

            void GetSpecificPolynome(Control canvas, int margin)
            {
                _points = new Vector2[] {
                    new(4, 12),
                    new(12, 12),
                    new(15, 5),
                    new(15, 4),
                    new(11, 0),
                    new(7, 0),
                    new(5, 1),
                    new(0, 4),
                    new(0, 9),
                    new(2, 11),
                };

                for (int i = 0; i < _points.Length; i++) {
                    _points[i] = 30 * _points[i] + new Vector2(margin, margin);
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
