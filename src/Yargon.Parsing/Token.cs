using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Yargon.Parsing
{
    /// <summary>
    /// A token.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    public struct Token<T> : IToken
    {
        /// <summary>
        /// Gets the value of the token.
        /// </summary>
        /// <value>The token value.</value>
        [CanBeNull]
        public T Value { get; }

        /// <summary>
        /// Gets the location of the token in the source.
        /// </summary>
        /// <value>The token location; or <see langword="null"/> when the token is virtual.</value>
        [CanBeNull]
        public SourceRange? Location { get; }

        /// <summary>
        /// Gets whether the token is virtual.
        /// </summary>
        /// <value><see langword="true"/> when the token is virtual;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsVirtual => this.Location == null;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Token{T}"/> class.
        /// </summary>
        /// <param name="value">The token value.</param>
        /// <param name="location">The token location; or <see langword="null"/> for a virtual token.</param>
        public Token([CanBeNull] T value, [CanBeNull] SourceRange? location)
        {
            this.Value = value;
            this.Location = location;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Token<T> && Equals((Token<T>)obj);

        /// <inheritdoc />
        public bool Equals(Token<T> other)
        {
            return Object.Equals(this.Value, other.Value)
                   && this.Location == other.Location;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Value?.GetHashCode() ?? 0;
                hash = hash * 29 + this.Location?.GetHashCode() ?? 0;
            }
            return hash;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Token{T}"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Token<T> left, Token<T> right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Token{T}"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Token<T> left, Token<T> right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            // TODO: Escape
            return this.Value != null ? "'" + this.Value + "'" : "";
        }
    }
}