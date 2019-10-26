using System;
using UnityEngine;

namespace Navigation
{
    [Serializable]
    public class Connection : ScriptableObject
    {
        public Node start;
        public Node end;
        public float Distance => Vector2.Distance(start.position, end.position);

        [SerializeField]
        private bool isActive;
        public bool IsActive { get => isActive; private set => isActive = value; }

        // Whenever this connection is a jumping connection
        public bool IsExtreme => start.isExtreme && end.isExtreme;

        public static Connection CreateConnection(Node start, Node end, bool isActive)
        {
            Connection connection = CreateInstance<Connection>();
            connection.start = start;
            connection.end = end;
            connection.isActive = isActive;
            return connection;
        }

        public void SetActive(bool active) => IsActive = active;
    }
}