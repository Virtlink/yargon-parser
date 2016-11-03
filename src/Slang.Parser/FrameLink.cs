using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// A link between two stack frames on the Graph-Structured Stack.
    /// </summary>
    /// <remarks>
    /// <para>Two links are considered to be equal if their parent frame
    /// and symbol are equal. The trees are not used in the equality
    /// check.</para>
    /// <para>A link may exist between state frames in the same phase.
    /// These are produced by empty productions.</para>
    /// </remarks>
    public sealed class FrameLink<TState>
    {
        /// <summary>
        /// Gets the parent frame.
        /// </summary>
        /// <value>The parent frame.</value>
        public Frame<TState> Parent { get; }

        // The symbol is used to compare links,
        // to be able to mark a link as rejected.

        /// <summary>
        /// Gets the symbol on this link.
        /// </summary>
        /// <value>The symbol on this link.</value>
        public ISymbol Symbol { get; }

        private readonly List<object> trees;

        /// <summary>
        /// Gets the trees associated with this link.
        /// </summary>
        /// <value>A list of trees, each of which may be <see langword="null"/>.</value>
        /// <remarks>
        /// More than one tree indicates an ambiguity.
        /// </remarks>
        public IReadOnlyList<object> Trees => this.trees;

        /// <summary>
        /// Gets whether the link has been rejected.
        /// </summary>
        /// <value><see langword="true"/> when the link has been rejected;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsRejected { get; private set; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameLink{TState}"/> class.
        /// </summary>
        /// <param name="parent">The parent frame.</param>
        /// <param name="symbol">The symbol on the link.</param>
        /// <param name="isRejected">Whether the link is rejected.</param>
        /// <param name="tree">The tree associated with the link, which may be <see langword="null"/>.</param>
        public FrameLink(Frame<TState> parent, ISymbol symbol, bool isRejected, [CanBeNull] object tree)
        {
            #region Contract
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            this.Parent = parent;
            this.Symbol = symbol;
            this.IsRejected = isRejected;
            this.trees = new List<object>(1) { tree };
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is FrameLink<TState> && Equals((FrameLink<TState>)obj);

        /// <inheritdoc />
        public bool Equals(FrameLink<TState> other)
        {
            return this.Parent.Equals(other.Parent)
                && this.Symbol.Equals(other.Symbol);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Parent.GetHashCode();
                hash = hash * 29 + this.Symbol.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="FrameLink{TState}"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(FrameLink<TState> left, FrameLink<TState> right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="FrameLink{TState}"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(FrameLink<TState> left, FrameLink<TState> right) => !(left == right);
        #endregion

        /// <summary>
        /// Merges the specified link with this link.
        /// </summary>
        /// <param name="other">The link to merge with this link.</param>
        public void MergeWith(FrameLink<TState> other)
        {
            // Add the trees from the specified link to this link.
            this.trees.AddRange(other.Trees);

            // This link is rejected if the other link was rejected.
            this.IsRejected = this.IsRejected || other.IsRejected;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.Parent} <-- {this.Symbol} --" + (this.IsRejected ? " {rejected}" : "");
        }
    }
}
