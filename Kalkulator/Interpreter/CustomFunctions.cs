using System;
using System.Numerics;
using NCalc;

namespace Kalkulator.Interpreter;

public static class CustomFunctions
{
    public static void Register(Expression expression)
    {
        expression.EvaluateFunction += HandleSqrFunction;
        expression.EvaluateFunction += HandleSqrtFunction;
        expression.EvaluateFunction += HandleFactFunction;
        expression.EvaluateFunction += HandlePow10Function;
        expression.EvaluateFunction += HandleLog10Function;
        expression.EvaluateFunction += HandleLnFunction;
        expression.EvaluateFunction += HandleCubeFunction;
        expression.EvaluateFunction += HandleCuberootFunction;
        expression.EvaluateFunction += HandleYrootFunction;
        expression.EvaluateFunction += HandlePow2Function;
        expression.EvaluateFunction += HandlePowEFunction;
        expression.EvaluateFunction += HandleDmsFunction;
        expression.EvaluateFunction += HandleDegFunction;
        expression.EvaluateFunction += HandleRndFunction;
        expression.EvaluateFunction += HandleSecFunction;
        expression.EvaluateFunction += HandleCscFunction;
        expression.EvaluateFunction += HandleSinhFunction;
        expression.EvaluateFunction += HandleCschFunction;
        expression.EvaluateFunction += HandleCoshFunction;
        expression.EvaluateFunction += HandleSechFunction;
        expression.EvaluateFunction += HandleTanhFunction;
        expression.EvaluateFunction += HandleCothFunction;
    }

    private static void HandleSqrFunction(string name, FunctionArgs args)
    {
        if (name != "Sqr") return;
        var arg = args.Parameters[0].Evaluate();

        args.Result = arg switch
        {
            double argValue => argValue * argValue,
            int argValue => argValue * argValue,
            _ => throw new ArgumentException("Argument must be a number for sqr")
        };
    }

    private static void HandleSqrtFunction(string name, FunctionArgs args)
    {
        if (name != "Sqrt") return;
        var arg = args.Parameters[0].Evaluate();
        
        args.Result = arg switch
        {
            double argValue and >= 0.0 => Math.Sqrt(argValue),
            int argValue and >= 0 => Math.Sqrt(argValue),
            _ => throw new ArgumentException("Argument must be a number for sqr")
        };
    }
    
    private static void HandleFactFunction(string name, FunctionArgs args)
    {
        if (name != "Fact") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            int argValue and >= 0 => Factorial(argValue),
            _ => throw new ArgumentException("Argument must be non-negative for sqrt")
        };
    }
    
    private static void HandlePow10Function(string name, FunctionArgs args)
    {
        if (name != "Pow10") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            int argValue => Math.Pow(10, argValue),
            double argValue => Math.Pow(10, argValue),
            _ => throw new ArgumentException("Argument must be a number")
        };
    }
    
    private static void HandleLog10Function(string name, FunctionArgs args)
    {
        if (name != "Log10") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            int argValue and > 0 => Math.Log10(argValue),
            double argValue and > 0 => Math.Log10(argValue),
            _ => throw new ArgumentException("Argument must be a number")
        };
    }
    
    private static void HandleLnFunction(string name, FunctionArgs args)
    {
        if (name != "Ln") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            int argValue and > 0 => Math.Log(argValue, Math.E),
            double argValue and > 0 => Math.Log(argValue, Math.E),
            _ => throw new ArgumentException("Argument must be a number")
        };
    }

    private static void HandleCubeFunction(string name, FunctionArgs args)
    {
        if (name != "Cube") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            int argValue => Math.Pow(argValue, 3),
            double argValue => Math.Pow(argValue, 3),
            _ => throw new ArgumentException("Argument must be a number")
        };
    }

    private static void HandleCuberootFunction(string name, FunctionArgs args)
    {
        if (name != "Cuberoot") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            int argValue => Math.Sign(argValue) * Math.Pow(Math.Abs(argValue), 1.0/3.0),
            double argValue => Math.Sign(argValue) * Math.Pow(Math.Abs(argValue), 1.0/3.0),
            _ => throw new ArgumentException("Argument must be a number")
        };
    }

    private static void HandleYrootFunction(string name, FunctionArgs args)
    {
        if (name != "Yroot") return;
        var arg1 = args.Parameters[0].Evaluate();
        var arg2 = args.Parameters[1].Evaluate();
        args.Result = (arg1, arg2) switch
        {
            (0,0) => throw new DivideByZeroException(),
            (IConvertible conv1, IConvertible conv2) => 
                Math.Sign(conv1.ToDouble(null)) * Math.Pow(Math.Abs(conv1.ToDouble(null)), 1.0/conv2.ToDouble(null)),
            _ => throw new ArgumentException("Argument must be a number")
        };
    }

    private static void HandlePow2Function(string name, FunctionArgs args)
    {
        if(name != "Pow2") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => Math.Pow(2.0, Convert.ToDouble(argValue)),
            _ => throw new ArgumentException("Argument must be a number")
        };
    }

    private static void HandlePowEFunction(string name, FunctionArgs args)
    {
        if(name != "PowE") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => Math.Pow(Math.E, Convert.ToDouble(argValue)),
            _ => throw new ArgumentException("Argument must be a number")
        };
    }

    private static void HandleDmsFunction(string name, FunctionArgs args)
    {
        if(name != "Dms") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => DecimalToDms(argValue),
            _ => throw new ArgumentException("Argument must be a number")
        };
    }
    
    private static void HandleDegFunction(string name, FunctionArgs args)
    {
        if(name != "Deg") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => RoundToHalfDegree(argValue),
            _ => throw new ArgumentException("Argument must be a number")
        };
    }

    private static void HandleRndFunction(string name, FunctionArgs args)
    {
        if(name != "Rnd") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => Math.Round(Convert.ToDouble(argValue)),
            _ => throw new ArgumentException("Argument must be a IConvertible")
        };
    }
    
    private static void HandleSecFunction(string name, FunctionArgs args)
    {
        if(name != "Sec") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => 1 / Math.Cos(Convert.ToDouble(argValue)),
            _ => throw new ArgumentException("Argument must be a IConvertible")
        };
    }
    
    private static void HandleCscFunction(string name, FunctionArgs args)
    {
        if(name != "Sec") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => 1 / Math.Sin(Convert.ToDouble(argValue)),
            _ => throw new ArgumentException("Argument must be a IConvertible")
        };
    }

    private static void HandleSinhFunction(string name, FunctionArgs args)
    {
        if(name != "Sinh") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => Math.Sinh(Convert.ToDouble(argValue)),
            _ => throw new ArgumentException("Argument must be a IConvertible")
        };
    }
    
    private static void HandleSechFunction(string name, FunctionArgs args)
    {
        if(name != "Sech") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => 1 / Math.Cosh(Convert.ToDouble(argValue)),
            _ => throw new ArgumentException("Argument must be a IConvertible")
        };
    }
    
    private static void HandleCoshFunction(string name, FunctionArgs args)
    {
        if(name != "Cosh") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => Math.Cosh(Convert.ToDouble(argValue)),
            _ => throw new ArgumentException("Argument must be a IConvertible")
        };
    }
    
    private static void HandleCschFunction(string name, FunctionArgs args)
    {
        if(name != "Csch") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => 1 / Math.Sinh(Convert.ToDouble(argValue)),
            _ => throw new ArgumentException("Argument must be a IConvertible")
        };
    }
    
    private static void HandleTanhFunction(string name, FunctionArgs args)
    {
        if(name != "Tanh") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => Math.Tanh(Convert.ToDouble(argValue)),
            _ => throw new ArgumentException("Argument must be a IConvertible")
        };
    }
    
    private static void HandleCothFunction(string name, FunctionArgs args)
    {
        if(name != "Coth") return;
        var arg = args.Parameters[0].Evaluate();
        args.Result = arg switch
        {
            IConvertible argValue => 1 / Math.Tanh(Convert.ToDouble(argValue)),
            _ => throw new ArgumentException("Argument must be a IConvertible")
        };
    }
    private static BigInteger Factorial(int n)
    {
        BigInteger res = 1;
        while (n != 1)
        {
            res *= n;
            n--;
        }
        return res;
    }

    private static double DecimalToDms(IConvertible decimalDegrees)
    {
        var degrees = decimalDegrees.ToDouble(null);
        var roundedDegrees = Math.Round(degrees * 2) / 2;
        var truncatedDegrees = Math.Truncate(roundedDegrees * 100) / 100; 
        return truncatedDegrees;
    }
    
    private static double RoundToHalfDegree(IConvertible decimalDegrees)
    {
        var roundedDegrees = Math.Round(Convert.ToDouble(decimalDegrees) * 2) / 2;
        return roundedDegrees;
    }
}