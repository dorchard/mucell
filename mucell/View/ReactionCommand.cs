using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;

namespace MuCell.View
{
    class ReactionCommand : ICommand
    {
        private Reaction reaction;
        private bool adding;
        private Model.SBML.Model model;

        public ReactionCommand(Reaction reaction, bool adding, Model.SBML.Model model)
        {
            this.reaction = reaction;
            this.adding = adding;
            this.model = model;
        }
        private void applyAction(bool reverse)
        {
            bool addIt = adding;
            if (reverse)
            {
                addIt = !addIt;
            }

            if (addIt)
            {
                this.model.listOfReactions.Add(reaction);
                if (!model.idExists(reaction.ID))
                {
                    model.AddId(reaction.ID, reaction);
                }
            }
            else
            {
                this.model.listOfReactions.Remove(reaction);
                model.RemoveId(reaction.ID);
            }
        }
        #region ICommand Members

        public void doAction()
        {
            applyAction(false);
        }

        public void undoAction()
        {
            applyAction(true);
        }

        #endregion
    }
}
