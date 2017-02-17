using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Yargon.Parser.Sdf.Productions.IO;
using Yargon.ATerms;

namespace Yargon.Parser.Sdf.ParseTrees
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
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
		    if (productionFormat == null)
		        throw new ArgumentNullException(nameof(productionFormat));
			#endregion

			this.factory = factory;
			this.productionFormat = productionFormat;
		}
		#endregion

		/// <inheritdoc />
		public ITerm Transform(IParseNode root)
		{
		    #region Contract
		    if (root == null)
		        throw new ArgumentNullException(nameof(root));
		    #endregion

            int ambiguityCount = 0;	// TODO
			return factory.Cons("parsetree", root.Accept(this), factory.Int(ambiguityCount));
		}

		/// <inheritdoc />
		ITerm IParseNodeVisitor<ITerm>.VisitApplication(IApplicationParseNode node)
		{
		    #region Contract
		    if (node == null)
		        throw new ArgumentNullException(nameof(node));
		    #endregion

		    var children = from c in node.Children select c.Accept(this);
			var productionTerm = this.productionFormat.Write(node.Production);
			return factory.Cons("appl", productionTerm, factory.List(children));
		}

		/// <inheritdoc />
		ITerm IParseNodeVisitor<ITerm>.VisitAmbiguity(IAmbiguityParseNode node)
		{
		    #region Contract
		    if (node == null)
		        throw new ArgumentNullException(nameof(node));
		    #endregion

		    var alternatives = from c in node.Children select c.Accept(this);
			// TODO: Flatten nested ambiguities. For example:
			// amb(amb("x", "y"), "z") should become amb("x", "y", "z").
			return factory.Cons("amb", factory.List(alternatives));
		}

		/// <inheritdoc />
		ITerm IParseNodeVisitor<ITerm>.VisitProduction(IProductionParseNode node)
		{
		    #region Contract
		    if (node == null)
		        throw new ArgumentNullException(nameof(node));
		    #endregion

		    return factory.Int(unchecked((int)node.Token.Value));
		}

		/// <inheritdoc />
		ITerm IParseNodeVisitor<ITerm>.VisitParseNode(IParseNode node)
		{
		    #region Contract
		    if (node == null)
		        throw new ArgumentNullException(nameof(node));
		    #endregion

		    throw new NotSupportedException("Unknown parse node type.");
		}

		/// <inheritdoc />
		ITerm IParseNodeVisitor<ITerm>.VisitCycle(ICycleParseNode node)
		{
		    #region Contract
		    if (node == null)
		        throw new ArgumentNullException(nameof(node));
		    #endregion

		    throw new NotImplementedException();
		}
	}
}
