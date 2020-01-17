using System;

using UnityEngine;

namespace Additions.Utils.ColorCombiner
{
    public partial class ColorAveragerWithTimer
    {
        private class ColorClass : IEquatable<ColorClass>, IComparable<ColorClass>
        {
            public readonly float r, g, b, a;

            public ColorClass(Color color)
            {
                r = color.r;
                g = color.g;
                b = color.b;
                a = color.a;
            }

            public bool Equals(ColorClass other)
            {
                if (other == null)
                    return false;
                return r == other.r && g == other.g && b == other.b && a == other.a;
            }

            public static bool operator ==(ColorClass left, ColorClass right)
            {
                if (left is null)
                    return right is null;
                return left.Equals(right);
            }

            public static bool operator !=(ColorClass left, ColorClass right)
            {
                if (left is null)
                    return !(right is null);
                return !left.Equals(right);
            }

            public override bool Equals(object other)
            {
                if (other is ColorClass color)
                    return r == color.r && g == color.g && b == color.b && a == color.a;
                return false;
            }

            public override int GetHashCode() => HashCode.Combine(r, g, b, a);

            public int CompareTo(ColorClass other)
            {
                if (other == null) return 1;
                if (r > other.r) return 1;
                if (r < other.r) return -1;

                if (g > other.g) return 1;
                if (g < other.g) return -1;

                if (b > other.b) return 1;
                if (b < other.b) return -1;

                if (a > other.a) return 1;
                if (a < other.a) return -1;

                return 0;
            }

            public static bool operator >(ColorClass left, ColorClass right) => left.CompareTo(right) == 1;

            public static bool operator <(ColorClass left, ColorClass right) => left.CompareTo(right) == -1;

            public static bool operator >=(ColorClass left, ColorClass right) => left.CompareTo(right) >= 0;

            public static bool operator <=(ColorClass left, ColorClass right) => left.CompareTo(right) <= 0;

            public static implicit operator Color(ColorClass color) => new Color(color.r, color.g, color.b, color.a);

            public static implicit operator ColorClass(Color color) => new ColorClass(color);

            public override string ToString() => $"RGBA({r}, {g}, {b}, {a})";
        }
    }
}