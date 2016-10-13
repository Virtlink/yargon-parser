using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Spoofax.Terms
{
	partial class TrivialTermFactory
	{
		/// <summary>
		/// A placeholder term.
		/// </summary>
		internal sealed class PlaceholderTerm : Term, IPlaceholderTerm
		{
			/// <inheritdoc />
			public ITerm Template { get; }

			/// <inheritdoc />
			public override IReadOnlyList<ITerm> SubTerms => new ITerm[] {this.Template};

		    #region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="PlaceholderTerm"/> class.
			/// </summary>
			/// <param name="template">The template.</param>
			/// <param name="annotations">The annotations of the term.</param>
			public PlaceholderTerm(ITerm template, IReadOnlyCollection<ITerm> annotations)
				: base(annotations)
            {
                #region Contract
                Debug.Assert(template != null);
                Debug.Assert(annotations != null);
                #endregion

				this.Template = template;
			}
			#endregion

			#region Equality
			/// <inheritdoc />
			public override int GetHashCode()
			{
				int hash = base.GetHashCode();
				unchecked
				{
					hash = hash * 29 + this.Template.GetHashCode();
				}
				return hash;
			}

			/// <inheritdoc />
			public override bool Equals(object other)
			{
				return this.Equals(other as PlaceholderTerm);
			}

			/// <summary>
			/// Determines whether this object is equal to another object.
			/// </summary>
			/// <param name="other">The other object.</param>
			/// <returns><see langword="true"/> when the objects are equal;
			/// otherwise, <see langword="false"/>.</returns>
			private bool Equals(PlaceholderTerm other)
			{
				return base.Equals(other)
				    && this.Template.Equals((other.Template));
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="PlaceholderTerm"/> objects are equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator ==(PlaceholderTerm left, PlaceholderTerm right)
			{
				return Object.Equals(left, right);
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="PlaceholderTerm"/> objects are not equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are not equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator !=(PlaceholderTerm left, PlaceholderTerm right)
			{
				return !(left == right);
			}
			#endregion

			/// <inheritdoc />
			public override void Accept(ITermVisitor visitor)
            {
                #region Contract
                if (visitor == null)
                    throw new ArgumentNullException(nameof(visitor));
                #endregion

                visitor.VisitPlaceholder(this);
			}
		}
	}
}
