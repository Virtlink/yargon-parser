using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// A parse tree builder.
    /// </summary>
    public sealed class ParseTreeBuilder : IParseTreeBuilder<String, IParseTree>
    {
        /// <summary>
        /// Gets the sort for ambiguous nodes.
        /// </summary>
        public static Sort Amb { get; } = new Sort("Amb");

        /// <inheritdoc />
        public IParseTree BuildToken(Token<String> token)
        {
            return new ParseTreeToken<String>(token);
        }

        /// <inheritdoc />
        public IParseTree BuildProduction(IReduction production, IEnumerable<IParseTree> arguments)
        {
            return new ParseTreeNode((Sort)production.Symbol, arguments.ToArray());
        }

        /// <inheritdoc />
        public IParseTree BuildAmbiguity(IEnumerable<IParseTree> alternatives)
        {
            return new ParseTreeNode(Amb, alternatives.ToArray());
        }
    }
}
