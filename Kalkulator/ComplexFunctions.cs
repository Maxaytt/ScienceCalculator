using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Kalkulator.StateMachines;

namespace Kalkulator;

public partial class MainWindow 
{
    private void BtnComplexFunction_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) throw new ArgumentException("Sender is not a button", nameof(sender));
        
        var functionName = btn.Name[3..];

        try
        {
            PerformComplexFunction(functionName);
        }
        catch (Exception)
        {
            PrintOnMainTextBlock("Invalid input");
            PrintOnSecondaryTextBlock("");
        }
    }

    private void BtnSecond_OnClick(object sender, RoutedEventArgs e)
    {
        _stateMachine.ChangeState();
    }
    
    private void PerformComplexFunction(string functionName)
    {
        var argsCount = GetArgsCount(functionName);
        if (argsCount == 1)
        {
            AddOneArgumentFunction(functionName);
        }
        else if (argsCount == 2)
        {
            AddTwoArgumentFunction(functionName);
        }
        else
        {
            throw new NotImplementedException("Theres no function with that amount of args");
        }
    }

    private int GetArgsCount(string functionName)
    { 
        return _interpreter.FunctionsAndArgCount[functionName];
    }

    private void AddTwoArgumentFunction(string functionName)
    {
        var sec = TextBlockSecondary.Text;
        var length = TextBlockSecondary.Text.Length;

        if (TryHandleNoContext(functionName, 2)) return;
        
        var lastChar = TextBlockSecondary.Text[^1];
        var index = lastChar == ')' ? FindIndexOfBracketPair(sec, length - 1) : 0;

        TextBlockSecondary.Text = lastChar switch
        {
            ')' => string.Concat(sec[..index], functionName, "(", sec[index..], ","),
            '(' => string.Concat(sec, functionName, "(", TextBlockMain.Text, ","),
            _ when IsOperator(lastChar) => string.Concat(sec, functionName, "(", TextBlockMain.Text, ","),
            _ => throw new NotImplementedException("This situation is not implemented")
        };
        _unpairedLBrackets++;
        _isResult = true;
    }
    
    private void AddOneArgumentFunction(string functionName)
    {
        var sec = TextBlockSecondary.Text;
        var length = TextBlockSecondary.Text.Length;

        if (TryHandleNoContext(functionName, 1)) return;
        
        var lastChar = TextBlockSecondary.Text[^1];
        var index = lastChar == ')' ? FindIndexOfBracketPair(sec, length - 1) : 0;

        TextBlockSecondary.Text = lastChar switch
        {
            ')' => string.Concat(sec[..index], functionName, "(", sec[index..], ")"),
            '(' => string.Concat(sec, functionName, "(", TextBlockMain.Text, ")"),
            _ when IsOperator(lastChar) => string.Concat(sec, functionName, "(", TextBlockMain.Text, ")"),
            _ => throw new NotImplementedException("This situation is not implemented")
        };
        _isResult = true;
        
        CalculateAndDisplay();
    }

    private void BtnRand_OnClick(object sender, RoutedEventArgs e)
    {
        var randomNumber = _rand.NextDouble() + _rand.Next(0,10001);
        TextBlockMain.Text = randomNumber.ToString(CultureInfo.CurrentCulture);
    }
}