using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;
using MuCell.Model;

namespace MuCell.View
{
    class CursorComponent : IModelComponent
    {

        private Vector2 mousePos;
        public CursorComponent(Vector2 mousePos)
        {
            this.mousePos = mousePos;
        }

        #region IModelComponent Members

        public void setPosition(float x, float y)
        {
            mousePos = new Vector2(x, y);
        }

        public Vector2 getPosition()
        {
            return mousePos;
        }

        public float getWidth()
        {
            return 0f;
        }

        public float getHeight()
        {
            return 0f;
        }

        public MuCell.Model.Vector2 getClosestPoint(MuCell.Model.Vector2 otherPosition)
        {
            return mousePos;
        }

        #endregion
    }
}
