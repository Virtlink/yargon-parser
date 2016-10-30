using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parsing
{
    /// <summary>
    /// Builds a parse tree.
    /// </summary>
    public interface IParseTreeBuilder<TToken, TTree>
    {
        /// <summary>
        /// Builds a parse tree node from the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The resulting parse tree node.</returns>
        TTree BuildToken(Token<TToken> token);

        /// <summary>
        /// Builds a parse tree node from the specified production and arguments.
        /// </summary>
        /// <param name="production">The production.</param>
        /// <param name="arguments">The argument parse tree nodes.</param>
        /// <returns>The resulting parse tree node.</returns>
        TTree BuildProduction(IReduction production, IEnumerable<TTree> arguments);

        /// <summary>
        /// Builds an ambiguity node from the specified alternatives.
        /// </summary>
        /// <param name="alternatives">The alternatives.</param>
        /// <returns>The resulting parse tree node.</returns>
        TTree BuildAmbiguity(IEnumerable<TTree> alternatives);
    }
}
