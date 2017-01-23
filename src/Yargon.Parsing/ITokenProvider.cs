using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yargon.Parsing;

namespace Yargon.Parsing
{
    /// <summary>
    /// Provides the tokens.
    /// </summary>
    /// <typeparam name="TToken">The type of tokens.</typeparam>
    public interface ITokenProvider<TToken> : IDisposable
        where TToken : IToken
    {
        /// <summary>
        /// Gets the current token.
        /// </summary>
        /// <value>The current token;
        /// or the default of <typeparamref name="T"/> when the provider is positioned
        /// before the first or after the last token.</value>
        TToken Current { get; }

        /// <summary>
        /// Moves to the next token.
        /// </summary>
        /// <returns><see langword="true"/> if the provider moved to the next token;
        /// otherwise, <see langword="false"/> when the provider has moved past the last token.</returns>
        /// <remarks>
        /// Initially the provider is positioned before the first token.
        /// </remarks>
        bool MoveNext();
    }
}
