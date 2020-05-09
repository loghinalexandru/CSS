using FunctionPlotter.Domain.Models;
using NUnit.Framework;

namespace FunctionPlotter.Domain.Tests
{
    [TestFixture]
    public sealed class OperatorObjectTests
    {

        [Test]
        public void Constructor_WhenCalled_ValueIsSet()
        {
            //Arrange
            const string opr = "+";

            //Act
            var result = new OperatorObject(opr);

            //Assert
            Assert.AreEqual(opr , result.Value);
            Assert.AreEqual(GraphObjectType.Operator, result.GraphObjectType);
        }

        [Test]
        public void GetPrecedence_WhenCalledWithAddition_CorrectValueIsReturned()
        {
            //Arrange
            const string opr = "+";
            var operatorObject = new OperatorObject(opr);

            //Act
            var result = operatorObject.GetPrecedence();

            //Assert
            Assert.AreEqual(result, 0);
        }

        [Test]
        public void GetPrecedence_WhenCalledWithDivision_CorrectValueIsReturned()
        {
            //Arrange
            const string opr = "/";
            var operatorObject = new OperatorObject(opr);

            //Act
            var result = operatorObject.GetPrecedence();

            //Assert
            Assert.AreEqual(result, 1);
        }

        [Test]
        public void GetPrecedence_WhenCalledWithMultiplication_CorrectValueIsReturned()
        {
            //Arrange
            const string opr = "*";
            var operatorObject = new OperatorObject(opr);

            //Act
            var result = operatorObject.GetPrecedence();

            //Assert
            Assert.AreEqual(result, 1);
        }

        [Test]
        public void GetPrecedence_WhenCalledWithPow_CorrectValueIsReturned()
        {
            //Arrange
            const string opr = "^";
            var operatorObject = new OperatorObject(opr);

            //Act
            var result = operatorObject.GetPrecedence();

            //Assert
            Assert.AreEqual(result, 2);
        }
    }
}
