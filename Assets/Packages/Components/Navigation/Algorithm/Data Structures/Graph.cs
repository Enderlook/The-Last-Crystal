using System;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    [Serializable]
    public class Graph
    {
        public enum PositionReference { LOCAL, WORLD }

        public Transform reference;

        [SerializeField]
        private List<Node> grid;
        public List<Node> Grid {
            get => grid ?? (grid = new List<Node>());
            set => grid = value;
        }

        public void RemoveDuplicatedPositionsFromGrid()
        {
            HashSet<Vector2> usedPositions = new HashSet<Vector2>();
            for (int i = Grid.Count - 1; i >= 0; i--)
            {
                if (usedPositions.Contains(Grid[i].position))
                    Grid.RemoveAt(i);
                else
                    usedPositions.Add(Grid[i].position);
            }
        }

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
    }
}