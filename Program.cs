using System.Reflection.Emit;
using System.Xml.Linq;

namespace LinkedListAdjacency
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedGraph graph = new();
            string input = "";

            //do
            //{
            //    Console.WriteLine("\nEnter a node letter: (to stop, say 'stop')");
            //    input = Console.ReadLine() ?? "";

            //    if (input == "stop") break;

            //    Node node = new(input[0]);
            //    Console.WriteLine("Now let's add some edges to Node {0}", node.Vertex);
            //    do
            //    {
            //        Console.WriteLine("Enter an edge letter: (to stop, say 'endnode')");
            //        input = Console.ReadLine() ?? "";

            //        if (input == "endnode") break;

            //        Console.WriteLine("Enter the weight of the edge: ");
            //        int weight = int.Parse(Console.ReadLine() ?? "0");
            //        node.AddEdge(new Edge(input[0], weight));
            //    } while (input != "endnode");

            //    graph.AddNode(node);
            //} while (input != "stop");

            Node nodeA = new('A');
            nodeA.AddEdge(new Edge('B', 6));
            nodeA.AddEdge(new Edge('C', 4));
            nodeA.AddEdge(new Edge('D', 5));
            graph.AddNode(nodeA);

            Node nodeB = new('B');
            nodeB.AddEdge(new Edge('E', 1));
            graph.AddNode(nodeB);

            Node nodeC = new('C');
            nodeC.AddEdge(new Edge('E', 1));
            graph.AddNode(nodeC);

            Node nodeD = new('D');
            nodeD.AddEdge(new Edge('F', 2));
            graph.AddNode(nodeD);

            Node nodeE = new('E');
            nodeE.AddEdge(new Edge('G', 9));
            nodeE.AddEdge(new Edge('H', 7));
            graph.AddNode(nodeE);

            Node nodeF = new('F');
            nodeF.AddEdge(new Edge('H', 4));
            graph.AddNode(nodeF);

            Node nodeG = new('G');
            nodeG.AddEdge(new Edge('I', 2));
            graph.AddNode(nodeG);

            Node nodeH = new('H');
            nodeH.AddEdge(new Edge('I', 4));
            graph.AddNode(nodeH);

            Node nodeI = new('I');
            graph.AddNode(nodeI);

            //Node nodeA = new('A');
            //nodeA.AddEdge(new Edge('B', 3));
            //nodeA.AddEdge(new Edge('C', 5));
            //nodeA.AddEdge(new Edge('E', 4));
            //graph.AddNode(nodeA);

            //Node nodeB = new('B');
            //nodeB.AddEdge(new Edge('D', 5));
            //nodeB.AddEdge(new Edge('I', 6));
            //graph.AddNode(nodeB);

            //Node nodeC = new('C');
            //nodeC.AddEdge(new Edge('F', 3));
            //graph.AddNode(nodeC);

            //Node nodeD = new('D');
            //nodeD.AddEdge(new Edge('I', 2));
            //graph.AddNode(nodeD);

            //Node nodeE = new('E');
            //nodeE.AddEdge(new Edge('G', 3));
            //graph.AddNode(nodeE);

            //Node nodeF = new('F');
            //nodeF.AddEdge(new Edge('H', 1));
            //graph.AddNode(nodeF);

            //Node nodeG = new('G');
            //nodeG.AddEdge(new Edge('H', 3));
            //nodeG.AddEdge(new Edge('J', 2));
            //graph.AddNode(nodeG);

            //Node nodeH = new('H');
            //nodeH.AddEdge(new Edge('K', 4));
            //graph.AddNode(nodeH);

            //Node nodeI = new('I');
            //nodeI.AddEdge(new Edge('J', 2));
            //graph.AddNode(nodeI);

            //Node nodeJ = new('J');
            //nodeJ.AddEdge(new Edge('K', 2));
            //nodeJ.AddEdge(new Edge('L', 8));
            //graph.AddNode(nodeJ);

            //Node nodeK = new('K');
            //nodeK.AddEdge(new Edge('L', 3));
            //graph.AddNode(nodeK);

            //Node nodeL = new('L');
            //graph.AddNode(nodeL);

            graph.PrintGraph();

            Console.WriteLine();

            //Analysis of Graph Critical Paths
            if (graph.Nodes.Count == 0)
            {
                Console.WriteLine("No nodes in graph.");
                return;
            }

            // Calculates the earliest start time for each node
            foreach (Node node in graph.Nodes)
            {
                foreach (Edge edge in node.Edges)
                {
                    int newEST = node.est + edge.Weight;
                    if (newEST > graph.getNode(edge.Vertex).est) // We need to keep the EST to highest possible to account for slack
                        graph.getNode(edge.Vertex).est = newEST; // Set the EST of the next node
                }
            }

            // Calculates the latest finish time for each node
            Dictionary<string, int> lft = new();
            for (int i = graph.Nodes.Count - 1; i >= 0; i--)
            {
                Node node = graph.Nodes.ElementAt(i);
                Node lastnode = graph.Nodes.Last();

                if (node == lastnode)
                {
                    node.lft = node.est;
                    continue;
                }

                foreach (Edge edge in node.Edges)
                {
                    int answer;
                    int lowestWeight = graph.getLowestEdgeWeight(node);
                    if (node.est == 0)
                        answer = 0;
                    else if (edge.Vertex == lastnode.Vertex)
                        answer = lastnode.lft - edge.Weight;
                    else
                    {
                        answer = graph.getNode(edge.Vertex).lft - edge.Weight;
                        Console.WriteLine(node.Vertex + "->" + edge.Vertex + " : " + graph.getNode(edge.Vertex).lft + "-" + lowestWeight + "=" + answer);
                    }

                    string direction = "" + node.Vertex + edge.Vertex;
                    lft.Add(direction, answer);
                    edge.lft = answer;
                    node.lft = answer;
                }
            }

            foreach (Node node in graph.Nodes)
            {
                foreach (Edge edge in node.Edges)
                {
                    int eft = node.est + edge.Weight;
                    int lst = edge.lft - edge.Weight;
                    string lookup = "" + node.Vertex + edge.Vertex;
                    int slack = lft[lookup] - node.est;
                    Console.WriteLine("{0} -> {1} est: {2} lst: {3} slack: {4}", node.Vertex, edge.Vertex, node.est,
                        lft[lookup], slack);
                }
            }

            Console.WriteLine("\nCritical Paths:");

            // Calculates the critical paths
            void traverseBranches(LinkedGraph graph, char nodeLetter, Dictionary<string, int> lft, string currentPath = "")
            {
                Node node = graph.getNode(nodeLetter);
                if (node == null) return;

                //Console.WriteLine("Traversing " + node.Vertex);
                // If the node has no edges, we have reached the end of the path
                if (node.Edges.Count == 0)
                {
                    Console.WriteLine(currentPath + nodeLetter);
                    return;
                }
                
                int duplicationCounter = 1;
                foreach (Edge edge in node.Edges)
                {
                    //Console.WriteLine("Checking " + node.Vertex + "->" + edge.Vertex);
                    string lookup = "" + node.Vertex + edge.Vertex;
                    int slack = lft[lookup] - node.est;
                    if (slack != 0) continue;

                    // Counter is used to prevent duplication of the current node in the path
                    if (duplicationCounter == 1)
                    {
                        currentPath += node.Vertex + ", ";
                        duplicationCounter = 0;
                    }

                    //Console.WriteLine("Adding " + node.Vertex);

                    traverseBranches(graph, edge.Vertex, lft, currentPath);
                }
            }

            traverseBranches(graph, graph.Nodes.First.Value.Vertex, lft);

            return;
            LinkedListNode<Node> processingNode= graph.Nodes.First;
            LinkedListNode<Edge> processingEdge = processingNode.Value.Edges.First;

            Console.WriteLine(processingEdge.Value.Vertex);

            

            Console.WriteLine();

            graph.PrintHighestLowestWeight();

        }
    }
}