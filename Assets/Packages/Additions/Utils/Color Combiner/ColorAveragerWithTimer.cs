using Additions.Collections;
using Additions.Extensions;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Additions.Utils.ColorCombiner
{
    public partial class ColorAveragerWithTimer : IColorAveragerTimer, ICollection<Color>
    {
        private readonly SortedList<ColorExpiration> expirationOrder = new SortedList<ColorExpiration>();

        private readonly Dictionary<ColorClass, SortedList<float>> colors = new Dictionary<ColorClass, SortedList<float>>();

        private float time;

        /// <summary>
        /// The list is sorted from highest to lowest, except anything lower than 0 that are always send to the beginning of the list regardless they value.
        /// </summary>
        internal static readonly IComparer<float> comparer = Comparer<float>.Create((left, right) =>
        {
            int i = 1;
            if (left < 0 || right < 0) i = -i;
            if (left > right) return -i;
            if (left < right) return i;
            return 0;
        });

        /// <summary>
        /// Whenever colors has been modified. Used to cache <see cref="lastColor"/>.
        /// </summary>
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
        public void Add(Color color) => Add(color, -1);

        /// <inheritdoc />
        public void UpdateBehaviour(float deltaTime)
        {
            time += deltaTime;
            for (int i = expirationOrder.Count - 1; i >= 0; i--)
            {
                if (expirationOrder[i].expiration <= time)
                {
                    Count--;
                    ColorClass color = expirationOrder[i].color;
                    expirationOrder.RemoveAt(i);
                    SortedList<float> list = colors[color];
                    list.RemoveAt(list.Count - 1);
                    if (list.Count == 0)
                    {
                        colors.Remove(color);
                        UniqueColorsCount--;
                    }
                }
                else
                    return;
            }
        }

        /// <inheritdoc />
        public void Add(Color color, float duration)
        {
            float endTime = duration + time;
            HasChanged = true;
            Count++;
            ColorClass c = color;
            if (!colors.TryGetValue(c, out SortedList<float> list))
            {
                list = new SortedList<float>(comparer);
                colors.Add(c, list);
                UniqueColorsCount++;
            }

            if (duration >= 0)
            {
                list.Add(endTime);
                if (list.Count > 1) // 1 because we just added a new color.
                {
                    float oldTime = list[list.Count - 2]; // Get the time previous to the new color.
                    if (oldTime > endTime)
                    {
                        expirationOrder.Remove(new ColorExpiration(c, oldTime));
                        expirationOrder.Add(new ColorExpiration(c, endTime));
                    }
                }
                else
                    expirationOrder.Add(new ColorExpiration(c, endTime));
            }
            else
                list.Add(duration);
        }

        /// <inheritdoc />
        public bool Remove(Color color, float expirationTime)
        {
            ColorClass c = color;
            if (colors.TryGetValue(c, out SortedList<float> list))
            {
                float oldTime = list[list.Count - 1];
                if (list.Remove(expirationTime))
                {
                    Count--;
                    if (Mathf.Approximately(expirationTime, oldTime) && (oldTime > 0 && expirationTime > 0))
                    {
                        expirationOrder.Remove(new ColorExpiration(c, oldTime));
                        if (list.Count == 0)
                        {
                            colors.Remove(c);
                            UniqueColorsCount--;
                        }
                        else
                            expirationOrder.Add(new ColorExpiration(c, list[list.Count - 1]));
                    }
                    HasChanged = true;
                    return true;
                }
            }
            return false;
        }

        /// <inheritdoc />
        public IEnumerable<(Color color, float expirationTime)> GetColorsAndExpirationTime()
        {
            foreach ((Color color, SortedList<float> list) in colors)
                foreach (float value in list)
                    yield return (color, value);
        }

        /// <inheritdoc />
        void ICollection<Color>.Add(Color color) => Add(color);

        /// <inheritdoc />
        bool ICollection<Color>.IsReadOnly => false;

        /// <inheritdoc />
        public bool Remove(Color color) => Remove(color, -1);

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
                foreach ((Color color, SortedList<float> list) in colors)
                {
                    int amount = list.Count;
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
            if (colors.TryGetValue(color, out SortedList<float> list))
                return list.Count;
            return 0;
        }

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<Color, int>> GetColorAndAmounts() => colors.Select(e => new KeyValuePair<Color, int>(e.Key, e.Value.Count));

        /// <inheritdoc />
        public IEnumerable<Color> GetColors()
        {
            foreach ((Color color, SortedList<float> list) in colors)
                for (int i = 0; i < list.Count; i++)
                    yield return color;
        }

        /// <inheritdoc />
        public IEnumerable<Color> GetUniqueColors() => colors.Keys.Cast<Color>();

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