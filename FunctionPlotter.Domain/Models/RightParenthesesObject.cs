﻿namespace FunctionPlotter.Domain.Models
{
    public sealed class RightParenthesesObject : GraphObject
    {
        public RightParenthesesObject()
        {
            GraphObjectType = GraphObjectType.RightParentheses;
        }

        public override string ToString()
        {
            return ")";
        }
    }
}