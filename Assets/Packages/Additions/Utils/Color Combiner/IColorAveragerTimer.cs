using System.Collections.Generic;

using UnityEngine;

namespace Additions.Utils.ColorCombiner
{
    /// <inheritdoc />
    public interface IColorAveragerTimer : IColorAverager, IUpdate
    {
        /// <summary>
        /// Add a <see cref="Color"/> to average.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to add.</param>
        /// <param name="duration">How much time will last this <paramref name="color"/>.</param>
        void Add(Color color, float duration);

        /// <summary>
        /// Remove a <see cref="Color"/> from average.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to remove.</param>
        /// <param name="expirationTime">Time when <paramref name="color"/> would expire. Use any negative to never expire.</param>
        /// <returns><see langword="true"/> if the <paramref name="color"/> was found and removed. <see langword="false"/> if the element was not found.</returns>
        bool Remove(Color color, float expirationTime);

        /*/// <summary>
        /// Remove the <see cref="Color"/> from average closer to expire.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to remove.</param>
        /// <returns><see langword="true"/> if the <paramref name="color"/> was found and removed. <see langword="false"/> if the element was not found.</returns>
        bool RemoveFirstToExpire(Color color);

        /// <summary>
        /// Remove the <see cref="Color"/> from average further to expire.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to remove.</param>
        /// <returns><see langword="true"/> if the <paramref name="color"/> was found and removed. <see langword="false"/> if the element was not found.</returns>
        bool RemoveLastToExpire(Color color);*/

        /// <summary>
        /// Get all the colors and their expiration time contained.
        /// </summary>
        /// <returns>Colors and their expiration time contained.</returns>
        IEnumerable<(Color color, float expirationTime)> GetColorsAndExpirationTime();
    }
}