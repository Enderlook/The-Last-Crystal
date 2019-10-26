using System;
using System.Collections.Generic;
using System.Linq;
using Serializables;
using UnityEngine;

namespace Navigation
{
    [Serializable]
    internal class Connections : SerializableList<Connection>
    {
        public Connections(int capacity) : base(capacity) { }

        public Connections(List<Connection> list) : base(list) { }
    }

    [Serializable]
    internal class NodeConnections : SerializableDictionary<Vector2, Connections> { }

    [Serializable]
    public class Graph : ISerializationCallbackReceiver
    {
        public enum PositionReference { LOCAL, WORLD }

        public Transform reference;

        [SerializeField]
        private List<Node> grid;
        public List<Node> Grid {
            get => grid ?? (grid = new List<Node>());
            set => grid = value;
        }

        [SerializeField]
        private NodeConnections connections;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            RemoveDuplicatedPositionsFromGrid();
            connections = new NodeConnections();
            foreach (Node node in Grid)
            {
                connections.Add(node.position, new Connections(node.Connections));
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Dictionary<Vector2, Node> nodesByPosition = grid.ToDictionary(e => e.position);

            foreach (Node node in Grid)
            {
                if (connections.TryGetValue(node.position, out Connections nodeConnections))
                {
                    node.Connections = nodeConnections.GetList();
                    foreach (Connection connection in node.Connections)
                    {
                        connection.Deserialize(nodesByPosition);
                    }
                }
            }
            connections = null;
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
            Node node = new Node(position, isActive);
            Grid.Add(node);
            return node;
        }
    }
}