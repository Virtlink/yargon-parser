using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yargon.Parsing;
using Virtlink.Utilib.Collections;

namespace Yargon.Parser.Sdf
{
    partial class SdfParseTable
    {
        /// <summary>
        /// A collection of SGLR reductions.
        /// </summary>
        public sealed class ReductionCollection
        {
            private readonly List<List<Tuple<CodePointSet, ListSet<Production>>>> productions = new List<List<Tuple<CodePointSet, ListSet<Production>>>>();

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the <see cref="ReductionCollection"/> class.
            /// </summary>
            public ReductionCollection()
            {
            }
            #endregion

            /// <summary>
            /// Gets the possible reductions for a given current state and lookahead.
            /// </summary>
            /// <param name="state">The current state.</param>
            /// <param name="lookahead">The lookahead.</param>
            /// <returns>A possibly empty list of productions.</returns>
            public IReadOnlyList<Production> Get(SdfStateRef state, CodePoint lookahead)
            {
                if (state.Index >= this.productions.Count)
                {
                    // No reductions.
                    return List.Empty<Production>();
                }
                
                return this.productions[state.Index]
                    .Where(t => t.Item1.Contains(lookahead))
                    .SelectMany(t => t.Item2)
                    .ToList();
            }

            /// <summary>
            /// Sets the possible reductions for the given current state and lookahead.
            /// </summary>
            /// <param name="state">The current state.</param>
            /// <param name="lookaheads">The lookahead tokens.</param>
            /// <param name="productions">The productions.</param>
            public void Set(SdfStateRef state, CodePointSet lookaheads, IEnumerable<Production> productions)
            {
                #region Contract
                if (productions == null)
                    throw new ArgumentNullException(nameof(productions));
                if (lookaheads == null)
                    throw new ArgumentNullException(nameof(lookaheads));
                #endregion

                Debug.Assert(state.Index >= 0);

                // Ensure there are at least enough states in the list.
                while (this.productions.Count <= state.Index)
                    this.productions.Add(new List<Tuple<CodePointSet, ListSet<Production>>>());
                
                var stateGotos = this.productions[state.Index];

                // Ensure there aren't any overlapping code points for this state.
                if (stateGotos.Any(t => t.Item1.Overlaps(lookaheads)))
                    throw new InvalidOperationException(
                        $"For the given state there is already a set of code points that overlaps with the given set. {state}: {lookaheads}");

                stateGotos.Add(Tuple.Create(lookaheads, new ListSet<Production>(productions)));
            }
        }
    }
}
