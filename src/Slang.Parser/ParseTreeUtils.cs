using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// Extension methods for the <see cref="IParseTreeBuilder{TToken,TTree}"/> interface.
    /// </summary>
    public static class ParseTreeUtils
    {
        /// <summary>
        /// Merges alternatives into a new parse tree node.
        /// </summary>
        /// <typeparam name="TToken">The type of tokens.</typeparam>
        /// <typeparam name="TTree">The type of tree.</typeparam>
        /// <param name="parseTreeBuilder">The parse tree builder.</param>
        /// <param name="alternatives">The alternatives to merge.</param>
        /// <returns>The merged alternatives.</returns>
        public static TTree Merge<TToken, TTree>(this IParseTreeBuilder<TToken, TTree> parseTreeBuilder,
            IReadOnlyCollection<TTree> alternatives)
        {
            #region Contract
            if (parseTreeBuilder == null)
                throw new ArgumentNullException(nameof(parseTreeBuilder));
            if (alternatives == null)
                throw new ArgumentNullException(nameof(alternatives));
            if (alternatives.Count == 0)
                throw new ArgumentException("There must be at least one alternative.", nameof(alternatives));
            #endregion

            if (alternatives.Count == 1)
            {
                return alternatives.Single();
            }
            else if (alternatives.Count > 1)
            {
                return parseTreeBuilder.BuildAmbiguity(alternatives);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
