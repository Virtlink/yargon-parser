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
		/// A string term.
		/// </summary>
		internal sealed class StringTerm : Term, IStringTerm
		{
			/// <inheritdoc />
			public string Value { get; }

			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="StringTerm"/> class.
			/// </summary>
			/// <param name="value">The value of the term.</param>
			/// <param name="annotations">The annotations of the term.</param>
			public StringTerm(string value, IReadOnlyCollection<ITerm> annotations)
				: base(annotations)
            {
                #region Contract
                Debug.Assert(value != null);
                Debug.Assert(annotations != null);
                #endregion

				this.Value = value;
			}
			#endregion

			#region Equality
			/// <inheritdoc />
			public override int GetHashCode()
			{
				int hash = base.GetHashCode();
				unchecked
				{
					hash = hash * 29 + this.Value.GetHashCode();
				}
				return hash;
			}

			/// <inheritdoc />
			public override bool Equals(object other)
			{
				return this.Equals(other as StringTerm);
			}

			/// <summary>
			/// Determines whether this object is equal to another object.
			/// </summary>
			/// <param name="other">The other object.</param>
			/// <returns><see langword="true"/> when the objects are equal;
			/// otherwise, <see langword="false"/>.</returns>
			private bool Equals(StringTerm other)
			{
				return base.Equals(other)
					&& this.Value == other.Value;
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="StringTerm"/> objects are equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator ==(StringTerm left, StringTerm right)
			{
				return Object.Equals(left, right);
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="StringTerm"/> objects are not equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are not equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator !=(StringTerm left, StringTerm right)
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

                visitor.VisitString(this);
			}
		}
	}
}
