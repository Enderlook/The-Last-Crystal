using System;
using System.Collections.Generic;
using System.Linq;

namespace Navigation
{
    public static class DijkstraSearchAlgorithm
    {
        /* https://code.msdn.microsoft.com/windowsdesktop/Dijkstras-Single-Soruce-69faddb3
         * https://www.geeksforgeeks.org/csharp-program-for-dijkstras-shortest-path-algorithm-greedy-algo-7/
         * https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
         */

        public static Dictionary<Node, Connection> DijkstraSearch(this NavigationGraph navigation, Node source, Node target = null)
        {
            Dictionary<Node, Connection> previous = new Dictionary<Node, Connection>();

            if (!navigation.Grid.Contains(source))
                return previous;

            Dictionary<Node, float> distances = InitializeDistances(navigation, source);
            HashSet<Node> visited = new HashSet<Node>();
            PriorityQueue<Node> toVisit = new PriorityQueue<Node>();
            toVisit.Enqueue(source, 0);

            while (toVisit.Count > 0)
            {
                Node node = toVisit.DequeueMin();
                if (visited.Contains(node))
                    continue;
                visited.Add(node);

                float distanceFromSource = distances[node];
                foreach (Connection connection in node.connections)
                {
                    if (connection == null || !connection.IsActive)
                        continue;

                    Node neighbour = connection.end;
                    if (neighbour.IsActive)
                    {
                        float distance = Relax(distances, previous, connection, distanceFromSource);
                        toVisit.Enqueue(neighbour, distance);
                        if (neighbour == target)
                            return previous;
                    }
                }
            }

            return previous;
        }

        public static List<Connection> DijkstraSearchPath(this NavigationGraph navigation, Node source, Node target)
        {
            Dictionary<Node, Connection> previous = navigation.DijkstraSearch(source, target);
            return FromPreviousDictionaryToListPath(previous, target);
        }

        private static List<Connection> FromPreviousDictionaryToListPath(Dictionary<Node, Connection> previous, Node target)
        {
            LinkedList<Connection> path = new LinkedList<Connection>();
            if (previous.Count == 0)
                return path.ToList();
            Node node = target;
            while (previous.TryGetValue(node, out Connection connection))
            {
                path.AddFirst(connection);
                node = connection.start;
            }
            return path.ToList();
        }

        private static Dictionary<Node, float> InitializeDistances(NavigationGraph navigation, Node source)
        {
            Dictionary<Node, float> distances = new Dictionary<Node, float>();
            foreach (Node node in navigation.Grid)
            {
                distances.Add(node, float.MaxValue);
            }
            distances[source] = 0;
            return distances;
        }

        private static float Relax(Dictionary<Node, float> distances, Dictionary<Node, Connection> previous, Connection connection, float distanceFromSource)
        {
            float newDistance = connection.Distance + distanceFromSource;
            if (newDistance < distances[connection.end])
            {
                distances[connection.end] = newDistance;
                previous[connection.end] = connection;
            }
            return newDistance;
        }
        private static void Relax(Dictionary<Node, float> distances, Dictionary<Node, Connection> path, Connection connection) => Relax(distances, path, connection, distances[connection.end]);
    }
}