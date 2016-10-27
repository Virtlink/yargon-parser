using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parser.Sdf
{
    /// <summary>
	/// Specifies the type of priority.
	/// </summary>
	public enum PriorityType
    {
        /// <summary>
        /// Non-associative.
        /// </summary>
        None = 0,
        /// <summary>
        /// Left-associative.
        /// </summary>
        Left,
        /// <summary>
        /// Right-associative.
        /// </summary>
        Right,
        /// <summary>
        /// Greater than.
        /// </summary>
        Greater,
    }
}
