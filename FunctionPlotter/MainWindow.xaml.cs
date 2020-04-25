using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Series;

namespace FunctionPlotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FunctionPlotterViewModel _functionPlotter = new FunctionPlotterViewModel();
        public PlotModel Graph { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitFunctionsComboBox();
            InitOperatorsComboBox();

            FunctionsComboBox.SelectionChanged += HandleSelectionChanged;
            FunctionsComboBox.DropDownOpened += HandleDropDownOpened;

            OperatorsComboBox.SelectionChanged += HandleSelectionChanged;
            OperatorsComboBox.DropDownOpened += HandleDropDownOpened;
            ConstructSeries();

            GraphPlot.Model = Graph;
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

        public FunctionSeries ConstructSeries()
        {
            var tmp = new PlotModel { Title = "Simple example", Subtitle = "using OxyPlot" };
            var series = new FunctionSeries();

            series.Points.Add(new DataPoint(0.1, 0.1));
            series.Points.Add(new DataPoint(500, -1));

            tmp.Series.Add(series);

            Graph = tmp;
            Graph.InvalidatePlot(true);

            return series;
        }
    }
}