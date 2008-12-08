using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model;

namespace MuCell.Controller
{
    /// <summary>
    /// Interface defining the methods the SpatialConfigurationPanelUI must provide
    /// the system controller with.
    /// </summary>
    public interface ISpatialConfigurationPanelUI : IControllable<SpatialConfigurationPanelController>
    {
        /// <summary>
        ///  register a system timer1 tick for animation synchronisation 
        /// </summary
        void timer1Tick();

        /// <summary>
        /// sets the list of distribution functions
        /// </summary>
        /// <param name="funcs">List of distribution functions that the user may choose from</param>
        void setDistributionFunctions(List<DistributionFunction> funcs);

        /// <summary>
        /// Configures UI to display the inital state (cell spatial config.) of the given simulation parameter set
        /// </summary>
        /// <param name="simulationParameters">Initial parameters for the simulation</param>
        void setSimulationParameters(Model.SimulationParameters simulationParameters);

        /// <summary>
        /// Sets the list of cell definitions that the user may choose from
        /// </summary>
        /// <param name="cellDefs">Cell definitions in the currently selected experiment</param>
        void setCellDefinitions(CellDefinition[] cellDefs);

        /// <summary>
        /// Sets the list of groups in the group tree view
        /// </summary>
        /// <param name="groups"></param>
        void setCellGroups(List<MuCell.Model.SBML.Group> groups);

        /// <summary>
        /// Sets which radio button is selected in the spatial settings tab.
        /// </summary>
        /// <param name="shape"></param>
        void setBoundaryShape(BoundaryShapes shape);

        /// <summary>
        /// Sets the value of the width numerical component in the spatial settings tab.
        /// </summary>
        /// <param name="width"></param>
        void setBoundaryWidth(float width);

        /// <summary>
        /// Sets the value of the height numerical component in the spatial settings tab.
        /// </summary>
        /// <param name="width"></param>
        void setBoundaryHeight(float height);

        /// <summary>
        /// Sets the value of the depth numerical component in the spatial settings tab.
        /// </summary>
        /// <param name="width"></param>
        void setBoundaryDepth(float depth);

        /// <summary>
        /// Sets the value of the radius numerical component in the spatial settings tab.
        /// </summary>
        /// <param name="width"></param>
        void setBoundaryRadius(float radius);

        /// <summary>
        /// Returns the index of the selected group in the group list box.
        /// Returns -1 if no group is selected.
        /// </summary>
        /// <returns>the index of the selected group in the group list box</returns>
        int GetSelectedGroupIndex();

        /// <summary>
        /// Sets which group is selected in the group list box.
        /// </summary>
        /// <param name="group"></param>
        void setSelectedGroup(MuCell.Model.SBML.Group group);


        /// <summary>
        /// Sets which group is selected in the group list box by the ID of the group.
        /// </summary>
        /// <param name="group"></param>
        void setSelectedGroupByIndex(int index);

        /// <summary>
        /// Sets which nutrient is selected in the nutrient list box
        /// </summary>
        /// <param name="nutrient"></param>
        void setSelectedNutrient(MuCell.Model.NutrientField nutrient);

        /// <summary>
        /// Sets which nutrient is selected in the nutrient list box given the id
        /// of the nutrient
        /// </summary>
        /// <param name="index"></param>
        void setSelectedNutrientByIndex(int index);

        /// <summary>
        /// Sets the UI to display the given list of nutrients in the nutrient list box
        /// </summary>
        /// <param name="nutrients"></param>
        void setNutrients(List<MuCell.Model.NutrientField> nutrients);


        /// <summary>
        /// Returns the index of the currently selected nutrient in the nutrients
        /// listbox
        /// </summary>
        /// <returns></returns>
        int GetSelectedNutrientIndex();

        /// <summary>
        /// Sets the list of initial distributions that the user may choose from
        /// </summary>
        /// <param name="List"></param>
        void setInitalNutrientDistributions();

        
        /// <summary>
        /// Displays a dialog reporting the given list of missing nutrients.
        /// </summary>
        /// <param name="names"></param>
        void DisplayMissingNutrientsDialog(List<String> names);
             
    }
}
