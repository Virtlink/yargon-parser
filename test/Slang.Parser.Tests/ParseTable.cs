using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// A parse table.
    /// </summary>
    public sealed class ParseTable : IParseTable<State, TypedToken<String>>
    {
        /// <summary>
        /// The goto-table.
        /// </summary>
        private readonly Dictionary<Tuple<State, ISort>, State> gotos;

        /// <summary>
        /// The goto-table.
        /// </summary>
        private readonly Dictionary<Tuple<State, ITokenType>, State> shifts;

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
        /// <param name="startState">The start state.</param>
        /// <param name="shifts">The shifts.</param>
        /// <param name="gotos">The gotos.</param>
        /// <param name="reductions">The reductions.</param>
        public ParseTable(State startState, Dictionary<Tuple<State, ITokenType>, State> shifts, Dictionary<Tuple<State, ISort>, State> gotos, Dictionary<Tuple<State, ITokenType>, IReadOnlyCollection<IReduction>> reductions)
        {
            #region Contract
            if (startState == null)
                throw new ArgumentNullException(nameof(startState));
            if (shifts == null)
                throw new ArgumentNullException(nameof(shifts));
            if (gotos == null)
                throw new ArgumentNullException(nameof(gotos));
            if (reductions == null)
                throw new ArgumentNullException(nameof(reductions));
            #endregion

            this.StartState = startState;
            this.shifts = shifts;
            this.gotos = gotos;
            this.reductions = reductions;
        }
        #endregion

        /// <inheritdoc />
        public IEnumerable<State> GetShifts(State state, TypedToken<String> token)
        {
            #region Contract
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (token == null)
                throw new ArgumentNullException(nameof(token));
            #endregion

            State nextState;
            if (!this.shifts.TryGetValue(Tuple.Create(state, token.Type), out nextState))
                return Enumerable.Empty<State>();
            return new[] { nextState };
        }

        /// <inheritdoc />
        public IEnumerable<State> GetGotos(State state, ISort label)
        {
            #region Contract
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (label == null)
                throw new ArgumentNullException(nameof(label));
            #endregion

            State nextState;
            if (!this.gotos.TryGetValue(Tuple.Create(state, label), out nextState))
                return Enumerable.Empty<State>();
            return new[] { nextState };
        }

        /// <inheritdoc />
        public IEnumerable<IReduction> GetReductions(State state, TypedToken<String> lookahead)
        {
            #region Contract
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (lookahead == null)
                throw new ArgumentNullException(nameof(lookahead));
            #endregion

            IReadOnlyCollection<IReduction> reductionList;
            if (!this.reductions.TryGetValue(Tuple.Create(state, lookahead.Type), out reductionList))
                return Enumerable.Empty<IReduction>();
            return reductionList;
        }
    }
}
