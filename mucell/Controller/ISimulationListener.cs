using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model;

namespace MuCell.Controller
{
    public interface ISimulationListener
    {
        void simulationIterationComplete(ISimulationView simulation);
        void simulationStopped(ISimulationView simulation);
    }
}
