using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parsing
{
    /// <summary>
    /// A source location.
    /// </summary>
    public struct SourceLocation
    {
        /// <summary>
        /// Gets the zero-based character offset in the source.
        /// </summary>
        /// <value>The zero-based character offset.</value>
        public int Offset { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceLocation"/> class.
        /// </summary>
        /// <param name="offset">The zero-based character offset.</param>
        public SourceLocation(int offset)
        {
            #region Contract
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            #endregion

            this.Offset = offset;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is SourceLocation && Equals((SourceLocation)obj);

        /// <inheritdoc />
        public bool Equals(SourceLocation other)
        {
            return this.Offset == other.Offset;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Offset.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="SourceLocation"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(SourceLocation left, SourceLocation right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="SourceLocation"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(SourceLocation left, SourceLocation right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString() => $"@{this.Offset}";
    }
}
