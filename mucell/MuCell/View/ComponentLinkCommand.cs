using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;
using MuCell.Model.SBML.ExtracellularComponents;

namespace MuCell.View
{
    /// <summary>
    /// The command used for adding and removing links between species and component link points
    /// </summary>
    public class ComponentLinkCommand : ICommand
    {
        private SpeciesReference speciesReference;
        private ComponentBase component;
        private int linkNumber;
        private ComponentLinkType linkType;
        private bool adding;
        public ComponentLinkCommand(SpeciesReference speciesReference, ComponentBase component, int linkNumber, ComponentLinkType linkType, bool adding)
        {
            this.speciesReference = speciesReference;
            this.component = component;
            this.linkNumber = linkNumber;
            this.linkType = linkType;
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
                component.setSpeciesReference(linkNumber, linkType, speciesReference);
            }
            else
            {
                component.setSpeciesReference(linkNumber, linkType, null);
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
