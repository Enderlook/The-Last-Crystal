using System;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    [Serializable]
    public class Connection : ISerializationCallbackReceiver
    {
        [NonSerialized]
        public Node start;
        [SerializeField]
        private Vector2 startPosition;
        [NonSerialized]
        public Node end;
        [SerializeField]
        private Vector2 endPosition;
        public float Distance => Vector2.Distance(start.position, end.position);

        [SerializeField]
        private bool isActive;
        public bool IsActive { get => isActive; private set => isActive = value; }

        // Whenever this connection is a jumping connection
        public bool IsExtreme => start.isExtreme && end.isExtreme;

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
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            startPosition = start.position;
            endPosition = end.position;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() { }

        public void Deserialize(Dictionary<Vector2, Node> nodesByPosition)
        {
            start = nodesByPosition[startPosition];
            end = nodesByPosition[endPosition];
        }
    }
}