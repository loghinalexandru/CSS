using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using FunctionPlotter.Helpers;
using System;
using System.Collections.Generic;
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

            var points = new List<Point>()
            {
                new Point(0, 1),
                new Point(0, 2),
                new Point(0, 3),
                new Point(500, 500)
            };

            var pointsX = Converters.GetScaledValues(points.Select(entry => entry.X).ToList(), 0,
                width);

            var pointsY = Converters.GetScaledValues(points.Select(entry => entry.Y).ToList(), 0,
                height);

            var convertedPoints = new List<Point>(pointsX.Count);
            convertedPoints.AddRange(pointsX.Select((t, i) => new Point(t, pointsY[i])));

            _painter.DrawFunction(convertedPoints);

            FunctionImage.Source = Converters.BitmapToImageSource(_painter.GetBitmap());
        }

        private void Draw_OnClick(object sender, RoutedEventArgs e)
        {
            Draw((int) WindowGrid.ActualWidth, (int) WindowGrid.RowDefinitions[1].ActualHeight);
        }
    }
}