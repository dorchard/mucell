using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML
{
    /// <summary>
    /// Components of an SBML model which are visibly drawn in the editor (so have position)
    /// </summary>
    public interface IModelComponent
    {
        void setPosition(float x, float y);
        Vector2 getPosition();
        float getWidth();
        float getHeight();
        Vector2 getClosestPoint(Vector2 otherPosition);
    }
}
