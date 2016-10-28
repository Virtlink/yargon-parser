using System;
using Slang.Parsing;

namespace Slang.Parser.Sdf.Productions
{
	/// <summary>
	/// A syntactic sort.
	/// </summary>
	public sealed class Sort : ISort, IEquatable<Sort>
    {
		/// <summary>
		/// Gets the name of the sort.
		/// </summary>
		/// <value>The name of the sort.</value>
		public string Name
		{ get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Sort"/> class.
		/// </summary>
		public Sort(string name)
		{
			#region Contract
			if (name == null)
                throw new ArgumentNullException(nameof(name));
			#endregion

			this.Name = name;
		}
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Sort);

        /// <inheritdoc />
        public bool Equals(Sort other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return Object.Equals(this.Name, other.Name);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Name.GetHashCode();
            }
            return hash;
        }
        #endregion

        /// <inheritdoc />
        public override string ToString()
		{
			return this.Name;
		}
	}
}
