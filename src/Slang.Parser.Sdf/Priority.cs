using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parser.Sdf
{
    /// <summary>
	/// 
	/// </summary>
	public sealed class Priority
    {
        /// <summary>
        /// Gets the label of the left production rule.
        /// </summary>
        /// <value>The label of the left production rule.</value>
        public Label Left { get; }

        /// <summary>
        /// Gets the label of the right production rule.
        /// </summary>
        /// <value>The label of the right production rule.</value>
        public Label Right { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int? Arg { get; }

        /// <summary>
        /// Gets the type of priority.
        /// </summary>
        /// <value>A member of the <see cref="PriorityType"/> enumeration.</value>
        public PriorityType Type { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Priority"/> class.
        /// </summary>
        /// <param name="left">The label of the left production rule.</param>
        /// <param name="right">The label of the right production rule.</param>
        /// <param name="type">The type of priority.</param>
        public Priority(Label left, Label right, PriorityType type)
            : this(left, right, null, type)
        {
            #region Contract
            if (!Enum.IsDefined(typeof(PriorityType), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(PriorityType));
            #endregion
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Priority"/> class.
        /// </summary>
        /// <param name="left">The label of the left production rule.</param>
        /// <param name="right">The label of the right production rule.</param>
        /// <param name="arg"></param>
        /// <param name="type">The type of priority.</param>
        public Priority(Label left, Label right, int? arg, PriorityType type)
        {
            #region Contract
            if (!Enum.IsDefined(typeof(PriorityType), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(PriorityType));
            #endregion

            this.Left = left;
            this.Right = right;
            this.Arg = arg;
            this.Type = type;
        }
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            // TODO: Better ToString();
            return "priority";
        }
    }
}
