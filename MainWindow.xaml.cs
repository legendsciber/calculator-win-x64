using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace RichCalculatorWPF
{
    public partial class MainWindow : Window
    {
        private string currentInput = "0";
        private string operand1 = string.Empty;
        private string? currentOperation = null;
        private string previousExpressionText = string.Empty; // For the upper display
        private bool isNewEntry = true; 

        public MainWindow()
        {
            InitializeComponent();
            UpdateDisplay(); 
        }

        private void UpdateDisplay()
        {
            CurrentInputDisplay.Text = currentInput;
            PreviousExpressionDisplay.Text = previousExpressionText;
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string digit = button.Content.ToString();

            if (isNewEntry || currentInput == "0" && digit != ".")
            {
                currentInput = digit;
                isNewEntry = false; 
            }
            else
            {
                // Check for decimal point first
                if (digit == "." && currentInput.Contains("."))
                {
                    return; 
                }

                // Character limit check
                if (currentInput.Length >= 18)
                {
                    return; // Stop adding if limit reached
                }
                
                currentInput += digit;
            }
            UpdateDisplay();
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string newOperation = button.Content.ToString();

            if (!isNewEntry && !string.IsNullOrEmpty(currentOperation)) 
            {
                EqualsButton_Click(this, new RoutedEventArgs()); 
                operand1 = currentInput; 
                currentOperation = newOperation;
                previousExpressionText = $"{operand1} {currentOperation}";
                currentInput = "0"; 
                isNewEntry = true;
            }
            else 
            {
                operand1 = currentInput; 
                currentOperation = newOperation;
                previousExpressionText = $"{operand1} {currentOperation}";
                currentInput = "0"; 
                isNewEntry = true; 
            }
            UpdateDisplay();
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            string localOperand2;

            if (string.IsNullOrEmpty(currentOperation) || string.IsNullOrEmpty(operand1))
            {
                if (string.IsNullOrEmpty(currentOperation)) previousExpressionText = "";
                UpdateDisplay();
                return;
            }

            if (isNewEntry) 
            {
                if (currentInput == "0" && !string.IsNullOrEmpty(operand1)) 
                {
                    localOperand2 = operand1;
                }
                else 
                {
                     localOperand2 = currentInput;
                }
            }
            else
            {
                localOperand2 = currentInput; 
            }
            
            try
            {
                string expressionToEvaluate = $"{operand1}{currentOperation}{localOperand2}";
                DataTable dt = new DataTable();
                var result = dt.Compute(expressionToEvaluate, string.Empty);
                string resultString = result.ToString();

                previousExpressionText = $"{operand1} {currentOperation} {localOperand2} =";
                currentInput = resultString;    
                operand1 = resultString;        
                currentOperation = null; 
                isNewEntry = true;              
            }
            catch (Exception)
            {
                previousExpressionText = "Error"; 
                currentInput = "0"; 
                operand1 = string.Empty;
                currentOperation = null;
                isNewEntry = true;
            }
            UpdateDisplay();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            currentInput = "0";
            operand1 = string.Empty;
            currentOperation = null;
            previousExpressionText = string.Empty; 
            isNewEntry = true;
            UpdateDisplay();
        }
    }
}
