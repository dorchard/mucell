using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using MuCell.Model;

namespace MuCell.Controller
{

    /// <summary>
    /// Controller class for the spatial configuration panel, providing the operations that
    /// the user may invoke by interacting with the SpatialConfigurationPanelUI.
    /// </summary>e
    public class SpatialConfigurationPanelController : IControllable<ApplicationController>
    {
        //reference to parent controller
        ApplicationController applicationController;
        //reference to UI panel this object is responsible for controlling
        ISpatialConfigurationPanelUI spatialConfigurationPanelUI;
        //the parameters of the currently selected simulation
        SimulationParameters selectedSimulationParameters;


        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="spatialConfigurationPanelUI"></param>
        public SpatialConfigurationPanelController(ISpatialConfigurationPanelUI spatialConfigurationPanelUI)
        {
            this.spatialConfigurationPanelUI = spatialConfigurationPanelUI;
            init();
        }

        /// <summary>
        /// Select the current set of simulation parameters to be configured in the 
        /// spatial configuration panel
        /// </summary>
        /// <param name="simulationParameters"></param>
        public void selectParameters(Model.SimulationParameters simulationParameters)
        {
            this.selectedSimulationParameters = simulationParameters;
            spatialConfigurationPanelUI.setSimulationParameters(simulationParameters);
        }

        /// <summary>
        /// Gets the parameters of the currently selected simulation
        /// </summary>
        /// <returns></returns>
        public SimulationParameters GetSelectedSimulationParameters()
        {
            return selectedSimulationParameters;
        }

        /// <summary>
        /// Register a timer tick
        /// </summary>
        public void timer1Tick()
        {
            spatialConfigurationPanelUI.timer1Tick();
        }


        /// <summary>
        /// Initialize this controller, populating combo boxes
        /// </summary>
        public void init()
        {
            initDistributionFunctions();
            initInitialNutrientDistributions();
            selectedSimulationParameters = null;
        }

        /// <summary>
        /// Initializes distribution functions, creating the objects responsible 
        /// for providing initial spatial data, and populates the relevent combo boxes
        /// </summary>
        public void initDistributionFunctions()
        {
            List<DistributionFunction> funcs = new List<DistributionFunction>();
            NormalDistribution normalDist = new NormalDistribution();
            UniformDistribution uniformDist = new UniformDistribution();

            funcs.Add(normalDist);
            funcs.Add(uniformDist);
            this.spatialConfigurationPanelUI.setDistributionFunctions(funcs);
        }

        /// <summary>
        /// Initializes the possible inital nutrient distribution fuction objects,
        /// populating the relevent combo boxes in the UI panel
        /// </summary>
        public void initInitialNutrientDistributions()
        {
            this.spatialConfigurationPanelUI.setInitalNutrientDistributions();
         }

        /// <summary>
        /// Updates the list of Cell Definitions currently displayed in the UI panel
        /// </summary>
        public void updateAvailableCellDefs()
        {
            if (applicationController.getCurrentExperiment() == null)
            {
                TestRigs.ErrorLog.LogError("Failed to get current exp.");
            }
            else
            {
                this.spatialConfigurationPanelUI.setCellDefinitions(applicationController.getCurrentExperiment().getCellDefinitions());
            }
        }

        /// <summary>
        /// Updates the list of cell groups currently displayed in the UI panel
        /// </summary>
        public void updateCellGroups()
        {
            List<int> groupIndexes = selectedSimulationParameters.InitialState.SimulationEnvironment.GetGroups();
            List<MuCell.Model.SBML.Group> groups = new List<MuCell.Model.SBML.Group>();

            foreach (int groupIndex in groupIndexes)
            {
                groups.Add(selectedSimulationParameters.InitialState.SimulationEnvironment.GetGroupObject(groupIndex));
            }

            this.spatialConfigurationPanelUI.setCellGroups(groups);
        }

        /// <summary>
        /// Adds a cell population to the Environment
        /// </summary>
        /// <param name="cellType">Cell definition</param>
        /// <param name="count">Number of cells to add</param>
        /// <param name="radius">Radius of the cluster of cells</param>
        /// <param name="func">Function used to distribute the cells</param>
        /// <param name="centre">The coordinates of the point from where to distribute the cells</param>
        /// <param name="groupIndex">The index of the group to which the cells are to be added</param>
        public void addCellPopulation(CellDefinition cellType, int count, float radius, DistributionFunction func, Vector3 centre,int groupIndex)
        {
            if (selectedSimulationParameters != null && cellType != null)
            {
                List<Vector3> positionList = func.getPositionList(centre, radius, count);

                //check that the environment supports the necessary nutrient fields
                List<String> missingNutrients;
                missingNutrients = cellType.MissingNutriendFields(selectedSimulationParameters.InitialState.SimulationEnvironment);

                //warn the user if the environment does not support the necessary nutrient fields
                if (missingNutrients.Count > 0)
                {
                    this.spatialConfigurationPanelUI.DisplayMissingNutrientsDialog(missingNutrients);
                    return;
                }


              
                foreach (Vector3 pos in positionList)
                {
                    if (selectedSimulationParameters.InitialState.SimulationEnvironment.Boundary.InsideBoundary(pos))
                    {

                        CellInstance cell = cellType.createCell();// new CellInstance(cellType);

                        //link the cells components to the environment
                        foreach (MuCell.Model.SBML.ExtracellularComponents.ComponentWorldStateBase component in cell.Components)
                        {
                            component.ComponentType.LinkToNutrientFields(selectedSimulationParameters.InitialState.SimulationEnvironment);
                        }
               


                        cell.CellInstanceSpatialContext.Position = pos;
                        selectedSimulationParameters.InitialState.Cells.Add(cell);
                        selectedSimulationParameters.InitialState.SimulationEnvironment.AddCellToGroup(groupIndex, cell);
                    }
                    
                }

          
            }
        }

        /// <summary>
        /// Adds a new group to the environment, and updates the UI group list box accordingly
        /// </summary>
        public void AddGroup()
        {
            int groupIndex = selectedSimulationParameters.InitialState.SimulationEnvironment.GetUnusedGroupIndex();
            selectedSimulationParameters.InitialState.SimulationEnvironment.AddGroup(groupIndex);
            updateCellGroups();
            this.spatialConfigurationPanelUI.setSelectedGroup(selectedSimulationParameters.InitialState.SimulationEnvironment.GetGroupObject(groupIndex));

        }

        /// <summary>
        /// Adds a new nutrient to the environement, and updates the UI list box accordingly 
        /// </summary>
        public void AddNutrient()
        {
            int nutrientIndex = selectedSimulationParameters.InitialState.SimulationEnvironment.GetUnusedNutrientIndex();
            Vector3 pos = selectedSimulationParameters.EnvironmentViewState.CrossHairPosition;

            //only add the nutrient if it is inside the boundaries of the environment
            if (selectedSimulationParameters.InitialState.SimulationEnvironment.Boundary.InsideBoundary(pos))
            {
                NutrientField newNutrientField = new NutrientField(nutrientIndex);
                newNutrientField.InitialPosition = pos;
                selectedSimulationParameters.InitialState.SimulationEnvironment.AddNutrient(nutrientIndex, newNutrientField);
                updateNutrientList();
                this.spatialConfigurationPanelUI.setSelectedNutrient(selectedSimulationParameters.InitialState.SimulationEnvironment.GetNutrientFieldObject(nutrientIndex));
            }
        }

        /// <summary>
        /// Updates the list of nutrients displayed
        /// </summary>
        public void updateNutrientList()
        {
            List<int> nutrientIndexes = selectedSimulationParameters.InitialState.SimulationEnvironment.GetNutrients();
            List<MuCell.Model.NutrientField> nutrients = new List<MuCell.Model.NutrientField>();

            foreach (int nutrientIndex in nutrientIndexes)
            {
                nutrients.Add(selectedSimulationParameters.InitialState.SimulationEnvironment.GetNutrientFieldObject(nutrientIndex));
            }

            this.spatialConfigurationPanelUI.setNutrients(nutrients);
        }


        /// <summary>
        /// Removes the selected group and all the cells in it.
        /// </summary>
        public void RemoveGroup()
        {
            int groupIndex = this.spatialConfigurationPanelUI.GetSelectedGroupIndex();

            foreach (CellInstance cell in selectedSimulationParameters.InitialState.SimulationEnvironment.CellsFromGroup(groupIndex))
            {
                selectedSimulationParameters.InitialState.Cells.Remove(cell);
            }
            selectedSimulationParameters.InitialState.SimulationEnvironment.DeleteGroup(groupIndex);
            updateCellGroups();
        }


        /// <summary>
        /// Removes the selected nutrient from the environment
        /// </summary>
        public void RemoveNutrient()
        {
            int nutrientIndex = this.spatialConfigurationPanelUI.GetSelectedNutrientIndex();

            selectedSimulationParameters.InitialState.SimulationEnvironment.DeleteNutrient(nutrientIndex);
            updateNutrientList();
        }

        /// <summary>
        /// Delete all cells in the environment
        /// </summary>
        public void deleteAllCells()
        {
            selectedSimulationParameters.InitialState.Cells.Clear();
            selectedSimulationParameters.InitialState.SimulationEnvironment.ClearGroups();
            updateCellGroups();
        }



        #region IControllable<ApplicationController> Members
        /// <summary>
        /// Sets the parent controller
        /// </summary>
        /// <param name="controller"></param>
        public void setController(ApplicationController controller)
        {
            applicationController = controller;
        }

        #endregion
    }


    /// <summary>
    /// A simple base class for defining cell distribution functions
    /// </summary>
    public abstract class DistributionFunction
    {
        private String name;

        public void setName(String newName)
        {
            this.name = newName;
        }

        public override String ToString()
        {
            return name;
        }

        public abstract List<Vector3> getPositionList(Vector3 centre,float scale, int count);

    }


 

    /// <summary>
    /// Uses the Box-Muller transform to generate a normally distributed set of values.
    /// </summary>
    public class NormalDistribution : DistributionFunction
    {

        public NormalDistribution()
        {
            setName("Normal distribution");
        }

        public override List<Vector3> getPositionList(Vector3 centre,float scale, int count)
        {

            //generate a list of coordinates using the Box-Muller transform to obtain a normal distribution
            List<Vector3> positionList = new List<Vector3>();
            Random rand = new Random();
           
            for (int n = 0; n < count; n++)
            {
                Vector3 pos = new Vector3();

                pos.x = centre.x + scale * (0.25f * (float)MathHelper.RandBoxMuller(rand));
                pos.y = centre.y + scale * (0.25f * (float)MathHelper.RandBoxMuller(rand));
                pos.z = centre.z + scale * (0.25f * (float)MathHelper.RandBoxMuller(rand));

                positionList.Add(pos);
            }

            return positionList;

        }

    }
    /// <summary>
    /// A simple uniform distribution
    /// </summary>
    public class UniformDistribution : DistributionFunction
    {

        public UniformDistribution()
        {
            setName("Uniform distribution");
        }


        public override List<Vector3> getPositionList(Vector3 centre,float scale, int count)
        {
            List<Vector3> positionList = new List<Vector3>();

            Random rand = new Random();

            for (int n = 0; n < count; n++)
            {
                Vector3 pos = new Vector3();
                pos.x = centre.x + scale * (-0.5f + (float)(rand.Next() % 10000) / 10000);
                pos.y = centre.y + scale * (-0.5f + (float)(rand.Next() % 10000) / 10000);
                pos.z = centre.z + scale * (-0.5f + (float)(rand.Next() % 10000) / 10000);

                positionList.Add(pos);
            }

            return positionList;

        }

    }


}
