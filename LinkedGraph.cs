using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListAdjacency
{
    internal class LinkedGraph
    {
        public LinkedList<Node> Nodes;
        public LinkedGraph()
        {
            Nodes = new();
        }

        public void AddNode(Node node)
        {
            Nodes.AddLast(node);
        }
        public void PrintGraph()
        {
            foreach (Node node in Nodes)
            {
                Console.Write($"Node: {node.Vertex} - Edges: ");
                foreach (Edge edge in node.Edges)
                {
                    Console.Write($"{edge.Vertex} (Weight {edge.Weight} ), ");
                }
                Console.WriteLine();
            }
        }

        public void PrintHighestLowestWeight()
        {
            Node highestNode = new('Z');
            Edge highest = new Edge('Z', -1);
            Node lowestNode = new Node('Z');
            Edge lowest = new Edge('Z', 1000);

            foreach (Node node in Nodes)
            {
                foreach (Edge edge in node.Edges)
                {
                    if (edge.Weight > highest.Weight)
                    {
                        highestNode = node;
                        highest = edge;
                    }
                    else if (edge.Weight < lowest.Weight)
                    {
                        lowestNode = node;
                        lowest = edge;
                    }
                }
            }

            Console.WriteLine($"Highest Weight: Node {highestNode.Vertex} -> {highest.Vertex} (Weight {highest.Weight})");
            Console.WriteLine($"Lowest Weight: Node {lowestNode.Vertex} -> {lowest.Vertex} (Weight {lowest.Weight})");
        }
        public Node getNode(char vertex)
        {
            Node temp;
            foreach (Node node in Nodes)
            {
                if (node.Vertex == vertex)
                {
                    temp = node;
                    return temp;
                }
            }
            return null;
        }

        public int getLowestEdgeWeight(Node node)
        {
            int lowest = Int32.MaxValue;
            foreach (Edge edge in node.Edges)
            {
                if (edge.Weight < lowest)
                    lowest = edge.Weight;
            }
            return lowest;
        }
    }
}
