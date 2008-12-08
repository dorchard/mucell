using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Controller
{
    public interface ITimeSeriesEditorPanelUI : IControllable<TimeSeriesEditorPanelController>
    {
        void setSimulationParameters(Model.SimulationParameters simulationParameters);

        /// <summary>
        /// Sets the cell definitions, groups and species in the UI
        /// </summary>
        void setIdentifiersLists(List<MuCell.Model.CellDefinition> cellDefinitionList, List<MuCell.Model.SBML.Group> groupList, List<MuCell.Model.SBML.Species> speciesList);

        /// <summary>
        /// Sets the global cell definitions, groups and species lists for use when determining context sensitivity in time series formula bo
        /// </summary>
        /// <param name="cellDefinitionList"></param>
        /// <param name="groupList"></param>
        /// <param name="speciesList"></param>
        void setGlobalIdentifiers(List<MuCell.Model.CellDefinition> cellDefinitionList, List<MuCell.Model.SBML.Group> groupList, List<MuCell.Model.SBML.Species> speciesList);

        /// <summary>
        /// Called when the time series list needs to be updated, on adding, removal or change of simulation
        /// </summary>
        /// <param name="timeSeries"></param>
        void setListOfTimeSeries(List<Model.TimeSeries> timeSeries);
    }
}
