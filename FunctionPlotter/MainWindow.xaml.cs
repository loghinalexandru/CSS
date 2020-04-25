using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using FunctionPlotter.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public MainWindow()
        {
            InitializeComponent();
            InitFunctionsComboBox();
            InitOperatorsComboBox();
            InitParenthesesComboBox();
            InitVariableComboBox();


            FunctionsComboBox.SelectionChanged += HandleSelectionChanged;
            FunctionsComboBox.DropDownOpened += HandleDropDownOpened;

            OperatorsComboBox.SelectionChanged += HandleSelectionChanged;
            OperatorsComboBox.DropDownOpened += HandleDropDownOpened;

            ParenthesesComboBox.SelectionChanged += HandleSelectionChanged;
            ParenthesesComboBox.DropDownOpened += HandleDropDownOpened;

            VariableComboBox.SelectionChanged += HandleSelectionChangedOnVariable;
            VariableComboBox.DropDownOpened += HandleDropDownOpened;

//            SizeChanged += Draw_OnClick; SOLVE ERROR ON INITIAL STARTUP

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
                MessageBox.Show("Hello, world!", "My App");
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
            _painter = new Painter(width, height);

            var points =
                _function.GetFunctionGraph(PlotterViewModel.Min, PlotterViewModel.Max, PlotterViewModel.StepSize);

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
            _function = new Function(PlotterViewModel.GetFunction());
            Draw((int) WindowGrid.ActualWidth, (int) WindowGrid.RowDefinitions[1].ActualHeight);
        }
    }
}