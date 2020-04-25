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

        public int GetPrecedence()
        {
            if ("^".Contains(Value))
                return 2;
            if ("*/".Contains(Value))
                return 1;
            return 0;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}