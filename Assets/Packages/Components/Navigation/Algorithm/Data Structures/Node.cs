using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Navigation
{
    [Serializable]
    public class Node : ScriptableObject
    {
        public Vector2 position;

        [SerializeField]
        private List<Connection> connections;
        /// <summary>
        /// All <see cref="Connection"/>s from this <see cref="Node"/> to other <see cref="Node"/>s.
        /// </summary>
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
        /// <summary>
        /// Whenever this <see cref="Node"/> is enabled or not.
        /// </summary>
        public bool IsActive { get => isActive; private set => isActive = value; }

        /// <summary>
        /// Set if this <see cref="Node"/> is active or not.
        /// </summary>
        /// <param name="actived">Whenever it's active or not.</param>
        public void SetActive(bool actived) => IsActive = actived;

        /// <summary>
        /// Whenever this node is the end of an island or not.
        /// </summary>
        public bool isExtreme = false;

        private static readonly InvalidOperationException CANNOT_CONNECT_TO_ITSELF = new InvalidOperationException($"A {nameof(Node)} can't connect with itself.");
        private static readonly InvalidOperationException ALREADY_END_TARGET = new InvalidOperationException($"A {nameof(Connection)} with the same {nameof(Connection.end)} has already been added.");

        /// <summary>
        /// Make a <see cref="Connection"/> from this <see cref="Node"/> to <paramref name="end"/> and store it.
        /// </summary>
        /// <param name="end"><see cref="Connection.end"/> = <paramref name="end"/>.</param>
        /// <param name="active">Whenever if the <see cref="Connection"/ is enabled or not</param>
        public void AddConnectionTo(Node end, bool active = true)
        {
            if (end == null)
                throw new ArgumentNullException(nameof(end));
            if (end == this)
                throw CANNOT_CONNECT_TO_ITSELF;
            if (Connections.Any(e => e.end == end))
                throw ALREADY_END_TARGET;

            Connections.Add(Connection.CreateConnection(this, end, active));
        }

        /// <summary>
        /// Make a <see cref="Connection"/> from <paramref name="end"/> to this <see cref="Node"/> and store it.
        /// </summary>
        /// <param name="end"><see cref="Connection.end"/> = <paramref name="end"/>.</param>
        /// <param name="active">Whenever if the <see cref="Connection"/ is enabled or not</param>
        public void AddConnectionFrom(Node start, bool active = true)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));
            if (start == this)
                throw CANNOT_CONNECT_TO_ITSELF;

            start.AddConnectionTo(this, active);
        }

        /// <summary>
        /// Add a <see cref="Connection"/> to this <see cref="Node"/>.<br>
        /// <paramref name="connection"/> <see cref="Connection.start"/> must be this <see cref="Node"/>, but <see cref="Connection.end"/> must not be this <see cref="Node"/>.
        /// </summary>
        /// <param name="connection"><see cref="Connection"/> to add.</param>
        public void AddConnection(Connection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (connection.start != this)
                throw new ArgumentNullException($"{nameof(connection.start)} must be this {nameof(Node)}.");
            if (connection.end != this)
                throw new ArgumentNullException($"{nameof(connection.start)} must be this {nameof(Node)}.");

            if (Connections.Any(e => e.end == connection.end))
                throw ALREADY_END_TARGET;

            Connections.Add(connection);
        }

        /// <summary>
        /// Get the <see cref="Connection"/> from this <see cref="Node"/> to <paramref name="end"/> <see cref="Node"/>.
        /// </summary>
        /// <param name="end">Target <see cref="Node"/>. <see cref="Connection.end"/> == <paramref name="end"/>.</param>
        /// <returns><see cref="Connection"/> from this <see cref="Node"/> to <paramref name="end"/> <see cref="Node"/></returns>
        public Connection GetConnectionTo(Node end)
        {
            if (end == null)
                throw new ArgumentNullException(nameof(end));
            if (end == this)
                throw CANNOT_CONNECT_TO_ITSELF;
            
            if (TryGetConnectionTo(end, out Connection connection))
                return connection;
            throw new KeyNotFoundException($"{nameof(Connection)} with {nameof(Connection.end)} in this {nameof(Node)} not found.");
        }

        /// <summary>
        /// Try get the <see cref="Connection"/> from this <see cref="Node"/> to <paramref name="end"/> <see cref="Node"/>.
        /// </summary>
        /// <param name="end">Target <see cref="Node"/>. <see cref="Connection.end"/> == <paramref name="end"/>.</param>
        /// <param name="connection"><see cref="Connection"/> from this <see cref="Node"/> to <paramref name="end"/> <see cref="Node"/>.</param>
        /// <returns>Whenever if the <see cref="Connection"/> was found or not.</returns>
        public bool TryGetConnectionTo(Node end, out Connection connection)
        {
            if (end == null)
                throw new ArgumentNullException(nameof(end));
            if (end == this)
                throw CANNOT_CONNECT_TO_ITSELF;

            foreach (Connection c in connections)
            {
                if (c.end == end)
                {
                    connection = c;
                    return true;
                }
            }

            connection = null;
            return false;
        }

        /// <summary>
        /// Create a new <see cref="Node"/>.
        /// </summary>
        /// <param name="position">Its position.</param>
        /// <param name="isActive">Whenever it's active or not.</param>
        /// <returns>New <see cref="Node"/>.</returns>
        public static Node CreateNode(Vector2 position, bool isActive)
        {
            Node node = CreateInstance<Node>();
            node.position = position;
            node.isActive = isActive;
            return node;
        }
    }
}