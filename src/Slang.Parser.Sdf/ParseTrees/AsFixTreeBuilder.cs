using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Slang.Parser.Sdf.Productions.IO;
using Virtlink.ATerms;

namespace Slang.Parser.Sdf.ParseTrees
{
	/// <summary>
	/// Builds an AsFix tree from a parse tree.
	/// </summary>
	public sealed class AsFixTreeBuilder : IParseTreeTransformer<ITerm>, IParseNodeVisitor<ITerm>
	{
		/// <summary>
		/// The term factory to use.
		/// </summary>
		private readonly TermFactory factory;

		private readonly IProductionFormat<ITerm> productionFormat;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="AsFixTreeBuilder"/> class.
		/// </summary>
		/// <param name="factory">The term factory to use.</param>
		/// <param name="productionFormat">The production rule format to use.</param>
		public AsFixTreeBuilder(TermFactory factory, IProductionFormat<ITerm> productionFormat)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(factory != null);
			Contract.Requires<ArgumentNullException>(productionFormat != null);
			#endregion
			this.factory = factory;
			this.productionFormat = productionFormat;
		}
		#endregion

		/// <inheritdoc />
		public ITerm Transform(IParseNode root)
		{
			// CONTRACT: Inherited from IParseTreeBuilder<T>
			int ambiguityCount = 0;	// TODO
			return factory.Cons("parsetree", root.Accept(this), factory.Int(ambiguityCount));
		}

		/// <inheritdoc />
		ITerm IParseNodeVisitor<ITerm>.VisitApplication(IApplicationParseNode node)
		{
			// CONTRACT: Inherited from IParseNodeVisitor
			var children = from c in node.Children select c.Accept(this);
			var productionTerm = this.productionFormat.Write(node.Production);
			return factory.Cons("appl", productionTerm, factory.List(children));
		}

		/// <inheritdoc />
		ITerm IParseNodeVisitor<ITerm>.VisitAmbiguity(IAmbiguityParseNode node)
		{
			// CONTRACT: Inherited from IParseNodeVisitor
			
			var alternatives = from c in node.Children select c.Accept(this);
			// TODO: Flatten nested ambiguities. For example:
			// amb(amb("x", "y"), "z") should become amb("x", "y", "z").
			return factory.Cons("amb", factory.List(alternatives));
		}

		/// <inheritdoc />
		ITerm IParseNodeVisitor<ITerm>.VisitProduction(IProductionParseNode node)
		{
			// CONTRACT: Inherited from IParseNodeVisitor
			return factory.Int(unchecked((int)node.Token.Value));
		}

		/// <inheritdoc />
		ITerm IParseNodeVisitor<ITerm>.VisitParseNode(IParseNode node)
		{
			// CONTRACT: Inherited from IParseNodeVisitor
			throw new NotSupportedException("Unknown parse node type.");
		}

		/// <inheritdoc />
		ITerm IParseNodeVisitor<ITerm>.VisitCycle(ICycleParseNode node)
		{
			// CONTRACT: Inherited from IParseNodeVisitor
			throw new NotImplementedException();
		}
	}
}
