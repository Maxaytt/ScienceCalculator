using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kalkulator;

public partial class MainWindow 
{
    private void BtnSimpleOperation_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
            
        var operation = btn.Content?.ToString();
        if (!string.IsNullOrEmpty(operation))
        {
            PerformOperation(operation);
        }
    }
    
    private void PerformOperation(string operation)
    {
        if (_isResult) return;
        TextBlockSecondary.Text += TextBlockSecondary.Text.LastOrDefault() switch
        {
            ')' => operation,
            _ => TextBlockMain.Text[0] switch
            {
                '-' => string.Concat("(", TextBlockMain.Text, ")", operation),
                _ => string.Concat(TextBlockMain.Text, operation)
            }
        };

        var expression = FindUnpairedBracketIndex(TextBlockSecondary.Text) switch
        {
            -1 => TextBlockSecondary.Text[..^1],
            var index => TextBlockSecondary.Text[(index + 1)..^1]
        };
        var result = _dataTable.Compute(expression, null);
        TextBlockMain.Text = result.ToString();

        _isResult = true;
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
    
    private void BtnEqual_OnClick(object sender, RoutedEventArgs e)
    {
        if (_unpairedLBrackets != 0)
        {
            if(!TextBlockSecondary.Text.EndsWith(')'))
                TextBlockSecondary.Text += TextBlockMain.Text;
                
            TextBlockSecondary.Text += new string(')',_unpairedLBrackets) + '=';
            _unpairedLBrackets = 0;
            var expression = TextBlockSecondary.Text[..^1];
            var result = _dataTable.Compute(expression, null);
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
            var result = _dataTable.Compute(expression, null);
            TextBlockMain.Text = result.ToString();
            ListBoxHistory.Items.Add(TextBlockSecondary.Text + TextBlockMain.Text);
        }
    }
    
    private async void BtnOneBy_OnClick(object sender, RoutedEventArgs e)
    {
        if (TextBlockMain.Text == "0")
        {
            TextBlockMain.Text = "Cannot divide by zero!";
            await Task.Delay(2000);
            TextBlockMain.Text = "0";
            return;
        }
        if (TextBlockSecondary.Text.EndsWith(')'))
        {
            var index = LastLBracketIndex(TextBlockSecondary.Text);
            if (index == 0)
            {
                TextBlockSecondary.Text = "1/" + TextBlockSecondary.Text;
            }
            else
            {
                var part1 = TextBlockSecondary.Text[..(index-1)];
                var part2 = TextBlockSecondary.Text[index..];
                TextBlockSecondary.Text = part1 + "1/" + part2;
            }
        }
        else
        {
            TextBlockSecondary.Text += "1/(" + TextBlockMain.Text + ')';
        }

        var result = _dataTable.Compute(TextBlockSecondary.Text, null);
        TextBlockMain.Text = result.ToString();
        //1. 1/(x+x) - ends with )
        //2. 1/(9)
        //3. 1/(1/(x+x))
        //TODO: check this
        //TODO: generic function for 1 arg and 2 arg complex function
        //TODO: i think it cant build because of many partials 
    }
}