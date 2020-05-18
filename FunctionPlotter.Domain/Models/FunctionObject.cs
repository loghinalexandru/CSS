using System;
using EnsureArg;

namespace FunctionPlotter.Domain.Models
{
    public sealed class FunctionObject : GraphObject
    {
        public Func<double, double> Value { get; }
        public string FunctionName { get; }

        public FunctionObject(Func<double, double> function)
        {
            Ensure.Arg(function).IsNotNull();

            GraphObjectType = GraphObjectType.Function;
            Value = function;
            FunctionName = function.Method.Name;
        }

        public override string ToString()
        {
            return Value.Method.Name;
        }
    }
}