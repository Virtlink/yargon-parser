using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parser.Sdf
{
    /// <summary>
	/// Shifts the current character, then goes to the specified state.
	/// </summary>
	public sealed class ShiftActionItem : ActionItem
    {
        /// <summary>
        /// Gets the state to go to.
        /// </summary>
        /// <value>A <see cref="SdfStateRef"/>.</value>
        public SdfStateRef NextState { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShiftActionItem"/> class.
        /// </summary>
        /// <param name="nextState">Reference to the next state.</param>
        public ShiftActionItem(SdfStateRef nextState)
        {
            this.NextState = nextState;
        }
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return $"shift({this.NextState})";
        }
    }
}
