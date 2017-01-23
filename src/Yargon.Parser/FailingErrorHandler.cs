using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yargon.Parsing;

namespace Yargon.Parser
{
    /// <summary>
    /// A failing error handler.
    /// </summary>
    /// <typeparam name="TState">The type of states.</typeparam>
    /// <typeparam name="TToken">The type of tokens.</typeparam>
    public class FailingErrorHandler<TState, TToken> : IErrorHandler<TState, TToken>
        where TToken : IToken
    {
        /// <inheritdoc />
        public bool Handle(TToken token, IParserInstance<TState, TToken> parserInstance)
        {
            #region Contract
            if (parserInstance == null)
                throw new ArgumentNullException(nameof(parserInstance));
            #endregion
            
            parserInstance.Messages.Add(new Message(MessageKind.Error, "Unexpected: " + token, token.Location));

            return false;
        }
    }
}
