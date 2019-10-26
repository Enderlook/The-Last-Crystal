using System;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    [Serializable]
    public class Node : ScriptableObject
    {
        public Vector2 position;

        [SerializeField]
        private List<Connection> connections;
        public List<Connection> Connections {
            get {
                if (connections == null)
                    connections = new List<Connection>();
                return connections;
            }
            set => connections = value;
        }

        [SerializeField]
        private bool isActive;
        public bool IsActive { get => isActive; private set => isActive = value; }

        // Whenever this node is the end of an island or not
        public bool isExtreme = false;

        public Node(Vector2 position, bool isActive = false)
        {
            this.position = position;
            IsActive = isActive;
        }

        public void SetActive(bool actived) => IsActive = actived;

        public void AddConnectionTo(Node end, bool active = true)
        {
            foreach (Connection connection in Connections)
            {
                if (connection.end == end)
                    return;
            }
            Connections.Add(Connection.CreateConnection(this, end, isActive));
        }

        public Connection GetConnectionTo(Node end)
        {
            foreach (Connection connection in connections)
            {
                if (connection.end == end)
                    return connection;
            }
            return null;
        }

        public static Node CreateNode(Vector2 position, bool isActive)
        {
            Node node = CreateInstance<Node>();
            node.position = position;
            node.isActive = isActive;
            return node;
        }
    }
}