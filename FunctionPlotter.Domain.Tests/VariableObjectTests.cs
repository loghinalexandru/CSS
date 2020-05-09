using FunctionPlotter.Domain.Models;
using NUnit.Framework;

namespace FunctionPlotter.Domain.Tests
{
    [TestFixture]
    public sealed class VariableObjectTests
    {

        [Test]
        public void Constructor_WhenCalled_ValueIsSet()
        {
            //Arrange
            //Act
            var result = new VariableObject();

            //Assert
            Assert.AreEqual(result.Value, "x");
            Assert.AreEqual(result.GraphObjectType, GraphObjectType.Variable);
        }

        [Test]
        public void ToString_WhenCalled_CorrectValueIsReturned()
        {
            //Arrange
            var variableObject = new VariableObject();

            //Act
            var result = variableObject.ToString();

            //Assert
            Assert.AreEqual(result, "x");
        }
    }
}
