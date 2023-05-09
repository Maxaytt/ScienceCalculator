using System;
using NCalc;

namespace Kalkulator;

public static class CustomFunctions
{
    public static void Register(Expression expression)
    {
        expression.EvaluateFunction += HandleSqrFunction;
        expression.EvaluateFunction += HandleSqrtFunction;
    }

    private static void HandleSqrFunction(string name, FunctionArgs args)
    {
        if (name != "Sqr") return;
        var arg = args.Parameters[0].Evaluate();
        if (arg is not double argValue)
        {
            throw new ArgumentException("Argument must be a number for sqr");
        }

        args.Result = argValue * argValue;
    }

    private static void HandleSqrtFunction(string name, FunctionArgs args)
    {
        if (name != "Sqrt") return;
        var argValue = (int)args.Parameters[0].Evaluate();
        if (argValue < 0)
        {
            throw new ArgumentException("Argument must be non-negative for sqrt");
        }

        args.Result = Math.Sqrt(argValue);
    }
}