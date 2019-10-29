using System;
using UnityEngine;

namespace Navigation
{
    [Serializable]
    public class Connection : ScriptableObject
    {
        /// <summary>
        /// Starting node.
        /// </summary>
        public Node start;
        /// <summary>
        /// Ending node.
        /// </summary>
        public Node end;

        /// <summary>
        /// Distance between <see cref="start"/> and <see cref="end"/>.
        /// </summary>
        public float Distance => Vector2.Distance(start.position, end.position);

        [SerializeField]
        private bool isActive;
        /// <summary>
        /// Whenever it's active or not.
        /// </summary>
        public bool IsActive { get => isActive; private set => isActive = value; }

        /// <summary>
        /// Set if this <see cref="Connection"/> is active or not.
        /// </summary>
        /// <param name="actived">Whenever it's active or not.</param>
        public void SetActive(bool active) => IsActive = active;

        /// <summary>
        /// Whenever this <see cref="Connection"/> must be jumped.
        /// </summary>
        public bool IsExtreme => start.isExtreme && end.isExtreme;

        /// <summary>
        /// Create a new <see cref="Connection"/>.
        /// </summary>
        /// <param name="start">From <see cref="Node"/>.</param>
        /// <param name="end">To <see cref="Node"/>.</param>
        /// <param name="isActive">Whenever it's active or not.</param>
        /// <returns>New <see cref="Connection"/>.</returns>
        public static Connection CreateConnection(Node start, Node end, bool isActive)
        {
            Connection connection = CreateInstance<Connection>();
            connection.start = start;
            connection.end = end;
            connection.isActive = isActive;
            return connection;
        }

        /// <summary>
        /// Remove this <see cref="Connection"/> from <see cref="start"/>
        /// </summary>
        public void DisconnectFromNode() => start.RemoveConnection(this);
    }
}