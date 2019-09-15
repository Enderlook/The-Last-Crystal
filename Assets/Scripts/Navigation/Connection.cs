using UnityEngine;

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
        public Connection(Node start, Node end, bool isActive)
        {
            this.start = start;
            this.end = end;
            IsActive = isActive;
        }

        public void SetActive(bool active) => IsActive = active;
    }
}