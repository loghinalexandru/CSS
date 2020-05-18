using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EnsureArg;
using FunctionPlotter.Domain.Interfaces;

namespace FunctionPlotter.Helpers
{
    public sealed class Function : IFunction
    {
        private readonly Queue<GraphObject> _rpnQueue;

        public Function(ICollection<GraphObject> tokens)
        {
            _rpnQueue = Parse(tokens);
        }

        public List<PointF> GetFunctionGraph(double low, double high, double stepSize)
        {
            if (stepSize <= 0)
                throw new InvalidOperationException("Invalid step size");

            if (low >= high)
                throw new InvalidOperationException("Invalid interval");

            if (high - low < stepSize)
                throw new InvalidOperationException("Too high step size");

            var points = new List<PointF>();
            for (var x = low; x <= high; x += stepSize)
            {
                points.Add(new PointF((float) x, (float) Compute(x)));
            }

            Ensure.Arg(points.Count == (int) ((high - low) / stepSize + 1));

            return points;
        }

        public List<(PointF, PointF)> GetIntegralPoints(List<PointF> functionPoints)
        {
            Ensure.Arg(functionPoints).IsNotNullOrEmpty();
            Ensure.Arg(functionPoints.Count > 1);

            var points = new List<(PointF, PointF)>();

            var pointsY = functionPoints.Select(entry => entry.Y).ToList();
            var minY = pointsY.Min();
            var maxY = pointsY.Max();

            if (Math.Abs(minY - maxY) < 0.001)
            {
                if (minY > 0)
                    minY = 0;

                if (maxY < 0)
                    maxY = 0;
            }

            Ensure.Arg(Math.Abs(minY - maxY) < 0.001);

            for (var i = 0; i < functionPoints.Count - 1; i += 1)
            {
                PointF upperLeftPoint;
                PointF lowerRightPoint;

                if (functionPoints[i].Y > 0)
                {
                    upperLeftPoint = functionPoints[i];
                    lowerRightPoint = new PointF(functionPoints[i + 1].X, Math.Max(0, minY));
                }
                else
                {
                    upperLeftPoint = new PointF(functionPoints[i].X, Math.Min(0, maxY));
                    lowerRightPoint = functionPoints[i + 1];
                }

                Ensure.Arg(upperLeftPoint.X < lowerRightPoint.X);
                Ensure.Arg(upperLeftPoint.Y > lowerRightPoint.Y);

                points.Add((upperLeftPoint, lowerRightPoint));
            }

            Ensure.Arg(points.Count == functionPoints.Count - 1);

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
                    stack.Push(((ConstantObject) graphObject).Value.Value);
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

            Ensure.Arg(stack.Count == 1);

            return stack.Pop();
        }

        public Queue<GraphObject> Parse(ICollection<GraphObject> graphObjects)
        {
            Ensure.Arg(graphObjects).IsNotNullOrEmpty();

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

            Ensure.Arg(operatorStack.Count == 0);
            Ensure.Arg(outputQueue.All(op => !(op is RightParenthesesObject) && !(op is LeftParenthesesObject)));

            return outputQueue;
        }
    }
}