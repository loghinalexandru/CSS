﻿using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using FunctionPlotter.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FunctionPlotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FunctionPlotterViewModel PlotterViewModel { get; }

        private Painter _painter;
        private Function _function;
        private readonly float _originOffset = 25;
        private readonly int _fontSize = 8;

        public MainWindow()
        {
            InitializeComponent();
            InitFunctionsComboBox();
            InitOperatorsComboBox();
            InitParenthesesComboBox();
            InitVariableComboBox();
            SetGlobalExceptionHandling();

            FunctionsComboBox.SelectionChanged += HandleSelectionChanged;
            FunctionsComboBox.DropDownOpened += HandleDropDownOpened;

            OperatorsComboBox.SelectionChanged += HandleSelectionChanged;
            OperatorsComboBox.DropDownOpened += HandleDropDownOpened;

            ParenthesesComboBox.SelectionChanged += HandleSelectionChanged;
            ParenthesesComboBox.DropDownOpened += HandleDropDownOpened;

            VariableComboBox.SelectionChanged += HandleSelectionChangedOnVariable;
            VariableComboBox.DropDownOpened += HandleDropDownOpened;

            SizeChanged += Draw_OnClick;

            PlotterViewModel = new FunctionPlotterViewModel();
            DataContext = PlotterViewModel;
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

        private void InitParenthesesComboBox()
        {
            ParenthesesComboBox.Items.Add(new LeftParenthesesObject());
            ParenthesesComboBox.Items.Add(new RightParenthesesObject());
        }

        private void InitVariableComboBox()
        {
            VariableComboBox.Items.Add(new VariableObject());
            VariableComboBox.Items.Add(new ConstantObject(null));
        }

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            PlotterViewModel.AddComponent(((ComboBox) sender).SelectedItem as GraphObject);

            CompositeFunction.Text = PlotterViewModel.GetCompositeFunction();
        }

        private void HandleSelectionChangedOnVariable(object sender,
            SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (((ComboBox) sender).SelectedItem is ConstantObject)
            {
                var result = new InputBox().ShowAndWaitForResult();
                PlotterViewModel.AddComponent(new ConstantObject(result));
                CompositeFunction.Text = PlotterViewModel.GetCompositeFunction();

                return;
            }

            HandleSelectionChanged(sender, selectionChangedEventArgs);
        }

        private void HandleDropDownOpened(object sender, EventArgs e)
        {
            ((ComboBox) sender).SelectedItem = null;
        }

        private void HandleRemoveButton(object sender, RoutedEventArgs e)
        {
            PlotterViewModel.RemoveComponent();
            CompositeFunction.Text = PlotterViewModel.GetCompositeFunction();
        }

        private void Draw(int width, int height)
        {
            if (PlotterViewModel.DrawIntegral)
            {
                DrawIntegralFunctionPlot(width, height);
                return;
            }

            DrawFunctionPlot(width, height);
        }

        private void Draw_OnClick(object sender, RoutedEventArgs e)
        {
            if (PlotterViewModel.GetFunction().Count == 0)
            {
                return;
            }

            _function = new Function(PlotterViewModel.GetFunction());
            Draw((int) WindowGrid.ActualWidth, (int) WindowGrid.RowDefinitions[1].ActualHeight);
        }

        private void DrawFunctionPlot(int width, int height)
        {
            _painter = new Painter(width, height);

            var points =
                _function.GetFunctionGraph(PlotterViewModel.Min, PlotterViewModel.Max, PlotterViewModel.StepSize);

            var pointsX = Converters.GetScaledValues(points.Select(entry => entry.X).ToList(), _originOffset,
                width);

            var pointsY = Converters.GetScaledValues(points.Select(entry => entry.Y).ToList(), _originOffset,
                height);

            var convertedPoints = new List<PointF>(pointsX.Count);
            convertedPoints.AddRange(pointsX.Select((t, i) => new PointF(t, pointsY[i])));

            _painter.DrawAxis((int) _originOffset);
            _painter.DrawFunction(convertedPoints);

            DrawScale(PlotterViewModel.Min, PlotterViewModel.Max, width, height, "x");
            DrawScale(points.Select(point => point.Y).Min(), points.Select(point => point.Y).Max(), width, height, "y");

            FunctionImage.Source = Converters.BitmapToImageSource(_painter.GetBitmap());
        }

        private void DrawIntegralFunctionPlot(int width, int height)
        {
            _painter = new Painter(width, height);

            var points =
                _function.GetFunctionGraph(PlotterViewModel.Min, PlotterViewModel.Max, PlotterViewModel.StepSize);
            var integralPoints = _function.GetIntegralPoints(points);

            var upperLeftPoints = integralPoints.Select(entry => entry.Item1).ToList();
            var lowerRightPoints = integralPoints.Select(entry => entry.Item2).ToList();

            var iPointsX = upperLeftPoints.Select(entry => entry.X).ToList();
            iPointsX.AddRange(lowerRightPoints.Select(entry => entry.X));
            iPointsX = Converters.GetScaledValues(iPointsX, _originOffset, width);

            var iPointsY = upperLeftPoints.Select(entry => entry.Y).ToList();
            iPointsY.AddRange(lowerRightPoints.Select(entry => entry.Y));
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

            _painter.DrawAxis((int) _originOffset);
            _painter.DrawFunction(convertedPoints);
            _painter.DrawIntegral(rectanglePoints);

            DrawScale(PlotterViewModel.Min, PlotterViewModel.Max, width, height, "x");
            DrawScale(points.Select(point => point.Y).Min(), points.Select(point => point.Y).Max(), width, height, "y");

            FunctionImage.Source = Converters.BitmapToImageSource(_painter.GetBitmap());
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

        private void SetGlobalExceptionHandling()
        {
            Application.Current.DispatcherUnhandledException += (s, ex) =>
            {
                MessageBox.Show(ex.Exception.Message);
                ex.Handled = true;
            };
        }
    }
}