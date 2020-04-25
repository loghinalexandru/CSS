using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FunctionPlotter.Helpers
{
    public sealed class Function
    {
        private readonly Queue<GraphObject> _rpnQueue;

        public Function(ICollection<GraphObject> tokens)
        {
            if (tokens == null || tokens.Count == 0)
            {
                tokens = new List<GraphObject>()
                {
                    new VariableObject(),
                    new OperatorObject("+"),
                    new ConstantObject(2)
                };
            }

            _rpnQueue = Parse(tokens);
        }

        public List<PointF> GetFunctionGraph(double low, double high, double stepSize)
        {
            if (low >= high)
                throw new InvalidOperationException("Invalid interval");

            if (high - low < stepSize)
                throw new InvalidOperationException("Invalid step size");

            var points = new List<PointF>();
            for (var x = low; x <= high; x += stepSize)
            {
                points.Add(new PointF((float) x, (float) Compute(x)));
            }

            return points;
        }

        public double Compute(double x)
        {
            var stack = new Stack<double>();
            var copyQueue = new Queue<GraphObject>(_rpnQueue);

            while (copyQueue.Count != 0)
            {
                var graphObject = copyQueue.Dequeue();

                if (graphObject.GraphObjectType == GraphObjectType.Constant)
                {
                    stack.Push(((ConstantObject) graphObject).Value);
                }

                if (graphObject.GraphObjectType == GraphObjectType.Variable)
                {
                    stack.Push(x);
                }

                if (graphObject.GraphObjectType == GraphObjectType.Operator)
                {
                    var operatorValue = ((OperatorObject) graphObject).Value;
                    double number;
                    switch (operatorValue)
                    {
                        case "+":
                        {
                            stack.Push(stack.Pop() + stack.Pop());
                            break;
                        }
                        case "-":
                        {
                            number = stack.Pop();
                            stack.Push(stack.Pop() - number);
                            break;
                        }
                        case "/":
                        {
                            number = stack.Pop();
                            stack.Push(stack.Pop() / number);
                            break;
                        }
                        case "*":
                        {
                            stack.Push(stack.Pop() * stack.Pop());
                            break;
                        }
                        case "^":
                        {
                            number = stack.Pop();
                            stack.Push(Math.Pow(stack.Pop(), number));
                            break;
                        }
                        default:
                            throw new InvalidOperationException(operatorValue);
                    }
                }

                if (graphObject.GraphObjectType == GraphObjectType.Function)
                {
                    var func = ((FunctionObject) graphObject).Value;
                    stack.Push(func(stack.Pop()));
                }
            }

            return stack.Pop();
        }

        public Queue<GraphObject> Parse(ICollection<GraphObject> graphObjects)
        {
            Queue<GraphObject> outputQueue = new Queue<GraphObject>();
            Stack<GraphObject> operatorStack = new Stack<GraphObject>();

            foreach (var graphObject in graphObjects)
            {
                if (graphObject.GraphObjectType == GraphObjectType.Constant ||
                    graphObject.GraphObjectType == GraphObjectType.Variable)
                {
                    outputQueue.Enqueue(graphObject);
                }

                if (graphObject.GraphObjectType == GraphObjectType.Function)
                {
                    operatorStack.Push(graphObject as FunctionObject);
                }

                if (graphObject.GraphObjectType == GraphObjectType.Operator)
                {
                    while (operatorStack.Count != 0)
                    {
                        var stackTop = operatorStack.Peek();

                        if (stackTop.GraphObjectType != GraphObjectType.Function &&
                            (stackTop.GraphObjectType != GraphObjectType.Operator ||
                             ((OperatorObject) stackTop).GetPrecedence() <
                             ((OperatorObject) graphObject).GetPrecedence()) ||
                            stackTop.GraphObjectType == GraphObjectType.LeftParentheses)
                            break;
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    operatorStack.Push(graphObject);
                }

                if (graphObject.GraphObjectType == GraphObjectType.LeftParentheses)
                {
                    operatorStack.Push(graphObject);
                }

                if (graphObject.GraphObjectType == GraphObjectType.RightParentheses)
                {
                    while (operatorStack.Count != 0)
                    {
                        var stackTop = operatorStack.Pop();
                        if (stackTop.GraphObjectType == GraphObjectType.LeftParentheses)
                            break;
                        outputQueue.Enqueue(stackTop);
                    }
                }
            }

            while (operatorStack.Count != 0)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }

            return outputQueue;
        }
    }
}