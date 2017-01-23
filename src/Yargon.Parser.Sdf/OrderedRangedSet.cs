using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtlink.Utilib.Collections;

namespace Yargon.Parser.Sdf
{
    /// <summary>
    /// An ordered set of elements.
    /// </summary>
    public sealed class OrderedRangedSet<T> : ISet<T>, IReadOnlySet<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// A sorted list of ranges.
        /// </summary>
        private readonly List<Interval<T>> ranges = new List<Interval<T>>();

        /// <inheritdoc />
        public int Count
        {
            get { return this.ranges.Sum(r => (int)(dynamic)Subtract(r.End, r.Start)); }
        }

        /// <summary>
        /// Gets whether the set is empty.
        /// </summary>
        /// <value><see langword="true"/> when the set is empty;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsEmpty
        {
            get { return this.ranges.Count == 0; }
        }

        /// <inheritdoc />
        bool ICollection<T>.IsReadOnly { get; } = false;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedRangedSet{T}"/> class.
        /// </summary>
        public OrderedRangedSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedRangedSet{T}"/> class.
        /// </summary>
        /// <param name="elements">The elements to include in the set.</param>
        public OrderedRangedSet(IEnumerable<T> elements)
        {
            #region Contract
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));
            #endregion

            if (elements is OrderedRangedSet<T>)
                this.ranges = new List<Interval<T>>(((OrderedRangedSet<T>)elements).ranges);
            else
                AddAll(elements);
        }
        #endregion

        #region Mutation
        /// <inheritdoc />
        public bool Add(T element)
        {
            // Assert: No range ends where another range starts (because then they'd be merged).
            // So, if we find a range that ends at `c`, then the next range doesn't contain `c`.

            // Find the index of the range that ends before or at (not including) the character.
            int rangeIndex = GetRangeThatEndsBeforeOrAt(element);
            if (rangeIndex >= 0)
            {
                if (Eq(this.ranges[rangeIndex].End, element))
                {
                    // The range can be extended with the character.
                    this.ranges[rangeIndex] = NewRange(this.ranges[rangeIndex].Start, Inc(element));
                    TryMergeWithNext(rangeIndex);
                    return true;
                }

                // The next range already contains the character.
                if (rangeIndex + 1 < this.ranges.Count && this.ranges[rangeIndex + 1].Contains(element))
                    return false;

                // Create a new range, and insert it.
                this.ranges.Insert(rangeIndex + 1, NewRange(element, Inc(element)));
                return true;
            }

            // Find the index of the range that starts after the character.
            rangeIndex = GetRangeThatStartsAfter(element);
            if (rangeIndex >= 0)
            {
                if (Eq(this.ranges[rangeIndex].Start, Inc(element)))
                {
                    // The range can be extended with the character.
                    this.ranges[rangeIndex] = NewRange(element, this.ranges[rangeIndex].End);
                    if (rangeIndex > 0)
                        TryMergeWithNext(rangeIndex - 1);
                    return true;
                }

                // Create a new range, and insert it.
                this.ranges.Insert(rangeIndex, NewRange(element, Inc(element)));
                return true;
            }

            // Create a new range, and insert it.
            this.ranges.Add(NewRange(element, Inc(element)));
            return true;
        }

        /// <inheritdoc />
        void ICollection<T>.Add(T element)
        {
            Add(element);
        }

        /// <summary>
        /// Adds a range of elements.
        /// </summary>
        /// <param name="first">The first element.</param>
        /// <param name="last">The last element.</param>
        public void AddRange(T first, T last)
        {
            #region Contract
            if (first.CompareTo(last) > 0)
                throw new ArgumentOutOfRangeException(nameof(first));
            #endregion

            for (T element = first; LtEq(element, last); element = Inc(element))
            {
                Add(element);
            }
        }

        /// <summary>
        /// Adds a range of elements.
        /// </summary>
        /// <param name="elements">The elements to add.</param>
        public void AddAll(IEnumerable<T> elements)
        {
            #region Contract
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));
            #endregion

            foreach (var element in elements)
            {
                Add(element);
            }
        }

        /// <inheritdoc />
        public bool Remove(T element)
        {
            // Find the index of the range that start before or at the character.
            int rangeIndex = GetRangeThatStartsBeforeOrAt(element);
            if (rangeIndex >= 0 && this.ranges[rangeIndex].Contains(element))
            {
                // The range contains the character. Split it.
                var rangeBefore = NewRange(this.ranges[rangeIndex].Start, element);
                var rangeAfter = NewRange(Inc(element), this.ranges[rangeIndex].End);
                InsertSplitRanges(rangeIndex, rangeBefore, rangeAfter);
                return true;
            }
            else
            {
                // No such range exists.
                return false;
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.ranges.Clear();
        }

        /// <summary>
        /// Creates a new character range.
        /// </summary>
        /// <param name="start">The start of the range.</param>
        /// <param name="end">The end of the range.</param>
        /// <returns>The created character range.</returns>
        private Interval<T> NewRange(T start, T end)
        {
            #region Contract
            Debug.Assert(LtEq(start, end));
            #endregion

            return new Interval<T>(start, end);
        }

        /// <summary>
        /// Attempts to merge the range with the next range.
        /// </summary>
        /// <param name="rangeIndex">The zero-based index of the range to try to merge.</param>
        private void TryMergeWithNext(int rangeIndex)
        {
            #region Contract
            Debug.Assert(rangeIndex >= 0);
            #endregion

            if (rangeIndex + 1 < this.ranges.Count && Eq(this.ranges[rangeIndex].End, this.ranges[rangeIndex + 1].Start))
            {
                this.ranges[rangeIndex] = NewRange(this.ranges[rangeIndex].Start, this.ranges[rangeIndex + 1].End);
                this.ranges.RemoveAt(rangeIndex + 1);
            }
        }

        /// <summary>
        /// Removes the range if it's empty.
        /// </summary>
        /// <param name="rangeIndex">The zero-based index of the range to remove.</param>
        // ReSharper disable once UnusedMember.Local
        private void RemoveIfEmpty(int rangeIndex)
        {
            #region Contract
            Debug.Assert(rangeIndex >= 0);
            #endregion

            if (this.ranges[rangeIndex].IsEmpty)
            {
                this.ranges.RemoveAt(rangeIndex);
            }
        }

        /// <summary>
        /// Gets the range that starts before or at the specified character.
        /// </summary>
        /// <param name="element">The character to look for.</param>
        /// <returns>The zero-based index of the range; or -1 when not found.</returns>
        private int GetRangeThatStartsBeforeOrAt(T element)
        {
            int i = 0;
            while (i < this.ranges.Count && LtEq(this.ranges[i].Start, element))
            {
                i++;
            }
            return i - 1;
        }

        /// <summary>
        /// Gets the range that starts after the specified character.
        /// </summary>
        /// <param name="element">The character to look for.</param>
        /// <returns>The zero-based index of the range; or -1 when not found.</returns>
        private int GetRangeThatStartsAfter(T element)
        {
            int i = 0;
            while (i < this.ranges.Count && LtEq(this.ranges[i].Start, element))
            {
                i++;
            }
            return i < this.ranges.Count ? i : -1;
        }

        /// <summary>
        /// Gets the range that ends at or before the specified character.
        /// </summary>
        /// <param name="element">The character to look for.</param>
        /// <returns>The zero-based index of the range; or -1 when not found.</returns>
        private int GetRangeThatEndsBeforeOrAt(T element)
        {
            int i = 0;
            while (i < this.ranges.Count && LtEq(this.ranges[i].End, element))
            {
                i++;
            }
            return i - 1;
        }

        /// <summary>
        /// Inserts two split ranges into the character set.
        /// </summary>
        /// <param name="rangeIndex">The zero-based index of the range.</param>
        /// <param name="rangeBefore">The range before the split.</param>
        /// <param name="rangeAfter">The range after the split.</param>
        private void InsertSplitRanges(int rangeIndex, Interval<T> rangeBefore, Interval<T> rangeAfter)
        {
            if (rangeBefore.IsEmpty && rangeAfter.IsEmpty)
                this.ranges.RemoveAt(rangeIndex);
            else if (rangeBefore.IsEmpty)
                this.ranges[rangeIndex] = rangeAfter;
            else if (rangeAfter.IsEmpty)
                this.ranges[rangeIndex] = rangeBefore;
            else
            {
                this.ranges[rangeIndex] = rangeBefore;
                this.ranges.Insert(rangeIndex + 1, rangeAfter);
            }
        }
        #endregion

        #region Set Mutation
        /// <inheritdoc />
        public void ExceptWith(IEnumerable<T> other)
        {
            // Every element in this set AND NOT in `other`.
            var elements = this.Except(other).ToArray();
            this.Clear();
            this.AddAll(elements);
        }

        /// <inheritdoc />
        public void UnionWith(IEnumerable<T> other)
        {
            // Every element in this set OR in `other`.
            var elements = this.Union(other).ToArray();
            this.Clear();
            this.AddAll(elements);
        }

        /// <inheritdoc />
        public void IntersectWith(IEnumerable<T> other)
        {
            // Every element in this set AND in `other`.
            var elements = this.Intersect(other).ToArray();
            this.Clear();
            this.AddAll(elements);
        }

        /// <inheritdoc />
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            // Every element in this set XOR in `other`.
            var enumerable = other as T[] ?? other.ToArray();
            var elements = this.Union(enumerable).Except(this.Intersect(enumerable)).ToArray();
            this.Clear();
            this.AddAll(elements);
        }
        #endregion

        #region Querying

        /// <inheritdoc />
        public bool Contains(T element)
        {
            return this.ranges.Any(range => range.Contains(element));
        }

        /// <inheritdoc />
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            var enumerable = other as T[] ?? other.ToArray();
            return IsSubsetOf(enumerable) && !SetEquals(enumerable);
        }

        /// <inheritdoc />
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            var enumerable = other as T[] ?? other.ToArray();
            return IsSupersetOf(enumerable) && !SetEquals(enumerable);
        }

        /// <inheritdoc />
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return this.Except(other).Any();
        }

        /// <inheritdoc />
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return other.Except(this).Any();
        }

        /// <inheritdoc />
        public bool Overlaps(IEnumerable<T> other)
        {
            return this.Intersect(other).Any();
        }

        /// <inheritdoc />
        public bool SetEquals(IEnumerable<T> other)
        {
            return this.SequenceEqual(other.OrderBy(c => c));
        }
        #endregion

        /// <summary>
        /// Returns a collection of ranges.
        /// </summary>
        /// <returns>A collection of ranges.</returns>
        public IEnumerable<Interval<T>> GetRanges()
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var range in this.ranges)
            {
                yield return range;
            }
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            #region Contract
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (arrayIndex + this.Count > array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            #endregion
            foreach (var element in this)
            {
                array[arrayIndex++] = element;
            }
        }

        /// <summary>
        /// Increments a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result.</returns>
        [Pure]
        private T Inc(T value)
        {
            return (dynamic)value + 1;
        }

        /// <summary>
        /// Decrements a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result.</returns>
        [Pure]
        // ReSharper disable once UnusedMember.Local
        private T Dec(T value)
        {
            return (dynamic)value - 1;
        }

#if false
		/// <summary>
		/// Adds one value to another.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>The result.</returns>
		[Pure]
		private T Add(T x, T y)
		{
			return (dynamic)x + y;
		}
#endif

        /// <summary>
        /// Subtracts one value from another.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>The result.</returns>
        [Pure]
        private T Subtract(T x, T y)
        {
            return (dynamic)x - y;
        }

        /// <summary>
        /// Compares one value to another.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>The result.</returns>
        [Pure]
        private bool Lt(T x, T y)
        {
            return x.CompareTo(y) < 0;
        }

#if false
		/// <summary>
		/// Compares one value to another.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>The result.</returns>
		[Pure]
		private bool Gt(T x, T y)
		{
			return !LtEq(x, y);
		}
#endif

        /// <summary>
        /// Compares one value to another.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>The result.</returns>
        [Pure]
        private bool LtEq(T x, T y)
        {
            return x.CompareTo(y) <= 0;
        }

#if false
		/// <summary>
		/// Compares one value to another.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>The result.</returns>
		[Pure]
		private bool GtEq(T x, T y)
		{
			return !Lt(x, y);
		}
#endif

        /// <summary>
        /// Compares one value to another.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>The result.</returns>
        [Pure]
        private bool Eq(T x, T y)
        {
            return Object.Equals(x, y);
        }

#if false
		/// <summary>
		/// Compares one value to another.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>The result.</returns>
		[Pure]
		private bool NEq(T x, T y)
		{
			return !Eq(x, y);
		}
#endif

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var range in this.ranges)
            {
                for (var element = range.Start; Lt(element, range.End); element = Inc(element))
                {
                    yield return element;
                }
            }
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[{String.Join(",", this.ranges.Select(IntervalToString))}]";
        }

        /// <summary>
        /// Returns a string representation of the specified interval.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <returns>The string representation.</returns>
        private string IntervalToString(Interval<T> interval)
        {
            return interval.ToString();
        }
    }
}
