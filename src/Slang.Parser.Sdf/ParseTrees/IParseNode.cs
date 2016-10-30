using System.Collections.Generic;
using Virtlink.Utilib.Collections.Graphs;

namespace Slang.Parser.Sdf.ParseTrees
{
	/// <summary>
	/// A node in a parse tree.
	/// </summary>
	public interface IParseNode : INode<IParseNode>
	{
		/// <summary>
		/// Gets whether this node and its children have been rejected.
		/// </summary>
		/// <value><see langword="true"/> when the node has been rejected;
		/// otherwise, <see langword="false"/>.</value>
		bool IsRejected
		{ get; }
        
		/// <summary>
		/// Gets the child nodes of this node.
		/// </summary>
		/// <value>A list of child nodes.</value>
		new IReadOnlyList<IParseNode> Children
		{ get; }

		/// <summary>
		/// Rejects this node.
		/// </summary>
		void Reject();

		/// <summary>
		/// Accepts the visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		void Accept(IParseNodeVisitor visitor);

		/// <summary>
		/// Accepts the visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <returns>The result of the visitor.</returns>
		TResult Accept<TResult>(IParseNodeVisitor<TResult> visitor);
	}
}
