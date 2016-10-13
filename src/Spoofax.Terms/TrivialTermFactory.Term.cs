using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Spoofax.Terms.IO;

namespace Spoofax.Terms
{
	partial class TrivialTermFactory
	{
		/// <summary>
		/// A term.
		/// </summary>
		internal abstract class Term : ITerm
		{
			/// <inheritdoc />
			public virtual IReadOnlyList<ITerm> SubTerms { get; } = TermFactory.EmptyTermList;

			/// <inheritdoc />
			public ITerm this[int index] => this.SubTerms[index];

		    /// <inheritdoc />
			public IReadOnlyCollection<ITerm> Annotations { get; }

			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="Term"/> class.
			/// </summary>
			/// <param name="annotations">The annotations of the term.</param>
			protected Term(IReadOnlyCollection<ITerm> annotations)
            {
                #region Contract
                Debug.Assert(annotations != null);
                #endregion

				this.Annotations = annotations;
			}
			#endregion

			#region Equality
			/// <inheritdoc />
			public override int GetHashCode()
			{
				// NOTE: We don't use the annotations in the hash code
				// calculation for performance reasons.
				return 17;
			}

			/// <inheritdoc />
			public override bool Equals(object other)
			{
				return this.Equals(other as Term);
			}

			/// <summary>
			/// Determines whether this object is equal to another object.
			/// </summary>
			/// <param name="other">The other object.</param>
			/// <returns><see langword="true"/> when the objects are equal;
			/// otherwise, <see langword="false"/>.</returns>
			private bool Equals(Term other)
			{
				// When 'other' is null or of a different type
				// then they are not equal.
				if (Object.ReferenceEquals(other, null) ||
					other.GetType() != this.GetType())
					return false;
				return this.Annotations.SequenceEqual(other.Annotations);
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="Term"/> objects are equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator ==(Term left, Term right)
			{
				return Object.Equals(left, right);
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="Term"/> objects are not equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are not equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator !=(Term left, Term right)
			{
				return !(left == right);
			}
			#endregion

			/// <inheritdoc />
			public abstract void Accept(ITermVisitor visitor);

			/// <inheritdoc />
			public override string ToString()
			{
				return ATermFormat.Instance.CreateWriter().ToString(this);
			}
		}
	}
}
