using System.Collections.Generic;
using System.Drawing;

namespace FunctionPlotter.Helpers
{
    public sealed class Painter
    {
        private readonly Graphics _graphics;
        private readonly Bitmap _bitmap;
        private readonly int _axisGap = 5;

        public Painter(int width, int height)
        {
            _bitmap = new Bitmap(width, height);
            _graphics = Graphics.FromImage(_bitmap);

            Init();
        }

        private void Init()
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
            foreach (var pointPair in points)
            {
                DrawRec(pointPair.Item1, pointPair.Item2);
            }
        }

        public Bitmap GetBitmap()
        {
            return _bitmap;
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
    }
}