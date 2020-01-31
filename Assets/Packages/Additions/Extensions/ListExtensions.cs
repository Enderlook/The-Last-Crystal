using System;
using System.Collections.Generic;

namespace Additions.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Removes an element from a list if matches a criteria determined by <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="T">Type of element.</typeparam>
        /// <param name="source">List to remove item.</param>
        /// <param name="selector">Function to determine if the item must be removed.</param>
        /// <param name="ascendOrder">Whenever it must remove in ascending or descending order.</param>
        /// <param name="removeAmount">Amount of items which must the criteria must be removed. If 0, remove all the matched elements.</param>
        /// <returns><paramref name="source"/>.</returns>
        private static List<T> RemoveBy<T>(this List<T> source, Func<T, bool> selector, bool ascendOrder = true, int removeAmount = 1)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            if (removeAmount < 0)
                throw new Exception($"{nameof(removeAmount)} parameter can't be negative.");
            int removed = 0;
            for (int i = ascendOrder ? 0 : source.Count; i < (ascendOrder ? source.Count : 0); i += ascendOrder ? 1 : -1)
                if (selector(source[i]))
                {
                    source.RemoveAt(i);
                    removed++;
                    if (removeAmount == 0 || removed >= removeAmount) break;
                }
            return source;
        }

        /// <summary>
        /// Removes the fist(s) element(s) from a list which matches a criteria determined by <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="T">Type of element.</typeparam>
        /// <param name="source">List to remove item.</param>
        /// <param name="selector">Function to determine if the item must be removed.</param>
        /// <param name="removeAmount">Amount of items which must the criteria must be removed. Value can't be 0.</param>
        /// <returns><paramref name="source"/>.</returns>
        /// <see cref="RemoveBy{T}(List{T}, Func{T, bool}, bool, int)"/>
        /// <seealso cref="RemoveLastBy{T}(List{T}, Func{T, bool}, int)"/>
        /// <seealso cref="RemoveByAll{T}(List{T}, Func{T, bool})"/>
        public static List<T> RemoveFirstBy<T>(this List<T> source, Func<T, bool> selector, int removeAmount = 1)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (removeAmount == 0) throw new ArgumentOutOfRangeException($"{nameof(removeAmount)} parameter can't be 0.");

            return source.RemoveBy(selector, removeAmount: removeAmount);
        }

        /// <summary>
        /// Removes the last(s) element(s) from a list which matches a criteria determined by <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="T">Type of element.</typeparam>
        /// <param name="source">List to remove item.</param>
        /// <param name="selector">Function to determine if the item must be removed.</param>
        /// <param name="removeAmount">Amount of items which must the criteria must be removed. Value can't be 0.</param>
        /// <returns><paramref name="source"/>.</returns>
        /// <see cref="RemoveBy{T}(List{T}, Func{T, bool}, bool, int)"/>
        /// <seealso cref="RemoveFirstBy{T}(List{T}, Func{T, bool}, int)"/>
        /// <seealso cref="RemoveByAll{T}(List{T}, Func{T, bool})"/>
        public static List<T> RemoveLastBy<T>(this List<T> source, Func<T, bool> selector, int removeAmount = 1)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (removeAmount == 0) throw new ArgumentOutOfRangeException($"{nameof(removeAmount)} parameter can't be 0.");

            return source.RemoveBy(selector, ascendOrder: false, removeAmount: removeAmount);
        }

        /// <summary>
        /// Removes all the elements from a list which matches a criteria determined by <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="T">Type of element.</typeparam>
        /// <param name="source">List to remove item.</param>
        /// <param name="selector">Function to determine if the item must be removed.</param>
        /// <returns><paramref name="source"/>.</returns>
        /// <see cref="RemoveBy{T}(List{T}, Func{T, bool}, bool, int)"/>
        /// <seealso cref="RemoveFirstBy{T}(List{T}, Func{T, bool}, int)"/>
        /// <seealso cref="RemoveLastBy{T}(List{T}, Func{T, bool}, int)"/>
        public static List<T> RemoveByAll<T>(this List<T> source, Func<T, bool> selector) => source.RemoveBy(selector, removeAmount: 0);

        /// <summary>
        /// Performs the specified <paramref name="action"/> on each element of the <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">Type of the element inside <paramref name="source"/>.</typeparam>
        /// <param name="source">Source to look for element to perform the <paramref name="function"/></param>
        /// <param name="function">Function to perform on each element of <paramref name="source"/></param>
        /// <returns>Updated <paramref name="source"/>.</returns>
        /// <seealso cref="Array.ForEach{T}(T[], Action{T})"/>
        public static List<T> ChangeEach<T>(this List<T> source, Func<T, T> function)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (function is null) throw new ArgumentNullException(nameof(function));
            if (source.Count == 0)
                return new List<T>(0);

            for (int i = 0; i < source.Count; i++)
                source[i] = function(source[i]);

            return source;
        }
    }
}