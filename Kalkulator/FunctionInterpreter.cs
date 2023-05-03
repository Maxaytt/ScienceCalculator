using System;
using System.Collections.Generic;

namespace Kalkulator;

public class FunctionInterpreter
{
    private Dictionary<string, Delegate> _functionDictionary = new() { 
        { "abs", new Func<double, double>(Math.Abs) },
        { "fact", new Func<double, double>(Factorial) },
        { "sqr", new Func<double, double>(Square) },
        { "sqrt", new Func<double, double>(SquareRoot) },
        { "log", new Func<double, double>(Log) },
        { "ln", new Func<double, double>(NaturalLog) },
        { "cube", new Func<double, double>(Cube) },
        { "cuberoot", new Func<double, double>(CubeRoot) },
        { "^", new Func<double, double, double>(Power) },
        { "yroot", new Func<double, double, double>(YRoot) },
        { "logbase", new Func<double, double, double>(LogBase) }
    private string _testString = "sqr(2)+5^3";
    
    
}