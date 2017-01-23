using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yargon.Parsing
{
    /// <summary>
    /// A source range.
    /// </summary>
    public struct SourceRange
    {
        /// <summary>
        /// Gets the inclusive start of the range.
        /// </summary>
        /// <value>The inclusive start.</value>
        public SourceLocation Start { get; }

        /// <summary>
        /// Gets the inclusive end of the range.
        /// </summary>
        /// <value>The inclusive end.</value>
        public SourceLocation End { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceRange"/> class.
        /// </summary>
        /// <param name="start">The inclusive start.</param>
        /// <param name="end">The inclusive end.</param>
        public SourceRange(SourceLocation start, SourceLocation end)
        {
            this.Start = start;
            this.End = end;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is SourceRange && Equals((SourceRange)obj);

        /// <inheritdoc />
        public bool Equals(SourceRange other)
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
        /// Returns a value that indicates whether two specified <see cref="SourceRange"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(SourceRange left, SourceRange right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="SourceLocation"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(SourceRange left, SourceRange right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString() => $"{this.Start}-{this.End}";
    }
}
