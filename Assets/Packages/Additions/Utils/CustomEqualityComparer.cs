using System;
using System.Collections.Generic;

namespace Additions.Utils
{
    // https://stackoverflow.com/questions/716552/can-you-create-a-simple-equalitycomparert-using-a-lambda-expression

    public static class CustomEqualityComparer
    {
        public static IEqualityComparer<TSource> Create<TSource, TKey>(Func<TSource, TKey> getter) => new CustomComparer<TSource, TKey>(getter);

        public static IEqualityComparer<TSource> Create<TSource, TKey>(Func<TSource, TKey> getter, IEqualityComparer<TKey> comparer) => new CustomComparer<TSource, TKey>(getter, comparer);

        private class CustomComparer<TSource, TKey> : IEqualityComparer<TSource>
        {
            private readonly Func<TSource, TKey> getter;
            private readonly IEqualityComparer<TKey> comparer;

            public CustomComparer(Func<TSource, TKey> getter) : this(getter, null) { }

            public CustomComparer(Func<TSource, TKey> getter, IEqualityComparer<TKey> comparer)
            {
                if (getter is null) throw new ArgumentNullException(nameof(getter));
                this.comparer = comparer ?? EqualityComparer<TKey>.Default;
                this.getter = getter;
            }

            public bool Equals(TSource x, TSource y)
            {
                if (x == null && y == null)
                    return true;
                if (x == null || y == null)
                    return false;
                return comparer.Equals(getter(x), getter(y));
            }

            public int GetHashCode(TSource obj)
            {
                // Don't use is pattern because it won't work with Unity.Object
                if (obj == null) throw new ArgumentNullException(nameof(obj));
                return comparer.GetHashCode(getter(obj));
            }
        }
    }
}