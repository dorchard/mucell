using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model.SBML
{
    [XmlRoot(ElementName="sbml",Namespace="http://www.sbml.org/sbml/level2")]
    public class SBMLroot
    {
        private Model model;
        [XmlAttribute]
        public int level;
        [XmlAttribute]
        public int version;

        [XmlElement("model")]
        public Model Model
        {
            get { return model; }
            set { }
        }

        public SBMLroot()
        {
        }

        public SBMLroot(Model model)
        {
            this.model = model;
            this.level = model.level;
            this.version = model.version;
        }

    }
}
