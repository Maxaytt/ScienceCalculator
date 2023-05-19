using System;
using System.Linq;
using Kalkulator.Interpreter;
using Kalkulator.StateMachines;

namespace Kalkulator
{
    /// <summary>
    ///  helpers and fields of MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static bool _isResult;
        private static int _unpairedLBrackets;
        private readonly FunctionInterpreter _interpreter = new();
        private readonly ComplexFunctionsStateMachine _stateMachine;
        private readonly Lazy<TrigonometryStateMachine> _trigonometryStateMachineLazy;
        private TrigonometryStateMachine TrigonometryStateMachineInstance => _trigonometryStateMachineLazy.Value;
        private readonly Random _rand = new((int)DateTime.Now.Ticks);
        private const int MaxLength = 100;
        public MainWindow()
        {
            InitializeComponent();
            _stateMachine = new ComplexFunctionsStateMachine(
            BtnSqr,
            BtnSqrt,
            BtnPow,
            BtnPow10,
            BtnLog10,
            BtnLn);
            _trigonometryStateMachineLazy = new Lazy<TrigonometryStateMachine>(() => new TrigonometryStateMachine(
                BtnSin,
                BtnSec,
                BtnCos,
                BtnCsc,
                BtnTan,
                BtnCot));
        }

        
        private static string? AddMissingBrackets(string expression)
        {
            var lBrackets = expression.Count(c => c == '(');
            var rBrackets = expression.Count(c => c == ')');
            if (lBrackets >= rBrackets)
            {
                var missingRightBrackets = lBrackets - rBrackets;
                expression += new string(')', missingRightBrackets);
                return expression;
            }
            
            return null;
        }
        
        private static int FindIndexOfBracketPair(string str, int indexOfElement)
        {
            var element = str[indexOfElement];
            if (element != ')') throw new Exception("Element isn't a right bracket");
            var rBracketCount = 0;
            for (var i = indexOfElement - 1; i >= 0; i--)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (str[i] == ')')
                {
                    rBracketCount++;
                }
                else if (str[i] == '(' && rBracketCount == 0)
                {
                    return i;
                }
                else if (str[i] == '(')
                {
                    rBracketCount--;
                }
            }

            throw new Exception("Wrong amount of brackets");
        }
        
        private static bool IsOperator(char c)
        {
            var ops = new[] { '+', '-', '*', '/' };
            return ops.Contains(c);
        }
        
        private static int LastLBracketIndex(string expression)
        {
            var i = 0;
            var resultIndex = 0;
            foreach (var c in expression)
            {
                if (c == '(')
                {
                    resultIndex = i;
                }

                i++;
            }

            return resultIndex;
        }
        
        private bool TryHandleNoContext(string function, int argumentsCount)
        {
            if (TextBlockSecondary.Text.Length != 0) return false;

            if (argumentsCount == 1)
            {
                TextBlockSecondary.Text = string.Concat(function, "(", TextBlockMain.Text, ")");
                CalculateAndDisplay();
            }
            else if(argumentsCount == 2)
            {
                TextBlockSecondary.Text = string.Concat(function, "(", TextBlockMain.Text, ",");
                _unpairedLBrackets++;
            }
            _isResult = true;
            
            return true;
        }

        private void CalculateAndDisplay()
        {
            var expression = AddMissingBrackets(TextBlockSecondary.Text);
            if (expression is not null)
            {
                var result = _interpreter.Calculate(expression);
                TextBlockMain.Text = result;
                _isResult = true;
            }
        }
        
        private void CalculateAndDisplay(string expression)
        {
            var result = _interpreter.Calculate(expression);
            TextBlockMain.Text = result;
            _isResult = true;
        }
    }
}