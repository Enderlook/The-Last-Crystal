using System;

namespace Additions.Utils.ColorCombiner
{
    public partial class ColorAveragerWithTimer
    {
        private readonly struct ColorExpiration : IEquatable<ColorExpiration>, IComparable<ColorExpiration>
        {
            public readonly ColorClass color;

            public readonly float expiration;

            public ColorExpiration(ColorClass color, float expiration)
            {
                this.color = color;
                this.expiration = expiration;
            }

            public bool Equals(ColorExpiration other) => expiration == other.expiration && color == other.color;

            public static bool operator ==(ColorExpiration left, ColorExpiration right) => left.Equals(right);

            public static bool operator !=(ColorExpiration left, ColorExpiration right) => !left.Equals(right);

            public override bool Equals(object other)
            {
                if (other is ColorExpiration cast)
                    return expiration == cast.expiration && color == cast.color;
                return false;
            }

            public override int GetHashCode() => HashCode.Combine(expiration, color);

            public int CompareTo(ColorExpiration other)
            {
                int value = ColorAveragerWithTimer.comparer.Compare(expiration, other.expiration);
                if (value != 0)
                    return value;
                return color.CompareTo(other.color);
            }

            public static bool operator >(ColorExpiration left, ColorExpiration right) => left.CompareTo(right) == 1;

            public static bool operator <(ColorExpiration left, ColorExpiration right) => left.CompareTo(right) == -1;

            public static bool operator >=(ColorExpiration left, ColorExpiration right) => left.CompareTo(right) >= 0;

            public static bool operator <=(ColorExpiration left, ColorExpiration right) => left.CompareTo(right) <= 0;

            public override string ToString() => $"(color:{color}, expiration:{expiration})";
        }
    }
}