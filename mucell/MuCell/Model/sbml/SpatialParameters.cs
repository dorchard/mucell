using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML
{
    public class SpatialParameters
    {
        [XmlElement("xPosition")]
        public XPosition x;
        [XmlElement("yPosition")]
        public YPosition y;

        public SpatialParameters()
        {
        }

        public SpatialParameters(float x, float y)
        {
            this.x = new XPosition(x);
            this.y = new YPosition(y);
        }
    }
}
