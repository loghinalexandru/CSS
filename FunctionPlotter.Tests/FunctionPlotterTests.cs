using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using FunctionPlotter.Domain.Interfaces;
using Moq;
using NUnit.Framework;

namespace FunctionPlotter.Tests
{
    [TestFixture]

    public sealed class FunctionPlotterTests
    {
        private readonly MockRepository _mockRepository;

        private Mock<IPainter> _painterMock;
        private Mock<IFunction> _functionMock;
        private FunctionPlotterViewModel _functionPlotterViewModel;
        private IFunctionPlotter _functionPlotter;

        public FunctionPlotterTests()
        {
            _functionPlotterViewModel = new FunctionPlotterViewModel();
            _mockRepository = new MockRepository(MockBehavior.Strict);
        }

        [SetUp]
        public void Setup()
        {
            SetupMocking();
            _functionPlotter = CreateSystemUnderTest();
        }

        [Test]
        public void When_DrawIntegralFunctionPlotWithConstantFunction_ShouldDrawIntegralFunctionPlot()
        {
            //Arrange
            const int width = 800;
            const int height = 600;

            var yValue = Math.Round(1d, 2);
            var maxY = (yValue + _functionPlotterViewModel.MaxY).ToString(CultureInfo.InvariantCulture);
            var minY = (yValue + _functionPlotterViewModel.MinY).ToString(CultureInfo.InvariantCulture);

            var functionPoints = new List<PointF>
            {
                new PointF(-1, 1),
                new PointF(0, 1),
                new PointF(1, 1)
            };
            var integralRectangles = new List<(PointF, PointF)>
            {
                (new PointF(-1, 1), new PointF(0, 0)),
                (new PointF(0, 1), new PointF(1, 0))
            };

            _painterMock.Setup(x => x.DrawAxis(It.IsAny<int>()));
            _painterMock.Setup(x => x.DrawFunction(It.IsAny<List<PointF>>()));
            _painterMock.Setup(x => x.DrawIntegral(It.IsAny<List<(PointF, PointF)>>()));
            _painterMock.Setup(x => x.ResetTransform());
            _painterMock.Setup(x => x.DrawString(It.IsAny<PointF>(), It.IsAny<string>(), It.IsAny<int>()));

            _functionMock
                .Setup(x => x.GetFunctionGraph(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Returns(functionPoints);
            _functionMock
                .Setup(x => x.GetIntegralPoints(It.IsAny<List<PointF>>()))
                .Returns(integralRectangles);

            //Act
            _functionPlotter.DrawIntegralFunctionPlot(width, height);

            //Assert
            const int twice = 2;

            _painterMock.Verify(x => x.DrawAxis(It.IsAny<int>()), Times.Once);
            _painterMock.Verify(x => x.DrawFunction(It.IsAny<List<PointF>>()), Times.Once);
            _painterMock.Verify(x => x.DrawIntegral(It.IsAny<List<(PointF, PointF)>>()), Times.Once);
            _painterMock.Verify(x => x.ResetTransform(), Times.Exactly(twice));
            _painterMock.Verify(x => x.DrawString(It.IsAny<PointF>(), minY, It.IsAny<int>()), Times.Once);
            _painterMock.Verify(x => x.DrawString(It.IsAny<PointF>(), maxY, It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void When_DrawIntegralFunctionPlotWithNonConstantFunction_ShouldDrawIntegralFunctionPlot()
        {
            //Arrange
            const int width = 800;
            const int height = 600;

            var maxY = Math.Round(2d, 2).ToString(CultureInfo.InvariantCulture);
            var minY = Math.Round(-2d, 2).ToString(CultureInfo.InvariantCulture);

            var functionPoints = new List<PointF>
            {
                new PointF(-2, -2),
                new PointF(-1, -1), 
                new PointF(0, 0),
                new PointF(1, 1),
                new PointF(2, 2),
            };
            var integralRectangles = new List<(PointF, PointF)>
            {
                (new PointF(-2, 0), new PointF(-1, -1)),
                (new PointF(-1, 0), new PointF(0, 0)),
                (new PointF(0, 0), new PointF(1, 0)),
                (new PointF(1, 1), new PointF(2, 1)),
            };

            _painterMock.Setup(x => x.DrawAxis(It.IsAny<int>()));
            _painterMock.Setup(x => x.DrawFunction(It.IsAny<List<PointF>>()));
            _painterMock.Setup(x => x.DrawIntegral(It.IsAny<List<(PointF, PointF)>>()));
            _painterMock.Setup(x => x.ResetTransform());
            _painterMock.Setup(x => x.DrawString(It.IsAny<PointF>(), It.IsAny<string>(), It.IsAny<int>()));

            _functionMock
                .Setup(x => x.GetFunctionGraph(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Returns(functionPoints);
            _functionMock
                .Setup(x => x.GetIntegralPoints(It.IsAny<List<PointF>>()))
                .Returns(integralRectangles);

            //Act
            _functionPlotter.DrawIntegralFunctionPlot(width, height);

            //Assert
            const int twice = 2;

            _painterMock.Verify(x => x.DrawAxis(It.IsAny<int>()), Times.Once);
            _painterMock.Verify(x => x.DrawFunction(It.IsAny<List<PointF>>()), Times.Once);
            _painterMock.Verify(x => x.DrawIntegral(It.IsAny<List<(PointF, PointF)>>()), Times.Once);
            _painterMock.Verify(x => x.ResetTransform(), Times.Exactly(twice));
            _painterMock.Verify(x => x.DrawString(It.IsAny<PointF>(), minY, It.IsAny<int>()), Times.Once);
            _painterMock.Verify(x => x.DrawString(It.IsAny<PointF>(), maxY, It.IsAny<int>()), Times.Once);
        }

        public IFunctionPlotter CreateSystemUnderTest()
        {
            return new FunctionPlotter(_painterMock.Object, _functionMock.Object, _functionPlotterViewModel);
        }

        private void SetupMocking()
        {
            _painterMock = _mockRepository.Create<IPainter>();
            _functionMock = _mockRepository.Create<IFunction>();
        }
    }
}
