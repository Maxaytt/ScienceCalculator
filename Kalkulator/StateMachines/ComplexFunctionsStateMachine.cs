using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Kalkulator.StateMachines;

public enum State
{
    FirstMode,
    SecondMode
}

public class ComplexFunctionsStateMachine
{
    private State _currentState; 
    private readonly Button _squareOrCubeButton;
    private readonly Button _squareOrCubeRootButton;
    private readonly Button _powOrRootButton;
    private readonly Button _tenOrTwoPowButton;
    private readonly Button _log10LogBaseButton;
    private readonly Button _lnOrEPowButton;

    public ComplexFunctionsStateMachine(
        Button squareOrCubeButton, Button squareOrCubeRootButton, Button powOrRootButton, 
        Button tenOrTwoPowButton, Button log10LogBaseButton, Button lnOrEPowButton)
    {
        _squareOrCubeButton = squareOrCubeButton;
        _squareOrCubeRootButton = squareOrCubeRootButton;
        _powOrRootButton = powOrRootButton;
        _tenOrTwoPowButton = tenOrTwoPowButton;
        _log10LogBaseButton = log10LogBaseButton;
        _lnOrEPowButton = lnOrEPowButton; //natural log or e^x
        _currentState = State.FirstMode;
    }

    public void ChangeState()
    {
        if (_currentState == State.FirstMode)
        {
            _currentState = State.SecondMode;
            SecondModeState();
        }
        else if (_currentState == State.SecondMode)
        {
            _currentState = State.FirstMode;
            FirstModeState();
        }
    }

    private void FirstModeState()
    {
        _squareOrCubeButton.Name = "BtnSqr";
        _squareOrCubeButton.Content = BaseLineAlignmentContent("x", "2", BaselineAlignment.Top, 8);

        _squareOrCubeRootButton.Name = "BtnSqrt";
        _squareOrCubeRootButton.Content = RootContent("x", "2");

        _powOrRootButton.Name = "BtnPow";
        _powOrRootButton.Content = BaseLineAlignmentContent("x", "y", BaselineAlignment.Top, 9);
            
        _tenOrTwoPowButton.Name = "BtnPow10";
        _tenOrTwoPowButton.Content = BaseLineAlignmentContent("10", "x", BaselineAlignment.Top, 9);
            
        _log10LogBaseButton.Name = "BtnLog10";
        _log10LogBaseButton.Content = "log10";
            
        _lnOrEPowButton.Name = "BtnLn";
        _lnOrEPowButton.Content = "ln";
    }

    private void SecondModeState()
    {
        _squareOrCubeButton.Name = "BtnCube";
        _squareOrCubeButton.Content = BaseLineAlignmentContent("x", "3", BaselineAlignment.Top, 8);

        _squareOrCubeRootButton.Name = "BtnCuberoot";
        _squareOrCubeRootButton.Content = RootContent("x", "3");

        _powOrRootButton.Name = "BtnYroot";
        _powOrRootButton.Content = RootContent("x", "y");

        _tenOrTwoPowButton.Name = "BtnPow2";
        _tenOrTwoPowButton.Content = BaseLineAlignmentContent("2", "x", BaselineAlignment.Top, 9);

        _log10LogBaseButton.Name = "BtnLog";
        var txtBlock = BaseLineAlignmentContent("log", "y", BaselineAlignment.Bottom, 8);
        txtBlock.Inlines.Add(new Run("x"));
        _log10LogBaseButton.Content = txtBlock;

        _lnOrEPowButton.Name = "BtnPowE";
        _lnOrEPowButton.Content = BaseLineAlignmentContent("e", "x", BaselineAlignment.Top, 9);
    }
    
    private TextBlock BaseLineAlignmentContent(string text, string topText, BaselineAlignment baseLineAlignment, double topTextFontSize)
    {
        var textBlock = new TextBlock();

        var baseTextRun = new Run(text);
        var alignmentTextRun = new Run(topText)
        {
            BaselineAlignment = baseLineAlignment,
            FontSize = topTextFontSize
        };

        textBlock.Inlines.Add(baseTextRun);
        textBlock.Inlines.Add(alignmentTextRun);

        return textBlock;
    }

    private TextBlock RootContent(string baseText, string rootDegree)
    {
        var textBlock = new TextBlock();

        var rootDegreeRun = new Run(rootDegree)
        {
            BaselineAlignment = BaselineAlignment.TextTop,
            FontSize = 8.0
        };
        var rootRun = new Run("\u221A");
        var baseTextRun = new Run(baseText); 
        
        textBlock.Inlines.Add(rootDegreeRun);
        textBlock.Inlines.Add(rootRun);
        textBlock.Inlines.Add(baseTextRun);

        return textBlock;
    }
}