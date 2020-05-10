using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using FunctionPlotter.Domain.Models;
using NUnit.Framework;

namespace FunctionPlotter.Helpers.Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public sealed class FiniteStateAutomatonValidatorTests
    {
        [Test]
        public void Constructor_WhenCalled_StateIsSet()
        {
            //Arrange
            var uiElements = GetTestComboBox();

            //Act 
            var automaton = new FiniteStateAutomatonValidator(uiElements);
            var result = automaton.GetUiElements();

            //Assert
            Assert.IsTrue(result != null);
            CollectionAssert.AreEqual(result, uiElements);
        }

        [Test]
        public void Constructor_WhenCalledWithEmptyList_StateIsSet()
        {
            //Arrange
            var uiElements = new List<Control>();

            //Act 
            var automaton = new FiniteStateAutomatonValidator(uiElements);
            var result = automaton.GetUiElements();

            //Assert
            Assert.IsTrue(result != null);
            CollectionAssert.AreEqual(result, uiElements);
        }

        [Test]
        public void DisableAllStates_WhenCalled_AllUiElementsAreDisabled()
        {
            //Arrange
            var uiElements = GetTestComboBox();
            var automaton = new FiniteStateAutomatonValidator(uiElements);

            //Act 
            automaton.DisableAllStates();
            var result = automaton.GetUiElements();

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.All(entry => entry.IsEnabled == false));
        }

        [Test]
        public void EnableValidTransitions_WhenCalledWithOperator_CorrectStatesAreEnabled()
        {
            //Arrange
            var uiElements = GetTestComboBox();
            var nextState = new OperatorObject("+");
            var automaton = new FiniteStateAutomatonValidator(uiElements);

            //Act 
            automaton.DoTransition(nextState);
            var result = automaton.GetUiElements();

            //Assert
            Assert.IsTrue(result[0].IsEnabled == false);
            Assert.IsTrue(result[2].IsEnabled); //var
            Assert.IsTrue(result[4].IsEnabled); //func
            Assert.IsTrue(result[1].IsEnabled == false);
            Assert.IsTrue(result[3].IsEnabled == false);
        }

        [Test]
        public void EnableValidTransitions_WhenCalledWithVariable_CorrectStatesAreEnabled()
        {
            //Arrange
            var uiElements = GetTestComboBox();
            var nextState = new VariableObject();
            var automaton = new FiniteStateAutomatonValidator(uiElements);

            //Act 
            automaton.DoTransition(nextState);
            var result = automaton.GetUiElements();

            //Assert
            Assert.IsTrue(result[0].IsEnabled == false);
            Assert.IsTrue(result[2].IsEnabled == false);
            Assert.IsTrue(result[3].IsEnabled); //op
            Assert.IsTrue(result[1].IsEnabled); //leftBracket
            Assert.IsTrue(result[4].IsEnabled == false);

        }

        [Test]
        public void EnableValidTransitions_WhenCalledWithConstant_CorrectStatesAreEnabled()
        {
            //Arrange
            var uiElements = GetTestComboBox();
            var nextState = new ConstantObject(12);
            var automaton = new FiniteStateAutomatonValidator(uiElements);

            //Act 
            automaton.DoTransition(nextState);
            var result = automaton.GetUiElements();

            //Assert

            Assert.IsTrue(result[0].IsEnabled == false);
            Assert.IsTrue(result[2].IsEnabled == false);
            Assert.IsTrue(result[3].IsEnabled); //op
            Assert.IsTrue(result[1].IsEnabled); //leftBracket
            Assert.IsTrue(result[4].IsEnabled == false);
        }

        [Test]
        public void EnableValidTransitions_WhenCalledWithFunction_CorrectStatesAreEnabled()
        {
            //Arrange
            var uiElements = GetTestComboBox();
            var nextState = new FunctionObject(Math.Sin);
            var automaton = new FiniteStateAutomatonValidator(uiElements);

            //Act 
            automaton.DoTransition(nextState);
            var result = automaton.GetUiElements();

            //Assert

            Assert.IsTrue(result[1].IsEnabled == false);
            Assert.IsTrue(result[2].IsEnabled); //var
            Assert.IsTrue(result[0].IsEnabled); //rightBracket
            Assert.IsTrue(result[3].IsEnabled == false);
            Assert.IsTrue(result[4].IsEnabled == false);
        }


        [Test]
        public void EnableValidTransitions_WhenCalledWithRightParentheses_CorrectStatesAreEnabled()
        {
            //Arrange
            var uiElements = GetTestComboBox();
            var nextState = new RightParenthesesObject();
            var automaton = new FiniteStateAutomatonValidator(uiElements);

            //Act 
            automaton.DoTransition(nextState);
            var result = automaton.GetUiElements();

            //Assert
            Assert.IsTrue(result[0].IsEnabled == false);
            Assert.IsTrue(result[1].IsEnabled == false);
            Assert.IsTrue(result[2].IsEnabled == false);
            Assert.IsTrue(result[3].IsEnabled); //op
            Assert.IsTrue(result[4].IsEnabled == false);
        }

        [Test]
        public void EnableValidTransitions_WhenCalledWithLeftParentheses_CorrectStatesAreEnabled()
        {
            //Arrange
            var uiElements = GetTestComboBox();
            var nextState = new LeftParenthesesObject();
            var automaton = new FiniteStateAutomatonValidator(uiElements);

            //Act 
            automaton.DoTransition(nextState);
            var result = automaton.GetUiElements();

            //Assert
            Assert.IsTrue(result[0].IsEnabled == false);
            Assert.IsTrue(result[1].IsEnabled == false);
            Assert.IsTrue(result[2].IsEnabled); //var
            Assert.IsTrue(result[3].IsEnabled == false);
            Assert.IsTrue(result[4].IsEnabled); //func
        }

        [Test]
        public void EnableState_WhenCalled_UiElementIsEnabled()
        {
            //Arrange
            var uiElements = GetTestComboBox();
            var automaton = new FiniteStateAutomatonValidator(uiElements);

            //Act 
            automaton.DisableAllStates();
            automaton.EnableState(0);
            var result = automaton.GetUiElements();

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Take(1).All(entry => entry.IsEnabled));
            Assert.IsTrue(result.Skip(1).All(entry => entry.IsEnabled == false));
        }

        private List<Control> GetTestComboBox()
        {
            return new List<Control>()
            {

                new ComboBox(),
                new ComboBox(),
                new ComboBox(),
                new ComboBox(),
                new ComboBox()
            };
        }
    }
}
