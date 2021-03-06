﻿using FunctionPlotter.Domain;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualBasic;

namespace FunctionPlotter
{
    public sealed class FunctionPlotterViewModel : INotifyPropertyChanged
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public double MinY => -5;
        public double MaxY => 5;

        public double StepSize { get; set; }
        public bool DrawIntegral { get; set; }

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

        public List<GraphObject> GetFunction()
        {
            return _compositeFunction;
        }

        public GraphObject GetLast()
        {
            return _compositeFunction.Count > 0 ? _compositeFunction.Last() : null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}