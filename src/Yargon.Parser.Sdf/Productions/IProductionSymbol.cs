using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yargon.Parser.Sdf.Productions
{
    /// <summary>
    /// A production symbol.
    /// </summary>
    public interface IProductionSymbol
    {
        /// <summary>
		/// Accepts the visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		void Accept(IProductionSymbolVisitor visitor);

        /// <summary>
        /// Accepts the visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The result of the visitor.</returns>
        TResult Accept<TResult>(IProductionSymbolVisitor<TResult> visitor);
    }
}
