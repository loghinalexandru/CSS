using NUnit.Framework;
using System.Drawing;

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
            Assert.AreEqual(result.Width, 800);
            Assert.AreEqual(result.Height, 600);
        }


        [Test]
        public void GetScaledValues_WhenCalled_CorrectResultIsReturned()
        {
            //Arrange
            var bitmap = new Bitmap(800, 600);
            //Act

            var result = Converters.BitmapToImageSource(bitmap);

            //Assert
            Assert.AreEqual(result.Width, 800);
            Assert.AreEqual(result.Height, 600);
        }
    }
}