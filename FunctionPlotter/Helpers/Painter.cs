using EnsureArg;
using FunctionPlotter.Domain.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FunctionPlotter.Helpers
{
    public sealed class Painter : IPainter
    {
        private readonly Graphics _graphics;
        private readonly Bitmap _bitmap;
        private readonly int _axisGap = 5;

        public Painter(int width, int height)
        {
            Ensure.Arg(width != 0);
            Ensure.Arg(height != 0);

            _bitmap = new Bitmap(width, height);
            _graphics = Graphics.FromImage(_bitmap);

            Init();
        }

        public void Init()
        {
            _graphics.ScaleTransform(1.0F, -1.0F);
            _graphics.TranslateTransform(0.0F, -_bitmap.Height);
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _graphics.FillRectangle(Brushes.White, 0, 0, _bitmap.Width, _bitmap.Height);
        }

        public void DrawAxis(int originOffset)
        {
            _graphics.DrawLine(new Pen(Brushes.Black, 1), new Point(originOffset - _axisGap, originOffset - _axisGap),
                new Point(originOffset - _axisGap, _bitmap.Height));
            _graphics.DrawLine(new Pen(Brushes.Black, 1), new Point(originOffset - _axisGap, originOffset - _axisGap),
                new Point(_bitmap.Width, originOffset - _axisGap));
        }

        public void DrawFunction(List<PointF> points)
        {
            for (var i = 0; i < points.Count - 1; ++i)
            {
                DrawLine(points[i], points[i + 1]);
            }
        }

        public void DrawIntegral(List<(PointF, PointF)> points)
        {
            Ensure.Arg(points).IsNotNull();

            foreach (var pointPair in points)
            {
                DrawRec(pointPair.Item1, pointPair.Item2);
            }
        }

        public Bitmap GetBitmap()
        {
            return _bitmap;
        }

        public void SaveImage(string filePath)
        {
            Ensure.Arg(filePath)
                .IsNotNullOrEmpty()
                .And()
                .IsNotNullOrWhiteSpace();

            _bitmap.Save(filePath);
        }

        private void DrawLine(PointF firstPoint, PointF secondPoint)
        {
            _graphics.DrawLine(new Pen(Brushes.Black, 2), firstPoint, secondPoint);
        }

        private void DrawRec(PointF upperLeftPoint, PointF lowerRightPoint)
        {
            var width = lowerRightPoint.X - upperLeftPoint.X;
            var height = upperLeftPoint.Y - lowerRightPoint.Y;

            _graphics.DrawRectangle(new Pen(Color.FromArgb(64, 0, 255, 0), 1), upperLeftPoint.X, lowerRightPoint.Y,
                width, height);
        }

        public void DrawString(PointF point, string input, int size)
        {
            _graphics.DrawString(input, new Font("Arial", size), new SolidBrush(Color.Black), point,
                new StringFormat());
        }

        public void ResetTransform()
        {
            _graphics.ResetTransform();
        }

        public Matrix GetTransform()
        {
            return
                _graphics.Transform;
        }
    }
}