using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Yargon.Parsing
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
        /// and the next states to go to after shifting.
        /// </summary>
        /// <param name="state">The current state.</param>
        /// <param name="token">The token to shift.</param>
        /// <returns>A non-empty set of states if the shift is possible;
        /// otherwise, an empty set of states if the shift is not possible.</returns>
        /// <remarks>
        /// Generally when a shift is possible the set should contain only one state,
        /// as multiple states indicate a shift/shift conflict. However, low quality
        /// parse tables may have more than one state after a shift. This does not
        /// necessarily have to be supported by the parser, as they can be eliminated.
        /// </remarks>
        IEnumerable<TState> GetShifts(TState state, TToken token);

        /// <summary>
        /// Gets the next states to go to after reducing.
        /// </summary>
        /// <param name="state">The top state to which to append the reduction.</param>
        /// <param name="label">The label of the reduction.</param>
        /// <returns>A non-empty set of states if the goto is possible;
        /// otherwise, an empty set of states if the goto is not possible.</returns>
        /// <remarks>
        /// Generally when a goto is possible the set should contain only one goto,
        /// as multiple gotos indicate two or more transitions with the same label
        /// leading out from the same state. However, low quality parse tables may have
        /// more than one goto state. This does not necessarily have to be supported
        /// by the parser, as they can be eliminated.
        /// </remarks>
        IEnumerable<TState> GetGotos(TState state, ISort label);

        /// <summary>
        /// Gets all productions that can reduce from the specified state
        /// with the specified lookahead token type.
        /// </summary>
        /// <param name="state">The current state.</param>
        /// <param name="lookahead">The lookahead token.</param>
        /// <returns>A non-empty set of reductions if a reduction is possible;
        /// otherwise, an empty set of reductions if no reductions are possible.</returns>
        /// <remarks>
        /// When this method returns more than one reduction, the state has a reduce/reduce
        /// conflict. Generalized parsers will support this.
        /// </remarks>
        IEnumerable<IReduction> GetReductions(TState state, TToken lookahead);
    }
}
