using System;
using System.Collections.Generic;
using System.Text;
using MuCell.Model;
using System.Threading;
using MuCell.View;

namespace MuCell.Controller
{
    class GraphLayoutManager_SpringEmbedder : GraphLayoutManager
    {
        public GraphLayoutManager_SpringEmbedder()
        {

        }
        
        protected override MacroCommand resolveGraph(GraphNode[] nodes, GraphLink[] links, MuCell.View.IDrawingInterface drawable)
        {
            Vector2 topLeft = drawable.getScreenTopLeftInWorldCoordinates();
            Vector2 bottomRight = drawable.getScreenBottomRightInWorldCoordinates();

            float screenWidth = bottomRight.x - topLeft.x;
            float screenHeight = bottomRight.y - topLeft.y;

            float G = 60f;
            float sideForce = 30f;
            running = true;
            long iterationCount = 0;
            long renderTime = 1000;// (long)(1000.0 * (10.0 / nodes.Length));
            while (running)
            {
                for (int l = 0; l < links.Length; l++)
                {
                    GraphLink link = links[l];
                    link.step();
                }
                //push all nodes away from each other
                for (int a = 0; a < nodes.Length-1; a++)
                {
                    GraphNode nodeA = nodes[a];
                    for (int b = a + 1; b < nodes.Length; b++)
                    {
                        GraphNode nodeB = nodes[b];

                        Vector2 aToB = nodeB.getComponent().getPosition() - nodeA.getComponent().getPosition();

                        float distSqrd = aToB.getSqrdLength();
                        if (distSqrd < 1)
                        {
                            distSqrd = 1;
                        }
                        aToB.normalise();
                        Vector2 force = (aToB*G*(nodeA.getMass() * nodeB.getMass())) / distSqrd;

                        nodeA.push(-force);
                        nodeB.push(force);
                    }
                }

                
                //push all nodes in from the sides
                for (int a = 0; a < nodes.Length; a++)
                {
                    GraphNode nodeA = nodes[a];
                    Vector2 pos=nodeA.getComponent().getPosition();
                    float leftDistance = pos.x - topLeft.x;
                    if (leftDistance < 1f)
                    {
                        leftDistance = 1f;
                    }
                    float topDistance = pos.y - topLeft.y;
                    if (topDistance < 1f)
                    {
                        topDistance = 1f;
                    }
                    float rightDistance =bottomRight.x-pos.x;
                    if (rightDistance < 1f)
                    {
                        rightDistance = 1f;
                    }
                    float bottomDistance = bottomRight.y-pos.y;
                    if (bottomDistance < 1f)
                    {
                        bottomDistance = 1f;
                    }

                    Vector2 force = new Vector2((sideForce / leftDistance) - (sideForce / rightDistance), (sideForce / topDistance) - (sideForce / bottomDistance));
                    nodeA.push(force);
                }


                for (int n = 0; n < nodes.Length; n++)
                {
                    GraphNode node = nodes[n];
                    node.step();
                }
                iterationCount++;
                if (iterationCount % renderTime == 0)
                {
                    drawable.redrawGraphPanel();
                }
                
                //Thread.Sleep(100);
            }
            drawable.redrawGraphPanel();

            MacroCommand rearrangeCommand = new MacroCommand();
            for (int n = 0; n < nodes.Length; n++)
            {
                GraphNode node = nodes[n];
                MoveComponentCommand moveCommand = new MoveComponentCommand(node.getComponent(), node.getStartPosition(), node.getComponent().getPosition());
                rearrangeCommand.addCommand(moveCommand);
            }

            return rearrangeCommand;
        }
        
    }
}
