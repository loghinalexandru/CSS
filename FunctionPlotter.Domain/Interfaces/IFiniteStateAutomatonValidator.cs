using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionPlotter.Domain.Interfaces
{
    public interface IFiniteStateAutomatonValidator
    {
        public void DoTransition(GraphObject nextState);
    }
}
