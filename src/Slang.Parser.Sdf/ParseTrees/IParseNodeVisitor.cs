using System;
using System.Diagnostics.Contracts;

namespace Slang.Parser.Sdf.ParseTrees
{
	/// <summary>
	/// A visitor for parse nodes.
	/// </summary>
	public interface IParseNodeVisitor
	{
		/// <summary>
		/// Visits a parse node.
		/// </summary>
		/// <param name="node">The node.</param>
		void VisitParseNode(IParseNode node);

		/// <summary>
		/// Visits an application node.
		/// </summary>
		/// <param name="node">The node.</param>
		void VisitApplication(IApplicationParseNode node);

		/// <summary>
		/// Visits a production node.
		/// </summary>
		/// <param name="node">The node.</param>
		void VisitProduction(IProductionParseNode node);

		/// <summary>
		/// Visits an ambiguity node.
		/// </summary>
		/// <param name="node">The node.</param>
		void VisitAmbiguity(IAmbiguityParseNode node);

		/// <summary>
		/// Visits a cycle node.
		/// </summary>
		/// <param name="node">The node.</param>
		void VisitCycle(ICycleParseNode node);
	}
}
