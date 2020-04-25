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
        }

        private void Init()
        {
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _graphics.FillRectangle(Brushes.White, 0, 0, _bitmap.Width, _bitmap.Height);
        }

        public void DrawFunction(List<Point> points)
        {
            for (var i = 0; i < points.Count - 1; ++i)
            {
                DrawLine(points[i], points[i + 1]);
            }
        }

        public Bitmap GetBitmap()
        {
            return _bitmap;
        }

        private void DrawLine(Point firstPoint, Point secondPoint)
        {
            _graphics.DrawLine(new Pen(Brushes.Black, 2), firstPoint, secondPoint);
        }
    }
}