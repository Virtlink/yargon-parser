using System;
using Yargon.Parsing;

namespace Yargon.Parser.Sdf.Productions
{
	/// <summary>
	/// The special layout sort.
	/// </summary>
	public sealed class Layout : INonTerminal, IEquatable<Layout>
    {
		/// <summary>
		/// The default instance.
		/// </summary>
		public static Layout Instance { get; } = new Layout();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Layout"/> class.
		/// </summary>
		private Layout()
		{
			// Nothing to do.
		}
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Layout);

        /// <inheritdoc />
        public bool Equals(Layout other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return true;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return 17;
        }
        #endregion

        /// <inheritdoc />
        public void Accept(IProductionSymbolVisitor visitor)
        {
            #region Contract
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));
            #endregion

            visitor.VisitLayout(this);
        }

        /// <inheritdoc />
        public TResult Accept<TResult>(IProductionSymbolVisitor<TResult> visitor)
        {
            #region Contract
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));
            #endregion

            return visitor.VisitLayout(this);
        }

        public override string ToString()
		{
			return "layout";
		}
	}
}
