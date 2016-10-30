using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Slang.Parser.Sdf.ParseTrees
{
	partial class TrivialParseNodeFactory
	{
		internal sealed class CycleParseNode : ParseNode, ICycleParseNode
		{
			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="CycleParseNode"/> class.
			/// </summary>
			/// <param name="alternatives">The children of this node.</param>
			public CycleParseNode(IReadOnlyList<IParseNode> alternatives)
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

                visitor.VisitCycle(this);
			}

			/// <inheritdoc />
			public override TResult Accept<TResult>(IParseNodeVisitor<TResult> visitor)
			{
                #region Contract
                if (visitor == null)
                    throw new ArgumentNullException(nameof(visitor));
                #endregion

                return visitor.VisitCycle(this);
			}

			/// <inheritdoc />
			public override string ToString()
			{
				return $"cycle([{String.Join(", ", this.Children)}])";
			}
		}
	}
}
