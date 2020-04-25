using System;

namespace FunctionPlotter.Domain.Models
{
    public sealed class FunctionObject : GraphObject
    {
        public Func<double, double> Value { get; }

        public FunctionObject(Func<double, double> function)
        {
            GraphObjectType = GraphObjectType.Function;
            Value = function;
        }

        public override string ToString()
        {
            return Value.Method.Name;
        }
    }
}