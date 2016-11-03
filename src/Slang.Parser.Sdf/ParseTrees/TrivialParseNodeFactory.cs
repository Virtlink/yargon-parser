using System;
using System.Collections.Generic;
using System.Linq;
using Slang.Parsing;
using Virtlink.ATerms;

namespace Slang.Parser.Sdf.ParseTrees
{
	/// <summary>
	/// A trivial parse node factory.
	/// </summary>
	public sealed partial class TrivialParseNodeFactory : IParseTreeBuilder<Token<CodePoint>, IParseNode> // , IParseNodeFactory
    {
//		/// <inheritdoc />
//		public IAmbiguityParseNode CreateAmbiguity(IReadOnlyList<IParseNode> alternatives)
//		{
//			// CONTRACT: Inherited from IParseNodeFactory
//			return new AmbiguityParseNode(alternatives);
//		}
//
//		/// <inheritdoc />
//		public IApplicationParseNode CreateApplication(Production production, ParseNodePreference preference, IReadOnlyList<IParseNode> children)
//		{
//			// CONTRACT: Inherited from IParseNodeFactory
//			return new ApplicationParseNode(production, preference, children);
//		}
//
//		/// <inheritdoc />
//		public IProductionParseNode CreateProduction(CodePoint token)
//		{
//			// CONTRACT: Inherited from IParseNodeFactory
//			return new ProductionParseNode(token);
//		}

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
