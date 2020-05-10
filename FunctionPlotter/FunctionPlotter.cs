using FunctionPlotter.Domain.Interfaces;
using FunctionPlotter.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Media.Imaging;

namespace FunctionPlotter
{
    public sealed class FunctionPlotter : IFunctionPlotter
    {
        private readonly IPainter _painter;
        private readonly IFunction _function;
        private readonly float _originOffset = 25;
        private readonly int _fontSize = 8;
        private readonly FunctionPlotterViewModel _model;

        public FunctionPlotter(IPainter painter, IFunction function, FunctionPlotterViewModel model)
        {
            _painter = painter;
            _function = function;
            _model = model;
        }

        public void DrawIntegralFunctionPlot(int width, int height)
        {
            var points =
                _function.GetFunctionGraph(_model.Min, _model.Max, _model.StepSize);

            var functionPointsY = points.Select(entry => entry.Y).ToList();
            var minY = functionPointsY.Min();
            var maxY = functionPointsY.Max();

            var integralPoints = _function.GetIntegralPoints(points);

            var upperLeftPoints = integralPoints.Select(entry => entry.Item1).ToList();
            var lowerRightPoints = integralPoints.Select(entry => entry.Item2).ToList();

            var iPointsX = upperLeftPoints.Select(entry => entry.X).ToList();
            iPointsX.AddRange(lowerRightPoints.Select(entry => entry.X));
            iPointsX = Converters.GetScaledValues(iPointsX, _originOffset, width);

            var iPointsY = upperLeftPoints.Select(entry => entry.Y).ToList();
            iPointsY.AddRange(lowerRightPoints.Select(entry => entry.Y));

            if (Math.Abs(minY - maxY) < 0.001)
            {
                iPointsY.Add(minY + (float)_model.MinY);
                iPointsY.Add(maxY + (float)_model.MaxY);
            }

            iPointsY = Converters.GetScaledValues(iPointsY, _originOffset, height);

            var convertedIntegralPoints = new List<PointF>(iPointsX.Count / 2);
            convertedIntegralPoints.AddRange(iPointsX.Select((t, i) => new PointF(t, iPointsY[i])));
            var rectanglePoints = convertedIntegralPoints.GetRange(0, convertedIntegralPoints.Count / 2).Zip(
                convertedIntegralPoints.GetRange(convertedIntegralPoints.Count / 2, convertedIntegralPoints.Count / 2),
                (u, l) => (u, l)).ToList();

            var pointsX = Converters.GetScaledValues(points.Select(entry => entry.X).ToList(), _originOffset,
                width);

            var pointsY = Converters.GetScaledValues(points.Select(entry => entry.Y).ToList(), _originOffset,
                height);

            var convertedPoints = new List<PointF>(pointsX.Count);
            convertedPoints.AddRange(pointsX.Select((t, i) => new PointF(t, pointsY[i])));

            _painter.DrawAxis((int)_originOffset);
            _painter.DrawFunction(convertedPoints);
            _painter.DrawIntegral(rectanglePoints);

            DrawScale(_model.Min, _model.Max, width, height, "x");
            if (Math.Abs(minY - maxY) < 0.001)
            {
                DrawScale(minY + (float)_model.MinY, maxY + (float)_model.MaxY, width, height, "y");
            }
            else
            {
                DrawScale(minY, maxY, width, height, "y");
            }

        }

        public void DrawFunctionPlot(int width, int height)
        {
            var points =
                _function.GetFunctionGraph(_model.Min, _model.Max, _model.StepSize);

            var pointsX = Converters.GetScaledValues(points.Select(entry => entry.X).ToList(), _originOffset,
                width);

            var functionPointsY = points.Select(entry => entry.Y).ToList();
            var minY = functionPointsY.Min();
            var maxY = functionPointsY.Max();

            if (Math.Abs(minY - maxY) < 0.001)
            {
                functionPointsY.Add(minY + (float)_model.MinY);
                functionPointsY.Add(maxY + (float)_model.MaxY);
            }

            var pointsY = Converters.GetScaledValues(functionPointsY, _originOffset,
                height);

            var convertedPoints = new List<PointF>(pointsX.Count);
            convertedPoints.AddRange(pointsX.Select((t, i) => new PointF(t, pointsY[i])));

            _painter.DrawAxis((int) _originOffset);
            _painter.DrawFunction(convertedPoints);

            DrawScale(_model.Min, _model.Max, width, height, "x");
            if (Math.Abs(minY - maxY) < 0.001)
            {
                DrawScale(minY + (float)_model.MinY, maxY + (float)_model.MaxY, width, height,
                    "y");
            }
            else
            {
                DrawScale(minY, maxY, width, height,
                    "y");
            }
        }

        private void DrawScale(double min, double max, int width, int height, string mode)
        {
            if (mode == "x")
            {
                _painter.ResetTransform();
                _painter.DrawString(new PointF(_originOffset, height - _originOffset + _fontSize),
                    Math.Round(min, 2).ToString(CultureInfo.InvariantCulture),
                    _fontSize);
                _painter.DrawString(new PointF(width - _originOffset + _fontSize, height - _originOffset + _fontSize),
                    Math.Round(max, 2).ToString(CultureInfo.InvariantCulture),
                    _fontSize);
            }
            else
            {
                _painter.ResetTransform();
                _painter.DrawString(new PointF(0, height - _fontSize - _originOffset),
                    Math.Round(min, 2).ToString(CultureInfo.InvariantCulture), _fontSize);
                _painter.DrawString(new PointF(0, 0), Math.Round(max, 2).ToString(CultureInfo.InvariantCulture),
                    _fontSize);
            }
        }

        public IPainter GetPainter()
        {
            return _painter;
        }

        public IFunction GetFunction()
        {
            return _function;
        }

        public BitmapImage GetFunctionImage()
        {
            return 
                Converters.BitmapToImageSource(_painter.GetBitmap());
        }
    }
}
