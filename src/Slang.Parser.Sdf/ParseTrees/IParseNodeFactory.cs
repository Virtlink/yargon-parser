using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace Slang.Parser.Sdf.ParseTrees
{
	/// <summary>
	/// Factory for parse nodes.
	/// </summary>
	public interface IParseNodeFactory
	{
		/// <summary>
		/// Creates an application node.
		/// </summary>
		/// <param name="production">The production.</param>
		/// <param name="preference">The parse node preference.</param>
		/// <param name="children">The child nodes.</param>
		/// <returns>The created node.</returns>
		IApplicationParseNode CreateApplication(Production production, ParseNodePreference preference, IReadOnlyList<IParseNode> children);

		/// <summary>
		/// Creates a production node.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns>The created node.</returns>
		IProductionParseNode CreateProduction(CodePoint token);

		/// <summary>
		/// Creates an ambiguity node.
		/// </summary>
		/// <param name="alternatives">The alternatives.</param>
		/// <returns>The created node.</returns>
		IAmbiguityParseNode CreateAmbiguity(IReadOnlyList<IParseNode> alternatives);
	}
}
