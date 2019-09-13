using Navigation;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace Navigation
{
    public class Connection
    {
        public Node start;
        public Node end;
        public float Distance => Vector2.Distance(start.position, end.position);
        public bool IsActive { get; private set; }

        public Connection(Node start, Node end)
        {
            this.start = start;
            this.end = end;
        }

        public void SetActive(bool active) => IsActive = active;

#if UNITY_EDITOR
        public void DrawConnection(Color active, Color inactive) => DrawConnection(IsActive ? active : inactive);
        public void DrawConnection(Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(start.position, end.position);
        }
#endif
    }
}