using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// A simple token implementation.
    /// </summary>
    public struct Token : IParseTree, IToken, IEquatable<Token>
    {
        /// <summary>
        /// Gets the text in the token.
        /// </summary>
        /// <value>The token text.</value>
        public string Text { get; }

        /// <summary>
        /// Gets the type of token.
        /// </summary>
        /// <value>The token type.</value>
        public TokenType Type { get; }

        /// <inheritdoc />
        ITokenType IToken.Type => this.Type;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="type">The type of token.</param>
        /// <param name="text">The token text.</param>
        public Token(TokenType type, string text)
        {
            #region Contract
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            #endregion

            this.Type = type;
            this.Text = text;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(Token other)
        {
            return Object.Equals(this.Type, other.Type)
                && this.Text == other.Text;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Type?.GetHashCode() ?? 0;
                hash = hash * 29 + this.Text?.GetHashCode() ?? 0;
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Token && Equals((Token)obj);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Token"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Token left, Token right)
        {
            return Object.Equals(left, right);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Token"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Token left, Token right)
        {
            return !(left == right);
        }
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            // TODO: Escape
            return "\"" + (this.Text ?? String.Empty) + "\"";
        }
    }
}
