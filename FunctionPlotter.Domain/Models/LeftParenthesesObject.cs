namespace FunctionPlotter.Domain.Models
{
    public sealed class LeftParenthesesObject : GraphObject
    {
        public LeftParenthesesObject()
        {
            GraphObjectType = GraphObjectType.LeftParentheses;
        }

        public override string ToString()
        {
            return "(";
        }
    }
}