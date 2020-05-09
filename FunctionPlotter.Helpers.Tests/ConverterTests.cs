using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FunctionPlotter.Helpers.Tests
{
    public sealed class ConverterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BitmapToImageSource_WhenCalled_CorrectResultIsReturned()
        {
            //Arrange
            var bitmap = new Bitmap(800, 600);
            //Act

            var result = Converters.BitmapToImageSource(bitmap);

            //Assert
            Assert.AreEqual((int) result.Width, 799);
            Assert.AreEqual((int) result.Height, 599);
        }


        [Test]
        public void GetScaledValues_WhenCalledAndPointsListIsEmpty_CorrectResultIsReturned()
        {
            //Arrange
            var pointsList = new List<float>();
            var minVal = -5;
            var maxVal = 5;

            //Act
            var result = Converters.GetScaledValues(pointsList, minVal, maxVal);

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetScaledValues_WhenCalledAndPointsListIsNotEmpty_CorrectResultIsReturned()
        {
            //Arrange
            var pointsList = GetTestPointsList();
            var expectedResultList = new List<float>()
            {
                2.81042576F,
                3.50891852F,
                5,
                -5,
                2.10038686F,
                2.81940246F
            };

            var minVal = -5.0f;
            var maxVal = 5.0f;

            //Act
            var result = Converters.GetScaledValues(pointsList, minVal, maxVal);

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.Max() <= maxVal);
            Assert.IsTrue(result.Min() >= minVal);
            CollectionAssert.AreEqual(expectedResultList, result);
        }

        [Test]
        public void GetScaledValues_WhenCalledAndPointsListIsNotEmptyAndMaxAndMinAreEqual_CorrectResultIsReturned()
        {
            //Arrange
            var pointsList = GetTestPointsList();
            var expectedResultList = new List<float>()
            {
                5,
                5,
                5,
                5,
                5,
                5
            };

            var minVal = 5.0f;
            var maxVal = 5.0f;

            //Act
            var result = Converters.GetScaledValues(pointsList, minVal, maxVal);

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.Max() <= maxVal);
            Assert.IsTrue(result.Min() >= minVal);
            CollectionAssert.AreEqual(expectedResultList, result);
        }


        [Test]
        public void GetScaledValues_WhenCalledAndPointsListIsNotEmptyAndAllValuesAreEqual_CorrectResultIsReturned()
        {
            //Arrange
            var pointsList = new List<float>()
            {
                7, 7, 7, 7, 7, 7
            };

            var expectedResultList = new List<float>()
            {
                0,
                0,
                0,
                0,
                0,
                0
            };

            var minVal = -120.0f;
            var maxVal = 120.0f;

            //Act
            var result = Converters.GetScaledValues(pointsList, minVal, maxVal);

            //Assert
            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.Max() <= maxVal);
            Assert.IsTrue(result.Min() >= minVal);
            CollectionAssert.AreEqual(expectedResultList, result);
        }

        private List<float> GetTestPointsList()
        {
            return new List<float>()
            {
                12.3F,
                24.4F,
                50.23F,
                -123F,
                0F,
                12.4555F
            };
        }
    }
}