using System;
using System.Collections.Generic;
using NCalc;

namespace Kalkulator.Interpreter;

public class FunctionInterpreter
{
    public readonly Dictionary<string, int> FunctionsAndArgCount = new()
    {
        { "Abs", 1},
        { "Fact", 1 },
        { "Sqr", 1 },
        { "Sqrt", 1 },
        { "Log10", 1 },
        { "Ln", 1 },
        { "Cube", 1 },
        { "Cuberoot", 1 },
        { "Pow10", 1 },
        { "Pow2", 1 },
        { "PowE", 1 },
        { "Floor", 1 },
        { "Sin", 1 },
        { "Cos", 1 },
        { "Tan", 1 },
        { "Dms", 1 },
        { "Deg", 1 },
        { "Rnd", 1 },
        { "Sec", 1 },
        { "Csc", 1 },
        { "Cot", 1 },
        
        { "Sinh", 1 },
        { "Sech", 1 },
        { "Cosh", 1 },
        { "Csch", 1 },
        { "Tanh", 1 },
        { "Coth", 1 },
        { "Asin", 1 },
        { "Asec", 1 },
        { "Acos", 1 },
        { "Acsc", 1 },
        { "Atan", 1 },
        { "Acot", 1 },
        
        { "Pow", 2 },
        { "Yroot", 2 },
        { "Log", 2 }
    };
    public string Calculate(string expression)
    {
        var e = new Expression(expression);
        CustomFunctions.Register(e);
        var result = e.Evaluate();
        return result.ToString() ?? throw new InvalidOperationException();
    }
}