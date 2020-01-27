using System;
using System.Collections.Generic;

namespace Additions.Extensions
{
    public static class BinarySearchExtensions
    {
        // TODO: Implement https://stackoverflow.com/a/1766369/7655838 and https://stackoverflow.com/a/2948872/7655838

        /// <summary>
        /// Returns the zero-based index of the first occurrence of a value in the <paramref name="source"/> or in a portion of it.<br>
        /// Only use if <paramref name="source"/> is sorted.
        /// </summary>
        /// <typeparam name="T">Element <paramref name="item"/>.</typeparam>
        /// <param name="source">Where the index of <paramref name="item"/> will be looked for.</param>
        /// <param name="item">The object to locate in the <paramref name="source"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements. Use <see langword="null"/> to use <see cref="Comparer{T}.Default"/>.</param>
        /// <returns>The zero-based index of value in the sorted <paramref name="source"/>, if <paramref name="item"/> is found; otherwise -1.</returns>
        public static int BinarySearchFirst<T>(this List<T> source, T item, IComparer<T> comparer = null)
        {
            // https://stackoverflow.com/a/40358244/7655838 from https://stackoverflow.com/questions/40357945/how-to-get-first-index-of-binary-searchs-results
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (comparer is null) comparer = Comparer<T>.Default;

            int index = source.BinarySearch(item, comparer);
            while (index > 0 && comparer.Compare(source[index], source[index - 1]) == 0)
                index--;
            return index;
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of a value in the <paramref name="source"/> or in a portion of it.<br>
        /// Only use if <paramref name="source"/> is sorted.
        /// </summary>
        /// <typeparam name="T">Element <paramref name="item"/>.</typeparam>
        /// <param name="source">Where the index of <paramref name="item"/> will be looked for.</param>
        /// <param name="item">The object to locate in the <paramref name="source"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements. Use <see langword="null"/> to use <see cref="Comparer{T}.Default"/>.</param>
        /// <returns>The zero-based index of value in the sorted <paramref name="source"/>, if <paramref name="item"/> is found; otherwise -1.</returns>
        public static int BinarySearchFirst<T>(this List<T> source, T item, int index, int count, IComparer<T> comparer = null)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (comparer is null) comparer = Comparer<T>.Default;

            index = source.BinarySearch(index, count, item, comparer);
            while (index > 0 && comparer.Compare(source[index], source[index - 1]) == 0)
                index--;
            return index;
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of a value in the <paramref name="source"/> or in a portion of it.<br>
        /// Only use if <paramref name="source"/> is sorted.
        /// </summary>
        /// <typeparam name="T">Element <paramref name="item"/>.</typeparam>
        /// <param name="source">Where the index of <paramref name="item"/> will be looked for.</param>
        /// <param name="item">The object to locate in the <paramref name="source"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements. Use <see langword="null"/> to use <see cref="Comparer{T}.Default"/>.</param>
        /// <returns>The zero-based index of value in the sorted <paramref name="source"/>, if <paramref name="item"/> is found; otherwise -1.</returns>
        public static int BinarySearchFirst<T>(this T[] source, T item, IComparer<T> comparer = null)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (comparer is null) comparer = Comparer<T>.Default;

            int index = Array.BinarySearch(source, item, comparer);
            while (index > 0 && comparer.Compare(source[index], source[index - 1]) == 0)
                index--;
            return index;
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of a value in the <paramref name="source"/> or in a portion of it.<br>
        /// Only use if <paramref name="source"/> is sorted.
        /// </summary>
        /// <typeparam name="T">Element <paramref name="item"/>.</typeparam>
        /// <param name="source">Where the index of <paramref name="item"/> will be looked for.</param>
        /// <param name="item">The object to locate in the <paramref name="source"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements. Use <see langword="null"/> to use <see cref="Comparer{T}.Default"/>.</param>
        /// <returns>The zero-based index of value in the sorted <paramref name="source"/>, if <paramref name="item"/> is found; otherwise -1.</returns>
        public static int BinarySearchFirst<T>(this T[] source, T item, int index, int count, IComparer<T> comparer = null)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (comparer is null) comparer = Comparer<T>.Default;

            index = Array.BinarySearch(source, index, count, item, comparer);
            while (index > 0 && comparer.Compare(source[index], source[index - 1]) == 0)
                index--;
            return index;
        }

        /// <summary>
        /// Searches the entire sorted <see cref="IList{T}"/> for <paramref name="item"/> and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="T">Type of <paramref name="item"/>.</typeparam>
        /// <param name="source"><see cref="IList{T}"/> where <paramref name="item"/> will be looked for.</param>
        /// <param name="item">Element to look for.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements. Use <see langword="null"/> to use <see cref="Comparer{T}.Default"/>.</param>
        /// <returns>The zero-based index of item in the sorted <see cref="IList{T}"/>, if item is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, if there is no larger element, the bitwise complement of <see cref="ICollection{T}.Count"/>.</returns>
        public static int BinarySearch<T>(this IList<T> source, T item, IComparer<T> comparer = null)
        {
            // https://stackoverflow.com/a/967098/7655838 from https://stackoverflow.com/questions/967047/how-to-perform-a-binary-search-on-ilistt
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (comparer is null) comparer = Comparer<T>.Default;

            int lower = 0;
            int upper = source.Count - 1;

            while (lower <= upper)
            {
                int middle = lower + ((upper - lower) / 2);
                int comparisonResult = comparer.Compare(item, source[middle]);
                if (comparisonResult == 0)
                    return middle;
                if (comparisonResult < 0)
                    upper = middle - 1;
                else
                    lower = middle + 1;
            }

            return ~lower;
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of a value in the <paramref name="source"/> or in a portion of it.<br>
        /// Only use if <paramref name="source"/> is sorted.
        /// </summary>
        /// <typeparam name="T">Element <paramref name="item"/>.</typeparam>
        /// <param name="source">Where the index of <paramref name="item"/> will be looked for.</param>
        /// <param name="item">The object to locate in the <paramref name="source"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements. Use <see langword="null"/> to use <see cref="Comparer{T}.Default"/>.</param>
        /// <returns>The zero-based index of value in the sorted <paramref name="source"/>, if <paramref name="item"/> is found; otherwise -1.</returns>
        public static int BinarySearchFirst<T>(this IList<T> source, T item, IComparer<T> comparer = null)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (comparer is null) comparer = Comparer<T>.Default;

            int index = source.BinarySearch(item, comparer);
            while (index > 0 && comparer.Compare(source[index], source[index - 1]) == 0)
                index--;
            return index;
        }
    }
}