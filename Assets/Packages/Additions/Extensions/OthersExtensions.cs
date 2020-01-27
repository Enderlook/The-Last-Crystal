using System.Collections.Generic;

namespace Additions.Extensions
{
    public static class OthersExtensions
    {
        /// <summary>
        /// Deconstruction of <seealso cref="KeyValuePair{TKey, TValue}"/>.
        /// </summary>
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
        // https://stackoverflow.com/questions/42549491/deconstruction-in-foreach-over-dictionary
        {
            // TODO: Remove when Unity implement .Net Standard 2.1
            key = tuple.Key;
            value = tuple.Value;
        }
    }
}