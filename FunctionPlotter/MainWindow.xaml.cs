using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using FunctionPlotter.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Point = System.Drawing.Point;

namespace FunctionPlotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Painter _painter;
        private Function _function;
        private readonly FunctionPlotterViewModel _functionPlotter;

        public MainWindow()
        {
            InitializeComponent();
            InitFunctionsComboBox();
            InitOperatorsComboBox();

            FunctionsComboBox.SelectionChanged += HandleSelectionChanged;
            FunctionsComboBox.DropDownOpened += HandleDropDownOpened;

            OperatorsComboBox.SelectionChanged += HandleSelectionChanged;
            OperatorsComboBox.DropDownOpened += HandleDropDownOpened;
            SizeChanged += Draw_OnClick;

            _functionPlotter = new FunctionPlotterViewModel();
        }

        private void InitFunctionsComboBox()
        {
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Sin));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Cos));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Tan));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Abs));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Sqrt));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Log10));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Exp));
        }

        private void InitOperatorsComboBox()
        {
            OperatorsComboBox.Items.Add(new OperatorObject("+"));
            OperatorsComboBox.Items.Add(new OperatorObject("-"));
            OperatorsComboBox.Items.Add(new OperatorObject("/"));
            OperatorsComboBox.Items.Add(new OperatorObject("*"));
            OperatorsComboBox.Items.Add(new OperatorObject("^"));
        }

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            _functionPlotter.AddComponent(((ComboBox) sender).SelectedItem as GraphObject);

            CompositeFunction.Text = _functionPlotter.GetCompositeFunction();
        }

        private void HandleDropDownOpened(object sender, EventArgs e)
        {
            ((ComboBox) sender).SelectedItem = null;
        }

        private void HandleRemoveButton(object sender, RoutedEventArgs e)
        {
            _functionPlotter.RemoveComponent();
            CompositeFunction.Text = _functionPlotter.GetCompositeFunction();
        }

        private void Draw(int width, int height)
        {
            _painter = new Painter(width, height);

            var points = _function.GetFunctionGraph(-5, 5, 0.01);

            var pointsX = Converters.GetScaledValues(points.Select(entry => entry.X).ToList(), 0,
                width);

            var pointsY = Converters.GetScaledValues(points.Select(entry => entry.Y).ToList(), 0,
                height);

            var convertedPoints = new List<PointF>(pointsX.Count);
            convertedPoints.AddRange(pointsX.Select((t, i) => new PointF(t, pointsY[i])));

            _painter.DrawFunction(convertedPoints);

            FunctionImage.Source = Converters.BitmapToImageSource(_painter.GetBitmap());
        }

        private void Draw_OnClick(object sender, RoutedEventArgs e)
        {
            _function = new Function(new List<GraphObject>()
            {
                new VariableObject(),
                new OperatorObject("^"),
                new ConstantObject(2),
                new OperatorObject("-"),
                new ConstantObject(500)
            });

            Draw((int) WindowGrid.ActualWidth, (int) WindowGrid.RowDefinitions[1].ActualHeight);
        }
    }
}