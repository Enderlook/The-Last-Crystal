using UnityEngine;

namespace Navigation
{
    public interface IGraphReader
    {
        /// <summary>
        /// Reference point of all <see cref="Node"/>s positions.
        /// </summary>
        Transform Reference { get; }

        /// <summary>
        /// Get the world position of <paramref name="node"/>.
        /// </summary>
        /// <param name="node"><see cref="Node"/> to get world position.</param>
        /// <returns>World position of <paramref name="node"/>.</returns>
        Vector2 GetWorldPosition(Node node);

        /// <summary>
        /// Get the local position of <paramref name="node"/> in respect to <see cref="reference"/>.
        /// </summary>
        /// <param name="position"><see cref="Vector2"/> to get world position.</param>
        /// <returns>Local position of <paramref name="node"/>.</returns>
        Vector2 GetLocalPosition(Vector2 position);
    }
}