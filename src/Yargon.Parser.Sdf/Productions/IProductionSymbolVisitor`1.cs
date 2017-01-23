using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yargon.Parsing;

namespace Yargon.Parser.Sdf.Productions
{
    /// <summary>
	/// A visitor for production symbols.
	/// </summary>
	/// <typeparam name="TResult">The type of results.</typeparam>
	public interface IProductionSymbolVisitor<out TResult>
    {
        /// <summary>
        /// Visits a symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The result.</returns>
        TResult VisitSymbol(IProductionSymbol symbol);

        /// <summary>
        /// Visits an ALT symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The result.</returns>
        TResult VisitAlt(Alt symbol);

        /// <summary>
        /// Visits a CF symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The result.</returns>
        TResult VisitCf(Cf symbol);

        /// <summary>
        /// Visits a CHAR-CLASS symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The result.</returns>
        TResult VisitCharacterClass(CharacterClass symbol);

        /// <summary>
        /// Visits a ITER symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The result.</returns>
        TResult VisitIter(Iter symbol);

        /// <summary>
        /// Visits a LAYOUT symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The result.</returns>
        TResult VisitLayout(Layout symbol);

        /// <summary>
        /// Visits a LIT symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The result.</returns>
        TResult VisitLit(Lit symbol);

        /// <summary>
        /// Visits a LEX symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The result.</returns>
        TResult VisitLex(Lex symbol);

        /// <summary>
        /// Visits an OPT symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The result.</returns>
        TResult VisitOpt(Opt symbol);

        /// <summary>
        /// Visits a SORT symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The result.</returns>
        TResult VisitSort(Sort symbol);
    }
}
