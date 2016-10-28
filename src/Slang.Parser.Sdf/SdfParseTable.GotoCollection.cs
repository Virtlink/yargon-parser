using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;
using Virtlink.Utilib.Collections;

namespace Slang.Parser.Sdf
{
    partial class SdfParseTable
    {
        /// <summary>
        /// A collection of SGLR gotos.
        /// </summary>
        public sealed class GotoCollection
        {
            private readonly List<Dictionary<ISort, ListSet<SdfStateRef>>> gotos = new List<Dictionary<ISort, ListSet<SdfStateRef>>>();

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the <see cref="GotoCollection"/> class.
            /// </summary>
            public GotoCollection()
            {
            }
            #endregion

            /// <summary>
            /// Gets the possible goto states for a given current state and sort.
            /// </summary>
            /// <param name="state">The current state.</param>
            /// <param name="sort">The current sort.</param>
            /// <returns>A possibly empty list of states.</returns>
            public IReadOnlyCollection<SdfStateRef> Get(SdfStateRef state, ISort sort)
            {
                #region Contract
                if (sort == null)
                    throw new ArgumentNullException(nameof(sort));
                #endregion

                if (state.Index >= this.gotos.Count)
                {
                    // No gotos.
                    return Collection.Empty<SdfStateRef>();
                }

                var gotoDictionary = this.gotos[state.Index];
                if (gotoDictionary == null)
                {
                    // No gotos.
                    return Collection.Empty<SdfStateRef>();
                }

                ListSet<SdfStateRef> nextStates;
                if (!gotoDictionary.TryGetValue(sort, out nextStates))
                {
                    // No gotos.
                    return Collection.Empty<SdfStateRef>();
                }

                return nextStates;
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
                    this.gotos.Add(null);

                var gotoDictionary = this.gotos[state.Index];
                if (gotoDictionary == null)
                {
                    gotoDictionary = new Dictionary<ISort, ListSet<SdfStateRef>>();
                    this.gotos[state.Index] = gotoDictionary;
                }

                ListSet<SdfStateRef> nextStates;
                if (!gotoDictionary.TryGetValue(sort, out nextStates))
                {
                    nextStates = new ListSet<SdfStateRef>();
                    gotoDictionary.Add(sort, nextStates);
                }

                nextStates.Add(nextState);
            }
        }
    }
}
