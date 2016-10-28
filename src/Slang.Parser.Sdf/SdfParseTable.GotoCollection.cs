using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser.Sdf
{
    partial class SdfParseTable
    {
        /// <summary>
        /// A collection of SGLR gotos.
        /// </summary>
        public sealed class GotoCollection
        {
            private readonly List<Dictionary<ISort, SdfStateRef>> gotos = new List<Dictionary<ISort, SdfStateRef>>();

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the <see cref="GotoCollection"/> class.
            /// </summary>
            public GotoCollection()
            {
            }
            #endregion

            /// <summary>
            /// Gets the state to go to for a given current state and sort.
            /// </summary>
            /// <param name="state">The current state.</param>
            /// <param name="sort">The current sort.</param>
            /// <param name="nextState">The next state.</param>
            /// <returns><see langword="true"/> when there is a goto;
            /// otherwise, <see langword="false"/>.</returns>
            public bool TryGet(SdfStateRef state, ISort sort, out SdfStateRef nextState)
            {
                #region Contract
                if (sort == null)
                    throw new ArgumentNullException(nameof(sort));
                #endregion

                if (state.Index >= this.gotos.Count)
                {
                    // No goto.
                    nextState = default(SdfStateRef);
                    return false;
                }

                return this.gotos[state.Index].TryGetValue(sort, out nextState);
            }

            /// <summary>
            /// Add the state to go to for the given current state and sort.
            /// </summary>
            /// <param name="state">The current state.</param>
            /// <param name="sort">The current sort.</param>
            /// <param name="nextState">The next state.</param>
            public void Add(SdfStateRef state, ISort sort, SdfStateRef nextState)
            {
                #region Contract
                if (sort == null)
                    throw new ArgumentNullException(nameof(sort));
                #endregion

                Debug.Assert(state.Index >= 0);

                // Ensure there are at least enough states in the list.
                while (this.gotos.Count <= state.Index)
                    this.gotos.Add(new Dictionary<ISort, SdfStateRef>());

                var stateGotos = this.gotos[state.Index];

                SdfStateRef existingNextState;
                if (stateGotos.TryGetValue(sort, out existingNextState) && !existingNextState.Equals(nextState))
                {
                    throw new InvalidOperationException(
                        $"For the given state there is already a goto for the given sort. {state}: {sort}");
                }
                
                stateGotos[sort] = nextState;
            }
        }
    }
}
