using System;
using System.Diagnostics.Contracts;

namespace Yargon.Parser.Sdf.ParseTrees
{
	/// <summary>
	/// A visitor for parse nodes.
	/// </summary>
	/// <typeparam name="TResult">The type of results.</typeparam>
	public interface IParseNodeVisitor<TResult>
	{
		/// <summary>
		/// Visits a parse node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns>The result.</returns>
		TResult VisitParseNode(IParseNode node);

		/// <summary>
		/// Visits an application node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns>The result.</returns>
		TResult VisitApplication(IApplicationParseNode node);

		/// <summary>
		/// Visits a production node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns>The result.</returns>
		TResult VisitProduction(IProductionParseNode node);

		/// <summary>
		/// Visits an ambiguity node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns>The result.</returns>
		TResult VisitAmbiguity(IAmbiguityParseNode node);

		/// <summary>
		/// Visits a cycle node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns>The result.</returns>
		TResult VisitCycle(ICycleParseNode node);
	}
}
