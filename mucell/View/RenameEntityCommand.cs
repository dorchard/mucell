using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;

namespace MuCell.View
{
    class RenameEntityCommand : ICommand
    {
        private Model.SBML.Model model;
        private SBase entity;
        private String oldID;
        private String newID;

        public RenameEntityCommand(SBase entity, String oldID, String newID, Model.SBML.Model model)
        {
            this.entity = entity;
            this.oldID = oldID;
            this.newID = newID;
            this.model = model;
        }

        #region ICommand Members

        public void doAction()
        {
            //set the right name order and update id
            entity.ID = oldID;
            entity.ID = newID;
            model.updateID(entity);
        }

        public void undoAction()
        {
            entity.ID = newID;
            entity.ID = oldID;
            model.updateID(entity);
        }

        #endregion
    }
}
