using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Slang.Parser.Sdf.ParseTrees
{
	partial class TrivialParseNodeFactory
	{
		internal sealed class AmbiguityParseNode : ParseNode, IAmbiguityParseNode
		{
			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="AmbiguityParseNode"/> class.
			/// </summary>
			/// <param name="alternatives">The children of this node.</param>
			public AmbiguityParseNode(IReadOnlyList<IParseNode> alternatives)
				: base(alternatives)
			{
				#region Contract
				if (alternatives == null)
                    throw new ArgumentNullException(nameof(alternatives));
				#endregion
			}
			#endregion

			/// <inheritdoc />
			public override void Accept(IParseNodeVisitor visitor)
            {
                #region Contract
                if (visitor == null)
                    throw new ArgumentNullException(nameof(visitor));
                #endregion

                visitor.VisitAmbiguity(this);
			}

			/// <inheritdoc />
			public override TResult Accept<TResult>(IParseNodeVisitor<TResult> visitor)
			{
                #region Contract
                if (visitor == null)
                    throw new ArgumentNullException(nameof(visitor));
                #endregion

                return visitor.VisitAmbiguity(this);
			}

			/// <inheritdoc />
			public override string ToString()
			{
				return $"amb([{String.Join(", ", this.Children)}])";
			}
		}
	}
}
