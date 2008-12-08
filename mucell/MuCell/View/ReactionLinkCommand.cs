using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;

namespace MuCell.View
{
    /// <summary>
    /// The command object used for adding and removing links from a species to a reaction and vice versa
    /// </summary>
    public class ReactionLinkCommand : ICommand
    {
        public enum LinkType
        {
            Reactant, Product, Modifier
        }

        private SpeciesReference speciesReference;
        private Reaction reaction;
        private LinkType linkType;
        private bool adding;

        public ReactionLinkCommand(SpeciesReference speciesReference, Reaction reaction, LinkType linkType, bool adding)
        {
            this.speciesReference = speciesReference;
            this.reaction = reaction;
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
            List<SpeciesReference> relevantList = reaction.Reactants;
            if (linkType == LinkType.Product)
            {
                relevantList = reaction.Products;
            }

            if (addIt)
            {
                //add the link to the reaction if it isn't there
                if(!relevantList.Contains(speciesReference))
                {
                    relevantList.Add(speciesReference);
                }
            }
            else
            {
                if(relevantList.Contains(speciesReference))
                {
                    relevantList.Remove(speciesReference);
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
