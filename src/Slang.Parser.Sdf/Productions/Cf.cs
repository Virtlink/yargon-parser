using System;
using Slang.Parsing;

namespace Slang.Parser.Sdf.Productions
{
	public sealed class Cf : ISort, IEquatable<Cf>
    {
		public ISymbol Child
		{ get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Cf"/> class.
		/// </summary>
		public Cf(ISymbol child)
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
        public override bool Equals(object obj) => Equals(obj as Cf);

        /// <inheritdoc />
        public bool Equals(Cf other)
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
        public override string ToString()
		{
			return $"CF-{this.Child}";
		}
	}
}
