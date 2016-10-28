using System;
using Slang.Parsing;

namespace Slang.Parser.Sdf.Productions
{
	public sealed class Lit : ISort, IEquatable<Lit>
    {
		public string Text
		{ get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Lit"/> class.
		/// </summary>
		public Lit(string text)
        {
            #region Contract
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            #endregion

            this.Text = text;
		}
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Lit);

        /// <inheritdoc />
        public bool Equals(Lit other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return Object.Equals(this.Text, other.Text);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Text.GetHashCode();
            }
            return hash;
        }
        #endregion

        /// <inheritdoc />
        public override string ToString()
		{
			return $"\"{this.Text}\"";
		}
	}
}
