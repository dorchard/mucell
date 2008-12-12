using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML
{
    public class SpatialParameters
    {
        [XmlElement]
        public float xPosition;
        [XmlElement]
        public float yPosition;

        public SpatialParameters()
        {
        }

        public SpatialParameters(float x, float y)
        {
            this.xPosition = x;
            this.yPosition = y;
        }
    }
}
