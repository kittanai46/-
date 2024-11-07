using System;
using System.Windows;

namespace BmiCalculatorApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateBMI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double height = Convert.ToDouble(HeightInput.Text);
                double weight = Convert.ToDouble(WeightInput.Text);

                if (height <= 0 || weight <= 0)
                {
                    ResultText.Text = "Height and Weight must be positive numbers";
                    return;
                }

                double bmi = weight / (height * height);
                ResultText.Text = $"Your BMI is {bmi:F2}";

                if (bmi < 18.5)
                {
                    ResultText.Text += " (Underweight)";
                }
                else if (bmi >= 18.5 && bmi < 24.9)
                {
                    ResultText.Text += " (Normal weight)";
                }
                else if (bmi >= 25 && bmi < 29.9)
                {
                    ResultText.Text += " (Overweight)";
                }
                else
                {
                    ResultText.Text += " (Obesity)";
                }
            }
            catch (FormatException)
            {
                ResultText.Text = "Please enter valid numbers for height and weight";
            }
        }
    }
}
