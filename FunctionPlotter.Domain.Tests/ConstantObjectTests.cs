using System.Globalization;
using FunctionPlotter.Domain.Models;
using NUnit.Framework;

namespace FunctionPlotter.Domain.Tests
{
    [TestFixture]
    public class ConstantObjectTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Constructor_WhenCalled_ValueIsSet()
        {
            //Arrange
            const int newValue = 12;

            //Act
            var constantObject = new ConstantObject(newValue);

            //Assert
            Assert.AreEqual(newValue, constantObject.Value);
        }


        [Test]
        public void Constructor_WhenCalledWithNull_ValueIsSet()
        {
            //Arrange
            double? newValue = null;

            //Act
            var constantObject = new ConstantObject(newValue);

            //Assert
            Assert.AreEqual(newValue, constantObject.Value);
        }

        [Test]
        public void ToString_WhenCalledAndHasValue_CorrectStringIsSet()
        {
            //Arrange
            const double newValue = 12;
            var constantObject = new ConstantObject(newValue);

            //Act
            var result = constantObject.ToString();

            //Assert
            Assert.AreEqual(result, newValue.ToString(CultureInfo.InvariantCulture));
        }
        [Test]

        public void ToString_WhenCalledAndIsNull_CorrectStringIsSet()
        {
            //Arrange
            double? newValue = null;
            var constantObject = new ConstantObject(newValue);

            //Act
            var result = constantObject.ToString();

            //Assert
            Assert.AreEqual(result, "constant");
        }
    }
}