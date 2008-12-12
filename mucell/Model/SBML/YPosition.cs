using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML
{
    public class YPosition
    {
        [XmlAttribute]
        public float value;

        public YPosition()
        {
        }

        public YPosition(float value)
        {
            this.value = value;
        }
    }
}
