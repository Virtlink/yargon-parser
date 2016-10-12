using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Slang.Parsing
{
    /// <summary>
    /// A parse table.
    /// </summary>
    /// <typeparam name="TState">The type of states.</typeparam>
    /// <typeparam name="TToken">The type of tokens.</typeparam>
    public interface IParseTable<TState, TToken>
        where TToken : IToken
    {
        /// <summary>
        /// Gets the start state of the parser.
        /// </summary>
        /// <value>The start state.</value>
        TState StartState { get; }

        /// <summary>
        /// Gets whether the specified token can be shifted onto the specified state,
        /// and the next state to go to after shifting.
        /// </summary>
        /// <param name="state">The current state.</param>
        /// <param name="token">The token type to shift.</param>
        /// <param name="nextState">The next state to go to.</param>
        /// <returns><see langword="true"/> if the shift is possible;
        /// otherwise, <see langword="false"/>.</returns>
        bool TryGetShift(TState state, ITokenType token, out TState nextState);

        /// <summary>
        /// Gets the next state to go to after reducing.
        /// </summary>
        /// <param name="state">The top state to which to append the reduction.</param>
        /// <param name="label">The label of the reduction.</param>
        /// <param name="nextState">The next state to go to.</param>
        /// <returns><see langword="true"/> if the goto is possible;
        /// otherwise, <see langword="false"/>.</returns>
        bool TryGetGoto(TState state, ISort label, out TState nextState);

        /// <summary>
        /// Gets all productions that can reduce from the specified state
        /// with the specified lookahead token type.
        /// </summary>
        /// <param name="state">The current state.</param>
        /// <param name="lookahead">The lookahead token type.</param>
        /// <returns>An enumerable sequence of productions;
        /// or an empty sequence when no reductions are possible.</returns>
        IEnumerable<IReduction> GetReductions(TState state, ITokenType lookahead);
    }
}
