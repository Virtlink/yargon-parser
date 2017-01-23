using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Yargon.Parser.Sdf.ParseTrees
{
	partial class TrivialParseNodeFactory
	{
		internal sealed class ApplicationParseNode : ParseNode, IApplicationParseNode
		{
			/// <inheritdoc />
			public Production Production { get; }

			/// <inheritdoc />
			public ParseNodePreference Preference { get; }

			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="ApplicationParseNode"/> class.
			/// </summary>
			/// <param name="production">The production of this node.</param>
			/// <param name="preference">The parse node preference.</param>
			/// <param name="children">The children of this node.</param>
			public ApplicationParseNode(Production production, ParseNodePreference preference, IReadOnlyList<IParseNode> children)
				: base(children)
			{
                #region Contract
                if (production == null)
                    throw new ArgumentNullException(nameof(production));
                if (!Enum.IsDefined(typeof(ParseNodePreference), preference))
                    throw new InvalidEnumArgumentException(nameof(preference), (int)preference, typeof(ParseNodePreference));
                if (children == null)
                    throw new ArgumentNullException(nameof(children));
                #endregion

                this.Production = production;
				this.Preference = preference;
			}
			#endregion

			/// <inheritdoc />
			public override void Accept(IParseNodeVisitor visitor)
			{
                #region Contract
                if (visitor == null)
                    throw new ArgumentNullException(nameof(visitor));
                #endregion

                visitor.VisitApplication(this);
			}

			/// <inheritdoc />
			public override TResult Accept<TResult>(IParseNodeVisitor<TResult> visitor)
			{
                #region Contract
                if (visitor == null)
                    throw new ArgumentNullException(nameof(visitor));
                #endregion

                return visitor.VisitApplication(this);
			}

			/// <inheritdoc />
			public override string ToString()
			{
				return $"appl({this.Production}; [{String.Join(", ", this.Children)}])";
			}
		}
	}
}
