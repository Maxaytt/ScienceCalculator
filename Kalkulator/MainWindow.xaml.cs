using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly DataTable _dataTable = new DataTable();
        private readonly FunctionInterpreter _interpreter = new FunctionInterpreter();
        private const int MaxLength = 50;

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


            TextBlockSecondary.Text += TextBlockSecondary.Text[^1] switch
            {
                ')' => ")",
                _ => string.Concat(TextBlockMain.Text, ")")
            };
            _unpairedLBrackets--;
        }

        private int LastLBracketIndex(string expression)
        {
            var ind = 0;
            var res = 0;
            foreach (var c in expression)
            {
                if (c == '(')
                {
                    res = ind;
                }
                ind++;
            }

            return res;
        }

        private void BtnDegree_OnClick(object sender, RoutedEventArgs e)
        {
            var sec = TextBlockSecondary.Text;
            var lastChar = TextBlockSecondary.Text[^1];
            var length = TextBlockSecondary.Text.Length;
            var index = lastChar == ')' ?
                FindIndexOfBracketPair(sec,length-1) : 0;
            
            TextBlockSecondary.Text = lastChar switch
            {
                ')' => string.Concat(sec[..index], "Pow(", sec[index..], ","),
                '(' => string.Concat(sec, "Pow(", TextBlockMain.Text, ","),
                _ when IsOperator(lastChar) => string.Concat(sec, "Pow(", TextBlockMain.Text, ","),
                _ => string.Concat(sec, "Pow(", ","),
            };
            _unpairedLBrackets++;
        }

        private bool IsOperator(char c)
        {
            var ops = new [] { '+', '-', '*', '/'};
            return ops.Contains(c);
        }
        private int FindIndexOfBracketPair(string str, int indexOfElement)
        {
            var element = str[indexOfElement];
            if (element != ')') throw new Exception("Element isn't a right bracket");
            var rBracketCount = 0;
            for(var i = indexOfElement-1; i >= 0; i--)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (str[i] == ')')
                {
                    rBracketCount++;
                }
                else if(str[i] == '(' && rBracketCount == 0)
                {
                    return i;
                }
                else if(str[i] == '(')
                {
                    rBracketCount--;
                }
            }
            
            throw new Exception("Wrong amount of brackets");
        }
        
        private void BtnSqr_OnClick(object sender, RoutedEventArgs e)
        {
            var str = "Sqr(2+4/5*3)+Pow(3,2)+Sqrt(4)";
            TextBlockSecondary.Text = str;
            var res = _interpreter.Calculate(str);
            TextBlockMain.Text = res;
        }
    }
}