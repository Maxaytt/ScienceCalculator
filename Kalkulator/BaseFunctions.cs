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
        switch (TextBlockSecondary.Text.LastOrDefault())
        {
            case ')':
                TextBlockSecondary.Text += operation;
                break;
            case ',':
                TextBlockSecondary.Text += string.Concat(TextBlockMain.Text, ")", operation);
                _unpairedLBrackets++;
                break;
            default:
                TextBlockSecondary.Text += TextBlockMain.Text[0] switch
                {
                    '-' => string.Concat("(", TextBlockMain.Text, ")", operation),
                    _ => string.Concat(TextBlockMain.Text, operation)
                };
                break;
        }

        var expression = FindUnpairedBracketIndex(TextBlockSecondary.Text) switch
        {
            -1 => TextBlockSecondary.Text[..^1],
            var index => TextBlockSecondary.Text[(index + 1)..^1]
        };
        CalculateAndDisplay(expression);
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
                
            AddMissingBrackets();
            TextBlockSecondary.Text += '=';
            
            var expression = TextBlockSecondary.Text[..^1];
            CalculateAndDisplay(expression);
            //TODO: potential exception 
            ListBoxHistory.Items.Add(TextBlockSecondary.Text + TextBlockMain.Text);
            TextBlockSecondary.Text = TextBlockMain.Text;
        }
        else
        {
            TextBlockSecondary.Text += TextBlockSecondary.Text.EndsWith(')') switch
            {
                true => "=",
                false => string.Concat(TextBlockMain.Text, "=")
            };
            var expression = TextBlockSecondary.Text[..^1];
            CalculateAndDisplay(expression);
            ListBoxHistory.Items.Add(TextBlockSecondary.Text + TextBlockMain.Text);
            TextBlockSecondary.Text = TextBlockMain.Text;
        }
    }

    private void BtnOneBy_OnClick(object sender, RoutedEventArgs e)
    {
        if (TextBlockMain.Text == "0")
        {
            PrintOnMainTextBlock("Cannot divide by zero");
            return;
        } 

        if (TryFindLastBracketPairIndex(TextBlockSecondary.Text, out var pairIndex)) 
        {
            ProcessBracketPairIndex(pairIndex);
        }
        else
        {
            AppendFractionToEnd();
        }

        CalculateAndDisplay();
    }
    
    private void ProcessBracketPairIndex(int pairIndex)
    {
        if (TryFindFunctionBeforeIndex(TextBlockSecondary.Text, pairIndex, out var index))
        {
            TextBlockSecondary.Text = index switch
            {
                0 => string.Concat("1/(", TextBlockSecondary.Text, ")"),
                _ => string.Concat(
                    TextBlockSecondary.Text[..(index - 1)],
                    "1/(", TextBlockSecondary.Text[index..], ")")
            };
        }
        else if(pairIndex != 0 && 
                (TextBlockSecondary.Text[pairIndex-1] == '/' 
                 || TextBlockSecondary.Text[pairIndex-1] == '*' ))
        {
            TextBlockSecondary.Text = string.Concat(
                TextBlockSecondary.Text[..pairIndex],
                "(1/", TextBlockSecondary.Text[pairIndex..], ")");
        }
        else
        {
            TextBlockSecondary.Text = pairIndex switch
            {
                0 => string.Concat("1/", TextBlockSecondary.Text),
                _ => string.Concat(
                    TextBlockSecondary.Text[..(pairIndex)],
                    "1/(", TextBlockSecondary.Text[pairIndex..], ")")
            };
        }
    }

    private void AppendFractionToEnd()
    {
        TextBlockSecondary.Text = string.Concat(TextBlockSecondary.Text, "1/(", TextBlockMain.Text, ")");
    }
    private static bool TryFindLastBracketPairIndex(string input, out int index)
    {
        index = -1;
        var openBracketCount = 0;

        for (var i = input.Length - 1; i >= 0; i--)
        {
            if (input[i] == ')')
            {
                openBracketCount++;
            }
            else if (input[i] == '(')
            {
                openBracketCount--;
                if (openBracketCount != 0) continue;
                
                index = i;
                return true;
            }
        }

        return false;
    }
    
    private static bool TryFindFunctionBeforeIndex(string input, int endIndex, out int startIndex)
    {
        startIndex = -1;

        for (var i = endIndex - 1; i >= 0; i--)
        {
            if (!char.IsLetter(input[i]))
            {
                break;
            }

            if (i == 0 || !char.IsLetter(input[i - 1]))
            {
                startIndex = i;
                return true;
            }
        }

        return false;
    }

    private void AddMissingBrackets()
    {
        TextBlockSecondary.Text += new string(')', _unpairedLBrackets);
        _unpairedLBrackets = 0;
    }

    
    private async void PrintOnMainTextBlock(string text)
    {
        TextBlockMain.Text = text;
        await Task.Delay(2000);
        TextBlockMain.Text = "0";
    }
    
    private async void PrintOnSecondaryTextBlock(string text)
    {
        TextBlockSecondary.Text = text;
        await Task.Delay(3000);
        TextBlockSecondary.Text = "";
    }
}