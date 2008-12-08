/// <summary>
/// References a "group" of cells in a simulation, used to reference a group in a MathTree
/// </summary>

using System;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace MuCell.Model.SBML
{
    public class Group : SBase
    {

        /// <summary>
        /// List of cells in the group
        /// </summary>
        private List<MuCell.Model.CellInstance> cells;
        [XmlIgnore] // this info already in CellInstance.GroupID
        public List<MuCell.Model.CellInstance> Cells
        {
            get { return cells; }
            set { cells = value; }
        }

        /// <summary>
        /// The name of this group.
        /// </summary>
        private string name;
        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Index into the parent dictionary where this group is contained.
        /// </summary>
        private int index;
        [XmlAttribute]
        public int Index
        {
            get { return index; }
            set { index = value; }
        }


        /// <summary>
        /// How to color the cells in this group.
        /// </summary>
        private System.Drawing.Color col;
        [XmlIgnore]
        public System.Drawing.Color Col
        {
            get { return col; }
            set { col = value; }
        }

        [XmlAttribute]
        public String Colour
        {
            get { return this.col.ToString(); }
            set { this.col = System.Drawing.Color.FromName(value); }
        }




        /// <summary>
        /// Base constructor
        /// </summary>
        public Group()
        {
            this.cells = new List<MuCell.Model.CellInstance>();
        }

        /// <summary>
        /// Constructor from the XML attributes
        /// </summary>
        /// <param name="attrs">
        /// A <see cref="Hashtable"/>
        /// </param>
        public Group(Hashtable attrs)
        {
            this.setId(attrs);
            this.cells = new List<MuCell.Model.CellInstance>();
        }

        /// <summary>
        ///  Constructor given an explicit ID
        /// </summary>
        /// <param name="id">
        /// A <see cref="String"/>
        /// </param>
        public Group(String id)
        {
            this.ID = id;
            this.cells = new List<MuCell.Model.CellInstance>();
        }

        /// <summary>
        /// Constructor given a parent dictionary index.
        /// </summary>
        /// <param name="id"></param>
        public Group(int i)
        {
            this.index = i;
            this.Name = "group" + this.Index;
            this.col = System.Drawing.Color.Black;

            this.cells = new List<MuCell.Model.CellInstance>();
        }


        public bool containsCellDefinition(CellDefinition cellDef)
        {
            foreach (CellInstance cellInstance in cells)
            {
                if (cellInstance.CellInstanceDefinition.Name.Equals(cellDef.Name))
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return name;
        }

    }
}
