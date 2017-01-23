using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Yargon.Parser.Sdf
{
    /// <summary>
    /// An interval.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    public struct Interval<T> : IEquatable<Interval<T>>
        where T : IComparable<T>
    {
        /// <summary>
        /// Gets the inclusive start of the interval.
        /// </summary>
        /// <value>The inclusive start.</value>
        public T Start { get; }

        /// <summary>
        /// Gets the exclusive end of the interval.
        /// </summary>
        /// <value>The exclusive end.</value>
        public T End { get; }

        /// <summary>
        /// Gets whether the interval is empty.
        /// </summary>
        /// <value><see langword="true"/> when the interval is empty;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsEmpty => this.Start.CompareTo(this.End) == 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval{T}"/> class.
        /// </summary>
        /// <param name="start">The inclusive start of the interval.</param>
        /// <param name="end">The exclusive end of the interval.</param>
        public Interval(T start, T end)
        {
            #region Contract
            if (start.CompareTo(end) > 0)
                throw new ArgumentOutOfRangeException(nameof(start));
            #endregion

            this.Start = start;
            this.End = end;
        }

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Interval<T> && Equals((Interval<T>)obj);

        /// <inheritdoc />
        public bool Equals(Interval<T> other)
        {
            return this.Start.CompareTo(other.Start) == 0
                   && this.End.CompareTo(other.End) == 0;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Start.GetHashCode();
                hash = hash * 29 + this.End.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Interval{T}"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Interval<T> left, Interval<T> right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Interval{T}"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Interval<T> left, Interval<T> right) => !(left == right);
        #endregion

        /// <summary>
        /// Determines whether the specified value is in the interval.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns><see langword="true"/> when the value is in the interval;
        /// otherwise, <see langword="false"/>.</returns>
        [Pure]
        public bool Contains(T value)
        {
            return this.Start.CompareTo(value) <= 0
                   && value.CompareTo(this.End) < 0;
        }

        /// <summary>
        /// Determines whether the specified interval intersects this interval.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns><see langword="true"/> when the other interval intersects this interval;
        /// otherwise, <see langword="false"/>.</returns>
        [Pure]
        public bool Intersects(Interval<T> other)
        {
            // FIXME: This is wrong.
            return Contains(other.Start)
                   || Contains(other.End);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[{this.Start}, {this.End})";
        }
    }
}
