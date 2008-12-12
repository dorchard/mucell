using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace MuCell.Model.SBML.ExtracellularComponents
{
    public enum ComponentLinkType
    {
        Input, Output
    }
    /// <summary>
    /// A storage class used in the editor when creating links (they get displayed as the labelled "hardpoints" that you drag links to)
    /// </summary>
    public class ComponentLink : IModelComponent
    {
        private float xOffset;
        private float yOffset;
        private ComponentBase parent;
        private ComponentLinkType linkType;
        private int linkNumber;

        public ComponentLink(ComponentBase parent, ComponentLinkType type, int linkNumber, float xOffset, float yOffset)
        {
            this.parent = parent;
            this.linkType = type;
            this.linkNumber = linkNumber;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }
        public ComponentBase getParent()
        {
            return parent;
        }
        public ComponentLinkType getLinkType()
        {
            return linkType;
        }
        public int getLinkNumber()
        {
            return linkNumber;
        }

        #region IModelComponent Members

        public void setPosition(float x, float y)
        {
        }

        public Vector2 getPosition()
        {
            Vector2 pos = parent.getPosition();
            return new Vector2(xOffset + pos.x, yOffset + pos.y);
        }

        public float getWidth()
        {
            return 8f;
        }

        public float getHeight()
        {
            return 8f;
        }

        public Vector2 getClosestPoint(Vector2 otherPosition)
        {
            //find angle to other point
            Vector2 pos = getPosition();
            double angleTo = Math.Atan2(otherPosition.y - pos.y, otherPosition.x - pos.x);

            //then project along that angle of length radius

            return new Vector2(((float)Math.Cos(angleTo) * getWidth() * 0.5f) + pos.x, ((float)Math.Sin(angleTo) * getHeight() * 0.5f) + pos.y);
        }

        #endregion
    }
    public class ComponentBase : SBase, IModelComponent
    {
        

        protected SpeciesReference[] reactants;

        [BrowsableAttribute(false)]
        public SpeciesReference[] Reactants{get{return reactants;}}

        protected SpeciesReference[] products;

        [BrowsableAttribute(false)]
        public SpeciesReference[] Products { get { return products; } }

        private String componentType;
        public String ComponentType { get { return componentType; } }

        private float xPosition;
        private float yPosition;

        //don't serialise these
        private ComponentLink[] linkComponents=null;


        public virtual void DoTimeStep(CellInstance cell,ComponentWorldStateBase compData, StateSnapshot state, double time, double timeStep)
        {
            
        }

        public virtual void InitializeInEnvironment(CellInstance cell, ComponentWorldStateBase compData, StateSnapshot state)
        {

        }
        


        /// <summary>
        /// Returns a list of all the nutrient field names required to
        /// exist in the environment for this component
        /// </summary>
        /// <returns></returns>
        public virtual String[] GetRequiredNutrientFieldNames()
        {
            return new String[0];
        }

        /// <summary>
        /// Links the nutrient fields in the given environment to 
        /// this component
        /// </summary>
        /// <param name="env"></param>
        public virtual void LinkToNutrientFields(Environment env)
        {

        }

        /// <summary>
        /// Returns an object assosiated with this component containing world
        /// state information (specific to an individual cell)
        /// </summary>
        /// <returns></returns>
        public virtual ComponentWorldStateBase CreateWorldStateObject()
        {
           // return new ComponentWorldStateBase(this);
            return null;
        }



        public static String[] ComponentFactoryTypes()
        {
            return new String[]{"Flagella","Receptor"};
        }
        public virtual String[] getReactantNames()
        {
            return new String[] { "Input A" };
        }
        public virtual String[] getProductNames()
        {
            return new String[] { "Output A" };
        }
        public static ComponentBase ComponentFactory(string componentType)
        {
           switch(componentType)
           {
               case("Flagella"):
                   {
                       FlagellaComponent component= new FlagellaComponent();
                       component.componentType = componentType;
                       return component;
                   }
               case("Receptor"):
                   {
                       ReceptorComponent component = new ReceptorComponent();
                       component.componentType = componentType;
                       return component;
                   }
           }

            return null;
        }
        public ComponentLink getLinkPoint(int linkNumber, ComponentLinkType linkType)
        {
            if (linkComponents == null)
            {
                generateLinkComponents();
            }
            int address=linkNumber;
            if (linkType == ComponentLinkType.Output)
            {
                address += reactants.Length;
            }
            ComponentLink link=linkComponents[address];
            return link;
        }
        public SpeciesReference getSpeciesReference(int linkNumber, ComponentLinkType linkType)
        {
            switch (linkType)
            {
                case ComponentLinkType.Input:
                    return reactants[linkNumber];
                case ComponentLinkType.Output:
                    return products[linkNumber];
            }
            return null;
        }
        public void setSpeciesReference(int linkNumber, ComponentLinkType linkType, SpeciesReference reference)
        {
            switch (linkType)
            {
                case ComponentLinkType.Input:
                    reactants[linkNumber] = reference;
                    break;
                case ComponentLinkType.Output:
                    products[linkNumber] = reference;
                    break;
            }
        }
        private void generateLinkComponents()
        {
            linkComponents = new ComponentLink[reactants.Length + products.Length];
            for (int i = 0; i < reactants.Length; i++)
            {
                Vector2 linkPos = getLinkPointOffset(i, ComponentLinkType.Input);
                linkComponents[i]=new ComponentLink(this, ComponentLinkType.Input, i,linkPos.x,linkPos.y);
            }
            for (int i = 0; i < products.Length; i++)
            {
                Vector2 linkPos = getLinkPointOffset(i, ComponentLinkType.Output);
                linkComponents[i+reactants.Length] = new ComponentLink(this, ComponentLinkType.Output, i,linkPos.x,linkPos.y);
            }
        }
        public Vector2 getLinkPointOffset(int linkNumber, ComponentLinkType linkType)
        {
            Vector2 offset = new Vector2();

            float angle = (float)Math.PI * -0.5f;
            float angleSpacePerLink;
            if (linkType == ComponentLinkType.Input)
            {
                //the angle given to each link around the circle
                angleSpacePerLink = -(float)Math.PI / (reactants.Length+1f);
            }
            else
            {
                angleSpacePerLink = (float)Math.PI / (products.Length+1f);
            }
            angle += angleSpacePerLink * (1+linkNumber);

            offset.x = (float)Math.Cos(angle) * ((this.getWidth()*0.5f)+getLinkPointRadius(linkNumber,linkType)*0.5f);
            offset.y = (float)Math.Sin(angle) * ((this.getHeight() * 0.5f) + getLinkPointRadius(linkNumber, linkType)*0.5f);

            return offset;
        }
        public float getLinkPointRadius(int linkNumber, ComponentLinkType linkType)
        {
            return 4f;
        }
        public String getLinkPointName(int linkNumber, ComponentLinkType linkType)
        {
            switch (linkType)
            {
                case ComponentLinkType.Input:
                    return getReactantNames()[linkNumber];
                case ComponentLinkType.Output:
                    return getProductNames()[linkNumber];
            }
            return "Unknown";
        }

        #region IModelComponent Members

        public void setPosition(float x, float y)
        {
            xPosition = x;
            yPosition = y;
        }

        public Vector2 getPosition()
        {
            return new Vector2(xPosition, yPosition);
        }

        public float getWidth()
        {
            return 32f;
        }

        public float getHeight()
        {
            return 32f;
        }

        public Vector2 getClosestPoint(Vector2 otherPosition)
        {
            //find angle to other point

            double angleTo = Math.Atan2(otherPosition.y - yPosition, otherPosition.x - xPosition);

            //then project along that angle of length radius

            return new Vector2(((float)Math.Cos(angleTo) * getWidth() * 0.5f) + xPosition, ((float)Math.Sin(angleTo) * getHeight() * 0.5f) + yPosition);
        }

        #endregion
    }
}
