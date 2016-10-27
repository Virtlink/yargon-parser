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
    public class SdfParseTable : IParseTable<int>
    {

        /// <summary>
        /// Gets the initial state of the parser.
        /// </summary>
        /// <value>The initial state of the parser.</value>
        public SdfStateRef InitialState { get; }

        /// <summary>
        /// Gets the states in the parse table.
        /// </summary>
        /// <value>A list of states.</value>
        /// <remarks>
        /// Any state index refers to a state in this list.
        /// </remarks>
        public IReadOnlyList<State> States { get; }

        /// <summary>
        /// Gets the production rules.
        /// </summary>
        /// <value>A list of production rules.</value>
        /// <remarks>
        /// Any label index refers to a production rule in this list.
        /// </remarks>
        public IReadOnlyList<Production> Productions { get; }

        /// <summary>
        /// Gets the priorities of production rules.
        /// </summary>
        /// <value>A list of priorities.</value>
        public IReadOnlyList<Priority> Priorities { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasRejects
        {
            // TODO: If there are any reject productions.
            get { return true; }
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SdfParseTable"/> class.
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        /// <param name="states">The states.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="priorities">The priorities.</param>
        public SdfParseTable(SdfStateRef initialState, IReadOnlyList<State> states, IReadOnlyList<Production> productions, IReadOnlyList<Priority> priorities)
        {
            #region Contract
            if (states == null)
                throw new ArgumentNullException(nameof(states));
            if (productions == null)
                throw new ArgumentNullException(nameof(productions));
            if (priorities == null)
                throw new ArgumentNullException(nameof(priorities));
            #endregion

            this.InitialState = initialState;
            this.States = states.ToArray();
            this.Productions = productions.ToArray();
            this.Priorities = priorities.ToArray();
        }
        #endregion

        /// <inheritdoc />
        public int StartState { get; }
        
        /// <inheritdoc />
        public bool TryGetShift(int state, ITokenType token, out int nextState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool TryGetGoto(int state, ISort label, out int nextState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<IReduction> GetReductions(int state, ITokenType lookahead)
        {
            throw new NotImplementedException();
        }
    }
}
