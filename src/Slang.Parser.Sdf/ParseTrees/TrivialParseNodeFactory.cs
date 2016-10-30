using System.Collections.Generic;

namespace Slang.Parser.Sdf.ParseTrees
{
	/// <summary>
	/// A trivial parse node factory.
	/// </summary>
	public sealed partial class TrivialParseNodeFactory : IParseNodeFactory
	{
		/// <inheritdoc />
		public IAmbiguityParseNode CreateAmbiguity(IReadOnlyList<IParseNode> alternatives)
		{
			// CONTRACT: Inherited from IParseNodeFactory
			return new AmbiguityParseNode(alternatives);
		}

		/// <inheritdoc />
		public IApplicationParseNode CreateApplication(Production production, ParseNodePreference preference, IReadOnlyList<IParseNode> children)
		{
			// CONTRACT: Inherited from IParseNodeFactory
			return new ApplicationParseNode(production, preference, children);
		}

		/// <inheritdoc />
		public IProductionParseNode CreateProduction(CodePoint token)
		{
			// CONTRACT: Inherited from IParseNodeFactory
			return new ProductionParseNode(token);
		}
	}
}
