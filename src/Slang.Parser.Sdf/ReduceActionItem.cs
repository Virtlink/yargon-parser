using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtlink.Utilib.Collections;

namespace Slang.Parser.Sdf
{
    /// <summary>
	/// 
	/// </summary>
	public sealed class ReduceActionItem : ActionItem
    {
        // Perhaps merge with ReduceLookahead, given 0 lookahead?

        /// <summary>
        /// Gets the label of a production rule.
        /// </summary>
        /// <value>The label.</value>
        public Label Label
        { get; }

        /// <summary>
        /// Gets the follow restriction.
        /// </summary>
        /// <value>A list of character ranges; or an empty list
        /// when there are no restrictions.</value>
        public IReadOnlyList<IReadOnlySet<CodePoint>> FollowRestriction { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ReduceActionItem"/> class.
        /// </summary>
        public ReduceActionItem(Label label, IReadOnlyList<IReadOnlySet<CodePoint>> followRestriction)
        {
            #region Contract
            if (followRestriction == null)
                throw new ArgumentNullException(nameof(followRestriction));
            #endregion

            this.Label = label;
            this.FollowRestriction = followRestriction.ToArray();
        }
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return $"reduce({this.Label})";
        }
    }
}
