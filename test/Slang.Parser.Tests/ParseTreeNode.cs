using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parser
{
    /// <summary>
    /// A parse tree node.
    /// </summary>
    public sealed class ParseTreeNode : IParseTree
    {
        /// <summary>
        /// Gets the symbol of the node.
        /// </summary>
        /// <value>The symbol.</value>
        public Sort Symbol { get; }

        /// <summary>
        /// Gets the tree node children.
        /// </summary>
        /// <value>The tree node children.</value>
        public IReadOnlyList<IParseTree> Children { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseTreeNode"/> class.
        /// </summary>
        public ParseTreeNode(Sort symbol, IReadOnlyList<IParseTree> children)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            if (children == null)
                throw new ArgumentNullException(nameof(children));
            #endregion

            this.Symbol = symbol;
            this.Children = children;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as ParseTreeNode);

        /// <inheritdoc />
        public bool Equals(ParseTreeNode other)
        {
            return !Object.ReferenceEquals(other, null)
                && this.GetType() == other.GetType()
                && Object.Equals(this.Symbol, other.Symbol)
                && this.Children.SequenceEqual(other.Children);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Symbol.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="ParseTreeNode"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(ParseTreeNode left, ParseTreeNode right)
        {
            return Object.Equals(left, right);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="ParseTreeNode"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(ParseTreeNode left, ParseTreeNode right)
        {
            return !(left == right);
        }
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.Symbol}({String.Join(", ", this.Children)})";
        }
    }
}
