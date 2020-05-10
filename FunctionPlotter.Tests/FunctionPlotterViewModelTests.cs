using FunctionPlotter.Domain.Models;
using NUnit.Framework;
using System;
using System.Linq;

namespace FunctionPlotter.Tests
{
    [TestFixture]
    public sealed class FunctionPlotterViewModelTests
    {
        [Test]
        public void AddComponent_WhenCalled_ObjectIsAdded()
        {
            //Arrange
            var plotter = new FunctionPlotterViewModel();
            var testObject = new VariableObject();

            //Act
            plotter.AddComponent(testObject);

            //Assert
            Assert.IsTrue(plotter.GetFunction() != null);
            Assert.IsTrue(plotter.GetFunction().Count() == 1);
            Assert.IsTrue(plotter.GetFunction().Contains(testObject));
        }

        [Test]
        public void RemoveComponent_WhenCalledWithNonEmptyList_ObjectIsRemoved()
        {
            //Arrange
            var plotter = new FunctionPlotterViewModel();
            var testObject = new VariableObject();
            plotter.AddComponent(testObject);

            //Act
            plotter.RemoveComponent();

            //Assert
            Assert.IsTrue(plotter.GetFunction() != null);
            Assert.IsTrue(!plotter.GetFunction().Any());
            Assert.IsTrue(!plotter.GetFunction().Contains(testObject));
        }

        [Test]
        public void RemoveComponent_WhenCalledWithEmptyList_NoErrorIsThrown()
        {
            //Arrange
            var plotter = new FunctionPlotterViewModel();

            //Act
            plotter.RemoveComponent();
            var result = plotter.GetFunction();

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(!result.Any());
        }


        [Test]
        public void GetCompositeFunction_WhenCalledWithEmptyList_EmptyStringIsReturned()
        {
            //Arrange
            var plotter = new FunctionPlotterViewModel();

            //Act
            var result = plotter.GetCompositeFunction();

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [Test]
        public void GetCompositeFunction_WhenCalledWithNonEmptyList_CorrectFunctionIsReturned()
        {
            //Arrange
            var plotter = new FunctionPlotterViewModel();
            var funcObject = new FunctionObject(Math.Sin);
            var testObject = new VariableObject();

            //Act
            plotter.AddComponent(funcObject);
            plotter.AddComponent(testObject);
            var result = plotter.GetCompositeFunction();

            //Assert
            Assert.IsTrue(!string.IsNullOrEmpty(result));
            Assert.IsTrue(result == "Sinx");
        }

        [Test]
        public void GetLast_WhenCalledWithNonEmptyList_CorrectObjectIsReturned()
        {
            //Arrange
            var plotter = new FunctionPlotterViewModel();
            var funcObject = new FunctionObject(Math.Sin);
            var testObject = new VariableObject();

            //Act
            plotter.AddComponent(funcObject);
            plotter.AddComponent(testObject);
            var result = plotter.GetLast();

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(result == testObject);
        }

        [Test]
        public void GetLast_WhenCalledWithEmptyList_NullIsReturned()
        {
            //Arrange
            var plotter = new FunctionPlotterViewModel();

            //Act
            var result = plotter.GetLast();

            //Assert
            Assert.IsTrue(result == null);
        }
    }
}