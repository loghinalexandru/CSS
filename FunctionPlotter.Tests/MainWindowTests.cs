using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using FunctionPlotter.Domain.Interfaces;
using Moq;
using NUnit.Framework;

namespace FunctionPlotter.Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public sealed class MainWindowTests
    {
        private Mock<IFunction> _functionMock;
        private Mock<IPainter> _painterMock;
        private Mock<IFunctionPlotter> _plotter;
        private MainWindow _window;

        [SetUp]
        public void SetUp()
        {
            _functionMock = new Mock<IFunction>();
            _painterMock = new Mock<IPainter>();
            _plotter = new Mock<IFunctionPlotter>();

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

            _plotter.Setup(x => x.DrawFunctionPlot(It.IsAny<int>(), It.IsAny<int>()));
            _plotter.Setup(x => x.DrawIntegralFunctionPlot(It.IsAny<int>(), It.IsAny<int>()));

            _window = new MainWindow {_plotter = _plotter.Object};
        }


        [Test]
        public void Constructor_WhenCalled_EverythingIsSet()
        {
            //Arrange
            //Act
            var mainWindow = new MainWindow();

            //Assert

            Assert.IsTrue(mainWindow.IsEnabled);
            Assert.IsTrue(mainWindow.PlotterViewModel != null);
        }

        [Test]
        public void Draw_WhenCalledWithEmptyRangeOrStepSize_ErrorIsThrown()
        {
            //Arrange
            var mainWindow = new MainWindow();

            //Act
            //Assert
            Assert.IsTrue(mainWindow.IsEnabled);
            Assert.Throws<NullReferenceException>(() => mainWindow.Draw(800, 600));
        }

        [Test]
        public void Draw_WhenCalledWithCorrectValues_DrawIsCompleted()
        {
            //Arrange
            _window.PlotterViewModel = GetTestPlotterViewModel();

            //Act
            _window.Draw(800,600);

            //Assert
            Assert.IsTrue(_window.IsEnabled);
        }

        [Test]
        public void Draw_WhenCalledWithDrawIntegralFalse_DrawFunctionIsCompleted()
        {
            //Arrange
            _window.PlotterViewModel = GetTestPlotterViewModel();

            //Act
            _window.Draw(800, 600);

            //Assert
            Assert.IsTrue(_window.IsEnabled);
            _plotter.Verify( x => x.DrawFunctionPlot(800,600), Times.Once());
            _plotter.Verify(x => x.DrawIntegralFunctionPlot(800, 600), Times.Never());
        }

        [Test]
        public void Draw_WhenCalledWithDrawIntegralTrue_DrawIntegralFunctionIsCompleted()
        {
            //Arrange
            var model = GetTestPlotterViewModel();
            model.DrawIntegral = true;
            _window.PlotterViewModel = model;

            //Act
            _window.Draw(800, 600);

            //Assert
            Assert.IsTrue(_window.IsEnabled);
            _plotter.Verify(x => x.DrawFunctionPlot(800, 600), Times.Never());
            _plotter.Verify(x => x.DrawIntegralFunctionPlot(800, 600), Times.Once());
        }

        private FunctionPlotterViewModel GetTestPlotterViewModel()
        {
            return new FunctionPlotterViewModel()
            {
                Min = -5,
                Max = 5,
                StepSize = 0.1,
                DrawIntegral = false
            };
        }
    }
}
