using System.Collections;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Kalkulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static bool _isResult;
        private static int _unpairedLBrackets;

        private void BtnNumber_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            
            var number = btn.Content?.ToString();
            if (!string.IsNullOrEmpty(number))
            {
                PerformNumber(number);
            }
        }

        private void BtnSimpleOperation_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            
            var operation = btn.Content?.ToString();
            if (!string.IsNullOrEmpty(operation))
            {
                PerformOperation(operation);
            }
        }

        private void BtnZero_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextBlockMain.Text != "0" && TextBlockMain.Text.Length < 26 && !_isResult)
                TextBlockMain.Text += "0";
            else if (_isResult)
                TextBlockMain.Text = "0";
        }

        private void BtnClear_OnClick(object sender, RoutedEventArgs e)
        {
            _isResult = false;
            if (TextBlockMain.Text == "0")
            {
                TextBlockSecondary.Text = string.Empty;
                return;
            }
            TextBlockMain.Text = "0";
        }

        private void BtnPi_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockMain.Text = "3.141592653589793238462643";
            _isResult = false;
        }

        private void BtnEuler_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockMain.Text = "2.718281828459045235360287";
            _isResult = false;
        }

        private void BtnDel_OnClick(object sender, RoutedEventArgs e)
        {
            var len = TextBlockMain.Text.Length;
            if (_isResult) return;

            TextBlockMain.Text = len switch
            {
                > 1 => TextBlockMain.Text[..^1],
                1 => "0",
                _ => TextBlockMain.Text
            };
        }
        
        private void BtnNeg_OnClick(object sender, RoutedEventArgs e)
        {
            if(_isResult) return;

            TextBlockMain.Text = (-int.Parse(TextBlockMain.Text)).ToString();
        }
        
        private void PerformOperation(string operation)
        {
            if (_isResult) return;
            
            if (TextBlockSecondary.Text.Length > 0 && TextBlockSecondary.Text[^1] == ')')
            {
                TextBlockSecondary.Text += operation;
            }
            else
            {
                if (TextBlockMain.Text[0] == '-')
                {
                    TextBlockSecondary.Text += '(' + TextBlockMain.Text + ')' + operation;
                }
                else
                {
                    TextBlockSecondary.Text += TextBlockMain.Text + operation; 
                }
            }

            var expression = FindUnpairedBracketIndex(TextBlockSecondary.Text) switch
            {
                -1 => TextBlockSecondary.Text[..^1],
                var index => TextBlockSecondary.Text[(index + 1)..^1]
            };
            var result = new DataTable().Compute(expression, null);
            TextBlockMain.Text = result.ToString();

            _isResult = true;
        }

        private void PerformNumber(string number)
        {
            if (!_isResult)
            {
                if (TextBlockMain.Text == "0")
                    TextBlockMain.Text = number;
                else if (TextBlockMain.Text.Length < 26)
                    TextBlockMain.Text += number;
            }
            else
            {
                TextBlockMain.Text = number;
                _isResult = false;
            }
        }

        private int FindUnpairedBracketIndex(string expression)
        {
            var stack = new Stack();
            var ind = 0;
            foreach (var c in expression)
            {
                if (c == '(')
                {
                    stack.Push(ind);
                }
                else if(c == ')')
                {
                    stack.Pop();
                }
                ind++;
            }
            var result = stack.Count > 0 ? (int)(stack.Pop() ?? 0) : -1;
            return result;
        }
        private void BtnLBracket_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBlockSecondary.Text))
            {
                TextBlockSecondary.Text += '(';
            }
            else 
            {
                var secondaryLen = TextBlockSecondary.Text.Length;
                var lastChar = TextBlockSecondary.Text[secondaryLen - 1];
                switch (lastChar)
                {
                    case '(':
                        TextBlockSecondary.Text += '(';
                        break;
                    case ')':
                        TextBlockSecondary.Text += "*(";
                        break;
                    default:
                        if (_isResult)
                        {
                            TextBlockSecondary.Text += "(";
                            TextBlockMain.Text = "0";
                        }
                        else
                        {
                            TextBlockSecondary.Text += TextBlockMain.Text + "*(";
                        }
                        break;
                }
            }
            _unpairedLBrackets++;
        }
        
        private void BtnRBracket_OnClick(object sender, RoutedEventArgs e)
        {
            if (_unpairedLBrackets == 0) return;
            
            TextBlockSecondary.Text += TextBlockMain.Text + ')';
            _unpairedLBrackets--;
        }
        
        private void BtnEqual_OnClick(object sender, RoutedEventArgs e)
        {
            if (_unpairedLBrackets != 0)
            {
                TextBlockSecondary.Text += new string(')',_unpairedLBrackets) + '=';
                var expression = TextBlockSecondary.Text[..^1];
                var result = new DataTable().Compute(expression, null);
                TextBlockMain.Text = result.ToString();
                ListBoxHistory.Items.Add(TextBlockSecondary.Text + TextBlockMain.Text);
            }
            else
            {
                if(!TextBlockSecondary.Text.EndsWith(')'))
                    TextBlockSecondary.Text += TextBlockMain.Text + '=';
                else
                    TextBlockSecondary.Text += '=';
                var expression = TextBlockSecondary.Text[..^1];
                var result = new DataTable().Compute(expression, null);
                TextBlockMain.Text = result.ToString();
                ListBoxHistory.Items.Add(TextBlockSecondary.Text + TextBlockMain.Text);
            }
        }
    }
    //TODO: function compiler for complex functions
    //TODO: equation tests
}