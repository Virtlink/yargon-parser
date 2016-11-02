using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// A parse tree token.
    /// </summary>
    public struct ParseTreeToken<T> : IParseTree
    {
        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <value>The token.</value>
        public TypedToken<T> Token { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseTreeToken{T}"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ParseTreeToken(TypedToken<T> token)
        {
            this.Token = token;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is ParseTreeToken<T> && Equals((ParseTreeToken<T>)obj);

        /// <inheritdoc />
        public bool Equals(ParseTreeToken<T> other) => this.Token == other.Token;

        /// <inheritdoc />
        public override int GetHashCode() => this.Token.GetHashCode();

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="ParseTreeToken{T}"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(ParseTreeToken<T> left, ParseTreeToken<T> right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="ParseTreeToken{T}"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(ParseTreeToken<T> left, ParseTreeToken<T> right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString() => this.Token.ToString();
    }
}
