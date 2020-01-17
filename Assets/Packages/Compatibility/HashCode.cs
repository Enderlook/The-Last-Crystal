namespace System
{
    /// <summary>
    /// Compatibility prior .Net Standard 2.1
    /// <see href="https://stackoverflow.com/a/263416/7655838"/>.
    /// <see href="https://stackoverflow.com/a/56539595/7655838"/>
    /// <see href="https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode"/>.
    /// <see href="https://github.com/dotnet/standard/issues/1093"/>
    /// </summary>
    public static class HashCode
    {
        private const int HASH = 17;
        private const int MULTIPLIER = 23;

        private static int Add<T1>(this int hash, T1 value)
        {
            unchecked
            {
                return (hash * MULTIPLIER) + value?.GetHashCode() ?? 0;
            }
        }

        public static int Combine<T1>(T1 value1) => HASH.Add(value1);

        public static int Combine<T1, T2>(T1 value1, T2 value2) => HASH.Add(value1).Add(value2);

        public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3) => HASH.Add(value1).Add(value2).Add(value3);

        public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4) => HASH.Add(value1).Add(value2).Add(value3).Add(value4);

        public static int Combine<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) => HASH.Add(value1).Add(value2).Add(value3).Add(value4).Add(value5);

        public static int Combine<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) => HASH.Add(value1).Add(value2).Add(value3).Add(value4).Add(value5).Add(value6);

        public static int Combine<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) => HASH.Add(value1).Add(value2).Add(value3).Add(value4).Add(value5).Add(value6).Add(value7);

        public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8) => HASH.Add(value1).Add(value2).Add(value3).Add(value4).Add(value5).Add(value6).Add(value7).Add(value8);
    }
}
