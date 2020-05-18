using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using EnsureArg;
using FunctionPlotter.Domain.Interfaces;

namespace FunctionPlotter.Helpers
{
    public sealed class FiniteStateAutomatonValidator : IFiniteStateAutomatonValidator
    {
        // Predefined Order = (),var,op,func
        private readonly List<Control> _states;
        private GraphObject _currentState;

        public FiniteStateAutomatonValidator(List<Control> states)
        {
            Ensure.Arg(states).IsNotNull();

            _states = states;
            EnableValidTransitions();
        }

        public void DoTransition(GraphObject nextState)
        {
            Ensure.Arg(nextState).IsNotNull();

            _currentState = nextState;

            Ensure.Arg(_currentState == nextState);

            EnableValidTransitions();
        }

        private void EnableValidTransitions()
        {
            switch (_currentState)
            {
                case OperatorObject op:
                    DisableAllStates();
                    EnableState(2);
                    EnableState(4);
                    break;
                case VariableObject var:
                    DisableAllStates();
                    EnableState(3);
                    EnableState(1);
                    break;
                case ConstantObject constant:
                    DisableAllStates();
                    EnableState(3);
                    EnableState(1);
                    break;
                case FunctionObject func:
                    DisableAllStates();
                    EnableState(2);
                    EnableState(0);
                    break;
                case RightParenthesesObject rightpar:
                    DisableAllStates();
                    EnableState(3);
                    break;
                case LeftParenthesesObject leftpar:
                    DisableAllStates();
                    EnableState(2);
                    EnableState(4);
                    break;
                case null:
                    DisableAllStates();
                    EnableState(0);
                    EnableState(2);
                    EnableState(4);
                    break;
            }
        }

        public void DisableAllStates()
        {
            _states.ForEach(state => state.IsEnabled = false);
        }

        public void EnableState(int stateIndex)
        {
            Ensure.Arg(stateIndex).IsGreaterThanOrEqualTo(0);

            _states[stateIndex].IsEnabled = true;
        }

        public List<Control> GetUiElements()
        {
            return _states;
        }
    }
}