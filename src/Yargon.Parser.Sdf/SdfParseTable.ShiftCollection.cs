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
        /// A collection of SGLR shifts.
        /// </summary>
        public sealed class ShiftCollection
        {
            private readonly List<List<Tuple<CodePointSet, ListSet<SdfStateRef>>>> shifts =
                new List<List<Tuple<CodePointSet, ListSet<SdfStateRef>>>>();

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the <see cref="ShiftCollection"/> class.
            /// </summary>
            public ShiftCollection()
            {
            }
            #endregion

            /// <summary>
            /// Gets the possible states for a given current state and lookahead.
            /// </summary>
            /// <param name="state">The current state.</param>
            /// <param name="token">The token to shift.</param>
            /// <returns>A possibly empty list of states to go to after the shift.</returns>
            public IReadOnlyCollection<SdfStateRef> Get(SdfStateRef state, CodePoint token)
            {
                if (state.Index >= this.shifts.Count)
                {
                    // No shift.
                    return Collection.Empty<SdfStateRef>();
                }
                
                return this.shifts[state.Index]
                    .Where(t => t.Item1.Contains(token))
                    .SelectMany(t => t.Item2)
                    .Distinct()
                    .ToList();
            }

            /// <summary>
            /// Sets the state to shift to for the given current state and set of tokens.
            /// </summary>
            /// <param name="state">The current state.</param>
            /// <param name="tokens">The tokens to shift.</param>
            /// <param name="nextState">The next state.</param>
            public void Set(SdfStateRef state, CodePointSet tokens, SdfStateRef nextState)
            {
                #region Contract
                if (tokens == null)
                    throw new ArgumentNullException(nameof(tokens));
                #endregion

                Debug.Assert(state.Index >= 0);

                while (this.shifts.Count <= state.Index)
                {
                    this.shifts.Add(new List<Tuple<CodePointSet, ListSet<SdfStateRef>>>());
                }

                var shiftList = this.shifts[state.Index];
                var tuple = shiftList.SingleOrDefault(t => t.Item1 == tokens);
                if (tuple == null)
                {
                    tuple = Tuple.Create(tokens, new ListSet<SdfStateRef>());
                    shiftList.Add(tuple);
                }
                
                ListSet<SdfStateRef> nextStates = tuple.Item2;
                nextStates.Add(nextState);
            }
        }
    }
}
