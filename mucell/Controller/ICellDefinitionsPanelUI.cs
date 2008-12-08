using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MuCell.Model;
using MuCell.View;

namespace MuCell.Controller
{
    /// <summary>
    /// Interface defining the methods the CellDefinitions must provide
    /// the system controller with.
    /// </summary> 
    public interface ICellDefinitionsPanelUI : IControllable<CellDefinitionsPanelController>, IDrawingInterface
    {


        /// <summary>
        /// Display the given cellDefinition for editing
        /// </summary> 
        void editCellDefinition(Model.CellDefinition cellDefinition);
        float getViewWidth();
        float getViewHeight();
        void refresh();
        void overlayCellInstance(CellInstance cellInstance);
        void clearSelection();
        void removeSelection();
        void changeEditMode(EditMode newMode, bool internalCall);
        bool isVisible();
    }
}
