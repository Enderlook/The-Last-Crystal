using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LINQExtensions
{
    /// <summary>
    /// Add a the <paramref name="element"/> at the end of the returned <seealso cref="IEnumerable{T}"/> <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="T">Type of the <paramref name="element"/> to append to <paramref name="source"/>.</typeparam>
    /// <param name="source">Source to append the <paramref name="element"/>.</param>
    /// <param name="element">Element to append to <paramref name="source"/>.</param>
    /// <returns><paramref name="source"/> with the <paramref name="element"/> added at the end of it.</returns>
    public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T element)
    {
        if (source is null) throw new System.ArgumentNullException(nameof(source));

        return source.Concat(new T[] { element });
    }

    /// <summary>
    /// Check if the <paramref name="source"/> contains an elements which match the given criteria by <paramref name="selector"/>.
    /// </summary>
    /// <typeparam name="T">Type of the element inside <paramref name="source"/>.</typeparam>
    /// <param name="source">Source to look for a matching element.</param>
    /// <param name="selector">Check if the element match the criteria.</param>
    /// <returns>Whenever the matched item was found or not.</returns>
    public static bool ContainsBy<T>(this IEnumerable<T> source, Func<T, bool> selector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        foreach (T item in source)
        {
            if (selector(item))
                return true;
        }
        return false;
    }


    /// <summary>
    /// Return the element which the highest property returned by <paramref name="selector"/>, using <paramref name="comparer"/>.
    /// </summary>
    /// <typeparam name="TSource">Type the of the <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">Type returned by the <paramref name="selector"/>.</typeparam>
    /// <param name="source"><seealso cref="IEnumerable{T}"/> to get the highest value.</param>
    /// <param name="selector">Function which provides the property to compare.</param>
    /// <param name="comparer">Comparer used to compare the values returned by <paramref name="selector"/>.</param>
    /// <returns>The element with the highest property.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Estilo", "IDE0054:Usar la asignación compuesta", Justification = "Unity no lo soporta.")]
    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        comparer = comparer ?? Comparer<TKey>.Default;
        return source.Aggregate((a, c) => comparer.Compare(selector(a), selector(c)) > 0 ? a : c);
    }


    /// <summary>
    /// Return the element which the lowest property returned by <paramref name="selector"/>, using <paramref name="comparer"/>.
    /// </summary>
    /// <typeparam name="TSource">Type the of the <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">Type returned by the <paramref name="selector"/>.</typeparam>
    /// <param name="source"><seealso cref="IEnumerable{T}"/> to get the lowest value.</param>
    /// <param name="selector">Function which provides the property to compare.</param>
    /// <param name="comparer">Comparer used to compare the values returned by <paramref name="selector"/>.</param>
    /// <returns>The element with the lowest property.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Estilo", "IDE0054:Usar la asignación compuesta", Justification = "Unity no lo soporta.")]
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        comparer = comparer ?? Comparer<TKey>.Default;
        return source.Aggregate((a, c) => comparer.Compare(selector(a), selector(c)) < 0 ? a : c);
    }

    /// <summary>
    /// Returns a random element from <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="T">Type of the element inside <paramref name="source"/>.</typeparam>
    /// <param name="source">Source to look for a random element.</param>
    /// <returns></returns>
    public static T RandomElement<T>(this IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.ElementAt(UnityEngine.Random.Range(0, source.Count()));
    }

    /// <summary>
    /// Performs the specified <paramref name="action"/> on each element of the <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="T">Type of the element inside <paramref name="source"/>.</typeparam>
    /// <param name="source">Source to look for element to perform the <paramref name="action"/></param>
    /// <param name="action">Action to perform on each element of <paramref name="source"/></param>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (action is null) throw new ArgumentNullException(nameof(action));

        foreach (T element in source)
        {
            action(element);
        }
    }
}