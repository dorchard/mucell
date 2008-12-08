using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Controller
{
    public class TimeSeriesEditorPanelController : IControllable<ApplicationController>
    {
        ApplicationController applicationController;
        ITimeSeriesEditorPanelUI timeSeriesEditorPanelUI;

        public TimeSeriesEditorPanelController(ITimeSeriesEditorPanelUI timeSeriesEditorPanelUI)
        {
            this.timeSeriesEditorPanelUI = timeSeriesEditorPanelUI;
        }

        #region IControllable<ApplicationController> Members

        public void setController(ApplicationController controller)
        {
            applicationController = controller;
        }

        #endregion

        /*
        public Model.TimeSeries createNewTimeSeries(string name)
        {
            return applicationController.createTimeSeries(name);
        }
         */

        public bool validateTimeSeries(Model.TimeSeries ts)
        {
            return applicationController.validateTimeSeries(ts);
        }

        public void setListOfTimeSeries(List<Model.TimeSeries> timeSeries)
        {
            timeSeriesEditorPanelUI.setListOfTimeSeries(timeSeries);
        }

        public void requestListOfTimeSeries()
        {
            Model.Simulation currentSimulation = applicationController.getCurrentSimulation();
            if (currentSimulation != null)
            {
                timeSeriesEditorPanelUI.setListOfTimeSeries(currentSimulation.Parameters.TimeSeries);
            }
        }

        public void setSimulationParameters(MuCell.Model.SimulationParameters simulationParameters)
        {
            timeSeriesEditorPanelUI.setSimulationParameters(simulationParameters);
        }

        public void setIdentifiers()
        {
            timeSeriesEditorPanelUI.setGlobalIdentifiers(getCellDefinitionList(null, null, null),
                getCellGroups(null, null, null),
                getSpeciesList(null, null, null));
        }

        public void requestIdentifiers(Model.CellDefinition cellDef, Model.SBML.Group group, Model.SBML.Species species)
        {
            timeSeriesEditorPanelUI.setIdentifiersLists(getCellDefinitionList(cellDef, group, species),
                getCellGroups(cellDef, group, species),
                getSpeciesList(cellDef, group, species));
        }

        public List<Model.CellDefinition> getCellDefinitionList(Model.CellDefinition cellDef, Model.SBML.Group group, Model.SBML.Species species)
        {
            if (cellDef == null && group == null && species == null)
            {
                // only return cell definitions if it's the first thing in the identifier
                return new List<Model.CellDefinition>(applicationController.getCurrentExperiment().getCellDefinitions());
            }
            return new List<Model.CellDefinition>();
        }

        public List<Model.SBML.Group> getCellGroups(Model.CellDefinition cellDef, Model.SBML.Group group, Model.SBML.Species species)
        {
            List<Model.SBML.Group> groups = new List<MuCell.Model.SBML.Group>();

            // if a cell def is specified, return the groups in which the cell def is contained
            // if a group is specified, don't return anything
            // if a species is specified, don't return anything

            if (group == null && species == null)
            {

                List<int> groupIDs = new List<int>();
                try
                {
                    groupIDs = applicationController.getCurrentSimulation().Parameters.InitialState.SimulationEnvironment.GetGroups();
                }
                catch
                {
                    // the current simulation might be null
                }

                foreach (int groupID in groupIDs)
                {
                    Model.SBML.Group g = applicationController.getCurrentSimulation().Parameters.InitialState.SimulationEnvironment.GetGroupObject(groupID);
                    if (cellDef != null)
                    {
                        if (g.containsCellDefinition(cellDef))
                        {
                            groups.Add(g);
                        }
                    }
                    else
                    {
                        groups.Add(g);
                    }
                }

            }
            return groups;
        }

        public List<Model.SBML.Species> getSpeciesList(Model.CellDefinition cellDef, Model.SBML.Group group, Model.SBML.Species species)
        {
            List<Model.SBML.Species> speciesList = new List<MuCell.Model.SBML.Species>();

            // if only a cell def is specified, return species in that cell def
            // if only a group is specified, return species in all cell defs in that group
            // if a cell def and group are specified, return species in the cell def provided it's in the group
            // if a species is specified, return nothing

            if (species == null)
            {
                if (cellDef != null)
                {
                    if (group != null)
                    {
                        // group and cell def
                        if (group.containsCellDefinition(cellDef))
                        {
                            speciesList.AddRange(cellDef.getModel().listOfSpecies);
                        }
                    }
                    else
                    {
                        // only a cell def
                        speciesList.AddRange(cellDef.getModel().listOfSpecies);
                    }
                }
                else
                {
                    if (group != null)
                    {
                        // only a group
                        foreach (Model.CellDefinition cDef in applicationController.getCurrentExperiment().getCellDefinitions())
                        {
                            if (group.containsCellDefinition(cDef))
                            {
                                if (cDef.getModel() != null)
                                {
                                    speciesList.AddRange(cDef.getModel().listOfSpecies);
                                }
                            }
                        }
                    }
                    else
                    {
                        // nothing
                        foreach (Model.CellDefinition cDef in applicationController.getCurrentExperiment().getCellDefinitions())
                        {
                            if (cDef.getModel() != null)
                            {
                                speciesList.AddRange(cDef.getModel().listOfSpecies);
                            }
                        }

                    }
                }
            }

            return speciesList;
        }


        public void removeTimeSeries(MuCell.Model.TimeSeries ts)
        {
            applicationController.removeTimeSeries(ts);
        }
    }
}
