using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yargon.Parser.Sdf
{
    /// <summary>
	/// Specifies the type of production.
	/// </summary>
	public enum ProductionType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// The production is a reject production.
        /// </summary>
        Reject = 1,
        /// <summary>
        /// The production is a prefer production.
        /// </summary>
        Prefer = 2,
        /// <summary>
        /// The production is a bracket production.
        /// </summary>
        Bracket = 3,
        /// <summary>
        /// The production is an avoid production.
        /// </summary>
        Avoid = 4,
        /// <summary>
        /// The production is a left-associative production.
        /// </summary>
        LeftAssociative = 5,
        /// <summary>
        /// The production is a right-associative production.
        /// </summary>
        RightAssociative = 6,
    }
}
