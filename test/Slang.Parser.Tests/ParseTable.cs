using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Slang.Parsing;

namespace Slang.Parser.Tests
{
    /// <summary>
    /// A parse table.
    /// </summary>
    public sealed class ParseTable : IParseTable<State, Token>
    {
        /// <summary>
        /// The goto-table.
        /// </summary>
        private readonly Dictionary<Tuple<State, ISymbol>, State> gotos;

        /// <summary>
        /// The reduction table.
        /// </summary>
        private readonly Dictionary<Tuple<State, ITokenType>, IReadOnlyCollection<IReduction>> reductions;

        /// <inheritdoc />
        public State StartState { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseTable"/> class.
        /// </summary>
        public ParseTable(State startState, Dictionary<Tuple<State, ISymbol>, State> gotos, Dictionary<Tuple<State, ITokenType>, IReadOnlyCollection<IReduction>> reductions)
        {
            #region Contract
            if (startState == null)
                throw new ArgumentNullException(nameof(startState));
            if (gotos == null)
                throw new ArgumentNullException(nameof(gotos));
            if (reductions == null)
                throw new ArgumentNullException(nameof(reductions));
            #endregion

            this.StartState = startState;
            this.gotos = gotos;
            this.reductions = reductions;
        }
        #endregion

        /// <inheritdoc />
        public bool TryGetShift(State state, ITokenType token, out State nextState)
        {
            #region Contract
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (token == null)
                throw new ArgumentNullException(nameof(token));
            #endregion

            return this.gotos.TryGetValue(Tuple.Create(state, (ISymbol)token), out nextState);
        }

        /// <inheritdoc />
        public bool TryGetGoto(State state, ISort label, out State nextState)
        {
            #region Contract
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (label == null)
                throw new ArgumentNullException(nameof(label));
            #endregion

            return this.gotos.TryGetValue(Tuple.Create(state, (ISymbol)label), out nextState);
        }

        /// <inheritdoc />
        public IEnumerable<IReduction> GetReductions(State state, ITokenType lookahead)
        {
            #region Contract
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (lookahead == null)
                throw new ArgumentNullException(nameof(lookahead));
            #endregion

            IReadOnlyCollection<IReduction> reductionList;
            if (!this.reductions.TryGetValue(Tuple.Create(state, lookahead), out reductionList))
                return Enumerable.Empty<IReduction>();
            return reductionList;
        }
    }
}
