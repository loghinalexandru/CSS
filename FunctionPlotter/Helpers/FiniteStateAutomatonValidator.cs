using FunctionPlotter.Domain;
using FunctionPlotter.Domain.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FunctionPlotter.Helpers
{
    public sealed class FiniteStateAutomatonValidator
    {
        // Predefined Order = (),var,op,func
        private List<Control> _states;
        private GraphObject _currentState;
        private double _openBracketCounter = 0;

        public FiniteStateAutomatonValidator(List<Control> states)
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
            _states[stateIndex].IsEnabled = true;
        }

        public List<Control> GetUiElements()
        {
            return _states;
        }
    }
}