namespace FunctionPlotter.Domain.Models
{
    public sealed class VariableObject : GraphObject
    {
        public string Value { get; }

        public VariableObject()
        {
            GraphObjectType = GraphObjectType.Variable;
            Value = "x";
        }

        public override string ToString()
        {
            return Value;
        }
    }
}