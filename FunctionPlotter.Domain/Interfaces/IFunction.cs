using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FunctionPlotter.Domain.Interfaces
{
    public interface IFunction
    {
        public List<PointF> GetFunctionGraph(double low, double high, double stepSize);
        public List<(PointF, PointF)> GetIntegralPoints(List<PointF> functionPoints);
    }
}
