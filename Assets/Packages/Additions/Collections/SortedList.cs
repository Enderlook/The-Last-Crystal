using Additions.Extensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Additions.Collections
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "+}")]
    public class SortedList<T> : IList<T>, IList, IReadOnlyList<T>
    {
        // https://stackoverflow.com/a/40846923/7655838 from https://stackoverflow.com/questions/3663613/why-is-there-no-sortedlistt-in-net/40846923

        private readonly List<T> list;

        private readonly IComparer<T> comparer;

        /// <summary>
        /// Internal capacity of this <see cref="SortedList{T}"/>.
        /// </summary>
        public int Capacity {
            get => list.Capacity;
            set => list.Capacity = value;
        }

        /// <summary>
        ///  How may elements are in this <see cref="SortedList{T}"/>
        /// </summary>
        public int Count => list.Count;

        /// <inheritdoc />
        bool IList.IsReadOnly => false;

        /// <inheritdoc />
        bool ICollection<T>.IsReadOnly => false;

        /// <inheritdoc />
        bool IList.IsFixedSize => false;

        /// <inheritdoc />
        bool ICollection.IsSynchronized => false;

        /// <inheritdoc />
        public object SyncRoot => this;

        /// <inheritdoc />
        object IList.this[int index] {
            get => list[index];
            set => throw new NotSupportedException($"{nameof(SortedList<T>)} doesn't support random set. Use {nameof(Add)} instead.");
        }

        /// <summary>
        /// Gets the element at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index of element.</param>
        /// <returns>Element at <paramref name="index"/>.</returns>
        public T this[int index] {
            get => list[index];
            set => throw new NotSupportedException($"{nameof(SortedList<T>)} doesn't support random set. Use {nameof(Add)} instead.");
        }

        public SortedList(IComparer<T> comparer)
        {
            this.comparer = comparer ?? Comparer<T>.Default;
            list = new List<T>();
        }

        public SortedList() : this(Comparer<T>.Default) { }

        public SortedList(int capacity, IComparer<T> comparer = null)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Can't be negative");

            list = new List<T>(capacity);
            this.comparer = comparer ?? Comparer<T>.Default;
        }

        public SortedList(IEnumerable<T> collection, IComparer<T> comparer = null)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            list = new List<T>(collection);
            this.comparer = comparer ?? Comparer<T>.Default;
        }

        private SortedList(List<T> list, IComparer<T> comparer)
        {
            this.comparer = comparer ?? Comparer<T>.Default;
            this.list = list;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindSortedIndexOf(T item, out int index)
        {
            index = list.BinarySearch(item, comparer);
            if (index >= 0)
                return true;

            index = Math.Abs(index) - 1;
            return false;
        }

        /// <summary>
        /// Adds the given <paramref name="item"/> to this <see cref="SortedList{T}"/> in a sorted index.
        /// </summary>
        /// <param name="item">Element to add.</param>
        /// <returns>Index where this <paramref name="item"/> was added.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Add(T item)
        {
            FindSortedIndexOf(item, out int index);
            list.Insert(index, item);
            return index;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ICollection<T>.Add(T item) => Add(item);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int IList.Add(object item)
        {
            try
            {
                return Add((T)item);
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException($"Can only be of type {typeof(T)} or castable to it. Was {item.GetType()}", nameof(item), e);
            }
        }

        /// <summary>
        /// Add the elements in <paramref name="collection"/> to this <see cref="SortedList{T}"/>. Elements will added sorted.
        /// </summary>
        /// <param name="collection">Elements to add.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
                Add(item);
        }

        /// <summary>
        /// Add the elements in <paramref name="collection"/> to this <see cref="SortedList{T}"/>. Elements will added sorted.
        /// </summary>
        /// <param name="collection">Elements to add.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(IEnumerable collection)
        {
            try
            {
                foreach (object item in collection)
                    Add((T)item);
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException($"Can only contain type {typeof(T)} or castable to it.", nameof(collection), e);
            }
        }

        /// <summary>
        /// Produces a readonly version of this <see cref="SortedList{T}"/>.
        /// </summary>
        /// <returns>Readonly version of this <see cref="SortedList{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyCollection<T> AsReadOnly() => new ReadOnlyCollection<T>(this);

        /// <summary>
        /// Uses a binary search algorithm to locate the element <see cref="item"/> in this <see cref="SortedList{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate. The value can be <see langword="null"/> for reference types.</param>
        /// <returns>The zero-based index of <see cref="item"/> in this <see cref="SortedList{T}"/>, if item is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, if there is no larger element, the bitwise complement of <see cref="Count"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BinarySearch(T item) => list.BinarySearch(item, comparer);

        /// <summary>
        /// Uses a binary search algorithm to locate the element <see cref="item"/> in this <see cref="SortedList{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The object to locate. The value can be <see langword="null"/> for reference types.</param>
        /// <returns>The zero-based index of <see cref="item"/> in this <see cref="SortedList{T}"/>, if item is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, if there is no larger element, the bitwise complement of <see cref="Count"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BinarySearch(int index, int count, T item) => list.BinarySearch(index, count, item, comparer);

        /// <summary>
        /// Clear the content of this <see cref="SortedList{T}"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => list.Clear();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => IndexOf(item) != -1;

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IList.Contains(object value)
        {
            T item;
            try
            {
                item = (T)value;
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException($"Can only be of type {typeof(T)} or castable to it. Was {value.GetType()}", nameof(value), e);
            }
            return Contains(item);
        }

        /// <summary>
        /// Copies the content of this <see cref="SortedList{T}"/> to <paramref name="array"/>.
        /// </summary>
        /// <param name="array">Array where elements will be copied to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array) => list.CopyTo(array);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ICollection.CopyTo(Array array, int index) => ((ICollection)list).CopyTo(array, index);

        /// <summary>
        /// Copies a section of this <see cref="SortedList{T}"/> to <paramref name="array"/>.
        /// </summary>
        /// <param name="index">Starting index in this <see cref="SortedList{T}"/>.</param>
        /// <param name="array">Array where elements will be copied to.</param>
        /// <param name="arrayIndex">Starting index in the target <paramref name="array"/>.</param>
        /// <param name="count">Amount of elements to be copied.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(int index, T[] array, int arrayIndex, int count) => list.CopyTo(index, array, arrayIndex, count);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

        /// <summary>
        /// Creates a shallow copy of a range of elements in this <see cref="SortedList{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based <see cref="SortedList{T}"/> index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>A shallow copy of a range of elements in the source <see cref="SortedList{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> GetRangeList(int index, int count) => list.GetRange(index, count);

        /// <summary>
        /// Creates a shallow copy of a range of elements in this <see cref="SortedList{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based <see cref="SortedList{T}"/> index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>A shallow copy of a range of elements in the source <see cref="SortedList{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SortedList<T> GetRange(int index, int count) => new SortedList<T>(GetRangeList(index, count), comparer);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="SortedList{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="SortedList{T}"/>. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire <see cref="SortedList{T}"/>, if found; otherwise, -1.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
        {
            int index = list.BinarySearchFirst(item, comparer);
            if (index < 0 || index >= list.Count)
                return -1;
            return index;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="SortedList{T}"/>.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="SortedList{T}"/>. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire <see cref="SortedList{T}"/>, if found; otherwise, -1.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int IList.IndexOf(object value)
        {
            T item;
            try
            {
                item = (T)value;
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException($"Can only be of type {typeof(T)} or castable to it. Was {value.GetType()}", nameof(value), e);
            }
            return IndexOf(item);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(T item)
        {
            if (FindSortedIndexOf(item, out int index))
            {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IList.Remove(object item)
        {
            T _item;
            try
            {
                _item = (T)item;
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException($"Can only be of type {typeof(T)} or castable to it. Was {item.GetType()}", nameof(item), e);
            }
            if (FindSortedIndexOf(_item, out int index))
                list.RemoveAt(index);
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements removed from the <see cref="SortedList{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int RemoveAll(Predicate<T> match) => list.RemoveAll(match);

        /// <summary>
        /// Removes the element at the specified index of the <see cref="SortedList{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index) => list.RemoveAt(index);

        /// <summary>
        /// Removes a range of elements from the <see cref="SortedList{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveRange(int index, int count) => list.RemoveRange(index, count);

        /// <summary>
        /// Removes the elements in <paramref name="collection"/> from <see cref="SortedList{T}"/>.
        /// </summary>
        /// <param name="collection">Elements to remove from <see cref="SortedList{T}"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveElements(IEnumerable<T> collection)
        {
            foreach (T item in collection)
                Remove(item);
        }

        /// <summary>
        /// Removes the elements in <paramref name="collection"/> from <see cref="SortedList{T}"/>.
        /// </summary>
        /// <param name="collection">Elements to remove from <see cref="SortedList{T}"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveElements(IEnumerable collection)
        {
            try
            {
                foreach (object item in collection)
                    Remove((T)item);
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException($"Can only contain type {typeof(T)} or castable to it.", nameof(collection), e);
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="SortedList{T}"/> to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the <see cref="SortedList{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray() => list.ToArray();

        /// <summary>
        /// Copies the elements of the <see cref="SortedList{T}"/> to a new <see cref="List{T}"/>.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> containing copies of the elements of the <see cref="SortedList{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ToList() => new List<T>(list);

        /// <summary>
        /// Sets the <see cref="Capacity"/> to <see cref="Count"/> minimizing the memory overhead.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TrimExcess() => list.TrimExcess();

        /// <summary>
        /// This method is not supported.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T item) => throw new NotSupportedException($"{nameof(SortedList<T>)} doesn't support random insertion. Use {nameof(Add)} instead.");

        /// <summary>
        /// This method is not supported.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, object value) => throw new NotSupportedException($"{nameof(SortedList<T>)} doesn't support random insertion. Use {nameof(Add)} instead.");

        /// <summary>
        /// Performs the specified action on each element of the <see cref="SortedList{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> delegate to perform on each element of the <see cref="SortedList{T}"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ForEach(Action<T> action) => list.ForEach(action);
    }
}