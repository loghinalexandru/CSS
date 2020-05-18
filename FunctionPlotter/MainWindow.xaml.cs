using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Interfaces;
using FunctionPlotter.Domain.Models;
using FunctionPlotter.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using EnsureArg;

namespace FunctionPlotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IFiniteStateAutomatonValidator _validator;
        public IFunctionPlotter _plotter;
        public FunctionPlotterViewModel PlotterViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitFunctionsComboBox();
            InitOperatorsComboBox();
            InitVariableComboBox();
            SetGlobalExceptionHandling();
            SetValidator();

            FunctionsComboBox.SelectionChanged += HandleSelectionChanged;
            FunctionsComboBox.DropDownOpened += HandleDropDownOpened;

            OperatorsComboBox.SelectionChanged += HandleSelectionChanged;
            OperatorsComboBox.DropDownOpened += HandleDropDownOpened;

            VariableComboBox.SelectionChanged += HandleSelectionChangedOnVariable;
            VariableComboBox.DropDownOpened += HandleDropDownOpened;

            SizeChanged += Draw_OnClick;

            PlotterViewModel = new FunctionPlotterViewModel();
            DataContext = PlotterViewModel;
        }

        public void InitFunctionsComboBox()
        {
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Sin));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Cos));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Tan));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Abs));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Sqrt));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Log10));
            FunctionsComboBox.Items.Add(new FunctionObject(Math.Exp));
        }

        public void InitOperatorsComboBox()
        {
            OperatorsComboBox.Items.Add(new OperatorObject("+"));
            OperatorsComboBox.Items.Add(new OperatorObject("-"));
            OperatorsComboBox.Items.Add(new OperatorObject("/"));
            OperatorsComboBox.Items.Add(new OperatorObject("*"));
            OperatorsComboBox.Items.Add(new OperatorObject("^"));
        }

        public void InitVariableComboBox()
        {
            VariableComboBox.Items.Add(new VariableObject());
            VariableComboBox.Items.Add(new ConstantObject(null));
        }

        public void HandleSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var selectedItem = ((ComboBox) sender).SelectedItem as GraphObject;

            if (selectedItem == null)
            {
                return;
            }

            PlotterViewModel.AddComponent(selectedItem);
            _validator.DoTransition(selectedItem);

            CompositeFunction.Text = PlotterViewModel.GetCompositeFunction();
        }

        public void HandleSelectionChangedOnVariable(object sender,
            SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (((ComboBox) sender).SelectedItem is ConstantObject)
            {
                var result = new InputBox().ShowAndWaitForResult();
                var item = new ConstantObject(result);

                PlotterViewModel.AddComponent(item);

                _validator.DoTransition(item);

                CompositeFunction.Text = PlotterViewModel.GetCompositeFunction();

                return;
            }

            HandleSelectionChanged(sender, selectionChangedEventArgs);
        }

        public void HandleDropDownOpened(object sender, EventArgs e)
        {
            ((ComboBox) sender).SelectedItem = null;
        }

        private void HandleRemoveButton(object sender, RoutedEventArgs e)
        {
            PlotterViewModel.RemoveComponent();
            _validator.DoTransition(PlotterViewModel.GetLast());
            CompositeFunction.Text = PlotterViewModel.GetCompositeFunction();
        }

        public void Draw(int width, int height)
        {
            Ensure.Arg(width)
                .IsGreaterThan(0);

            Ensure.Arg(height)
                .IsGreaterThan(0);

            if (PlotterViewModel.DrawIntegral)
            {
                _plotter.DrawIntegralFunctionPlot(width, height);
            }
            else
            {
                _plotter.DrawFunctionPlot(width, height);
            }

            FunctionImage.Source = _plotter.GetFunctionImage();

            Ensure.Arg(FunctionImage.Source).IsNotNull();
        }

        public void Draw_OnClick(object sender, RoutedEventArgs e)
        {
            if (PlotterViewModel.GetFunction().Count == 0)
            {
                return;
            }

            _plotter = new FunctionPlotter(new Painter((int)WindowGrid.ActualWidth, (int)WindowGrid.RowDefinitions[1].ActualHeight), new Function(PlotterViewModel.GetFunction()),
                PlotterViewModel);

            Draw((int) WindowGrid.ActualWidth, (int) WindowGrid.RowDefinitions[1].ActualHeight);
        }

        public void Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_plotter?.GetFunction() == null)
                {
                    MessageBox.Show("There is no data available to export.", "Warning", MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                SaveFileDialog exportDialog = new SaveFileDialog();
                exportDialog.Filter =
                    "csv files (*.csv)|*.csv|jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|All files (*.*)|*.*";

                if (exportDialog.ShowDialog() == true)
                {
                    if (exportDialog.FileName.EndsWith(".csv"))
                    {
                        var points = _plotter.GetFunction().GetFunctionGraph(PlotterViewModel.Min, PlotterViewModel.Max,
                            PlotterViewModel.StepSize);
                        Exporter.ExportAsCsv(exportDialog.FileName, points);
                        MessageBox.Show("Data has been sucessfully exported to CSV file", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        _plotter.GetPainter().SaveImage(exportDialog.FileName);
                        MessageBox.Show("Function Graph has been sucessfully exported to image file", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                // Info.  
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(ex);
            }
        }

        public void SetGlobalExceptionHandling()
        {
            try
            {
                Application.Current.DispatcherUnhandledException += (s, ex) =>
                {
                    MessageBox.Show(ex.Exception.Message);
                    ex.Handled = true;
                };
            }
            catch
            {
                // ignored
            }
        }

        public void SetValidator()
        {
            _validator = new FiniteStateAutomatonValidator(new List<Control>()
            {
                LeftParentheses,
                RightParentheses,
                VariableComboBox,
                OperatorsComboBox,
                FunctionsComboBox
            });
        }

        public void RightParentheses_OnClick(object sender, RoutedEventArgs e)
        {
            var item = new RightParenthesesObject();

            PlotterViewModel.AddComponent(item);
            _validator.DoTransition(item);
            CompositeFunction.Text = PlotterViewModel.GetCompositeFunction();
        }

        public void LeftParentheses_OnClick(object sender, RoutedEventArgs e)
        {
            var item = new LeftParenthesesObject();

            PlotterViewModel.AddComponent(item);
            _validator.DoTransition(item);
            CompositeFunction.Text = PlotterViewModel.GetCompositeFunction();
        }
    }
}