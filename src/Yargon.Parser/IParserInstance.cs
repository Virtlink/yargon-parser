using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yargon.Parsing;

namespace Yargon.Parser
{
    /// <summary>
    /// A parser instance.
    /// </summary>
    public interface IParserInstance<TState, TToken>
            where TToken : IToken
    {
        /// <summary>
        /// Gets the token provider.
        /// </summary>
        /// <value>The token provider.</value>
        ITokenProvider<TToken> TokenProvider { get; }

        /// <summary>
        /// Gets the stacks in the parser.
        /// </summary>
        /// <value>The stacks.</value>
        Stacks<TState> Stacks { get; }

        /// <summary>
        /// Gets the messages produced by the parser.
        /// </summary>
        /// <value>A collection of messages.</value>
        ICollection<IMessage> Messages { get; }

        /// <summary>
        /// Attempts to reduce all the stacks as much as possible.
        /// </summary>
        /// <param name="lookahead">The lookahead token.</param>
        /// <returns><see langword="true"/> when some stack was reduced;
        /// otherwise, <see langword="false"/> when no stack was reduced.</returns>
        /// <remarks>
        /// This method attempts to reduce each currently active stack.
        /// When this method returns <see langword="true"/>, the reduced stacks have been
        /// added to the set of active stacks. When this method returns <see langword="false"/>,
        /// the stacks are unchanged. The new stack top states have links to some earlier
        /// state, labelled with the sort of the reduction.
        /// </remarks>
        bool TryReduce(TToken lookahead);

        /// <summary>
        /// Attempts to shift the specified token on top of all the stacks.
        /// </summary>
        /// <param name="token">The token to shift.</param>
        /// <returns><see langword="true"/> when some stack accepted the token;
        /// otherwise, <see langword="false"/> when no stack accepted the token.</returns>
        /// <remarks>
        /// This method attempts to shift the token onto each currently active stack.
        /// When this method returns <see langword="true"/>, only the stacks that accepted
        /// the token remain. When this method returns <see langword="false"/>,
        /// the stacks are unchanged. The new stack top states each have links to the previous
        /// state, labelled with the token that was shifted.
        /// </remarks>
        bool TryShift(TToken token);
    }
}
