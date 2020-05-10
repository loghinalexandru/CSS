using System.Windows.Media.Imaging;

namespace FunctionPlotter.Domain.Interfaces
{
    public interface IFunctionPlotter
    {
        public void DrawIntegralFunctionPlot(int width, int height);
        public void DrawFunctionPlot(int width, int height);
        public IPainter GetPainter();
        public IFunction GetFunction();
        public BitmapImage GetFunctionImage();
    }
}