using System;
using FunctionPlotter.Domain;
using FunctionPlotter.Helpers;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FunctionPlotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Painter _painter;
        private readonly ExpressionParser _parser;
        private List<GraphObject> _function;

        public MainWindow()
        {
            InitializeComponent();
            _painter = new Painter();
            _parser = new ExpressionParser();
        }

        private void InputEntered(object sender, TextChangedEventArgs e)
        {
            _function.Add(new GraphObject()
            {
                GraphObjectType = GraphObjectType.Function,
                Value = FunctionType.Sin
            });
        }
    }
}