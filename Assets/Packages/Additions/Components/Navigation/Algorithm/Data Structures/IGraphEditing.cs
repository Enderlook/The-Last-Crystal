using UnityEngine;

namespace Additions.Components.Navigation
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
        /// Remove <see cref="Connection"/>s to missing <see cref="Node"/>.
        /// </summary>
        void RemoveConnectionsToNothing();

        /// <summary>
        /// Add missing <see cref="Node"/>s from <see cref="Connection"/>s.
        /// </summary>
        void AddMissingNodesFromConnections();

        /// <summary>
        /// Remove <see cref="Node"/>s which doesn't have <see cref="Connection"/> to any other <see cref="Node"/> or no <see cref="Node"/> is connected to them.
        /// </summary>
        void RemoveNodesWithoutToOrFromConnection();

        /// <summary>
        /// Toggle all <see cref="Node"/>s according to <paramref name="mode"/>.
        /// </summary>
        /// <param name="mode"></param>
        void ToggleAllNodes(ToggleMode mode);

        /// <summary>
        /// Toggle all <see cref="Connection"/>s according to <paramref name="mode"/>.
        /// </summary>
        /// <param name="mode"></param>
        void ToggleAllConnections(ToggleMode mode);
    }
}