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

        public static List<int> GetScaledValues(List<int> points, double scaledMin, double scaledMax)
        {
            var result = new List<int>(points.Count);

            var minValue = points.Min();
            var maxValue = points.Max();

            foreach (var point in points)
            {
                var newValue = (scaledMax - scaledMin) * (point - minValue) / (maxValue - minValue) + scaledMin;

                result.Add((int)newValue);
            }

            return result;
        }
    }
}