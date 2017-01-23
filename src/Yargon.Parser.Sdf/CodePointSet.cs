using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtlink.Utilib.Collections;

namespace Yargon.Parser.Sdf
{
    /// <summary>
	/// A set of code points.
	/// </summary>
	public sealed class CodePointSet : ISet<CodePoint>, IReadOnlySet<CodePoint>
    {
        /// <summary>
        /// A sorted list of ranges.
        /// </summary>
        private readonly List<CodePointRange> ranges = new List<CodePointRange>();

        /// <inheritdoc />
        public int Count
        {
            get { return this.ranges.Sum(r => r.Length); }
        }

        /// <summary>
        /// Gets whether the set is empty.
        /// </summary>
        /// <value><see langword="true"/> when the set is empty;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsEmpty => this.Count == 0;

        /// <inheritdoc />
        bool ICollection<CodePoint>.IsReadOnly { get; } = false;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CodePointSet"/> class.
        /// </summary>
        public CodePointSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodePointSet"/> class.
        /// </summary>
        /// <param name="elements">The elements to include in the set.</param>
        public CodePointSet(IEnumerable<CodePoint> elements)
        {
            #region Contract
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));
            #endregion

            var codePointSet = elements as CodePointSet;
            if (codePointSet != null)
                this.ranges = new List<CodePointRange>(codePointSet.ranges);
            else
                AddAll(elements);
        }
        #endregion

        #region Mutation
        /// <inheritdoc />
        public bool Add(CodePoint element)
        {
            // Find the index of the range that ends before the character.
            int rangeIndex = GetRangeThatEndsBefore(element);
            if (rangeIndex >= 0)
            {
                if (this.ranges[rangeIndex].End + 1 == element)
                {
                    // The range can be extended with the character.
                    this.ranges[rangeIndex] = new CodePointRange(this.ranges[rangeIndex].Start, element);
                    TryMergeWithNext(rangeIndex);
                    return true;
                }

                // The next range already contains the character.
                if (rangeIndex + 1 < this.ranges.Count && this.ranges[rangeIndex + 1].Contains(element))
                    return false;

                // Create a new range, and insert it.
                this.ranges.Insert(rangeIndex + 1, new CodePointRange(element, element));
                return true;
            }

            // Find the index of the range that starts after the character.
            rangeIndex = GetRangeThatStartsAfter(element);
            if (rangeIndex >= 0)
            {
                if (this.ranges[rangeIndex].Start - 1 == element)
                {
                    // The range can be extended with the character.
                    this.ranges[rangeIndex] = new CodePointRange(element, this.ranges[rangeIndex].End);
                    if (rangeIndex > 0)
                        TryMergeWithNext(rangeIndex - 1);
                    return true;
                }

                // Create a new range, and insert it.
                this.ranges.Insert(rangeIndex, new CodePointRange(element, element));
                return true;
            }

            // Create a new range, and insert it.
            this.ranges.Add(new CodePointRange(element, element));
            return true;
        }

        /// <inheritdoc />
        void ICollection<CodePoint>.Add(CodePoint element)
        {
            Add(element);
        }

        /// <summary>
        /// Adds a range of elements.
        /// </summary>
        /// <param name="first">The first element.</param>
        /// <param name="last">The last element.</param>
        public void AddRange(CodePoint first, CodePoint last)
        {
            #region Contract
            if (first > last)
                throw new ArgumentOutOfRangeException(nameof(first));
            #endregion

            for (var element = first; element <= last; element++)
            {
                Add(element);
            }
        }

        /// <summary>
        /// Adds a range of elements.
        /// </summary>
        /// <param name="elements">The elements to add.</param>
        public void AddAll(IEnumerable<CodePoint> elements)
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
        public bool Remove(CodePoint element)
        {
            // Find the index of the range that start before or at the character.
            int rangeIndex = GetRangeThatStartsBeforeOrAt(element);
            if (rangeIndex >= 0 && this.ranges[rangeIndex].Contains(element))
            {
                // The range contains the character. Split it.
                SplitRanges(rangeIndex, element);
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
        /// Attempts to merge the range with the next range.
        /// </summary>
        /// <param name="rangeIndex">The zero-based index of the range to try to merge.</param>
        private void TryMergeWithNext(int rangeIndex)
        {
            if (rangeIndex + 1 < this.ranges.Count && this.ranges[rangeIndex].End + 1 == this.ranges[rangeIndex + 1].Start)
            {
                this.ranges[rangeIndex] = new CodePointRange(this.ranges[rangeIndex].Start, this.ranges[rangeIndex + 1].End);
                this.ranges.RemoveAt(rangeIndex + 1);
            }
        }

        /// <summary>
        /// Gets the range that starts after the specified character.
        /// </summary>
        /// <param name="element">The character to look for.</param>
        /// <returns>The zero-based index of the range; or -1 when not found.</returns>
        private int GetRangeThatStartsBeforeOrAt(CodePoint element)
        {
            int i = 0;
            while (i < this.ranges.Count && this.ranges[i].Start <= element)
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
        private int GetRangeThatStartsAfter(CodePoint element)
        {
            int i = 0;
            while (i < this.ranges.Count && this.ranges[i].Start <= element)
            {
                i++;
            }
            return i < this.ranges.Count ? i : -1;
        }

        /// <summary>
        /// Gets the range that ends before the specified character.
        /// </summary>
        /// <param name="element">The character to look for.</param>
        /// <returns>The zero-based index of the range; or -1 when not found.</returns>
        private int GetRangeThatEndsBefore(CodePoint element)
        {
            int i = 0;
            while (i < this.ranges.Count && this.ranges[i].End < element)
            {
                i++;
            }
            return i - 1;
        }

        /// <summary>
        /// Split ranges at the specified character.
        /// </summary>
        /// <param name="rangeIndex">The zero-based index of the range.</param>
        /// <param name="element">The character to split at.</param>
        private void SplitRanges(int rangeIndex, CodePoint element)
        {
            var start = this.ranges[rangeIndex].Start;
            var end = this.ranges[rangeIndex].End;
            var rangeBefore = start < element ? new CodePointRange(start, element - 1) : (CodePointRange?)null;
            var rangeAfter = element < end ? new CodePointRange(element + 1, end) : (CodePointRange?)null;

            if (rangeBefore == null && rangeAfter == null)
                this.ranges.RemoveAt(rangeIndex);
            else if (rangeBefore == null)
                this.ranges[rangeIndex] = (CodePointRange)rangeAfter;
            else if (rangeAfter == null)
                this.ranges[rangeIndex] = (CodePointRange)rangeBefore;
            else
            {
                this.ranges[rangeIndex] = (CodePointRange)rangeBefore;
                this.ranges.Insert(rangeIndex + 1, (CodePointRange)rangeAfter);
            }
        }
        #endregion

        #region Set Mutation
        /// <inheritdoc />
        public void ExceptWith(IEnumerable<CodePoint> other)
        {
            // Every element in this set AND NOT in `other`.
            var elements = this.Except(other).ToArray();
            this.Clear();
            this.AddAll(elements);
        }

        /// <inheritdoc />
        public void UnionWith(IEnumerable<CodePoint> other)
        {
            // Every element in this set OR in `other`.
            var elements = this.Union(other).ToArray();
            this.Clear();
            this.AddAll(elements);
        }

        /// <inheritdoc />
        public void IntersectWith(IEnumerable<CodePoint> other)
        {
            // Every element in this set AND in `other`.
            var elements = this.Intersect(other).ToArray();
            this.Clear();
            this.AddAll(elements);
        }

        /// <inheritdoc />
        public void SymmetricExceptWith(IEnumerable<CodePoint> other)
        {
            // Every element in this set XOR in `other`.
            var codePoints = other as CodePoint[] ?? other.ToArray();
            var elements = this.Union(codePoints).Except(this.Intersect(codePoints)).ToArray();
            this.Clear();
            this.AddAll(elements);
        }
        #endregion

        #region Querying

        /// <inheritdoc />
        public bool Contains(CodePoint element)
        {
            return this.ranges.Any(range => range.Contains(element));
        }

        /// <inheritdoc />
        public bool IsProperSubsetOf(IEnumerable<CodePoint> other)
        {
            var codePoints = other as CodePoint[] ?? other.ToArray();
            return IsSubsetOf(codePoints) && !SetEquals(codePoints);
        }

        /// <inheritdoc />
        public bool IsProperSupersetOf(IEnumerable<CodePoint> other)
        {
            var codePoints = other as CodePoint[] ?? other.ToArray();
            return IsSupersetOf(codePoints) && !SetEquals(codePoints);
        }

        /// <inheritdoc />
        public bool IsSubsetOf(IEnumerable<CodePoint> other)
        {
            return this.Except(other).Any();
        }

        /// <inheritdoc />
        public bool IsSupersetOf(IEnumerable<CodePoint> other)
        {
            return other.Except(this).Any();
        }

        /// <inheritdoc />
        public bool Overlaps(IEnumerable<CodePoint> other)
        {
            return this.Intersect(other).Any();
        }

        /// <inheritdoc />
        public bool SetEquals(IEnumerable<CodePoint> other)
        {
            return this.SequenceEqual(other.OrderBy(c => c));
        }
        #endregion

        /// <summary>
        /// Returns a collection of ranges.
        /// </summary>
        /// <returns>A collection of ranges.</returns>
        public IEnumerable<CodePointRange> GetRanges()
        {
            // NOTE: Not a LINQ expression to prevent casting from this.ranges to a mutable type.
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var range in this.ranges)
            {
                yield return range;
            }
        }

        /// <inheritdoc />
        public void CopyTo(CodePoint[] array, int arrayIndex)
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

        /// <inheritdoc />
        public IEnumerator<CodePoint> GetEnumerator()
        {
            foreach (var range in this.ranges)
            {
                for (var element = range.Start; range.Start <= element && element <= range.End; element++)
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
            return $"[{String.Join(",", this.ranges)}]";
        }
    }
}
