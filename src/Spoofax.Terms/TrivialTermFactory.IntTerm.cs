using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Spoofax.Terms
{
	partial class TrivialTermFactory
	{
		/// <summary>
		/// An integer number term.
		/// </summary>
		internal sealed class IntTerm : Term, IIntTerm
		{
			/// <inheritdoc />
			public int Value { get; }

			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="IntTerm"/> class.
			/// </summary>
			/// <param name="value">The value of the term.</param>
			/// <param name="annotations">The annotations of the term.</param>
			public IntTerm(int value, IReadOnlyCollection<ITerm> annotations)
				: base(annotations)
            {
                #region Contract
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
				return this.Equals(other as IntTerm);
			}

			/// <summary>
			/// Determines whether this object is equal to another object.
			/// </summary>
			/// <param name="other">The other object.</param>
			/// <returns><see langword="true"/> when the objects are equal;
			/// otherwise, <see langword="false"/>.</returns>
			private bool Equals(IntTerm other)
			{
				return base.Equals(other)
					&& this.Value == other.Value;
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="IntTerm"/> objects are equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator ==(IntTerm left, IntTerm right)
			{
				return Object.Equals(left, right);
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="IntTerm"/> objects are not equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are not equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator !=(IntTerm left, IntTerm right)
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

                visitor.VisitInt(this);
			}
		}
	}
}
