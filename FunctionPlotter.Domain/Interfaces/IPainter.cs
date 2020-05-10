using System.Collections.Generic;
using System.Drawing;

namespace FunctionPlotter.Domain.Interfaces
{
    public interface IPainter
    {
        public void DrawAxis(int originOffset);
        public void DrawFunction(List<PointF> points);
        public void DrawIntegral(List<(PointF, PointF)> points);
        public void SaveImage(string filePath);
        public void DrawString(PointF point, string input, int size);
        public void ResetTransform();
        public Bitmap GetBitmap();
    }
}