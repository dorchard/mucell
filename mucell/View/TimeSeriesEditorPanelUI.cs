using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MuCell.Controller;
using System.Text.RegularExpressions;
using MuCell.Model;

namespace MuCell.View
{
    public partial class TimeSeriesEditorPanelUI : UserControl, ITimeSeriesEditorPanelUI
    {
        private TimeSeriesEditorPanelController controller;

        private List<Model.CellDefinition> cellDefinitions;
        private List<Model.SBML.Species> species;
        private List<Model.SBML.Group> groups;
        private bool showContextSensitiveMenu = false;

        private Model.SimulationParameters simParameters;

        public TimeSeriesEditorPanelUI()
        {
            InitializeComponent();

            listOfTimeSeries_SelectNone();
            btnRemoveTimeSeries.Enabled = false;
            txtFormulaHelper.Text = "Select from these items in your simulation then click \'Add to formula\', or type in the expression box above. Click \'Validate\' to set the formula to record.";
        }

        #region IControllable<TimeSeriesEditorPanelController> Members

        public void setController(TimeSeriesEditorPanelController controller)
        {
            this.controller = controller;
        }

        #endregion

        #region ITimeSeriesEditorPanelUI Members

        public void setSimulationParameters(MuCell.Model.SimulationParameters simulationParameters)
        {
            simParameters = simulationParameters;
            if (simParameters.StepTime > 0)
            {
                foreach (Model.TimeSeries ts in listOfTimeSeries.Items)
                {
                    decimal currentTimeInterval = (decimal)ts.Parameters.TimeInterval;
                    decimal currentSimStep = (decimal)simParameters.StepTime;
                    ts.Parameters.TimeInterval = (double)(Math.Ceiling(currentTimeInterval / currentSimStep) * currentSimStep);
                    if (ts == listOfTimeSeries.SelectedItem)
                    {
                        numericUpDown1.Value = Math.Ceiling(currentTimeInterval / currentSimStep);
                        setTimeIntervalHelperLabel(ts);
                    }
                }
            }
            else
            {
                foreach (Model.TimeSeries ts in listOfTimeSeries.Items)
                {

                }
            }
        }

        public void setGlobalIdentifiers(List<Model.CellDefinition> cellDefinitionList, List<Model.SBML.Group> groupList, List<Model.SBML.Species> speciesList)
        {
            cellDefinitions = cellDefinitionList;
            species = speciesList;
            groups = groupList;
            setIdentifiersLists(cellDefinitionList, groupList, speciesList);
        }

        public void setIdentifiersLists(List<Model.CellDefinition> cellDefinitionList, List<Model.SBML.Group> groupList, List<Model.SBML.Species> speciesList)
        {
            listOfContextCellDefs.Items.Clear();
            foreach (CellDefinition c in cellDefinitionList)
            {
                listOfContextCellDefs.Items.Add(c.Name.Replace(' ', '_'));
            }
            listOfContextGroups.Items.Clear();
            foreach (Model.SBML.Group g in groupList)
            {
                listOfContextGroups.Items.Add(g.Name.Replace(' ', '_'));
            }
            listOfContextSpecies.Items.Clear();
            foreach (Model.SBML.Species s in speciesList)
            {
                listOfContextSpecies.Items.Add(s.ID.Replace(' ', '_'));
            }
        }

        public void setListOfTimeSeries(List<TimeSeries> timeSeries)
        {
            listOfTimeSeries.Items.Clear();
            foreach (TimeSeries ts in timeSeries)
            {
                listOfTimeSeries.Items.Add(ts);
            }
            listOfTimeSeries_SelectNone();
        }

        #endregion

        #region Editing the list of time series

        private void btnAddTimeSeries_Click(object sender, EventArgs e)
        {
            if (simParameters != null)
            {
                Model.TimeSeries ts = new TimeSeries("Time Series " + (listOfTimeSeries.Items.Count + 1), "", simParameters.StepTime);
                simParameters.TimeSeries.Add(ts);
                    listOfTimeSeries.Items.Add(ts);
                    listOfTimeSeries.SelectedItem = ts;
            }
        }

        private void btnRemoveTimeSeries_Click(object sender, EventArgs e)
        {
            Model.TimeSeries ts = (Model.TimeSeries)listOfTimeSeries.SelectedItem;
            if (ts != null)
            {

                controller.removeTimeSeries(ts);
                controller.requestListOfTimeSeries();
                listOfTimeSeries_SelectNone();
                if (listOfTimeSeries.Items.Count == 0)
                {
                    btnRemoveTimeSeries.Enabled = false;
                }
            }

        }

        private void listOfTimeSeries_SelectedValueChanged(object sender, EventArgs e)
        {
            Model.TimeSeries ts = (Model.TimeSeries)listOfTimeSeries.SelectedItem;
            if (ts != null)
            {
                // display the newly selected time series parameters
                txtName.Enabled = true;
                txtName.Text = ts.Parameters.Name;
                txtFormula.Enabled = true;
                txtFormula.Text = ts.Parameters.FunctionString;
                numericUpDown1.Enabled = true;
                if (simParameters.StepTime > 0)
                {
                    numericUpDown1.Value = (decimal)Math.Ceiling(ts.Parameters.TimeInterval / simParameters.StepTime);
                    if ((decimal)ts.Parameters.TimeInterval != ((decimal)simParameters.StepTime * numericUpDown1.Value))
                    {
                        numericUpDown1_ValueChanged(sender, e);
                    }
                }
                else
                {
                    numericUpDown1.Value = 1;
                    numericUpDown1.Enabled = false;
                }
                setTimeIntervalHelperLabel(ts);
                btnRemoveTimeSeries.Enabled = true;
            }
            else
            {
                // disable the unused boxes
                listOfTimeSeries_SelectNone();
            }
        }

        private void listOfTimeSeries_SelectNone()
        {
            listOfTimeSeries.SelectedItem = null;
            txtName.Text = "";
            txtName.Enabled = false;
            txtFormula.Text = "";
            txtFormula.Enabled = false;
            numericUpDown1.Value = 1;
            numericUpDown1.Enabled = false;
            setTimeIntervalHelperLabel(null);
        }

        #endregion

        #region Editing the time series parameters

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            Model.TimeSeries ts = (Model.TimeSeries)listOfTimeSeries.SelectedItem;
            if (ts != null)
            {
                // assign the new name to the time series and refresh the list
                ts.Parameters.Name = txtName.Text;

                int currentSelected = listOfTimeSeries.SelectedIndex;
                controller.requestListOfTimeSeries();
                listOfTimeSeries.SelectedIndex = currentSelected;
            }
        }

        private void txtFormula_Validating(object sender, CancelEventArgs e)
        {
            // should assign the formula to the time series and refresh the list?
        }

        private void btnEvaluateFormula_Click(object sender, EventArgs e)
        {
            Model.TimeSeries ts = (Model.TimeSeries)listOfTimeSeries.SelectedItem;
            if (ts != null)
            {
                string oldFormula = ts.Parameters.FunctionString;
                ts.Parameters.FunctionString = txtFormula.Text;
                if (controller.validateTimeSeries(ts))
                {
                    // success
                }
                else
                {
                    // reset it
                    MessageBox.Show("Warning, formula not valid\nreturning to \'" + oldFormula + "\'");
                    ts.Parameters.FunctionString = oldFormula;
                    if (!oldFormula.Equals("") && !controller.validateTimeSeries(ts))
                    {
                        // set it to blank and don't validate it
                        ts.Parameters.FunctionString = "";
                    }

                    txtFormula.Text = ts.Parameters.FunctionString;
                }

                // refresh list
                int currentSelected = listOfTimeSeries.SelectedIndex;
                controller.requestListOfTimeSeries();
                listOfTimeSeries.SelectedIndex = currentSelected;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Model.TimeSeries ts = (Model.TimeSeries)listOfTimeSeries.SelectedItem;
            if (ts != null)
            {
                if (simParameters.StepTime > 0)
                {
                    // assign the new time interval to the time series
                    ts.Parameters.TimeInterval = (double)numericUpDown1.Value * simParameters.StepTime;
                }
                else
                {
                    ts.Parameters.TimeInterval = 1;
                    if (numericUpDown1.Value != 1)
                    {
                        numericUpDown1.Value = 1;
                    }
                }

                // display what this means in the label
                setTimeIntervalHelperLabel(ts);
            }
        }

        private void setTimeSeriesInterval(Model.TimeSeries ts, decimal value)
        {

        }

        private decimal getTimeSeriesStepSize(Model.TimeSeries ts)
        {
            return 0m;
        }

        private void setTimeIntervalHelperLabel(TimeSeries ts)
        {
            if (ts != null)
            {
                txtTimeIntervalHelper.Text = string.Format("With a simulation step time of {0}s this time series will record every {1}s.", simParameters.StepTime, ts.Parameters.TimeInterval);
            }
            else
            {
                txtTimeIntervalHelper.Text = "No time series currently selected.";
            }
        }

        #endregion

        #region Context sensitive formula lists

        private void txtFormula_TextChanged(object sender, EventArgs e)
        {
            evaluateContextSensitivity();
        }
        private void txtFormula_Click(object sender, EventArgs e)
        {
            evaluateContextSensitivity();
        }
        private void txtFormula_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            {
                evaluateContextSensitivity();
            }
        }

        private void evaluateContextSensitivity()
        {
            int caret = txtFormula.SelectionStart;
            if (caret > 0 && txtFormula.Text.Substring(caret - 1, 1).Equals("."))
            {
                showContextSensitiveMenu = true;

                string identifier = "";

                Regex re = new Regex(@"([a-zA-Z0-9_]*\.)+");

                MatchCollection matches = re.Matches(txtFormula.Text.Substring(0, caret));
                if (matches.Count > 0)
                {
                    identifier = matches[matches.Count - 1].Value;
                    identifier = identifier.Substring(0, identifier.Length - 1);
                }

                string[] identifierComponents = identifier.Split('.');

                switch (identifierComponents.Length)
                {
                    case 1:
                        // check if the identifier is a cell definition
                        foreach (CellDefinition cDef in cellDefinitions)
                        {
                            if (cDef.Name.Replace(' ', '_').Equals(identifierComponents[0]))
                            {
                                controller.requestIdentifiers(cDef, null, null);
                                return;
                            }
                        }

                        // check if the identifier is a group
                        foreach (Model.SBML.Group group in groups)
                        {
                            if (group.Name.Replace(' ', '_').Equals(identifierComponents[0]))
                            {
                                controller.requestIdentifiers(null, group, null);
                                return;
                            }
                        }

                        // check if the identifier is a species
                        foreach (Model.SBML.Species s in species)
                        {
                            if (s.ID.Replace(' ', '_').Equals(identifierComponents[0]))
                            {
                                controller.requestIdentifiers(null, null, s);
                                return;
                            }
                        }

                        MessageBox.Show("Warning, " + identifierComponents[0] + " is not a recognised cell definition, group or species");
                        break;

                    case 2:
                        // check if the first identifier is a cell definition
                        foreach (CellDefinition cDef in cellDefinitions)
                        {
                            if (cDef.Name.Replace(' ', '_').Equals(identifierComponents[0]))
                            {
                                // first is a cell definition

                                // check if the second identifier is a group
                                foreach (Model.SBML.Group group in groups)
                                {
                                    if (group.Name.Replace(' ', '_').Equals(identifierComponents[1]))
                                    {
                                        controller.requestIdentifiers(cDef, group, null);
                                        return;
                                    }
                                }

                                // check if the second identifier is a species
                                foreach (Model.SBML.Species s in species)
                                {
                                    if (s.ID.Replace(' ', '_').Equals(identifierComponents[1]))
                                    {
                                        controller.requestIdentifiers(cDef, null, s);
                                        return;
                                    }
                                }

                                MessageBox.Show("Warning, " + identifierComponents[1] + " (following cell definition " + identifierComponents[0] + ") is not a recognised group or species");
                                return;
                            }
                        }

                        // check if the first identifier is a group
                        foreach (Model.SBML.Group group in groups)
                        {
                            if (group.Name.Replace(' ', '_').Equals(identifierComponents[0]))
                            {
                                // first is a group

                                // check if the second identifier is a species
                                foreach (Model.SBML.Species s in species)
                                {
                                    if (s.ID.Replace(' ', '_').Equals(identifierComponents[1]))
                                    {
                                        controller.requestIdentifiers(null, group, s);
                                        return;
                                    }
                                }

                                MessageBox.Show("Warning, " + identifierComponents[1] + " (following group " + identifierComponents[0] + ") is not a recognised species");
                                return;
                            }
                        }

                        MessageBox.Show("Warning, " + identifierComponents[0] + " is not a recognised cell definition or group so cannot be followed by " + identifierComponents[1]);

                        break;

                    case 3:
                        // check that the first identifier is a cell definition
                        foreach (CellDefinition cDef in cellDefinitions)
                        {
                            if (cDef.Name.Replace(' ', '_').Equals(identifierComponents[0]))
                            {
                                // first is a cell definition

                                // check that the second identifier is a group
                                foreach (Model.SBML.Group group in groups)
                                {
                                    if (group.Name.Replace(' ', '_').Equals(identifierComponents[1]))
                                    {
                                        // second is a group

                                        // check that the third identifier is a species
                                        foreach (Model.SBML.Species s in species)
                                        {
                                            if (s.ID.Replace(' ', '_').Equals(identifierComponents[2]))
                                            {
                                                controller.requestIdentifiers(cDef, group, s);
                                                return;
                                            }
                                        }

                                        MessageBox.Show("Warning, " + identifierComponents[2] + " (following cell definition " + identifierComponents[0] + " and group " + identifierComponents[1] + ") is not a recognised species");
                                        return;

                                    }
                                }

                                MessageBox.Show("Warning, " + identifierComponents[1] + " (following cell definition " + identifierComponents[0] + ") is not a recognised group");
                                return;

                            }
                        }

                        MessageBox.Show("Warning, " + identifierComponents[0] + " is not a recognised cell definition");
                        break;
                    default:
                        // too many components in the identifier
                        MessageBox.Show("Too many components! (Max 3)");
                        break;
                }

            }
            else
            {
                if (showContextSensitiveMenu)
                {
                    controller.requestIdentifiers(null, null, null);
                    showContextSensitiveMenu = false;
                }
            }
        }

        private void listOfContextCellDefs_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateLists();
        }
        private void listOfContextGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateLists();
        }
        private void listOfContextSpecies_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateLists();
        }

        private void updateLists()
        {
            updateFormulaHelperLabel(listOfContextCellDefs.SelectedItem != null ? listOfContextCellDefs.SelectedItem.ToString() : null,
    listOfContextGroups.SelectedItem != null ? listOfContextGroups.SelectedItem.ToString() : null,
    listOfContextSpecies.SelectedItem != null ? listOfContextSpecies.SelectedItem.ToString() : null);
        }

        private void updateFormulaHelperLabel(string cellDef, string group, string species)
        {
            if (cellDef == null && group == null && species == null)
            {
                // nothing
                txtFormulaHelper.Text = "Nothing selected";
            }
            else if (cellDef != null && group == null && species == null)
            {
                // celldef - population
                txtFormulaHelper.Text = "Population of " + cellDef + " cells across the simulation";
            }
            else if (cellDef == null && group != null && species == null)
            {
                // group - population
                txtFormulaHelper.Text = "Population of cells in group " + group;
            }
            else if (cellDef != null && group != null && species == null)
            {
                // celldef.group - population
                txtFormulaHelper.Text = "Population of " + cellDef + " cells in group " + group;
            }
            else if (cellDef == null && group == null && species != null)
            {
                // species - concentration
                txtFormulaHelper.Text = "Sum of concentrations of " + species + " within all cells";
            }
            else if (cellDef != null && group == null && species != null)
            {
                // celldef.species - concentration
                txtFormulaHelper.Text = "Sum of concentrations of " + species + " within " + cellDef + " cells";
            }
            else if (cellDef == null && group != null && species != null)
            {
                // group.species - concentration
                txtFormulaHelper.Text = "Sum of concentrations of " + species + " within all cells in group " + group;
            }
            else if (cellDef != null && group != null && species != null)
            {
                // celldef.group.species
                txtFormulaHelper.Text = "Sum of concentrations of " + species + " within " + cellDef + " cells in group " + group;
            }
        }


        private void btnClearSelection_Click(object sender, EventArgs e)
        {
            clearSelection();
        }

        private void clearSelection()
        {
            showContextSensitiveMenu = true;
            evaluateContextSensitivity();
            listOfContextCellDefs.SelectedItem = null;
            listOfContextGroups.SelectedItem = null;
            listOfContextSpecies.SelectedItem = null;
            updateFormulaHelperLabel(null, null, null);

        }

        private void addToFormula(string text)
        {
            int caret = txtFormula.SelectionStart;
            txtFormula.Text = txtFormula.Text.Insert(caret, text);
            txtFormula.Focus();
            txtFormula.Select(caret + text.Length, 0);
        }

        private void btnAddToFormula_Click(object sender, EventArgs e)
        {
            List<string> identifiers = new List<string>();
            if (listOfContextCellDefs.SelectedItem != null)
                identifiers.Add(listOfContextCellDefs.SelectedItem.ToString());
            if (listOfContextGroups.SelectedItem != null)
                identifiers.Add(listOfContextGroups.SelectedItem.ToString());
            if (listOfContextSpecies.SelectedItem != null)
                identifiers.Add(listOfContextSpecies.SelectedItem.ToString());

            string identifier = string.Join(".", identifiers.ToArray());
            addToFormula(identifier);
            clearSelection();
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            addToFormula("+");
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            addToFormula("-");
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            addToFormula("*");
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            addToFormula("/");
        }
        #endregion

    }
}
