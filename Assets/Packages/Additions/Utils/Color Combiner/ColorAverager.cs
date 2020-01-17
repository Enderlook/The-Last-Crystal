using Additions.Extensions;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Additions.Utils.ColorCombiner
{
    public class ColorAverager : IColorAverager, IEnumerable<KeyValuePair<Color, int>>, ICollection<Color>
    {
        private readonly Dictionary<Color, int> colors = new Dictionary<Color, int>();

        /// <inheritdoc />
        public bool HasChanged { get; private set; }

        private Color lastColor;

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <inheritdoc />
        int ICollection<Color>.Count => Count;

        /// <inheritdoc />
        public int UniqueColorsCount { get; private set; }

        /// <inheritdoc />
        public Color DefaultColor { get; set; }

        /// <inheritdoc />
        public void Add(Color color)
        {
            HasChanged = true;
            if (colors.TryGetValue(color, out int value))
                colors[color] = value + 1;
            else
            {
                colors.Add(color, 1);
                UniqueColorsCount++;
            }

            Count++;
        }

        /// <inheritdoc />
        void ICollection<Color>.Add(Color color) => Add(color);

        /// <inheritdoc />
        bool ICollection<Color>.IsReadOnly => false;

        /// <inheritdoc />
        public bool Remove(Color color)
        {
            if (colors.TryGetValue(color, out int value))
            {
                Count--;
                value--;
                if (value > 0)
                    colors[color] = value;
                else
                {
                    colors.Remove(color);
                    UniqueColorsCount--;
                }
                HasChanged = true;
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        bool ICollection<Color>.Remove(Color color) => Remove(color);

        /// <inheritdoc />
        public Color GetColor()
        {
            // We can't put this inside the if statement because it will return black instead of DefaultColor if GetColor was called before adding a color.
            if (Count == 0)
                return DefaultColor;
            // Do some cache
            if (HasChanged)
            {
                HasChanged = false;
                float r = 0;
                float g = 0;
                float b = 0;
                float a = 0;
                foreach ((Color color, int amount) in colors)
                {
                    r += color.r * amount;
                    g += color.g * amount;
                    b += color.b * amount;
                    a += color.a * amount;
                }
                lastColor = new Color(r / Count, g / Count, b / Count, a / Count);
            }
            return lastColor;
        }

        /// <inheritdoc />
        public bool Contains(Color color) => colors.ContainsKey(color);

        /// <inheritdoc />
        bool ICollection<Color>.Contains(Color color) => Contains(color);

        /// <inheritdoc />
        public int GetColorAmount(Color color)
        {
            if (colors.TryGetValue(color, out int amount))
                return amount;
            return 0;
        }

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<Color, int>> GetColorAndAmounts() => colors;

        /// <inheritdoc />
        public IEnumerable<Color> GetColors()
        {
            foreach ((Color color, int amount) in colors)
                for (int i = 0; i < amount; i++)
                    yield return color;
        }

        /// <inheritdoc />
        public IEnumerable<Color> GetUniqueColors() => colors.Keys;

        IEnumerator<KeyValuePair<Color, int>> IEnumerable<KeyValuePair<Color, int>>.GetEnumerator() => colors.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetColors().GetEnumerator();

        public IEnumerator<Color> GetEnumerator() => GetColors().GetEnumerator();

        /// <inheritdoc />
        public void Clear()
        {
            HasChanged = true;
            colors.Clear();
        }

        /// <inheritdoc />
        void ICollection<Color>.Clear() => Clear();

        /// <inheritdoc />
        public void CopyTo(Color[] array, int arrayIndex)
        {
            foreach (Color color in this)
            {
                array[arrayIndex] = color;
                arrayIndex++;
            }
        }
    }
}