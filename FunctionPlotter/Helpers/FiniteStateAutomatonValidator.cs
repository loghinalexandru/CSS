using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FunctionPlotter.Helpers
{
    public sealed class FiniteStateAutomatonValidator
    {
        // Predefined Order = (),var,op,func
        private List<ComboBox> _states;
        private GraphObject _currentState;

        public FiniteStateAutomatonValidator(List<ComboBox> states)
        {
            _states = states;
            EnableValidTransitions();
        }

        public void DoTransition(GraphObject nextState)
        {
            _currentState = nextState;
            EnableValidTransitions();
        }

        private void EnableValidTransitions()
        {
            switch (_currentState)
            {
                case OperatorObject op:
                    DisableAllStates();
                    EnableState(1);
                    EnableState(3);
                    break;
                case VariableObject var:
                    DisableAllStates();
                    EnableState(2);
                    EnableState(0);
                    break;
                case ConstantObject constant:
                    DisableAllStates();
                    EnableState(2);
                    EnableState(0);
                    break;
                case FunctionObject func:
                    DisableAllStates();
                    EnableState(1);
                    EnableState(0);
                    break;
                case RightParenthesesObject rightpar:
                    DisableAllStates();
                    EnableState(2);
                    break;
                case LeftParenthesesObject leftpar:
                    DisableAllStates();
                    EnableState(1);
                    EnableState(3);
                    break;
                case null:
                    DisableAllStates();
                    EnableState(0);
                    EnableState(1);
                    EnableState(3);
                    break;
            }
        }

        private void DisableAllStates()
        {
            _states.ForEach(state => state.IsEnabled = false);
        }

        private void EnableState(int stateIndex)
        {
            _states[stateIndex].IsEnabled = true;
        }
    }
}