namespace FunctionPlotter.Domain.Models
{
    public sealed class OperatorObject : GraphObject
    {
        public string Value { get; }

        public OperatorObject(string operatorValue)
        {
            GraphObjectType = GraphObjectType.Operator;
            Value = operatorValue;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}