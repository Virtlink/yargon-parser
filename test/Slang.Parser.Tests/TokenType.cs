using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// A type of token.
    /// </summary>
    public sealed class TokenType : ITokenType
    {
        /// <summary>
        /// Gets the name of the token type.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks>This is for debugging purposes only.</remarks>
        public string Name { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenType"/> class.
        /// </summary>
        /// <param name="name">The name of the token type.</param>
        public TokenType(string name)
        {
            #region Contract
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            #endregion

            this.Name = name;
        }
        #endregion

        /// <inheritdoc />
        public override bool Equals(object obj) => Object.ReferenceEquals(this, obj);

        /// <inheritdoc />
        public override int GetHashCode() => RuntimeHelpers.GetHashCode(this);

        /// <inheritdoc />
        public override string ToString() => this.Name;
    }
}
