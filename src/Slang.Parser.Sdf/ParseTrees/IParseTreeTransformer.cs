using System;
using System.Diagnostics.Contracts;

namespace Slang.Parser.Sdf.ParseTrees
{
	/// <summary>
	/// Transforms a parse tree.
	/// </summary>
	public interface IParseTreeTransformer<T>
	{
		/// <summary>
		/// Transforms a parse tree.
		/// </summary>
		/// <param name="root">The root of the parse tree.</param>
		/// <returns>The transformation result.</returns>
		T Transform(IParseNode root);
	}
}
