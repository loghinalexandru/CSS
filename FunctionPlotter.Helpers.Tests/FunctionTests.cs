using System;
using System.Collections.Generic;
using System.Drawing;
using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using NUnit.Framework;

namespace FunctionPlotter.Helpers.Tests
{
    [TestFixture]
    public sealed class FunctionTests
    {
        private static readonly GraphObject exp = new FunctionObject(Math.Exp);
        private static readonly GraphObject left = new LeftParenthesesObject();
        private static readonly GraphObject sin = new FunctionObject(Math.Sin);
        private static readonly GraphObject x = new VariableObject();
        private static readonly GraphObject add = new OperatorObject("+");
        private static readonly GraphObject sub = new OperatorObject("-");
        private static readonly GraphObject div = new OperatorObject("/");
        private static readonly GraphObject pow = new OperatorObject("^");
        private static readonly GraphObject cos = new FunctionObject(Math.Cos);
        private static readonly GraphObject right = new RightParenthesesObject();
        private static readonly ConstantObject pi = new ConstantObject(Math.PI);
        private static readonly ConstantObject zero = new ConstantObject(0);
        private static readonly ConstantObject two = new ConstantObject(2);
        private static readonly ConstantObject negativeTwo = new ConstantObject(-2);

        [Test]
        public void When_ConstructorWithNullTokenCollection_Then_ShouldThrowArgumentNullException()
        {
            // Arrange
            void FunctionInstantiation() => new Function(null);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(FunctionInstantiation);
        }

        [Test]
        public void When_ConstructorWithEmptyTokenCollection_Then_ShouldArgumentException()
        {
            // Arrange
            void FunctionInstantiation()
            {
                var tokens = new List<GraphObject>();
                new Function(tokens);
            }

            // Act & Assert
            Assert.Throws<ArgumentException>(FunctionInstantiation);
        }

        [Test]
        public void When_ParseWithValidTokenCollection_Then_ShouldParse()
        {
            // Arrange
            var tokens = new List<GraphObject> { exp, left, sin, x, add, cos, pi, right };
            var rpnQueue = new Queue<GraphObject>(new GraphObject[] { x, sin, pi, cos, add, exp });

            var function = new Function(tokens);

            // Act
            var actualRpnQueue = function.Parse(tokens);

            // Assert
            Assert.AreEqual(rpnQueue, actualRpnQueue);
        }

        [Test]
        public void When_ParseWithUnclosedParanthesis_Then_ShouldThrow()
        {
            // Arrange
            var tokens = new List<GraphObject> { exp, left, sin, x, add, cos, x };

            TestDelegate action = () =>
            {
                var function = new Function(tokens);
                function.Parse(tokens);
            };

            // Act & Assert
            Assert.Throws<Exception>(action);
        }

        [Test]
        public void When_ParseWithUnopenedParanthesis_Then_ShouldThrow()
        {
            // Arrange
            var tokens = new List<GraphObject> { exp, sin, x, add, cos, x, right };

            TestDelegate action = () =>
            {
                var function = new Function(tokens);
                function.Parse(tokens);
            };

            // Act & Assert
            Assert.Throws<Exception>(action);
        }

        [Test]
        public void When_ParseWithOperatorAfterFunction_Then_ShouldThrow()
        {
            // Arrange
            var tokens = new List<GraphObject> { exp, sin, add, cos, x, right };

            TestDelegate action = () =>
            {
                var function = new Function(tokens);
                function.Parse(tokens);
            };

            // Act & Assert
            Assert.Throws<Exception>(action);
        }

        [Test]
        public void When_ParseWithOperatorAfterOtherOperator_Then_ShouldThrow()
        {
            // Arrange
            var tokens = new List<GraphObject> { exp, sin, x, add, sub, cos, x, right };

            TestDelegate action = () =>
            {
                var function = new Function(tokens);
                function.Parse(tokens);
            };

            // Act & Assert
            Assert.Throws<Exception>(action);
        }

        [Test]
        public void When_ComputeWithValidRpnQueue_Then_ShouldCompute()
        {
            // Arrange
            var tokens = new List<GraphObject> { exp, left, sin, x, add, cos, pi, right, sub, pi, pow, zero };
            var xValue = 5;
           
            var function = new Function(tokens);

            //  Act 
            var yValue = function.Compute(xValue);

            // Assert
            Assert.AreEqual(Math.Exp(Math.Sin(xValue) + Math.Cos(Math.PI)) - Math.Pow(Math.PI, 0), yValue);
        }

        [Test]
        public void When_ComputeWithDivisionByZero_Then_ShouldComputeInfinity()
        {
            // Arrange
            var tokens = new List<GraphObject> { exp, left, sin, x, add, cos, pi, right, div, zero };
            var xValue = 5;

            var function = new Function(tokens);

            //  Act 
            var yValue = function.Compute(xValue);

            // Assert
            Assert.AreEqual(Double.PositiveInfinity, yValue);
        }

        [Test]
        public void When_GetFunctionGraphWithInvalidStepSize_Then_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var tokens = new List<GraphObject> { exp, left, sin, x, add, cos, pi, right, sub, pi, pow, zero };
            var xMin = -5;
            var xMax = 5;
            var step = 0;

            TestDelegate action = () =>
            {
                var function = new Function(tokens);
                function.GetFunctionGraph(xMin, xMax, step);
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(action);
        }

        [Test]
        public void When_GetFunctionGraphWithInvalidInterval_Then_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var tokens = new List<GraphObject> { exp, left, sin, x, add, cos, pi, right, sub, pi, pow, zero };
            var xMin = 5;
            var xMax = -5;
            var step = 0.1;

            TestDelegate action = () =>
            {
                var function = new Function(tokens);
                function.GetFunctionGraph(xMin, xMax, step);
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(action);
        }

        [Test]
        public void When_GetFunctionGraphWithTooHighStep_Then_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var tokens = new List<GraphObject> { exp, left, sin, x, add, cos, pi, right, sub, pi, pow, zero };
            var xMin = -5;
            var xMax = 5;
            var step = 11;

            TestDelegate action = () =>
            {
                var function = new Function(tokens);
                function.GetFunctionGraph(xMin, xMax, step);
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(action);
        }

        [Test]
        public void When_GetFunctionGraph_Then_ShouldComputeTheListOfPoints()
        {
            // Arrange
            var tokens = new List<GraphObject> { exp, left, sin, x, add, cos, pi, right, sub, pi, pow, zero };
            var xMin = -5;
            var xMax = 5;
            var step = 0.1;

            var function = new Function(tokens);

            // Act
            var points = function.GetFunctionGraph(xMin, xMax, step);

            // Assert
            Assert.That(points, Has.Count.EqualTo((int)((xMax - xMin) / step + 1)));
        }

        [Test]
        public void When_GetIntegralPointsWithConstantFunction_Then_ShouldReturnRectangles()
        {
            // Arrange
            var tokens = new List<GraphObject> { two };
            var xMin = -1;
            var xMax = 1;
            var step = 1;

            var function = new Function(tokens);
            var points = function.GetFunctionGraph(xMin, xMax, step);

            var expectedRectangles = new List<(PointF, PointF)>
            {
                (new PointF(-1, 2), new PointF(0, 0)),
                (new PointF(0, 2), new PointF(1, 0)),
            };

            // Act
            var actualRectangles = function.GetIntegralPoints(points);

            // Assert
            Assert.That(actualRectangles, Has.Count.EqualTo(((xMax - xMin) / step)));
            Assert.AreEqual(expectedRectangles, actualRectangles);
        }

        [Test]
        public void When_GetIntegralPointsWithNegativeConstantFunction_Then_ShouldReturnRectangles()
        {
            // Arrange
            var tokens = new List<GraphObject> { negativeTwo };
            var xMin = -1;
            var xMax = 1;
            var step = 1;

            var function = new Function(tokens);
            var points = function.GetFunctionGraph(xMin, xMax, step);

            var expectedRectangles = new List<(PointF, PointF)>
            {
                (new PointF(-1, 0), new PointF(0, -2)),
                (new PointF(0, 0), new PointF(1, -2)),
            };

            // Act
            var actualRectangles = function.GetIntegralPoints(points);

            // Assert
            Assert.That(actualRectangles, Has.Count.EqualTo(((xMax - xMin) / step)));
            Assert.AreEqual(expectedRectangles, actualRectangles);
        }
    }
}
