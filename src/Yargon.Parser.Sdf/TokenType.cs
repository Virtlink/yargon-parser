using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yargon.Parsing;

namespace Yargon.Parser.Sdf
{
    /// <summary>
    /// A token type.
    /// </summary>
    public struct TokenType : ITokenType, IEquatable<TokenType>
    {
        /// <summary>
        /// Gets the code point in the token type.
        /// </summary>
        /// <value>The <see cref="CodePoint"/>.</value>
        public CodePoint CodePoint { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenType"/> class.
        /// </summary>
        /// <param name="codePoint">The code point.</param>
        public TokenType(CodePoint codePoint)
        {
            this.CodePoint = codePoint;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(TokenType other)
        {
            return this.CodePoint == other.CodePoint;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.CodePoint.GetHashCode();
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is TokenType && Equals((TokenType)obj);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="TokenType"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(TokenType left, TokenType right) =>  Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="TokenType"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(TokenType left, TokenType right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return this.CodePoint.ToString();
        }
    }
}
