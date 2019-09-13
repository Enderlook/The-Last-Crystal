#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Navigation
{
    public class Node
    {
        public Vector2 position;
        public Connection[] connections;
        public bool IsActive { get; private set; }

        public Node(Vector2 position)
        {
            this.position = position;
            connections = new Connection[8];
        }

        public void SetActive(bool actived) => IsActive = actived;

#if UNITY_EDITOR
        private float nodeDrawSize = 0.05f;
        public void DrawNode(Color active, Color inactive) => DrawNode(IsActive ? active : inactive);
        public void DrawNode(Color color)
        {
            Handles.color = color;
            Handles.DrawSolidDisc(position, Vector3.forward, nodeDrawSize);
        }
        public void DrawConnections(Color active, Color inactive)
        {
            foreach (Connection connection in connections)
            {
                connection?.DrawConnection(active, inactive);
            }
        }
        public void DrawConnections(Color color)
        {
            foreach (Connection connection in connections)
            {
                connection?.DrawConnection(color);
            }
        }
#endif
    }
}