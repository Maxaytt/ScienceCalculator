using System.Windows;
using System.Windows.Controls;

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
}