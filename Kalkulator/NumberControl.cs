using System.Windows;
using System.Windows.Controls;
using Kalkulator.StateMachines;

namespace Kalkulator;

public partial class MainWindow
{
    private void BtnNumber_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
            
        var number = btn.Content?.ToString();
        if (!string.IsNullOrEmpty(number))
        {
            PerformNumber(number);
        }
    }
    
    private void BtnNeg_OnClick(object sender, RoutedEventArgs e)
    {
        if(_isResult) return;

        TextBlockMain.Text = (-int.Parse(TextBlockMain.Text)).ToString();
    }
    
    private void BtnZero_OnClick(object sender, RoutedEventArgs e)
    {
        if (TextBlockMain.Text != "0" && TextBlockMain.Text.Length < MaxLength && !_isResult)
        {
            TextBlockMain.Text += "0";
        }
        else if (_isResult)
        {
            TextBlockMain.Text = "0";
        }
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
    
    private void BtnEuler_OnClick(object sender, RoutedEventArgs e)
    {
        TextBlockMain.Text = "2.718281828459045235360287";
        _isResult = false;
    }

    private void BtnPi_OnClick(object sender, RoutedEventArgs e)
    {
        TextBlockMain.Text = "3.141592653589793238462643";
        _isResult = false;
    }
    
    private void PerformNumber(string number)
    {
        if (!_isResult)
        {
            if (TextBlockMain.Text == "0")
                TextBlockMain.Text = number;
            else if (TextBlockMain.Text.Length < MaxLength)
                TextBlockMain.Text += number;
        }
        else
        {
            TextBlockMain.Text = number;
            _isResult = false;
        }
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

    private void BtnLBracket_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(TextBlockSecondary.Text))
        {
            TextBlockSecondary.Text += '(';
        }
        else
        {
            var lastChar = TextBlockSecondary.Text[^1];
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


        TextBlockSecondary.Text += TextBlockSecondary.Text[^1] switch
        {
            ')' => ")",
            _ => string.Concat(TextBlockMain.Text, ")")
        };
        _unpairedLBrackets--;
        
        var expression = AddMissingBrackets(TextBlockSecondary.Text);
        if (expression is not null)
        {
            var result = _interpreter.Calculate(expression);
            TextBlockMain.Text = result;
            _isResult = true;
        }
    }
    
    private void BtnDot_OnClick(object sender, RoutedEventArgs e)
    {
        var lastChar = TextBlockMain.Text[^1];
        if (char.IsDigit(lastChar) && !TextBlockMain.Text.Contains('.'))
        {
            TextBlockMain.Text += '.';
        }
    }

    private void BtnSecondTrigonometry_OnClick(object sender, RoutedEventArgs e)
    {
        TrigonometryStateMachineInstance.ChangeState(TrigonometryState.SecondMode);
    }

    private void BtnHypMode_OnClick(object sender, RoutedEventArgs e)
    {
        TrigonometryStateMachineInstance.ChangeState(TrigonometryState.HypoMode);
    }
} 