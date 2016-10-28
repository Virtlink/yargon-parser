using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser.Sdf
{
    /// <summary>
    /// An SDF parse table.
    /// </summary>
    public partial class SdfParseTable : IParseTable<SdfStateRef>
    {

//        /// <summary>
//        /// Gets the initial state of the parser.
//        /// </summary>
//        /// <value>The initial state of the parser.</value>
//        public SdfStateRef InitialState { get; }
//
//        /// <summary>
//        /// Gets the states in the parse table.
//        /// </summary>
//        /// <value>A list of states.</value>
//        /// <remarks>
//        /// Any state index refers to a state in this list.
//        /// </remarks>
//        public IReadOnlyList<State> States { get; }
//
//        /// <summary>
//        /// Gets the production rules.
//        /// </summary>
//        /// <value>A list of production rules.</value>
//        /// <remarks>
//        /// Any label index refers to a production rule in this list.
//        /// </remarks>
//        public IReadOnlyList<Production> Productions { get; }
//
//        /// <summary>
//        /// Gets the priorities of production rules.
//        /// </summary>
//        /// <value>A list of priorities.</value>
//        public IReadOnlyList<Priority> Priorities { get; }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        public bool HasRejects
//        {
//            // TODO: If there are any reject productions.
//            get { return true; }
//        }
//
//        #region Constructors
//        /// <summary>
//        /// Initializes a new instance of the <see cref="SdfParseTable"/> class.
//        /// </summary>
//        /// <param name="initialState">The initial state.</param>
//        /// <param name="states">The states.</param>
//        /// <param name="labels">The labels.</param>
//        /// <param name="priorities">The priorities.</param>
//        public SdfParseTable(SdfStateRef initialState, IReadOnlyList<State> states, IReadOnlyList<Production> productions, IReadOnlyList<Priority> priorities)
//        {
//            #region Contract
//            if (states == null)
//                throw new ArgumentNullException(nameof(states));
//            if (productions == null)
//                throw new ArgumentNullException(nameof(productions));
//            if (priorities == null)
//                throw new ArgumentNullException(nameof(priorities));
//            #endregion
//
//            this.InitialState = initialState;
//            this.States = states.ToArray();
//            this.Productions = productions.ToArray();
//            this.Priorities = priorities.ToArray();
//        }
//        #endregion
//
//        // ---

        /// <summary>
        /// A collection with for each state possible shifts.
        /// </summary>
        private readonly ShiftCollection shifts;

        /// <summary>
        /// A list with for each state a dictionary mapping a sort to the next state.
        /// </summary>
        private readonly GotoCollection gotos;

        /// <summary>
        /// A list with for each state a list of possible productions.
        /// </summary>
        private readonly ReductionCollection reductions;

        /// <inheritdoc />
        public SdfStateRef StartState { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SdfParseTable"/> class.
        /// </summary>
        /// <param name="startState">The start state.</param>
        /// <param name="shifts">The shifts.</param>
        /// <param name="gotos">The gotos.</param>
        /// <param name="reductions">The reductions.</param>
        public SdfParseTable(SdfStateRef startState,
            ShiftCollection shifts,
            GotoCollection gotos,
            ReductionCollection reductions)
        {
            #region Contract
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
        public bool TryGetShift(SdfStateRef state, ITokenType token, out SdfStateRef nextState)
        {
            #region Contract
            if (token == null)
                throw new ArgumentNullException(nameof(token));
            #endregion

            return this.shifts.TryGet(state, ((TokenType) token).CodePoint, out nextState);
        }

        /// <inheritdoc />
        public bool TryGetGoto(SdfStateRef state, ISort label, out SdfStateRef nextState)
        {
            #region Contract
            if (label == null)
                throw new ArgumentNullException(nameof(label));
            #endregion

            return this.gotos.TryGet(state, label, out nextState);
        }

        /// <inheritdoc />
        public IEnumerable<IReduction> GetReductions(SdfStateRef state, ITokenType lookahead)
        {
            #region Contract
            if (lookahead == null)
                throw new ArgumentNullException(nameof(lookahead));
            #endregion

            return this.reductions.Get(state, ((TokenType) lookahead).CodePoint);
        }
    }
}
