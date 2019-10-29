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
        /// Try get the <see cref="Connection"/> from this <see cref="Node"/> to <paramref name="end"/> <see cref="Node"/>.
        /// </summary>
        /// <param name="end">Target <see cref="Node"/>. <see cref="Connection.end"/> == <paramref name="end"/>.</param>
        /// <param name="connection"><see cref="Connection"/> from this <see cref="Node"/> to <paramref name="end"/> <see cref="Node"/>.</param>
        /// <param name="safe">On <see langword="true"/>, this method won't throw exception if <paramref name="end"/> and this <see cref="Node"/> are the same.<br>
        /// Though it throw raise exception for <see cref="ArgumentNullException"/> in <paramref name="end"/>.</param>
        /// <returns>Whenever if the <see cref="Connection"/> was found or not.</returns>
        public bool TryGetConnectionTo(Node end, out Connection connection, bool safe = false)
        {
            if (end == null)
                throw new ArgumentNullException(nameof(end));
            if (end == this && !safe)
                throw CANNOT_CONNECT_TO_ITSELF;

            foreach (Connection c in Connections)
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
        /// Try to remove a <see cref="Connection"/> from this <see cref="Node"/> to <paramref name="end"/>.
        /// </summary>
        /// <param name="end"><see cref="Connection.end"/> = <paramref name="end"/>.</param>
        /// <param name="safe">On <see langword="true"/>, this method won't throw exception if <paramref name="end"/> and this <see cref="Node"/> are the same.<br>
        /// Though it throw raise exception for <see cref="ArgumentNullException"/> in <paramref name="end"/>.</param>
        /// <returns>Whenever there was a <see cref="Connection"/> which was removed or nothing could be found.</returns>
        public bool TryRemoveConnectionTo(Node end, bool safe = false)
        {
            if (end == null)
                throw new ArgumentNullException(nameof(end));
            if (end == this && !safe)
                throw CANNOT_CONNECT_TO_ITSELF;

            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].end = end)
                {
                    Connections.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove a <see cref="Connection"/> from this <see cref="Node"/> to <paramref name="start"/>.
        /// </summary>
        /// <param name="start"><see cref="Connection.end"/> = <paramref name="start"/>.</param>
        public void RemoveConnectionTo(Node end)
        {
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            if (!TryRemoveConnectionTo(end))
                throw new KeyNotFoundException($"{nameof(Connection)} with {nameof(Connection.end)} {nameof(end)} not found.");
        }

        /// <summary>
        /// Try to remove a <see cref="Connection"/> from <paramref name="start"/> to this <see cref="Node"/> .
        /// </summary>
        /// <param name="start"><see cref="Connection.start"/> = <paramref name="start"/>.</param>
        /// <param name="safe">On <see langword="true"/>, this method won't throw exception if <paramref name="start"/> and this <see cref="Node"/> are the same.<br>
        /// Though it throw raise exception for <see cref="ArgumentNullException"/> in <paramref name="start"/>.</param> 
        /// <returns>Whenever there was a <see cref="Connection"/> which was removed or nothing could be found.</returns>
        public bool TryRemoveConnectionFrom(Node start, bool safe = false)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));
            if (start == this && !safe)
                throw CANNOT_CONNECT_TO_ITSELF;

            return start.TryRemoveConnectionTo(this, safe);
        }

        /// <summary>
        /// Remove a <see cref="Connection"/> from <paramref name="start"/> to this <see cref="Node"/>.
        /// </summary>
        /// <param name="start"><see cref="Connection.start"/> = <paramref name="start"/>.</param>
        public void RemoveConnectionFrom(Node start)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));

            start.TryRemoveConnectionFrom(this);
        }

        /// <summary>
        /// Try to remove <paramref name="connection"/> from <see cref="Connections"/>.
        /// </summary>
        /// <param name="connection"><see cref="Connection"/> to remove.</param>
        /// <returns>Whenever there was a <see cref="Connection"/> which was removed or nothing could be found.</returns>
        public bool TryRemoveConnection(Connection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i] == connection)
                {
                    Connections.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove a <see cref="Connection"/> from this <see cref="Node"/>.
        /// </summary>
        /// <param name="connection"><see cref="Connection"/> to remove.</param>
        public void RemoveConnection(Connection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (!TryRemoveConnection(connection))
                throw new KeyNotFoundException($"{nameof(Connection)} not found.");
        }

        /// <summary>
        /// Remove <see cref="Connection"/> <paramref name="other"/> from this, and this from <paramref name="other"/>.<br>
        /// Equivalent to:<br>
        /// <code><paramref name="other"/>.<see cref="TryRemoveConnectionTo(Node)"/>(this)
        /// this.<see cref="TryRemoveConnectionTo(Node)"/>(<paramref name="other"/>)</code>
        /// </summary>
        public void DisconnectNode(Node other)
        {
            other.TryRemoveConnectionTo(this);
            TryRemoveConnectionTo(other);
        }

        /// <summary>
        /// Create a new <see cref="Node"/>.
        /// </summary>
        /// <param name="position">Its position.</param>
        /// <param name="isActive">Whenever it's active or not.</param>
        /// <returns>New <see cref="Node"/>.</returns>
        public static Node CreateNode(Vector2 position, bool isActive = true)
        {
            Node node = CreateInstance<Node>();
            node.position = position;
            node.isActive = isActive;
            return node;
        }

        /// <summary>
        /// Create a new <see cref="Node"/>.
        /// </summary>
        /// <param name="position">Its position.</param>
        /// <param name="connectionsTo">The new <see cref="Node"/> will connect to all this <see cref="Node"/>s.</param>
        /// <param name="isActive">Whenever it's active or not.</param>
        /// <param name="areConnectionsActive">Whenever this <see cref="Connection"/>s are enabled or not.<br>
        /// Use <see langword="null"/> to use <paramref name="isActive"/> instead.</param>
        /// <returns>New <see cref="Node"/>.</returns>
        public static Node CreateNode(Vector2 position, IEnumerable<Node> connectionsTo, bool isActive = true, bool? areConnectionsActive = true)
        {
            Node node = CreateNode(position, isActive);
            bool connectionActive = areConnectionsActive ?? isActive;
            foreach (Node to in connectionsTo)
            {
                node.AddConnectionTo(to, connectionActive);
            }
            return node;
        }
    }
}