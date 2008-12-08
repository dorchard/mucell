using System;
using System.Collections.Generic;
using System.Text;
using MuCell.View;

namespace MuCell.Controller
{
    public interface IGraphRearrangementListener
    {
        void rearrangementComplete(MacroCommand command);
    }
}
