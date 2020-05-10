using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace FunctionPlotter.Helpers.Tests
{
    [TestFixture]
    public sealed class PainterTests
    {
        [Test]
        public void Constructor_WhenCalled_BitmapWithCorrectWidthAndHeightIsSet()
        {
            //Arrange
            var maxWidth = 800;
            var maxHeight = 600;
            var painter = new Painter(maxWidth, maxHeight);

            //Act
            var result = painter.GetBitmap();


            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(result.Height, maxHeight);
            Assert.AreEqual(result.Width, maxWidth);
        }

        [Test]
        public void Constructor_WhenCalled_BackgroundColorIsSetToWhite()
        {
            //Arrange
            var maxWidth = 800;
            var maxHeight = 600;
            var painter = new Painter(maxWidth, maxHeight);

            //Act
            var result = painter.GetBitmap();

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(Enumerable.Range(0, maxWidth).ToList().All(width =>
                Enumerable.Range(0, maxHeight).All(height => IsWhitePixel(result, width, height))));
        }

        [Test]
        public void DrawAxis_WhenCalled_AxisAreSet()
        {
            //Arrange
            var maxWidth = 800;
            var maxHeight = 600;
            var painter = new Painter(maxWidth, maxHeight);

            //Act
            painter.DrawAxis(0);
            var result = painter.GetBitmap();

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(Enumerable.Range(0, 0).ToList().All(width =>
                Enumerable.Range(0, maxHeight).All(height => IsBlackPixel(result, width, height))));
            Assert.IsTrue(Enumerable.Range(0, maxWidth).ToList().All(width =>
                Enumerable.Range(0, 0).All(height => IsBlackPixel(result, width, height))));
        }

        [Test]
        public void DrawFunction_WhenCalled_FunctionIsDrawn()
        {
            //Arrange
            var maxWidth = 800;
            var maxHeight = 600;
            var painter = new Painter(maxWidth, maxHeight);
            var points = GetTestPointsList();

            //Act
            painter.DrawFunction(points);
            var result = painter.GetBitmap();

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(Enumerable.Range(0, maxWidth).ToList().Any(width =>
                Enumerable.Range(0, maxHeight).Any(height => IsBlackPixel(result, width, height))));
        }

        [Test]
        public void DrawIntegral_WhenCalled_IntegralIsDrawn()
        {
            //Arrange
            var maxWidth = 800;
            var maxHeight = 600;
            var painter = new Painter(maxWidth, maxHeight);
            var points = GetTestRectPointsList();

            //Act
            painter.DrawIntegral(points);
            var result = painter.GetBitmap();

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(Enumerable.Range(0, maxWidth).ToList().Any(width =>
                Enumerable.Range(0, maxHeight).Any(height => IsGreenPixel(result, width, height))));
        }

        [Test]
        public void DrawString_WhenCalled_StringIsDrawn()
        {
            //Arrange
            var maxWidth = 800;
            var maxHeight = 600;
            var stringDrawPoint = new PointF(10, 10);
            var painter = new Painter(maxWidth, maxHeight);

            //Act
            painter.DrawString(stringDrawPoint,"TestInput", 10);
            var result = painter.GetBitmap();

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(Enumerable.Range(0, maxWidth).ToList().Any(width =>
                Enumerable.Range(0, maxHeight).Any(height => IsBlackPixel(result, width, height))));
        }

        [Test]
        public void ResetTransform_WhenCalled_TransformIsReset()
        {
            //Arrange
            var maxWidth = 800;
            var maxHeight = 600;
            var stringDrawPoint = new PointF(10, 10);
            var painter = new Painter(maxWidth, maxHeight);

            //Act
            painter.ResetTransform();
            var transformMatrix = painter.GetTransform();

            //Assert
            Assert.IsTrue(transformMatrix != null);
            Assert.IsTrue(transformMatrix.IsIdentity);
        }

        private bool IsWhitePixel(Bitmap bitmap, int width, int height)
        {
            var pixelColor = bitmap.GetPixel(width, height);

            return
                pixelColor.R == 255 && pixelColor.G == 255 && pixelColor.B == 255;
        }

        private bool IsBlackPixel(Bitmap bitmap, int width, int height)
        {
            var pixelColor = bitmap.GetPixel(width, height);

            return
                pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0;
        }

        private bool IsGreenPixel(Bitmap bitmap, int width, int height)
        {
            var pixelColor = bitmap.GetPixel(width, height);

            return
                pixelColor.R == 0 && pixelColor.G == 255 && pixelColor.B == 0;
        }


        private List<PointF> GetTestPointsList()
        {
            return new List<PointF>()
            {
                new PointF(5.2F, 5.4F),
                new PointF(10.25F, 5),
                new PointF(15, 0F),
            };
        }

        private List<(PointF, PointF)> GetTestRectPointsList()
        {
            return new List<(PointF, PointF)>()
            {
                (new PointF(5.2F, 5.4F),new PointF(26.2F, 55.4F)),
                (new PointF(0F, 10.4F),new PointF(5F, 50.4F)),
                (new PointF(3.2F, 5.4F),new PointF(120.2F, 500.4F))
            };
        }
    }
}