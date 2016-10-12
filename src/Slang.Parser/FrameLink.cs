using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Slang.Parser
{
    /// <summary>
    /// A link between two stack frames on the Graph-Structured Stack.
    /// </summary>
    internal sealed class FrameLink<TState>
    {
        /// <summary>
        /// Gets the parent frame.
        /// </summary>
        /// <value>The parent frame.</value>
        public Frame<TState> Parent { get; }

        private readonly List<object> trees;

        /// <summary>
        /// Gets the trees associated with this link.
        /// </summary>
        /// <value>A list of trees, which each may be <see langword="null"/>.</value>
        /// <remarks>
        /// More than one tree indicates an ambiguity.
        /// </remarks>
        public IReadOnlyList<object> Trees => this.trees;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameLink{TState}"/> class.
        /// </summary>
        /// <param name="parent">The parent frame.</param>
        /// <param name="tree">The tree associated with the link, which may be <see langword="null"/>.</param>
        public FrameLink(Frame<TState> parent, [CanBeNull] object tree)
        {
            #region Contract
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));
            #endregion

            this.Parent = parent;
            this.trees = new List<object>(1) { tree };
        }
        #endregion

        /// <summary>
        /// Adds a new tree to the link.
        /// </summary>
        /// <param name="tree">The tree to add, which may be <see langword="null"/>.</param>
        public void AddTree(object tree)
        {
            this.trees.Add(tree);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.Parent} <--";
        }
    }
}
