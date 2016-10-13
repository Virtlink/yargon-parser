using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Spoofax.Terms
{
	partial class TrivialTermFactory
	{
		/// <summary>
		/// A list term.
		/// </summary>
		internal sealed class ListTerm : Term, IListTerm
		{
			/// <inheritdoc />
			public int Count { get; }

			/// <inheritdoc />
			public bool IsEmpty => this.Count == 0;

		    /// <inheritdoc />
			public ITerm Head { get; }
			
			/// <inheritdoc />
			public IListTerm Tail { get; }

			/// <inheritdoc />
			public override IReadOnlyList<ITerm> SubTerms
			{
				get
				{
					List<ITerm> result = new List<ITerm>();
					IListTerm current = this;
					while (!current.IsEmpty)
					{
						result.Add(current.Head);
						current = current.Tail;
					}
					return result;
				}
			}

			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="ListTerm"/> class,
			/// that represents an empty list.
			/// </summary>
			/// <param name="annotations">The annotations of the term.</param>
			public ListTerm(IReadOnlyCollection<ITerm> annotations)
				: base(annotations)
            {
                #region Contract
                Debug.Assert(annotations != null);
                #endregion

				this.Head = null;
				this.Tail = null;
				this.Count = 0;
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="ListTerm"/> class,
			/// that represents a non-empty list.
			/// </summary>
			/// <param name="head">The head of the list.</param>
			/// <param name="tail">The tail of the list.</param>
			/// <param name="annotations">The annotations of the term.</param>
			public ListTerm(ITerm head, IListTerm tail, IReadOnlyCollection<ITerm> annotations)
				: base(annotations)
            {
                #region Contract
                Debug.Assert(head != null);
                Debug.Assert(tail != null);
                Debug.Assert(annotations != null);
                #endregion

				this.Head = head;
				this.Tail = tail;
				this.Count = tail.Count + 1;
			}
			#endregion

			#region Equality
			/// <inheritdoc />
			public override int GetHashCode()
			{
				int hash = base.GetHashCode();
				unchecked
				{
					hash = hash * 29 + (this.Head?.GetHashCode() ?? 0);
					hash = hash * 29 + (this.Tail?.GetHashCode() ?? 0);
				}
				return hash;
			}

			/// <inheritdoc />
			public override bool Equals(object other)
			{
				return this.Equals(other as ListTerm);
			}

			/// <summary>
			/// Determines whether this object is equal to another object.
			/// </summary>
			/// <param name="other">The other object.</param>
			/// <returns><see langword="true"/> when the objects are equal;
			/// otherwise, <see langword="false"/>.</returns>
			private bool Equals(ListTerm other)
			{
				return base.Equals(other)
				    && Object.Equals(this.Head, other.Head)
				    && Object.Equals(this.Tail, other.Tail);
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="ListTerm"/> objects are equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator ==(ListTerm left, ListTerm right)
			{
				return Object.Equals(left, right);
			}

			/// <summary>
			/// Returns a value that indicates whether two specified
			/// <see cref="ListTerm"/> objects are not equal.
			/// </summary>
			/// <param name="left">The first object to compare.</param>
			/// <param name="right">The second object to compare.</param>
			/// <returns><see langword="true"/> if <paramref name="left"/>
			/// and <paramref name="right"/> are not equal;
			/// otherwise, <see langword="false"/>.</returns>
			public static bool operator !=(ListTerm left, ListTerm right)
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

                visitor.VisitList(this);
			}
		}
	}
}
