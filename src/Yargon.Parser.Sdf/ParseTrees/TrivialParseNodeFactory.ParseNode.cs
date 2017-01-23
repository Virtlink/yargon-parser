using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Virtlink.Utilib.Collections.Graphs;

namespace Yargon.Parser.Sdf.ParseTrees
{
	partial class TrivialParseNodeFactory
	{
		internal abstract class ParseNode : IParseNode
		{
			/// <inheritdoc />
			public IReadOnlyList<IParseNode> Children
			{ get; }

            /// <inheritdoc />
			IReadOnlyCollection<IParseNode> INode<IParseNode>.Children => this.Children;

			/// <inheritdoc />
			public bool IsRejected
			{ get; private set; } = false;

			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="ParseNode"/> class.
			/// </summary>
			/// <param name="children">The children of this node.</param>
			protected ParseNode(IReadOnlyList<IParseNode> children)
            {
                #region Contract
                if (children == null)
                    throw new ArgumentNullException(nameof(children));
                #endregion

				this.Children = children;
			}
			#endregion

			/// <inheritdoc />
			public virtual void Accept(IParseNodeVisitor visitor)
			{
                #region Contract
                if (visitor == null)
                    throw new ArgumentNullException(nameof(visitor));
                #endregion

                visitor.VisitParseNode(this);
			}

			/// <inheritdoc />
			public virtual TResult Accept<TResult>(IParseNodeVisitor<TResult> visitor)
			{
                #region Contract
                if (visitor == null)
                    throw new ArgumentNullException(nameof(visitor));
                #endregion

                return visitor.VisitParseNode(this);
			}

			/// <inheritdoc />
			public void Reject()
			{
				this.IsRejected = true;
			}
		}
	}
}
