using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Navigation
{
    [Serializable]
    public class Graph
    {
        /// <summary>
        /// How position of <see cref="Node"/>s is calculated.
        /// </summary>
        public enum PositionReference
        {
            /// <summary>
            /// Position is calculated by local coordinates from <see cref="reference"/>.
            /// </summary>
            LOCAL,
            /// <summary>
            /// Position is calculated without <see cref="reference"/>.
            /// </summary>
            WORLD
        }

        /// <summary>
        /// Reference point of all <see cref="Node"/>s positions.
        /// </summary>
        public Transform reference;

        [SerializeField]
        private List<Node> grid;
        /// <summary>
        /// All nodes of this graph.
        /// </summary>
        public List<Node> Grid {
            get => grid ?? (grid = new List<Node>());
            set => grid = value;
        }

        /// <summary>
        /// Remove all duplicated <see cref="Node"/>s in <see cref="Grid"/>;
        /// </summary>
        public void RemoveDuplicatedPositionsFromGrid() => Grid = Grid.Distinct().ToList();

        /// <summary>
        /// Get the world position of <paramref name="node"/>.
        /// </summary>
        /// <param name="node"><see cref="Node"/> to get world position.</param>
        /// <returns>World position of <paramref name="node"/>.</returns>
        public Vector2 GetWorldPosition(Node node) => reference == null ? node.position : node.position + (Vector2)reference.position;

        /// <summary>
        /// Get the local position of <paramref name="node"/> in respect to <see cref="reference"/>.
        /// </summary>
        /// <param name="node"><see cref="Node"/> to get local position.</param>
        /// <returns>Local position of <paramref name="node"/>.</returns>
        public Vector2 GetLocalPosition(Vector2 position) => reference == null ? position : position - (Vector2)reference.position;

        /// <summary>
        /// Add <see cref="Node"/>.
        /// </summary>
        /// <param name="position">It's position.</param>
        /// <param name="isActive">Whenever it's enabled or not.</param>
        /// <param name="mode">Whenever <paramref name="position"/> is applied globally or locally in respect to <see cref="reference"/>.</param>
        /// <returns>New <see cref="Node"/>.</returns>
        public Node AddNode(Vector2 position, bool isActive = false, PositionReference mode = PositionReference.WORLD)
        {
            if (mode == PositionReference.WORLD)
                position -= (Vector2)reference.position;
            Node node = Node.CreateNode(position, isActive);
            Grid.Add(node);
            return node;
        }

        /// <summary>
        /// Remove <paramref name="node"/> from <see cref="Grid"/> and all its <see cref="Connection"/>s from and to it.
        /// </summary>
        /// <param name="node"><see cref="Node"/> to remove.</param>
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
    }
}