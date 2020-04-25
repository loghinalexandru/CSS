using System;
using FunctionPlotter.Domain;
using FunctionPlotter.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FunctionPlotter.Domain.Models;

namespace FunctionPlotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Painter _painter;
        private readonly ExpressionParser _parser;
        private readonly List<GraphObject> _compositeFunction;

        public MainWindow()
        {
            InitializeComponent();
            _painter = new Painter();
            _parser = new ExpressionParser();
            _compositeFunction = new List<GraphObject>();
        }

        private void InputEntered(object sender, TextChangedEventArgs e)
        {
            _compositeFunction.Add(new FunctionObject(Math.Sin));
            _compositeFunction.Add(new OperatorObject("("));
            _compositeFunction.Add(new FunctionObject(Math.Cos));
            _compositeFunction.Add(new VariableObject());
            _compositeFunction.Add(new OperatorObject(")"));

            var ceva = string.Join(" ", _compositeFunction);
        }
    }
}