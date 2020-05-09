using FunctionPlotter.Domain.Models;
using NUnit.Framework;

namespace FunctionPlotter.Domain.Tests
{
    [TestFixture]
    public sealed class LeftParenthesesObjectTests
    {
        [Test]
        public void Constructor_WhenCalled_ValueIsSet()
        {
            //Arrange
            //Act
            var result = new LeftParenthesesObject();

            //Assert
            Assert.AreEqual(result.GraphObjectType , GraphObjectType.LeftParentheses);
        }


        [Test]
        public void ToString_WhenCalled_CorrectValueIsReturned()
        {
            //Arrange
            var leftParenthesesObject = new LeftParenthesesObject();

            //Act
            var result = leftParenthesesObject.ToString();

            //Assert
            Assert.AreEqual(result, "(");
        }
    }
}
