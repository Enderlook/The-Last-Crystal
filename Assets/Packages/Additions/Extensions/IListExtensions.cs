using System;
using System.Collections.Generic;

namespace Additions.Extensions
{
    public static class IListExtensions
    {
        private const string CAN_NOT_BE_EMPTY = "Can't be empty.";
        private const string CAN_NOT_BE_NEGATIVE = "Can't be negative.";
        private const string CANT_NOT_BE_ZERO = "Can't be 0.";

        /// <summary>
        /// Removes an element from a list if matches a criteria determined by <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="T">Type of element.</typeparam>
        /// <param name="source">List to remove item.</param>
        /// <param name="selector">Function to determine if the item must be removed.</param>
        /// <param name="ascendOrder">Whenever it must remove in ascending or descending order.</param>
        /// <param name="removeAmount">Amount of items which must the criteria must be removed. If 0, remove all the matched elements.</param>
        /// <returns><paramref name="source"/>.</returns>
        private static IList<T> RemoveBy<T>(this IList<T> source, Func<T, bool> selector, bool ascendOrder = true, int removeAmount = 1)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (removeAmount < 0) throw new ArgumentException(CAN_NOT_BE_NEGATIVE, nameof(removeAmount));

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
        public static IList<T> RemoveFirstBy<T>(this IList<T> source, Func<T, bool> selector, int removeAmount = 1)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (removeAmount == 0) throw new ArgumentOutOfRangeException(CANT_NOT_BE_ZERO, nameof(removeAmount));

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
        public static IList<T> RemoveLastBy<T>(this IList<T> source, Func<T, bool> selector, int removeAmount = 1)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (removeAmount == 0) throw new ArgumentOutOfRangeException(CANT_NOT_BE_ZERO, nameof(removeAmount));

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
        public static IList<T> RemoveByAll<T>(this IList<T> source, Func<T, bool> selector) => source.RemoveBy(selector, removeAmount: 0);

        /// <summary>
        /// Performs the specified <paramref name="action"/> on each element of the <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">Type of the element inside <paramref name="source"/>.</typeparam>
        /// <param name="source">Source to look for element to perform the <paramref name="function"/></param>
        /// <param name="function">Function to perform on each element of <paramref name="source"/></param>
        /// <returns>Updated <paramref name="source"/>.</returns>
        /// <seealso cref="Array.ForEach{T}(T[], Action{T})"/>
        public static IList<T> ChangeEach<T>(this IList<T> source, Func<T, T> function)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (function is null) throw new ArgumentNullException(nameof(function));
            if (source.Count == 0)
                return new List<T>(0);

            for (int i = 0; i < source.Count; i++)
                source[i] = function(source[i]);

            return source;
        }

        /// <summary>
        /// Remove and returns the element at the begging of <paramref name="source"/>.<br/>
        /// </summary>
        /// <typeparam name="T">Type of element to remove.</typeparam>
        /// <param name="source"><see cref="IList{T}"/> where element is taken.</param>
        /// <returns>Element removed from the begging of <paramref name="source"/>.</returns>
        /// <remarks>This is an O(n) operation.</remarks>
        public static T PopFirst<T>(this IList<T> source)
        {
            if (source.Count == 0) throw new ArgumentException(CAN_NOT_BE_EMPTY, nameof(source));

            T element = source[0];
            source.RemoveAt(0);
            return element;
        }

        /// <summary>
        /// Try to remove and return the element at the begging of <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">Type of element to remove.</typeparam>
        /// <param name="source"><see cref="IList{T}"/> where element is taken.</param>
        /// <param name="element">Element removed from the begging of <paramref name="source"/>, if return is <see langword="true"/></param>
        /// <returns>Whenever an element was removed or not (<paramref name="source"/> is empty).</returns>
        /// <remarks>This is an O(n) operation.</remarks>
        public static bool TryPopFirst<T>(this IList<T> source, out T element)
        {
            if (source.Count == 0)
            {
                element = default;
                return false;
            }

            element = source[0];
            source.RemoveAt(0);
            return true;
        }

        /// <summary>
        /// Remove and returns the element at the end of <paramref name="source"/>.<br/>
        /// </summary>
        /// <typeparam name="T">Type of element to remove.</typeparam>
        /// <param name="source"><see cref="IList{T}"/> where element is taken.</param>
        /// <returns>Element removed from the end of <paramref name="source"/>.</returns>
        /// <remarks>This is an O(1) amortized O(n) operation.</remarks>
        public static T PopLast<T>(this IList<T> source)
        {
            if (source.Count == 0) throw new ArgumentException(CAN_NOT_BE_EMPTY, nameof(source));

            int index = source.Count - 1;
            T element = source[index];
            source.RemoveAt(index);
            return element;
        }

        /// <summary>
        /// Try to remove and return the element at the end of <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">Type of element to remove.</typeparam>
        /// <param name="source"><see cref="IList{T}"/> where element is taken.</param>
        /// <param name="element">Element removed from the end of <paramref name="source"/>, if return is <see langword="true"/></param>
        /// <returns>Whenever an element was removed or not (<paramref name="source"/> is empty).</returns>
        /// <remarks>This is an O(1) amortized O(n) operation.</remarks>
        public static bool TryPopLast<T>(this IList<T> source, out T element)
        {
            if (source.Count == 0)
            {
                element = default;
                return false;
            }

            int index = source.Count - 1;
            element = source[index];
            source.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Add <paramref name="element"/> at begging of <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">Type of element to add.</typeparam>
        /// <param name="source"><see cref="IList{T}"/> where elements are added.</param>
        /// <param name="element">Element to add at begging of <paramref name="source"/>.</param>
        /// <remarks>This is an O(n) operation.</remarks>
        public static void AddFirst<T>(this IList<T> source, T element) => source.Insert(0, element);
    }
}