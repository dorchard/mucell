using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model;
using MuCell.Model.SBML;
using System.Collections;
using MuCell.View;
using System.Windows.Forms;

namespace MuCell.Controller
{

    /// <summary>
    /// Controller for the cell definition editor panel
    /// </summary> 
    public class CellDefinitionsPanelController : IControllable<ApplicationController>, ISimulationListener, IGraphRearrangementListener
    {
        ApplicationController applicationController;
        ICellDefinitionsPanelUI cellDefinitionsPanelUI;

        CellDefinition currentDefinition;
        Simulation testSimulation;
        private ArrayList undoCommands;
        private ArrayList redoCommands;



        public CellDefinitionsPanelController(ICellDefinitionsPanelUI cellDefinitionsPanelUI)
        {
            this.cellDefinitionsPanelUI = cellDefinitionsPanelUI;
            undoCommands = new ArrayList();
            redoCommands = new ArrayList();
        }
        public void commandPerformed(ICommand command)
        {
            undoCommands.Add(command);
            redoCommands.Clear();
        }
        public void undo()
        {
            if (cellDefinitionsPanelUI.isVisible())
            {
                if (undoCommands.Count > 0)
                {
                    ICommand command = (ICommand)undoCommands[undoCommands.Count - 1];
                    command.undoAction();
                    undoCommands.RemoveAt(undoCommands.Count - 1);
                    redoCommands.Add(command);
                    cellDefinitionsPanelUI.clearSelection();
                    cellDefinitionsPanelUI.refresh();
                }
            }
        }
        public void redo()
        {
            if (cellDefinitionsPanelUI.isVisible())
            {
                if (redoCommands.Count > 0)
                {
                    ICommand command = (ICommand)redoCommands[redoCommands.Count - 1];
                    command.doAction();
                    redoCommands.RemoveAt(redoCommands.Count - 1);
                    undoCommands.Add(command);
                    cellDefinitionsPanelUI.clearSelection();
                    cellDefinitionsPanelUI.refresh();
                }
            }
        }
        public void removeSelection()
        {
            if (cellDefinitionsPanelUI.isVisible())
            {
                cellDefinitionsPanelUI.removeSelection();
            }
        }
        public void stopAutoRearranging()
        {
            GraphLayoutThreadManager.getGraphLayoutThreadManager().stopRearrangeThread();

            cellDefinitionsPanelUI.changeEditMode(EditMode.Pointer, false);
        }
        public void runAutoRerrange()
        {
            if (currentDefinition != null && currentDefinition.getModel() != null)
            {
                cellDefinitionsPanelUI.clearSelection();
                cellDefinitionsPanelUI.changeEditMode(EditMode.AutoRearrange, false);

                GraphLayoutThreadManager.getGraphLayoutThreadManager().setListener(this);
                GraphLayoutThreadManager.getGraphLayoutThreadManager().rearrangeGraphFromModel(currentDefinition.getModel(), cellDefinitionsPanelUI);
            }
        }




        public void stopTestModel()
        {
            if (testSimulation != null)
            {
                testSimulation.PauseSimulation();
                cellDefinitionsPanelUI.changeEditMode(EditMode.Pointer,false);
                testSimulation = null;
                cellDefinitionsPanelUI.overlayCellInstance(null);
            }
        }
        public void runTestModel()
        {
            if (currentDefinition != null && currentDefinition.getModel() != null)
            {
                if (testSimulation != null)
                {
                    testSimulation.PauseSimulation();
                    
                    testSimulation = null;
                }
                cellDefinitionsPanelUI.clearSelection();

                // Cells
                List<MuCell.Model.CellInstance> cells = new List<MuCell.Model.CellInstance>();
                MuCell.Model.CellInstance cell1 = currentDefinition.createCell();
                cells.Add(cell1);
                

                // StateSnapshot for initial state
                MuCell.Model.StateSnapshot initialState = new MuCell.Model.StateSnapshot(cells);
                MuCell.Model.Vector3 size = new MuCell.Model.Vector3(1, 1, 1);
                initialState.SimulationEnvironment = new MuCell.Model.Environment(size);

                // Parameters
                MuCell.Model.SimulationParameters parameters = new MuCell.Model.SimulationParameters();
                parameters.InitialState = initialState;
                parameters.SimulationLength = Double.MaxValue;
                parameters.SnapshotInterval = 0;
                parameters.StepTime = 0.01;

                parameters.SolverMethod = MuCell.Model.Solver.SolverMethods.RungeKutta;

                // Simulation
                testSimulation = new Simulation("Model Test");
                testSimulation.Parameters = parameters;

                Simulator.getSimulator().runThreadedSimulation(testSimulation, this);
                cellDefinitionsPanelUI.overlayCellInstance(cell1);

                cellDefinitionsPanelUI.changeEditMode(EditMode.Test, false);
            }

        }
        public void showDefinition(Model.CellDefinition cellDefinition)
        {
            if (testSimulation != null)
            {
                testSimulation.PauseSimulation();
                cellDefinitionsPanelUI.changeEditMode(EditMode.Pointer,false);
                testSimulation = null;
            }
            currentDefinition = cellDefinition;

            //rearrange the definition if no position information
            Model.SBML.Model sbmlModel = currentDefinition.getModel();
            if (sbmlModel != null)
            {
                List<Species> species = sbmlModel.listOfSpecies;
                if (species.Count>0 && species[0].getPosition()==Vector2.Zero)
                {
                    float maxX = cellDefinitionsPanelUI.getViewWidth();
                    float maxY = cellDefinitionsPanelUI.getViewHeight();
                    float gapX = maxX / (sbmlModel.listOfSpecies.Count*0.5f);
                    float gapY = maxY / (sbmlModel.listOfSpecies.Count * 1f);
                    float cX = gapX;
                    float cY = gapY;

                    foreach (Species s in species)
                    {
                        s.xPosition = cX;
                        s.yPosition = cY;
                        cX += gapX;
                        if (cX >= maxX)
                        {
                            cX = gapX;
                            cY += gapY;
                        }
                    }

                    List<Reaction> reactions = sbmlModel.listOfReactions;
                    foreach (Reaction r in reactions)
                    {
                        float reactionX = r.xPosition;
                        float reactionY = r.yPosition;
                        List<SpeciesReference> reactants = r.Reactants;
                        List<SpeciesReference> products = r.Products;

                        //find center point
                        float totalX = 0f;
                        float totalY = 0f;
                        int count = 0;
                        foreach (SpeciesReference reactant in reactants)
                        {
                            totalX += reactant.species.xPosition;
                            totalY += reactant.species.yPosition;
                            count++;
                        }
                        foreach (SpeciesReference product in products)
                        {
                            totalX += product.species.xPosition;
                            totalY += product.species.yPosition;
                            count++;
                        }
                        if (count > 0)
                        {
                            r.xPosition = totalX / count;
                            r.yPosition = totalY / count;
                        }
                    }
                }
            }

            this.cellDefinitionsPanelUI.editCellDefinition(cellDefinition);
            String details = "";
            if (cellDefinition != null)
            {
                details=cellDefinition.Name;
            }
            Console.WriteLine("Viewing cell definition " + details);
        }
        public CellDefinition getCurrentCellDefinition()
        {
            return currentDefinition;
        }

        /* Note: Searches for a cell definition in the experiment
         * with an identical name. If one is found, it is
         * replaced with the given definition, otherwise a
         * new defintition is added to the experiment object.
         */
        /// <summary>
        /// Commits a cell defition.
        /// </summary>  
        void commitCellDefinition(Model.CellDefinition cellDefinition)
        {

        }

        /// <summary>
        /// Commits the given cell definition under the given new name.
        /// Prompts the user with a warning if a definition with this
        /// name already exists.
        /// </summary> 
        void commitCellDefinitionAs(Model.CellDefinition cellDefinition, String newName)
        {


        }




        #region IControllable<ApplicationController> Members

        public void setController(ApplicationController controller)
        {
            applicationController = controller;
        }

        #endregion


        #region ISimulationListener Members

        void ISimulationListener.simulationIterationComplete(ISimulationView simulation)
        {
            cellDefinitionsPanelUI.refresh();
        }
        public void simulationStopped(ISimulationView simulation)
        {
            cellDefinitionsPanelUI.refresh();
        }

        #endregion



        #region IGraphRearrangementListener Members

        public void rearrangementComplete(MacroCommand command)
        {
            this.commandPerformed(command);
        }

        #endregion
    }
}
