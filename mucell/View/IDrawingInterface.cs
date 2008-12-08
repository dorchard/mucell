using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model;

namespace MuCell.View
{
    public interface IDrawingInterface
    {
        void redrawGraphPanel();
        Vector2 getScreenTopLeftInWorldCoordinates();
        Vector2 getScreenBottomRightInWorldCoordinates();
    }
}
