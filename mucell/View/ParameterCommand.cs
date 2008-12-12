using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;

namespace MuCell.View
{
    class ParameterCommand : ICommand
    {
        private Parameter parameter;
        private Reaction parentReaction;
        private Model.SBML.Model model;
        public ParameterCommand(Parameter parameter, Reaction parentReaction, Model.SBML.Model model)
        {
            this.parameter = parameter;
            this.parentReaction = parentReaction;
            this.model = model;
        }

        #region ICommand Members

        public void doAction()
        {
            parentReaction.Parameters.Add(parameter);
            model.AddId(parameter.ID, parameter);
        }

        public void undoAction()
        {
            parentReaction.Parameters.Remove(parameter);
            model.RemoveId(parameter.ID);
        }

        #endregion
    }
}
