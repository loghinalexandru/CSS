using System.Windows;
using System.Windows.Controls;
using FunctionPlotter.Helpers;

namespace FunctionDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Painter _painter;
        private readonly ExpressionParser _parser;

        public MainWindow()
        {
            InitializeComponent();
            _painter = new Painter();
            _parser = new ExpressionParser();
        }

        private void InputEntered(object sender, TextChangedEventArgs e)
        {
            var function = _parser.Parse(Input.Text);

            _painter.DrawFunction(function, -100,100,0.001);

            FunctionImage.Source = Converters.BitmapToImageSource(_painter.GetBitmap());
        }
    }
}