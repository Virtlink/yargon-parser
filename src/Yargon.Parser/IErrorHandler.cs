using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yargon.Parsing;

namespace Yargon.Parser
{
    /// <summary>
    /// Handles parse errors.
    /// </summary>
    public interface IErrorHandler<TState, TToken>
        where TToken : IToken
    {
        /// <summary>
        /// Handles a parse error.
        /// </summary>
        /// <param name="token">The token that could not be shifted.</param>
        /// <param name="parserInstance">The parser instance.</param>
        /// <returns><see langword="true"/> if the parse error was recovered and the parser may continue;
        /// otherwise, <see langword="false"/> when recovery failed and parsing must stop.</returns>
        bool Handle(TToken token, IParserInstance<TState, TToken> parserInstance);
    }
}
