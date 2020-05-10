using System.Windows.Media.Imaging;
using FunctionPlotter.Domain.Interfaces;

namespace FunctionPlotter
{
    public interface IFunctionPlotter
    {
         void DrawIntegralFunctionPlot(int width, int height);
         void DrawFunctionPlot(int width, int height);
         IPainter GetPainter();
         IFunction GetFunction();
         BitmapImage GetFunctionImage();
    }
}