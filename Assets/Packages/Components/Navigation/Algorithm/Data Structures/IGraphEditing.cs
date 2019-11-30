using UnityEngine;

namespace Navigation
{

    public interface IGraphEditing
    {
        /// <summary>
        /// Remove all duplicated <see cref="Node"/>s in <see cref="Grid"/>.
        /// </summary>
        void RemoveDuplicatedPositionsFromGrid();

        /// <summary>
        /// Add <see cref="Node"/>.
        /// </summary>
        /// <param name="position">It's position.</param>
        /// <param name="isActive">Whenever it's enabled or not.</param>
        /// <param name="mode">Whenever <paramref name="position"/> is applied globally or locally in respect to <see cref="reference"/>.</param>
        /// <returns>New <see cref="Node"/>.</returns>
        Node AddNode(Vector2 position, bool isActive = false, PositionReference mode = PositionReference.WORLD);

        /// <summary>
        /// Remove <paramref name="node"/> from <see cref="Grid"/> and all its <see cref="Connection"/>s from and to it.
        /// </summary>
        /// <param name="node"><see cref="Node"/> to remove.</param>
        void RemoveNodeAndConnections(Node node);

        /// <summary>
        /// Remove connections to missing nodes.
        /// </summary>
        void RemoveConnectionsToNothing();

        /// <summary>
        /// Add missing nodes from connections.
        /// </summary>
        void AddMissingNodesFromConnections();

        /// <summary>
        /// Remove nodes which doesn't have connection to any other node or no node is connected to them.
        /// </summary>
        void RemoveNodesWithoutToOrFromConnection();
    }
}