using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.View
{
    public interface ICommand
    {
        void doAction();
        void undoAction();
    }
}
