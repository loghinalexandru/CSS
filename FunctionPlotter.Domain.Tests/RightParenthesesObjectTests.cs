using FunctionPlotter.Domain.Models;
using NUnit.Framework;

namespace FunctionPlotter.Domain.Tests
{
    [TestFixture]
    public sealed class RightParenthesesObjectTests
    {
        [Test]
        public void Constructor_WhenCalled_ValueIsSet()
        {
            //Arrange
            //Act
            var result = new RightParenthesesObject();

            //Assert
            Assert.AreEqual(result.GraphObjectType, GraphObjectType.RightParentheses);
        }


        [Test]
        public void ToString_WhenCalled_CorrectValueIsReturned()
        {
            //Arrange
            var rightParenthesesObject = new RightParenthesesObject();

            //Act
            var result = rightParenthesesObject.ToString();

            //Assert
            Assert.AreEqual(result, ")");
        }
    }
}