// created on 24/03/2008 at 15:20

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace MuCell.Model.SBML
{

	public class ParentNode : MathNode
	{
		public List<MathNode> subtree;
	
		public ParentNode()
		{
			this.subtree = new List<MathNode>();
		}
	}
}