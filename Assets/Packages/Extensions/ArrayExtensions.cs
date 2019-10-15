using System;

namespace AdditionalExtensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Performs the specified <paramref name="action"/> on each element of the <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">Type of the element inside <paramref name="source"/>.</typeparam>
        /// <param name="source">Source to look for element to perform the <paramref name="action"/></param>
        /// <param name="action">Action to perform on each element of <paramref name="source"/></param>
        /// <seealso cref="Array.ForEach{T}(T[], Action{T})"/>
        public static void ForEach<T>(this T[] source, Action<T> action) => Array.ForEach(source, action);

        /// <summary>
        /// Performs the specified <paramref name="action"/> on each element of the <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">Type of the element inside <paramref name="source"/>.</typeparam>
        /// <param name="source">Source to look for element to perform the <paramref name="action"/></param>
        /// <param name="action">Action to perform on each element of <paramref name="source"/></param>
        /// <returns>Updated <paramref name="source"/>.</returns>
        /// <seealso cref="Array.ForEach{T}(T[], Action{T})"/>
        public static T[] ChangeEach<T>(this T[] source, Func<T, T> function)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (function is null) throw new ArgumentNullException(nameof(function));
            if (source.Length == 0)
                return Array.Empty<T>();

            for (int i = 0; i < source.Length; i++)
            {
                source[i] = function(source[i]);
            }

            return source;
        }
    }
}