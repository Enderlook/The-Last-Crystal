using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Navigation
{
    [Serializable]
    public class Graph : IGraphReader, IGraphEditing
    {
        [SerializeField]
        private Transform reference;
        public Transform Reference {
            get => reference;
            set => reference = value;
        }

        [SerializeField]
        private List<Node> grid;
        /// <summary>
        /// All nodes of this graph.
        /// </summary>
        public List<Node> Grid {
            get => grid ?? (grid = new List<Node>());
            set => grid = value;
        }

        public void RemoveDuplicatedPositionsFromGrid() => Grid = Grid.Distinct().ToList();

        public Vector2 GetWorldPosition(Node node) => reference == null ? node.position : node.position + (Vector2)reference.position;

        public Vector2 GetLocalPosition(Vector2 position) => reference == null ? position : position - (Vector2)reference.position;

        public Node AddNode(Vector2 position, bool isActive = false, PositionReference mode = PositionReference.WORLD)
        {
            if (mode == PositionReference.WORLD)
                position -= (Vector2)reference.position;
            Node node = Node.CreateNode(position, isActive);
            Grid.Add(node);
            return node;
        }

        public void RemoveNodeAndConnections(Node node)
        {
            for (int i = Grid.Count - 1; i >= 0; i--)
            {
                Node n = Grid[i];
                if (n == node)
                    Grid.RemoveAt(i);
                n.TryRemoveConnectionTo(node);
            }
        }

        public void RemoveConnectionsToNothing()
        {
            // Generate a temporal hashset to reduce search complexity from O(n) to O(1)
            HashSet<Node> nodes = new HashSet<Node>(grid);

            foreach (Node node in nodes)
            {
                node.Connections = node.Connections.Where(e => nodes.Contains(e.end)).ToList();
            }
        }
    }
}