using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace FunctionPlotter.Helpers
{
    public static class Converters
    {
        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using var memory = new MemoryStream();

            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Position = 0;

            var bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            bitmapimage.StreamSource = memory;
            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapimage.EndInit();

            return bitmapimage;
        }

        public static List<float> GetScaledValues(List<float> points, double scaledMin, double scaledMax)
        {
            var result = new List<float>(points.Count);

            var minValue = points.Min();
            var maxValue = points.Max();

            if (Math.Abs(minValue - maxValue) < 1e-3)
            {
                minValue -= 5;
                maxValue += 5;
            }

            foreach (var point in points)
            {
                var newValue = (scaledMax - scaledMin) * (point - minValue) / (maxValue - minValue) + scaledMin;

                result.Add((float)newValue);
            }

            return result;
        }
    }
}