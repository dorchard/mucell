using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model.SBML;
using MuCell.Model;
using MuCell.View;
using MuCell.Model.SBML.ExtracellularComponents;

namespace MuCell.Controller
{
    class GraphNode
    {
        private IModelComponent component;
        private Vector2 startPosition;
        private Vector2 nextPosition;
        private float mass;
        public GraphNode(IModelComponent component, Vector2 startPosition)
        {
            this.component = component;
            this.startPosition = startPosition;
            nextPosition = startPosition;
            mass = component.getWidth();
        }
        public Vector2 getStartPosition()
        {
            return startPosition;
        }
        public float getMass()
        {
            return mass;
        }
        public IModelComponent getComponent()
        {
            return component;
        }
        public void push(Vector2 force)
        {
            nextPosition += force / mass;
        }
        public void step()
        {
            component.setPosition(nextPosition.x, nextPosition.y);
        }
    }
    class GraphLink
    {
        private GraphNode nodeA;
        private GraphNode nodeB;
        private float linkLength;
        private float linkStrength;

        public GraphLink(GraphNode nodeA, GraphNode nodeB, int modelDegree)
        {
            this.nodeA = nodeA;
            this.nodeB = nodeB;
            linkLength = 32f;
            linkStrength = 0.03f;
        }
        public void step()
        {
            //apply force to the nodes based on their distance
            Vector2 aToB = nodeB.getComponent().getPosition() - nodeA.getComponent().getPosition();
            float distance = (float)aToB.getLength();
            float normalisedDifference=(distance-linkLength)/linkLength;

            Vector2 force = aToB * normalisedDifference*linkStrength;

            nodeA.push(force);
            nodeB.push(-force);
        }
    }
    class GraphLayoutManager
    {
        protected bool running=false;

        private GraphNode findNode(IModelComponent component, List<GraphNode> nodes)
        {
            foreach (GraphNode n in nodes)
            {
                if (n.getComponent() == component)
                {
                    return n;
                }
            }
            return null;
        }
        public MacroCommand rearrangeGraphFromModel(Model.SBML.Model model, IDrawingInterface drawable)
        {
            int modelDegree = model.listOfSpecies.Count;

            //convert the model to nodes and links

            List<GraphNode> nodes = new List<GraphNode>();
            List<GraphLink> links = new List<GraphLink>();

            foreach (Species n in model.listOfSpecies)
            {
                nodes.Add(new GraphNode(n, n.getPosition()));
            }
            foreach (Reaction n in model.listOfReactions)
            {
                nodes.Add(new GraphNode(n, n.getPosition()));
            }
            foreach (ComponentBase n in model.listOfComponents)
            {
                nodes.Add(new GraphNode(n, n.getPosition()));
            }

            foreach (Reaction r in model.listOfReactions)
            {
                GraphNode hubNode=findNode(r,nodes);
                if (hubNode != null)
                {
                    foreach (SimpleSpeciesReference s in r.Reactants)
                    {
                        GraphNode sNode = findNode(s.species, nodes);
                        if (sNode != null)
                        {
                            links.Add(new GraphLink(sNode, hubNode, modelDegree));
                        }
                    }
                    foreach (SimpleSpeciesReference s in r.Products)
                    {
                        GraphNode sNode = findNode(s.species, nodes);
                        if (sNode != null)
                        {
                            links.Add(new GraphLink(sNode, hubNode, modelDegree));
                        }
                    }
                    foreach (SimpleSpeciesReference s in r.Modifiers)
                    {
                        GraphNode sNode = findNode(s.species, nodes);
                        if (sNode != null)
                        {
                            links.Add(new GraphLink(sNode, hubNode, modelDegree));
                        }
                    }
                }
            }
            foreach (ComponentBase c in model.listOfComponents)
            {
                GraphNode hubNode = findNode(c, nodes);

                if (hubNode != null)
                {
                    foreach (SimpleSpeciesReference s in c.Reactants)
                    {
                        if (s != null)
                        {
                            GraphNode sNode = findNode(s.species, nodes);
                            if (sNode != null)
                            {
                                links.Add(new GraphLink(sNode, hubNode, modelDegree));
                            }
                        }
                    }
                    foreach (SimpleSpeciesReference s in c.Products)
                    {
                        if (s != null)
                        {
                            GraphNode sNode = findNode(s.species, nodes);
                            if (sNode != null)
                            {
                                links.Add(new GraphLink(sNode, hubNode, modelDegree));
                            }
                        }
                    }
                }
            }


            GraphNode[] graphNodes = nodes.ToArray();
            GraphLink[] graphLinks = links.ToArray();

            return resolveGraph(graphNodes, graphLinks, drawable);
        }

        protected virtual MacroCommand resolveGraph(GraphNode[] nodes, GraphLink[] links, IDrawingInterface drawable)
        {
            //try and rearrange the graph by setting node positions


            //if want to, call drawable update to refresh
            return null;
        }
        public bool isRunning()
        {
            return running;
        }
        public void stopRunning()
        {
            running = false;
        }
    }
}
