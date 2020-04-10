using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionPlotter.Helpers
{
    public class ExpressionParser
    {

        public List<Func<double, double>> Parse(string expression)
        {
        var builder = new StringBuilder();

        var result = new List<Func<double, double>>();

            for (var i = 0; i < expression.Length; i++)
            {
                builder.Append(expression[i]);

                switch (builder.ToString())
                {
                    case "x^":
                        var power = char.GetNumericValue(expression[i + 1]);
                        result.Add(x => Math.Pow(x, power));
                        builder.Clear();
                        break;
                }
            }

            return result;
        }
    }
}