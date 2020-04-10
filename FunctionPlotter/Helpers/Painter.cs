using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FunctionPlotter.Helpers
{
    public sealed class Painter
    {
        private readonly Graphics _graphics;
        private readonly Bitmap _bitmap;

        public Painter()
        {
            _bitmap = new Bitmap(1024, 1024);
            _graphics = Graphics.FromImage(_bitmap);
        }

        public void DrawFunction(List<Func<double, double>> function, double low, double high, double stepSize)
        {
            for (var range = low; range < high; range += stepSize)
            {
                var resultY = 0.0;
                resultY += function.Sum(entry => entry(range));
                DrawPoint(-(int)range + _bitmap.Width / 2, -(int)resultY + _bitmap.Height / 2);
            }
        }

        public Bitmap GetBitmap()
        {
            return _bitmap;
        }

        private void DrawPoint(int positionX, int positionY)
        {
            _graphics.FillRectangle(Brushes.Red, positionX, positionY, 1, 1);
        }
    }
}