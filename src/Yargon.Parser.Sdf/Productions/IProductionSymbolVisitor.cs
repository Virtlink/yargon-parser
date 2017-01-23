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
	public interface IProductionSymbolVisitor
    {
        /// <summary>
        /// Visits a symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        void VisitSymbol(IProductionSymbol symbol);

        /// <summary>
        /// Visits an ALT symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        void VisitAlt(Alt symbol);

        /// <summary>
        /// Visits a CF symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        void VisitCf(Cf symbol);

        /// <summary>
        /// Visits a CHAR-CLASS symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        void VisitCharacterClass(CharacterClass symbol);

        /// <summary>
        /// Visits a ITER symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        void VisitIter(Iter symbol);

        /// <summary>
        /// Visits a LAYOUT symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        void VisitLayout(Layout symbol);

        /// <summary>
        /// Visits a LIT symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        void VisitLit(Lit symbol);

        /// <summary>
        /// Visits a LEX symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        void VisitLex(Lex symbol);

        /// <summary>
        /// Visits an OPT symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        void VisitOpt(Opt symbol);

        /// <summary>
        /// Visits a SORT symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        void VisitSort(Sort symbol);
    }
}
