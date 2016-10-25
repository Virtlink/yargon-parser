using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// Provides the tokens.
    /// </summary>
    public interface ITokenProvider<T> : IDisposable
    {
        /// <summary>
        /// Gets the current token.
        /// </summary>
        /// <value>The current token;
        /// or the default of <typeparamref name="T"/> when the provider is positioned
        /// before the first or after the last token.</value>
        Token<T> Current { get; }

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
