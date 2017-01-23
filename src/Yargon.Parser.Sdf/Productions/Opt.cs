using System;
using Yargon.Parsing;

namespace Yargon.Parser.Sdf.Productions
{
	public sealed class Opt : INonTerminal, IEquatable<Opt>
    {
		public IProductionSymbol Child
		{ get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Opt"/> class.
		/// </summary>
		public Opt(IProductionSymbol child)
        {
            #region Contract
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            #endregion

			this.Child = child;
		}
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Opt);

        /// <inheritdoc />
        public bool Equals(Opt other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return Object.Equals(this.Child, other.Child);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Child.GetHashCode();
            }
            return hash;
        }
        #endregion

        /// <inheritdoc />
        public void Accept(IProductionSymbolVisitor visitor)
        {
            #region Contract
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));
            #endregion

            visitor.VisitOpt(this);
        }

        /// <inheritdoc />
        public TResult Accept<TResult>(IProductionSymbolVisitor<TResult> visitor)
        {
            #region Contract
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));
            #endregion

            return visitor.VisitOpt(this);
        }

        /// <inheritdoc />
        public override string ToString()
		{
			return $"{this.Child}?";
		}
	}
}
