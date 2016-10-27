using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtlink.Utilib.Collections;

namespace Slang.Parser.Sdf
{
    /// <summary>
	/// A parse state.
	/// </summary>
	public sealed class State
    {
        /// <summary>
        /// Gets a collection of gotos.
        /// </summary>
        /// <value>A collection of gotos.</value>
        public ReadOnlyCollection<Goto> Gotos { get; }

        /// <summary>
        /// Gets a collection of actions that can be taken in this state.
        /// </summary>
        /// <value>A collection of actions.</value>
        /// <remarks>
        /// An empty action collection indicates an erroneous state.
        /// </remarks>
        public ReadOnlyCollection<ActionSet> Actions { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class.
        /// </summary>
        /// <param name="gotos">A collection of gotos.</param>
        /// <param name="actions">A collection of actions.</param>
        public State(IEnumerable<Goto> gotos, IEnumerable<ActionSet> actions)
        {
            #region Contract
            if (gotos == null)
                throw new ArgumentNullException(nameof(gotos));
            if (actions == null)
                throw new ArgumentNullException(nameof(actions));
            #endregion

            this.Gotos = new ReadOnlyCollection<Goto>(gotos.ToArray());
            this.Actions = new ReadOnlyCollection<ActionSet>(actions.ToArray());
        }
        #endregion
        
        public SdfStateRef Go(Label label)
        {
            var @goto = this.Gotos.FirstOrDefault(g => g.HasProduction(label));
            if (@goto == null)
                throw new InvalidOperationException("Cannot go to label #" + label);

            return @goto.NextState;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var gotos = String.Join(", ", this.Gotos);
            var actions = String.Join(", ", this.Actions);
            return $"state({gotos}; {actions})";
        }

        #region Invariants
        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.Gotos != null);
            Contract.Invariant(this.Actions != null);
        }
        #endregion
    }
}
