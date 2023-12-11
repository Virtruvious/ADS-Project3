using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListAdjacency
{
    internal class Node
    {
        public char Vertex;
        public LinkedList<Edge> Edges;
        public int est = 0;
        public int lft = Int32.MaxValue;
        public Node(char vertex)
        {
            Vertex = vertex;
            Edges = new();
        }
        public void AddEdge(Edge edge)
        {
            Edges.AddLast(edge);
        }
    }
}
