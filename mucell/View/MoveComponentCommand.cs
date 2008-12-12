using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;
using MuCell.Model;

namespace MuCell.View
{
    public class MoveComponentCommand : ICommand
    {
        private IModelComponent component;
        private Vector2 startPos;
        private Vector2 endPos;

        public MoveComponentCommand(IModelComponent component, Vector2 startPos, Vector2 endPos)
        {
            this.component = component;
            this.startPos = startPos;
            this.endPos = endPos;
        }

        #region ICommand Members

        public void doAction()
        {
            component.setPosition(endPos.x, endPos.y);
        }

        public void undoAction()
        {
            component.setPosition(startPos.x, startPos.y);
        }

        #endregion
    }
}
