using System;
using System.Collections.Generic;
using System.Linq;
using Yargon.Parsing;

namespace Yargon.Parser.Sdf.ParseTrees
{
	/// <summary>
	/// A trivial parse node factory.
	/// </summary>
	public sealed partial class TrivialParseNodeFactory : IParseTreeBuilder<Token<CodePoint>, IParseNode>
    {
        /// <inheritdoc />
        public IParseNode BuildToken(Token<CodePoint> token)
        {
            return new ProductionParseNode(token.Value);
        }

        /// <inheritdoc />
        public IParseNode BuildProduction(IReduction production, IEnumerable<IParseNode> arguments)
        {
            // TODO: Preference
            var preference = ParseNodePreference.None;
            return new ApplicationParseNode((Production)production, preference, arguments.ToArray());
        }

        /// <inheritdoc />
        public IParseNode BuildAmbiguity(IEnumerable<IParseNode> alternatives)
        {
            return new AmbiguityParseNode(alternatives.ToArray());
        }
    }
}
