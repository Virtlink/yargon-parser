using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Spoofax.Terms
{
	partial class TrivialTermFactory
	{
		/// <summary>
		/// A constructor application term.
		/// </summary>
		internal sealed class ConsTerm : Term, IConsTerm
		{
			/// <inheritdoc />
			public string Name { get; }

			/// <inheritdoc />
			public bool IsTuple => this.Name == System.String.Empty;
			
			/// <inheritdoc />
			public override IReadOnlyList<ITerm> SubTerms { get; }

			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="ConsTerm"/> class.
			/// </summary>
			/// <param name="name">The name of the constructor.</param>
			/// <param name="terms">The list of sub terms.</param>
			/// <param name="annotations">The annotations of the term.</param>
			public ConsTerm(string name, IReadOnlyList<ITerm> terms, IReadOnlyCollection<ITerm> annotations)
				: base(annotations)
			{
                #region Contract
                Debug.Assert(name != null);
                Debug.Assert(terms != null);
                Debug.Assert(annotations != null);
				#endregion

				this.Name = name;
				this.SubTerms = terms;
			}
			#endregion

			#region Equality
			/// <inheritdoc />
			public override int GetHashCode()
			{
				int hash = base.GetHashCode();
				unchecked
				{
					hash = hash * 29 + this.Name.GetHashCode();
					hash = this.SubTerms.Aggregate(hash, (current, term) => current * 29 + term.GetHashCode());
				}
				return hash;
			}

			/// <inheritdoc />
			public override bool Equals(object other)
			{
				return this.Equals(other as ConsTerm);
			}

			/// <summary>
			/// Determines whether this object is equal to another object.
			/// </summary>
			/// <param name="other">The other object.</param>
			/// <returns><see langword="true"/> when the objects are equal;
			/// otherwise, <see langword="false"/>.</returns>
			private bool Equals(ConsTerm other)
			{
				return base.Equals(other)
					&& this.Name.Equals((other.Name))
					&& this.SubTerms.SequenceEqual(other.SubTerms);
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="ConsTerm"/> objects are equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator ==(ConsTerm left, ConsTerm right)
			{
				return Object.Equals(left, right);
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="ConsTerm"/> objects are not equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are not equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator !=(ConsTerm left, ConsTerm right)
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

                visitor.VisitCons(this);
			}
		}
	}
}
