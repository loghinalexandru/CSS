using System;
using System.Collections.Generic;
using FluentAssertions;
using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using FunctionPlotter.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace FunctionPlotterTests
{
    public class FunctionTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public FunctionTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void When_Parse_Then_ShouldObtainThePrefixNotation()
        {
            var expression = new List<GraphObject>()
            {
                new FunctionObject(Math.Cos),
                new LeftParenthesesObject(),
                new FunctionObject(Math.Sin),
                new LeftParenthesesObject(),
                new FunctionObject(Math.Exp),
                new LeftParenthesesObject(),
                new VariableObject(),
                new RightParenthesesObject(),
                new OperatorObject("/"),
                new ConstantObject(3),
                new OperatorObject("*"),
                new ConstantObject(-4),
                new RightParenthesesObject(),
                new RightParenthesesObject()
            };
            var systemUnderTest = new Function(expression);

            var graphObjects = systemUnderTest.Parse(expression);
            foreach (var graphObject in graphObjects)
            {
                _testOutputHelper.WriteLine(graphObject.ToString());
            }
        }

        [Fact]
        public void When_Parse_Then_ShouldObtainThePrefixNotationSecondExample()
        {
            var expression = new List<GraphObject>()
            {
                new FunctionObject(Math.Exp),
                new LeftParenthesesObject(),
                new FunctionObject(Math.Sin),
                new VariableObject(),
                new OperatorObject("+"),
                new FunctionObject(Math.Cos),
                new VariableObject(),
                new RightParenthesesObject()
            };
            var systemUnderTest = new Function(expression);

            var graphObjects = systemUnderTest.Parse(expression);
            foreach (var graphObject in graphObjects)
            {
                _testOutputHelper.WriteLine(graphObject.ToString());
            }
        }

        [Fact]
        public void When_Compute_Then_ShouldObtainThePrefixNotation2()
        {
            var expression = new List<GraphObject>()
            {
                new FunctionObject(Math.Exp),
                new LeftParenthesesObject(),
                new FunctionObject(Math.Sin),
                new VariableObject(),
                new OperatorObject("+"),
                new FunctionObject(Math.Cos),
                new VariableObject(),
                new RightParenthesesObject()
            };

            var systemUnderTest = new Function(expression);

            var result = systemUnderTest.Compute(2 * Math.PI);

            result.Should().Be(Math.Exp(Math.Sin(2 * Math.PI) + Math.Cos(2 * Math.PI)));
        }

        [Fact]
        public void When_GetIntegralPoints()
        {
            var expression = new List<GraphObject>()
            {
                new VariableObject(),
                new OperatorObject("+"),
                new ConstantObject(2)
            };

            var systemUnderTest = new Function(expression);

            var points = systemUnderTest.GetFunctionGraph(-5, 2, 0.5);
            var rectangles = systemUnderTest.GetIntegralPoints(points);

            foreach (var valueTuple in rectangles)
            {
                _testOutputHelper.WriteLine(valueTuple.ToString());
            }
        }
    }
}
