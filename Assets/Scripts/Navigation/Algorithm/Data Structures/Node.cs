using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    [System.Serializable]
    public class Node
    {
        public Vector2 position;

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

        public void AddConnectionTo(Node end, bool active)
        {
            foreach (Connection connection in Connections)
            {
                if (connection.end == end)
                    return;
            }
            Connections.Add(new Connection(this, end, active));
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
    }
}