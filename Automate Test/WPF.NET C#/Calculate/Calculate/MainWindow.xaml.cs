using System;
using System.Windows;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        private double _firstNumber = 0;
        private double _secondNumber = 0;
        private string _operation = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            Display.Text += button.Content.ToString();
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            _operation = button.Content.ToString();
            _firstNumber = Convert.ToDouble(Display.Text);
            Display.Text = "";
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            _secondNumber = Convert.ToDouble(Display.Text);
            double result = 0;

            switch (_operation)
            {
                case "+":
                    result = _firstNumber + _secondNumber;
                    break;
                case "-":
                    result = _firstNumber - _secondNumber;
                    break;
                case "*":
                    result = _firstNumber * _secondNumber;
                    break;
                case "/":
                    result = _firstNumber / _secondNumber;
                    break;
            }

            Display.Text = result.ToString();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "";
            _firstNumber = 0;
            _secondNumber = 0;
            _operation = "";
        }
    }
}
