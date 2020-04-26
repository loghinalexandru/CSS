using System.Windows;

namespace FunctionPlotter
{
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        public double InputValue { get; set; }

        public InputBox()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public double ShowAndWaitForResult()
        {
            ShowDialog();

            return InputValue;
        }
    }
}