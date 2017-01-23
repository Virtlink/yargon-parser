
using System;
using Virtlink.Utilib.Collections;

namespace Yargon.Parser.Sdf.ParseTrees
{
	partial class TrivialParseNodeFactory
	{
		internal sealed class ProductionParseNode : ParseNode, IProductionParseNode
		{
			/// <inheritdoc />
			public CodePoint Token
			{ get; }

			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="ApplicationParseNode"/> class.
			/// </summary>
			/// <param name="token">The token of this node.</param>
			public ProductionParseNode(CodePoint token)
				: base(Arrays.Empty<IParseNode>())
			{
				this.Token = token;
			}
			#endregion

			/// <inheritdoc />
			public override void Accept(IParseNodeVisitor visitor)
			{
                #region Contract
                if (visitor == null)
                    throw new ArgumentNullException(nameof(visitor));
                #endregion

                visitor.VisitProduction(this);
			}

			/// <inheritdoc />
			public override TResult Accept<TResult>(IParseNodeVisitor<TResult> visitor)
			{
                #region Contract
                if (visitor == null)
                    throw new ArgumentNullException(nameof(visitor));
                #endregion

                return visitor.VisitProduction(this);
			}

			/// <inheritdoc />
			public override string ToString()
			{
				return this.Token.Value.ToString();
			}
		}
	}
}
