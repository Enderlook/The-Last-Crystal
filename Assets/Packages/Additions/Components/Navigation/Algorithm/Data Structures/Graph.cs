using Additions.Attributes;
using Additions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Additions.Components.Navigation
{
    [Serializable]
    public class Graph : IGraphReader, IGraphEditing
    {
        [field: SerializeField, IsProperty]
        public Transform Reference { get; set; }

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

        public Vector2 GetWorldPosition(Node node) => Reference == null ? node.position : node.position + (Vector2)Reference.position;

        public Vector2 GetLocalPosition(Vector2 position) => Reference == null ? position : position - (Vector2)Reference.position;

        public Node AddNode(Vector2 position, bool isActive = false, PositionReference mode = PositionReference.WORLD)
        {
            if (mode == PositionReference.WORLD)
                position -= (Vector2)Reference.position;
            Node node = Node.CreateNode(position, isActive);
            Grid.Add(node);
            return node;
        }

        public void RemoveNodeAndConnections(Node node)
        {
            int toRemove = -1;
            for (int i = Grid.Count - 1; i >= 0; i--)
            {
                Node n = Grid[i];
                if (n == node)
                    toRemove = i;
                else
                    n.TryRemoveConnectionTo(node);
            }
            if (toRemove == -1)
                throw new KeyNotFoundException($"Not found {nameof(node)} in {nameof(Grid)}.");
            Grid.RemoveAt(toRemove);
        }

        public void RemoveConnectionsToNothing()
        {
            // Generate a temporal hashset to reduce search complexity from O(n) to O(1)
            HashSet<Node> nodes = new HashSet<Node>(grid);

            foreach (Node node in nodes)
                node.Connections = node.Connections.Where(e => nodes.Contains(e.end)).ToList();
        }

        public void AddMissingNodesFromConnections()
        {
            // Generate a temporal hashset to reduce search complexity from O(n) to O(1)
            HashSet<Node> nodes = new HashSet<Node>(grid);

            foreach (Node node in nodes)
                foreach (Connection connection in node.Connections)
                    if (!nodes.Contains(connection.end))
                    {
                        grid.Add(connection.end);
                        nodes.Add(connection.end);
                    }
        }

        public void RemoveNodesWithoutToOrFromConnection()
        {
            // Generate a temporal hashset to reduce search complexity from O(n) to O(1)
            HashSet<Node> nodes = new HashSet<Node>(grid);

            grid = grid.Where(e => e.Connections.Count > 0 || grid.Any(e2 => e2.Connections.Any(e3 => e3.end == e))).ToList();
        }

        public void ToggleAllNodes(ToggleMode mode)
        {
            foreach (Node node in Grid)
            {
                switch (mode)
                {
                    case ToggleMode.Enable:
                        node.SetActive(true);
                        break;
                    case ToggleMode.Disable:
                        node.SetActive(false);
                        break;
                    case ToggleMode.Toggle:
                        node.SetActive(!node.IsActive);
                        break;
                    default:
                        throw new ImpossibleStateException();
                }
            }
        }

        public void ToggleAllConnections(ToggleMode mode)
        {
            foreach (Node node in Grid)
            {
                foreach (Connection connection in node.Connections)
                {
                    switch (mode)
                    {
                        case ToggleMode.Enable:
                            connection.SetActive(true);
                            break;
                        case ToggleMode.Disable:
                            connection.SetActive(false);
                            break;
                        case ToggleMode.Toggle:
                            connection.SetActive(!node.IsActive);
                            break;
                        default:
                            throw new ImpossibleStateException();
                    }
                }
            }
        }
    }
}