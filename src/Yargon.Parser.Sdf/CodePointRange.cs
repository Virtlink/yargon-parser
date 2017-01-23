using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Yargon.Parser.Sdf
{
    /// <summary>
	/// A code point range.
	/// </summary>
	public struct CodePointRange : IEquatable<CodePointRange>
    {
        /// <summary>
        /// Gets the inclusive start of the range.
        /// </summary>
        /// <value>The inclusive start.</value>
        public CodePoint Start { get; }

        /// <summary>
        /// Gets the inclusive end of the interval.
        /// </summary>
        /// <value>The inclusive end.</value>
        public CodePoint End { get; }

        /// <summary>
        /// Gets the length of the range.
        /// </summary>
        /// <value>The number of code points in the range.</value>
        public int Length => this.End - this.Start + 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodePointRange"/> class.
        /// </summary>
        /// <param name="start">The inclusive start of the range.</param>
        /// <param name="end">The inclusive end of the range.</param>
        public CodePointRange(CodePoint start, CodePoint end)
            : this()
        {
            #region Contract
            if (start > end)
                throw new ArgumentOutOfRangeException(nameof(start));
            #endregion

            this.Start = start;
            this.End = end;
        }

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is CodePointRange && Equals((CodePointRange)obj);

        /// <inheritdoc />
        public bool Equals(CodePointRange other)
        {
            return this.Start == other.Start
                && this.End == other.End;
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
        /// Returns a value that indicates whether two specified <see cref="CodePointRange"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(CodePointRange left, CodePointRange right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="CodePointRange"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(CodePointRange left, CodePointRange right) => !(left == right);
        #endregion

        /// <summary>
        /// Determines whether the specified code point is in the range.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns><see langword="true"/> when the value is in the range;
        /// otherwise, <see langword="false"/>.</returns>
        [Pure]
        public bool Contains(CodePoint value)
        {
            return this.Start <= value
                && value <= this.End;
        }

        /// <summary>
        /// Determines whether the specified range intersects this range.
        /// </summary>
        /// <param name="other">The other range.</param>
        /// <returns><see langword="true"/> when the other range intersects this range;
        /// otherwise, <see langword="false"/>.</returns>
        [Pure]
        public bool Intersects(CodePointRange other)
        {
            return Contains(other.Start)
                || Contains(other.End);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (this.Start == this.End)
                return $"{this.Start:d}";
            else
                return $"{this.Start:d}-{this.End:d}";
        }
    }
}
