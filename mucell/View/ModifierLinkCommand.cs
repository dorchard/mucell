using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;

namespace MuCell.View
{
    class ModifierLinkCommand : ICommand
    {
        private ModifierSpeciesReference modifierReference;
        private Reaction reaction;
        private bool adding;
        public ModifierLinkCommand(ModifierSpeciesReference modifierReference, Reaction reaction, bool adding)
        {
            this.modifierReference = modifierReference;
            this.reaction = reaction;
            this.adding = adding;
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
                if (!reaction.Modifiers.Contains(modifierReference))
                {
                    reaction.Modifiers.Add(modifierReference);
                }
            }
            else
            {
                if (reaction.Modifiers.Contains(modifierReference))
                {
                    reaction.Modifiers.Remove(modifierReference);
                }
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
