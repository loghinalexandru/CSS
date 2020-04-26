namespace FunctionPlotter.Domain.Models
{
    public sealed class ConstantObject : GraphObject
    {
        public double? Value { get; }

        public ConstantObject(double? value)
        {
            GraphObjectType = GraphObjectType.Constant;
            Value = value;
        }

        public override string ToString()
        {
            return !Value.HasValue ? "constant" : $"{Value}";
        }
    }
}