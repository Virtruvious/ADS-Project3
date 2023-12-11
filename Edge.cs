using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListAdjacency
{
    internal class Edge
    {
        public char Vertex;
        public int Weight;
        public int lft = 1;
        public Edge(char vertex, int weight)
        {
            Vertex = vertex;
            Weight = weight;
        }
    }
}
