using FunctionPlotter.Domain;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;

namespace FunctionPlotter
{
    public sealed class FunctionPlotterViewModel
    {
        private readonly List<GraphObject> _compositeFunction;


        public FunctionPlotterViewModel()
        {
            _compositeFunction = new List<GraphObject>();
        }

        public void AddComponent(GraphObject functionItem)
        {
            if (functionItem != null)
            {
                _compositeFunction.Add(functionItem);
            }
        }

        public void RemoveComponent()
        {
            if (_compositeFunction.Count > 0)
            {
                _compositeFunction.RemoveAt(_compositeFunction.Count - 1);
            }
        }

        public string GetCompositeFunction()
        {
            return string.Join("", _compositeFunction);
        }
    }
}