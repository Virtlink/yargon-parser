using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yargon.Parser.Sdf
{
    /// <summary>
    /// An SDF state.
    /// </summary>
    public struct SdfStateRef : IEquatable<SdfStateRef>
    {
        /// <summary>
        /// Gets the state index.
        /// </summary>
        /// <value>The state index.</value>
        public int Index { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SdfStateRef"/> class.
        /// </summary>
        public SdfStateRef(int index)
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
        public override bool Equals(object obj) => obj is SdfStateRef && Equals((SdfStateRef)obj);

        /// <inheritdoc />
        public bool Equals(SdfStateRef other)
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
        /// Returns a value that indicates whether two specified <see cref="SdfStateRef"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(SdfStateRef left, SdfStateRef right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="SdfStateRef"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(SdfStateRef left, SdfStateRef right) => !(left == right);
        #endregion

        public override string ToString() => $"s{this.Index}";
    }
}
