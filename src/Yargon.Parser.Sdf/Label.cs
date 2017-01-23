using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yargon.Parser.Sdf
{
    /// <summary>
    /// A reference to a production.
    /// </summary>
    public struct Label : IEquatable<Label>
    {
        /// <summary>
        /// Gets the production index.
        /// </summary>
        /// <value>The production index.</value>
        public int Index { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        public Label(int index)
        {
            #region Contract
            if (index < 0)
                throw new ArgumentNullException(nameof(index));
            #endregion

            this.Index = index;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Label && Equals((Label)obj);

        /// <inheritdoc />
        public bool Equals(Label other)
        {
            return this.Index == other.Index;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Index.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Label"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Label left, Label right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Label"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Label left, Label right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString() => $"P{this.Index}";
    }
}
