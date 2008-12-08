using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;
using System.Xml.Serialization;

namespace MuCell.Model
{

    /*

    /// <summary>
    /// Nutrient function type, given a vector in the 3D environment, returns the nutrient level at that point
    /// </summary>
    /// <param name="position">
    /// A <see cref="Vector3"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.double"/>
    /// </returns>
    public delegate double NutrientFunction(Vector3 position);
    */


    /// <summary>
    /// This SerializableDictionary class was taken from Paul Welter's Weblog at http://weblogs.asp.net/pwelter34/archive/2006/05/03/444961.aspx
    /// Use of the code is announced in the muCell project report
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue>: Dictionary<TKey, TValue>, IXmlSerializable
    {

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {

            return null;

        }



        public void ReadXml(System.Xml.XmlReader reader)
        {

            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));

            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));



            bool wasEmpty = reader.IsEmptyElement;

            reader.Read();



            if (wasEmpty)

                return;



            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {

                reader.ReadStartElement("item");



                reader.ReadStartElement("key");

                TKey key = (TKey)keySerializer.Deserialize(reader);

                reader.ReadEndElement();



                reader.ReadStartElement("value");

                TValue value = (TValue)valueSerializer.Deserialize(reader);

                reader.ReadEndElement();



                this.Add(key, value);



                reader.ReadEndElement();

                reader.MoveToContent();

            }

            reader.ReadEndElement();

        }



        public void WriteXml(System.Xml.XmlWriter writer)
        {

            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));

            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));



            foreach (TKey key in this.Keys)
            {

                writer.WriteStartElement("item");



                writer.WriteStartElement("key");

                keySerializer.Serialize(writer, key);

                writer.WriteEndElement();



                writer.WriteStartElement("value");

                TValue value = this[key];

                valueSerializer.Serialize(writer, value);

                writer.WriteEndElement();



                writer.WriteEndElement();

            }

        }

        #endregion

    }

    public class Environment : ICloneable
    {

        private SerializableDictionary<int, NutrientField> nutrients;
        private SerializableDictionary<int, Group> groups;

        //[XmlIgnore]
        public SerializableDictionary<int, NutrientField> Nutrients { get { return nutrients; } set { nutrients = value; } }
        //[XmlIgnore]
        public SerializableDictionary<int, Group> Groups { get { return groups; } set { groups = value; } }

        // Optimisation -- a list of the nutrient field objects
        private List<NutrientField> nutrientFieldObjects;

        // !!! - Unused  (volume is depended upon the simulation boundary)
        public Vector3 volume;


        /// <summary>
        /// The viscosity of the environment: (ie how much drag there is upon cells
        /// in the environment).
        /// </summary>
        private float viscosity;
        [XmlAttribute]
        public float Viscosity
        {
            get { return viscosity; }
            set { viscosity = value; }
        }


        /// <summary>
        /// Where or not collisions between cells are enabled
        /// </summary>
        private bool enableCellCollisions;
        [XmlAttribute]
        public bool EnableCellCollisions
        {
            get { return enableCellCollisions; }
            set { enableCellCollisions = value; }
        }


        [XmlArray("Groups")]
        [XmlArrayItem("Group")]
        [XmlIgnore]
        public List<Group> XmlGroups
        {
            get
            {
                List<Group> myGroups = new List<Group>();
                foreach (Group group in this.groups.Values)
                {
                    myGroups.Add(group);
                }
                return myGroups;
            }
            set {
                foreach (Group group in value)
                {
                    this.groups.Add(this.groups.Count, group);
                }
            }
        }

        [XmlArray("Nutrients")]
        [XmlArrayItem("Nutrient")]
        [XmlIgnore]
        public List<NutrientField> XmlNutrients
        {
            get
            {
                List<NutrientField> myNutrients = new List<NutrientField>();
                foreach (NutrientField nutrient in this.nutrients.Values)
                {
                    myNutrients.Add(nutrient);
                }
                return myNutrients;
            }
            set
            {
                foreach (NutrientField nutrient in value)
                {
                    this.nutrients.Add(this.nutrients.Count, nutrient);
                }
            }
        }



        /// <summary>
        /// The shape and dimensions of the environment
        /// </summary>
        private Boundary boundary;
        public Boundary Boundary
        {
            get { return boundary; }
            set { boundary = value; }
        }



        public Environment()
        {
            this.volume = new Vector3(0, 0, 0);
            this.Boundary = new Boundary(BoundaryShapes.Cuboid, 80, 80, 80, 40);
            this.viscosity = 1;
            this.enableCellCollisions = false;
            this.nutrientFieldObjects = new List<NutrientField>();
        }

        /// <summary>
        /// Constructor that takes volume data.
        /// </summary>
        /// <param name="volume">
        /// A <see cref="Vector3"/>
        /// </param>
        public Environment(Vector3 volume)
        {
            this.nutrients = new SerializableDictionary<int, NutrientField>();
            this.nutrientFieldObjects = new List<NutrientField>();
            this.groups = new SerializableDictionary<int, Group>();
            this.volume = volume;
            this.boundary = new Boundary(BoundaryShapes.Cuboid, 80, 80, 80, 40);
            this.viscosity = 1;
            this.enableCellCollisions = false;
        }

        /// <summary>
        /// Constructor that takes volume data and a nutrient map
        /// </summary>
        /// <param name="volume">
        /// A <see cref="Vector3"/>
        /// </param>
        /// <param name="nutrients">
        /// A <see cref="Dictionary`2"/>
        /// </param>
        public Environment(Vector3 volume, SerializableDictionary<int, NutrientField> nutrients, SerializableDictionary<int, Group> groups)
        {
            this.nutrients = nutrients;
            // Add to the nutrientFieldObjects
            this.nutrientFieldObjects = new List<NutrientField>();
            foreach (KeyValuePair<int, NutrientField> kvp in this.nutrients)
            {
                this.nutrientFieldObjects.Add(kvp.Value);
            }
            this.volume = volume;
            this.boundary = new Boundary(BoundaryShapes.Cuboid, 80, 80, 80, 40);
            this.viscosity = 1;
            this.enableCellCollisions = false;
            this.groups = groups;
        }

        /// <summary>
        /// Another constructor which is used in cloning to speed up cloning nutrients
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="nutrients"></param>
        /// <param name="groups"></param>
        /// <param name="nutrientObjects"></param>
        public Environment(Vector3 volume, SerializableDictionary<int, NutrientField> nutrients, SerializableDictionary<int, Group> groups, List<NutrientField> nutrientObjects)
        {
            this.nutrients = nutrients;
            // Add to the nutrientFieldObjects
            this.nutrientFieldObjects = nutrientObjects;
            this.volume = volume;
            this.boundary = new Boundary(BoundaryShapes.Cuboid, 80, 80, 80, 40);
            this.viscosity = 1;
            this.enableCellCollisions = false;
            this.groups = groups;
        }

        /// <summary>
        /// Add a cell to the groups look up
        /// </summary>
        /// <param name="group">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <param name="cell">
        /// A <see cref="CellInstance"/>
        /// </param>
        public void AddCellToGroup(int group, CellInstance cell)
        {
            // If the group doesn't exists in the environment add it
            if (!this.groups.ContainsKey(group))
            {
                this.groups.Add(group, new Group(group));

            }
            // Add the cell to the group
            this.groups[group].Cells.Add(cell);
            // Set cell group ID to this cell
            cell.GroupID = group;

        }

        /// <summary>
        /// Gets all the cells from a particular group.
        /// </summary>
        /// <param name="group">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <returns>
        /// A <see cref="List`1"/>
        /// </returns>
        public List<CellInstance> CellsFromGroup(int group)
        {
            if (this.groups.ContainsKey(group))
            {
                return this.groups[group].Cells;
            }
            else
            {
                System.Console.WriteLine("Can't find group " + group + " in simulation");
                return new List<CellInstance>();
            }
        }

        /// <summary>
        /// Clones an environment (used by Simulator)
        /// </summary>
        /// <returns>
        /// A <see cref="Object"/>
        /// </returns>
        public Object Clone()
        {
            // clone the nutrients dictionary
            SerializableDictionary<int, NutrientField> clonedNutrients = new SerializableDictionary<int, NutrientField>();
            List<NutrientField> clonedNutrientObjects = new List<NutrientField>();
            NutrientField clonedField = null;

            foreach (KeyValuePair<int, NutrientField> kvp in this.nutrients)
            {
                clonedField = (NutrientField)kvp.Value.Clone();
                clonedNutrients.Add(kvp.Key, clonedField);
                clonedNutrientObjects.Add(clonedField);
            }

            // create the environment and return it
            Environment newInstance = new Environment(this.volume, clonedNutrients, this.groups, clonedNutrientObjects);
            newInstance.Boundary = this.boundary;
            newInstance.Viscosity = this.viscosity;
            newInstance.EnableCellCollisions = this.enableCellCollisions;

            return newInstance;
        }

        /// <summary>
        /// Test if the environment contains a certain group.
        /// </summary>
        /// <param name="id">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool ContainsGroup(int id)
        {
            return this.groups.ContainsKey(id);
        }

        public void AddGroup(int groupIndex)
        {
            // If the group doesn't exists in the environment add it
            if (!this.groups.ContainsKey(groupIndex))
            {
                this.groups.Add(groupIndex, new Group(groupIndex));

            }
        }



        /// <summary>
        /// Returns the index of an unused group
        /// </summary>
        /// <returns>
        /// A <see cref="System.Int32"/>
        /// </returns>
        public int GetUnusedGroupIndex()
        {
            int n = 0;

            while (ContainsGroup(n))
            {
                n++;
            }

            return n;
        }



        /// <summary>
        /// Given a group index, returns the Group object at that index. Returns null if there is no
        /// group at the given index.
        /// </summary>
        /// <returns>A <see cref="Group"/></returns>
        public Group GetGroupObject(int index)
        {
            if (this.ContainsGroup(index))
            {
                return this.groups[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Removes the group with specified index.
        /// </summary>
        /// <param name="index"></param>
        public void DeleteGroup(int index)
        {
            if (this.ContainsGroup(index))
            {
                this.groups.Remove(index);
            }
        }

        /// <summary>
        /// Clears all groups.
        /// </summary>
        public void ClearGroups()
        {
            this.groups.Clear();
        }


        /// <summary>
        /// Gets a list of the groups in the environment
        /// </summary>
        /// <returns>
        /// A <see cref="List`1"/>
        /// </returns>
        public List<int> GetGroups()
        {
            List<int> grps = new List<int>();
            foreach (KeyValuePair<int, Group> kvp in this.groups)
            {
                grps.Add(kvp.Key);
            }
            return grps;
        }


        /// <summary>
        /// Checks if the environment contains the nutrient with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsNutrient(int id)
        {
            return this.nutrients.ContainsKey(id);
        }

        /// <summary>
        /// Adds a nutrient field to the Environment
        /// </summary>
        /// <param name="nutrientIndex"></param>
        /// <param name="nutrient"></param>
        public void AddNutrient(int nutrientIndex, NutrientField nutrient)
        {
            if (!this.nutrients.ContainsKey(nutrientIndex))
            {
                this.nutrients.Add(nutrientIndex, nutrient);
                nutrient.Index = nutrientIndex;
                // add reference to optimised nutrient field list
                this.nutrientFieldObjects.Add(nutrient);
            }
        }

        /// <summary>
        /// Gets an unused nutrient index suitable for using to add
        /// a new nutrient to the environment
        /// </summary>
        /// <returns></returns>
        public int GetUnusedNutrientIndex()
        {
            int n = 0;

            while (ContainsNutrient(n))
            {
                n++;
            }

            return n;
        }

        /// <summary>
        /// Returns the NutrientField object with given ID
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public NutrientField GetNutrientFieldObject(int index)
        {

            if (index >= 0 && this.ContainsNutrient(index))
            {
                return this.nutrients[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a list of IDs of all the nutrients in the
        /// environment
        /// </summary>
        /// <returns></returns>
        public List<int> GetNutrients()
        {
            List<int> nutrients = new List<int>();
            foreach (KeyValuePair<int, NutrientField> kvp in this.nutrients)
            {
                nutrients.Add(kvp.Key);
            }
            return nutrients;
        }

        /// <summary>
        /// Returns a list of the nurient objects in the environment
        /// </summary>
        /// <returns></returns>
        public List<NutrientField> GetNutrientObjects()
        {
            return this.nutrientFieldObjects;
        }

        /// <summary>
        /// Attempts to find a nutrient field in the environment with the given name.
        /// If no such nutrient field is found, null is returned, otherwise a reference
        /// to the nutrient field is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NutrientField GetNutrientByName(String name)
        {
            foreach (NutrientField nut in nutrientFieldObjects)
            {
                if (nut.Name.Equals(name))
                {
                    return nut;
                }
            }

            return null;
        }


        /// <summary>
        /// Deletes the nutrient with given index from the environment
        /// </summary>
        /// <param name="index"></param>
        public void DeleteNutrient(int index)
        {
            if (this.ContainsNutrient(index))
            {
                NutrientField deleted = this.nutrients[index];
                this.nutrients.Remove(index);
                // remove from the list of fields
                this.nutrientFieldObjects.Remove(deleted);
            }
        }

        /// <summary>
        /// Clears all nutrients from the environment
        /// </summary>
        public void ClearNutrients()
        {
            this.nutrients.Clear();
            // clear optimised list
            this.nutrientFieldObjects.Clear();
        }

        /// <summary>
        /// Get the nutrient level at a point in space
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public double GetNutrientLevelAtPoint(int nutrientIndex, Vector3 position)
        {
            if (this.nutrients.ContainsKey(nutrientIndex))
            {
                // Evaluate the Nutrient function at the position
                return this.nutrients[nutrientIndex].getNutrientsAtPoint(position);
            }
            else
            {
                // It makes sense to not throw an exception if the
                // environment desn't know about the nutrient, but just
                // return 0, as there is none of that nutrient in the 
                // environment at all.
                return 0;
            }
        }

        public bool exptEquals(MuCell.Model.Environment other)
        {
            if (this.volume.exptEquals(other.volume) == false)
            {
                Console.Write("Environment objects not equal: ");
                Console.WriteLine("this.volume='" + this.volume + "'; other.volume='" + other.volume);
                return false;
            }
            return true;
        }

    }
}
