using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yargon.Parsing;

namespace Yargon.Parser
{
    /// <summary>
    /// A parser reduction.
    /// </summary>
    public sealed class Reduction : IReduction
    {
        /// <summary>
        /// Gets the non-terminal symbol resulting from the reduction.
        /// </summary>
        /// <value>The non-terminal symbol.</value>
        public Sort Symbol { get; }

        /// <inheritdoc />
        ISort IReduction.Symbol => this.Symbol;

        /// <inheritdoc />
        public int Arity { get; }

        /// <inheritdoc />
        public bool Rejects { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Reduction"/> class.
        /// </summary>
        /// <param name="symbol">The non-terminal symbol resulting from the reduction.</param>
        /// <param name="arity">The number of symbols consumed by the reduction.</param>
        public Reduction(Sort symbol, int arity)
            : this(symbol, arity, false)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Reduction"/> class.
        /// </summary>
        /// <param name="symbol">The non-terminal symbol resulting from the reduction.</param>
        /// <param name="arity">The number of symbols consumed by the reduction.</param>
        /// <param name="rejects">Whether the reduction rejects.</param>
        public Reduction(Sort symbol, int arity, bool rejects)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            if (arity < 0)
                throw new ArgumentOutOfRangeException(nameof(arity));
            #endregion

            this.Symbol = symbol;
            this.Arity = arity;
            this.Rejects = rejects;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Reduction);

        /// <inheritdoc />
        public bool Equals(Reduction other)
        {
            return !Object.ReferenceEquals(other, null)
                && this.GetType() == other.GetType()
                && Object.Equals(this.Symbol, other.Symbol)
                && this.Arity == other.Arity
                && this.Rejects == other.Rejects;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Symbol.GetHashCode();
                hash = hash * 29 + this.Arity.GetHashCode();
                hash = hash * 29 + this.Rejects.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Reduction"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Reduction left, Reduction right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Reduction"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Reduction left, Reduction right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString() => $"{this.Symbol}`{this.Arity}" + (this.Rejects ? " {rejects}" : "");
    }
}
