using System.Collections.Generic;

using UnityEngine;

namespace Additions.Utils.ColorCombiner
{
    public interface IColorAverager
    {
        /// <summary>
        /// Generate an average <see cref="Color"/> based on all the colors stored.
        /// </summary>
        /// <returns>Average <see cref="Color"/>.</returns>
        Color GetColor();

        /// <summary>
        /// Add a <see cref="Color"/> to average.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to add.</param>
        void Add(Color color);

        /// <summary>
        /// Remove a <see cref="Color"/> from average.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to remove.</param>
        /// <returns><see langword="true"/> if the <paramref name="color"/> was found and removed. <see langword="false"/> if the element was not found.</returns>
        bool Remove(Color color);

        /// <summary>
        /// Check if contains the given <see cref="Color"/>.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to check if contains.</param>
        /// <returns>Whenever it contains or not <paramref name="color"/>.</returns>
        bool Contains(Color color);

        /// <summary>
        /// Remove all colors.
        /// </summary>
        void Clear();

        /// <summary>
        /// Get the amount of times the color <paramref name="color"/> is contained.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to check the amount of time it's contained.</param>
        /// <returns>Amount of times <paramref name="color"/> is contained.</returns>
        int GetColorAmount(Color color);

        /// <summary>
        /// Get all the colors and their amounts contained.
        /// </summary>
        /// <returns>Colors and their amounts contained.</returns>
        IEnumerable<KeyValuePair<Color, int>> GetColorAndAmounts();

        /// <summary>
        /// Get all the colors. It can return repeated values if they are contained several times.
        /// </summary>
        /// <returns>Colors contained.</returns>
        IEnumerable<Color> GetColors();

        /// <summary>
        /// Get all the colors. It won't return repeated values even if the contained has them several times.
        /// </summary>
        /// <returns>Unique colors contained.</returns>
        IEnumerable<Color> GetUniqueColors();

        /// <summary>
        /// Whenever or not colors where added or removed from the container since last time <see cref="GetColor"/> was call.
        /// </summary>
        bool HasChanged { get; }

        /// <summary>
        /// Amount of colors stored.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Amount of unique colors stored.
        /// </summary>
        int UniqueColorsCount { get; }

        /// <summary>
        /// Default <see cref="Color"/> get from <see cref="GetColor"/> when there aren't any color stored.
        /// </summary>
        Color DefaultColor { get; set; }
    }
}