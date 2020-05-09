using FunctionPlotter.Domain.Models;
using NUnit.Framework;
using System;

namespace FunctionPlotter.Domain.Tests
{
    [TestFixture]
    public sealed class FunctionObjectTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Constructor_WhenCalled_ValueIsSet()
        {
            //Arrange
            var function = new Func<double, double>(Math.Sin);

            //Act
            var functionObject = new FunctionObject(function);

            //Assert
            Assert.AreEqual(function, functionObject.Value);
            Assert.AreEqual(GraphObjectType.Function, functionObject.GraphObjectType);
            Assert.AreEqual("Sin", functionObject.FunctionName);
        }


        [Test]
        public void ToString_WhenCalledAndHasValue_CorrectStringIsSet()
        {
            //Arrange
            var function = new Func<double, double>(Math.Sin);
            var functionObject = new FunctionObject(function);

            //Act
            var result = functionObject.ToString();

            //Assert
            Assert.AreEqual(result, "Sin");
        }
    }
}