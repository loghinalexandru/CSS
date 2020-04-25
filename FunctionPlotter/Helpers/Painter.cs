using System.Collections.Generic;
using System.Drawing;

namespace FunctionPlotter.Helpers
{
    public sealed class Painter
    {
        private readonly Graphics _graphics;
        private readonly Bitmap _bitmap;

        public Painter(int width, int height)
        {
            _bitmap = new Bitmap(width, height);
            _graphics = Graphics.FromImage(_bitmap);

            Init();
//            DrawAxis();
        }

        private void Init()
        {
            _graphics.ScaleTransform(1.0F, -1.0F);
            _graphics.TranslateTransform(0.0F, -_bitmap.Height);
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _graphics.FillRectangle(Brushes.White, 0, 0, _bitmap.Width, _bitmap.Height);
        }

        private void DrawAxis()
        {
            DrawLine(new Point(0, 50), new Point(0, _bitmap.Height - 1));
            DrawLine(new Point(50, 0), new Point(_bitmap.Width - 1, 0));

            var drawFont = new Font("Arial", 5);
            _graphics.DrawString("0", drawFont, new SolidBrush(Color.Black), 0, 0);
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
            foreach(var pointPair in points)
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

            _graphics.DrawRectangle(new Pen(Color.FromArgb(64, 0, 255, 0), 1), upperLeftPoint.X, lowerRightPoint.Y, width, height);
        }
    }
}