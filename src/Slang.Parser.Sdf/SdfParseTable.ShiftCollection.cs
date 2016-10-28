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
        /// A collection of SGLR shifts.
        /// </summary>
        public sealed class ShiftCollection
        {
            private readonly List<List<Tuple<CodePointSet, SdfStateRef>>> shifts =
                new List<List<Tuple<CodePointSet, SdfStateRef>>>();

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the <see cref="ShiftCollection"/> class.
            /// </summary>
            public ShiftCollection()
            {
            }
            #endregion

            /// <summary>
            /// Gets the state to shift to for a given current state and token to shift.
            /// </summary>
            /// <param name="state">The current state.</param>
            /// <param name="token">The token to shift.</param>
            /// <param name="nextState">The next state after shifting.</param>
            /// <returns><see langword="true"/> when the token can be shifted;
            /// otherwise, <see langword="false"/>.</returns>
            public bool TryGet(SdfStateRef state, CodePoint token, out SdfStateRef nextState)
            {
                if (state.Index >= this.shifts.Count)
                {
                    // No shift.
                    nextState = default(SdfStateRef);
                    return false;
                }

                // Get the candidates.
                var candidates = this.shifts[state.Index]
                    .Where(t => t.Item1.Contains(token))
                    .Select(t => t.Item2)
                    .ToList();

                if (candidates.Count == 0)
                {
                    // No candidates.
                    nextState = default(SdfStateRef);
                    return false;
                }

                if (candidates.Count > 1)
                {
                    // Too many candidates.
                    throw new InvalidParseTableException(
                        $"The parse table has more than one possible shift for the (state, token) pair: {state}, {token}");
                }

                nextState = candidates.Single();
                return true;
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

                // Ensure there are at least enough states in the list.
                while (this.shifts.Count <= state.Index)
                    this.shifts.Add(new List<Tuple<CodePointSet, SdfStateRef>>());

                var stateShifts = this.shifts[state.Index];

                // Ensure there aren't any overlapping code points for this state.
                if (stateShifts.Any(t => t.Item1.Overlaps(tokens)))
                    throw new InvalidOperationException(
                        $"For the given state there is already a set of code points that overlaps with the given set. {state}: {tokens}");

                stateShifts.Add(Tuple.Create(tokens, nextState));
            }
        }
    }
}
