using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yargon.Parser.Sdf
{
    /// <summary>
	/// Specifies the kind of reduction in a production rule.
	/// </summary>
	public enum ReductionKind
    {
        /// <summary>
        /// None specified.
        /// </summary>
        Invalid = 0,

        // FIXME: Combinations can be specified!
        // For example, optional layout would be Optional | Layout
        // And a list of literals would be Iterable | Literal
        // Optional, Iterable, Sequence, CF, LEX
        // Literal, Layout, VariableNode, ...?

        /// <summary>
        /// Lexical.
        /// </summary>
        Lexical,

        /// <summary>
        /// A literal or ciliteral.
        /// </summary>
        Literal,

        /// <summary>
        /// Layout.
        /// </summary>
        Layout,

        /// <summary>
        /// An iterable (e.g. a list).
        /// </summary>
        Iterable,

        /// <summary>
        /// A sequence (e.g. a tuple).
        /// </summary>
        Sequence,

        /// <summary>
        /// A variable node.
        /// </summary>
        VariableNode,

        /// <summary>
        /// Optional.
        /// </summary>
        Optional,
    }
}
