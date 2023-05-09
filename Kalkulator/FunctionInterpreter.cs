using System;
using System.Collections.Generic;
using NCalc;

namespace Kalkulator;

public class FunctionInterpreter
{
    private readonly Dictionary<string, Delegate> _functionDictionary = new()
    {
        { "abs", new Func<double, double>(Math.Abs) },
        { "fact", new Func<double, double>(Factorial) },
        { "sqr", new Func<double, double>(Square) },
        { "sqrt", new Func<double, double>(Math.Sqrt) },
        { "log", new Func<double, double>(Math.Log10) },
        { "ln", new Func<double, double>(NaturalLog) },
        { "cube", new Func<double, double>(Cube) },
        { "cuberoot", new Func<double, double>(Math.Cbrt) },
        { "pow", new Func<double, double, double>(Math.Pow) },
        { "yroot", new Func<double, double, double>(YRoot) },
        { "logbase", new Func<double, double, double>(LogBase) }
    };
    public string Calculate(string expression)
    {
        var e = new Expression(expression);
        CustomFunctions.Register(e);
        var result = e.Evaluate();
        return result.ToString() ?? throw new InvalidOperationException();
    }
    
    private static double Factorial(double n)
    {
        if (n == 0)
            return 1;
        else
            return n * Factorial(n - 1);
    }
    
    private static double Square(double n) => Math.Pow(n, 2);

    private static double NaturalLog(double n) => Math.Log(n, Math.E);

    private static double Cube(double n) => Math.Pow(n, 3);

    private static double YRoot(double x, double y) => Math.Pow(x, 1.0 / y);

    private static double LogBase(double x, double y) => Math.Log(x, y);

}