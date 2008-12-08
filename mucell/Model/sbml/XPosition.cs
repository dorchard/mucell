using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML
{
    public class XPosition
    {
        [XmlAttribute]
        public float value;

        public XPosition()
        {
        }

        public XPosition(float value)
        {
            this.value = value;
        }
    }
}
