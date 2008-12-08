using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML.ExtracellularComponents;

namespace MuCell.View
{
    class ComponentBaseCommand : ICommand
    {
        private ComponentBase component;
        private bool adding;
        private Model.SBML.Model model;

        public ComponentBaseCommand(ComponentBase component, bool adding, Model.SBML.Model model)
        {
            this.component = component;
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
                this.model.listOfComponents.Add(component);
                if (!model.idExists(component.ID))
                {
                    model.AddId(component.ID, component);
                }
            }
            else
            {
                this.model.listOfComponents.Remove(component);
                model.RemoveId(component.ID);
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
