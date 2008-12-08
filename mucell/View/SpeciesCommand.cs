using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;

namespace MuCell.View
{
    public class SpeciesCommand : ICommand
    {
        private Species species;
        private bool adding;
        private Model.SBML.Model model;

        public SpeciesCommand(Species species, bool adding, Model.SBML.Model model)
        {
            this.species = species;
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
                this.model.listOfSpecies.Add(species);
                if (!model.idExists(species.ID))
                {
                    model.AddId(species.ID, species);
                }
            }
            else
            {
                this.model.listOfSpecies.Remove(species);
                model.RemoveId(species.ID);
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
