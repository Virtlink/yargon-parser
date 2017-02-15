using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yargon.Parser.Sdf.Productions.IO;
using Virtlink.ATerms;

namespace Yargon.Parser.Sdf.ParseTrees
{
    /// <summary>
    /// A parse tree imploder.
    /// </summary>
    public sealed class AsFixImploder : IParseNodeVisitor<ITerm>
    {
        private readonly TermFactory factory;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AsFixImploder"/> class.
        /// </summary>
        /// <param name="factory">The term factory to use.</param>
        public AsFixImploder(TermFactory factory)
        {
            #region Contract
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            #endregion

            this.factory = factory;
        }
        #endregion

        /// <summary>
        /// Implodes the specified parse tree.
        /// </summary>
        /// <param name="parseTree">The parse tree to implode.</param>
        /// <returns>The resulting AST.</returns>
        public ITerm Implode(IParseNode parseTree)
        {
            #region Contract
            if (parseTree == null)
                throw new ArgumentNullException(nameof(parseTree));
            #endregion

            return parseTree.Accept(this);
        }

        /// <inheritdoc />
		ITerm IParseNodeVisitor<ITerm>.VisitApplication(IApplicationParseNode node)
        {
            #region Contract
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            #endregion

            var children = node.Children.Select(c => c.Accept(this)).ToArray();
            return this.factory.Cons(node.Production.Constructor, children);
        }

        /// <inheritdoc />
        ITerm IParseNodeVisitor<ITerm>.VisitAmbiguity(IAmbiguityParseNode node)
        {
            #region Contract
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            #endregion

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        ITerm IParseNodeVisitor<ITerm>.VisitProduction(IProductionParseNode node)
        {
            #region Contract
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            #endregion

            throw new NotImplementedException();
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
