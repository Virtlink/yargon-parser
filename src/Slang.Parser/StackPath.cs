using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// A stack path node.
    /// </summary>
    internal sealed class StackPath<TState>
    {
        /// <summary>
		/// Gets the next path node.
		/// </summary>
		/// <value>The next path node;
		/// or <see langword="null"/> when the end of the path was reached.</value>
		public StackPath<TState> Next { get; }

        /// <summary>
        /// Gets the depth of the path.
        /// </summary>
        /// <value>The depth of the path.</value>
        public int Depth { get; }

        /// <summary>
        /// Gets the stack frame associated with this path node.
        /// </summary>
        /// <value>The associated stack frame.</value>
        public Frame<TState> Frame { get; }

        /// <summary>
        /// Gets the link associated with this path node and the stack frame.
        /// </summary>
        /// <value>The associated link;
        /// or <see langword="null"/> when the end of the path was reached.</value>
        public FrameLink<TState> Link { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StackPath{TState}"/> class.
        /// </summary>
        /// <param name="link">The associated link.</param>
        /// <param name="next">The next node in the path.</param>
        public StackPath(FrameLink<TState> link, StackPath<TState> next)
        {
            #region Contract
            if (link == null ^ next == null)
                throw new ArgumentNullException(nameof(link));
            #endregion

            // FIXME: link can be null?
            this.Frame = link.Parent;
            this.Link = link;
            this.Next = next;
            this.Depth = next?.Depth + 1 ?? 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StackPath{TState}"/> class.
        /// </summary>
        /// <param name="frame">The associated stack frame.</param>
        public StackPath(Frame<TState> frame)
        {
            #region Contract
            if (frame == null)
                throw new ArgumentNullException(nameof(frame));
            #endregion

            this.Frame = frame;
            this.Link = null;
            this.Next = null;
            this.Depth = 0;
        }
        #endregion
        
        /// <summary>
        /// Gets all the parse trees for this path.
        /// </summary>
        /// <param name="parseTreeBuilder">The parse tree builder.</param>
        /// <returns>A list of parse trees, one for each path node.</returns>
        public IReadOnlyList<TTree> GetParseTrees<TToken, TTree>(IParseTreeBuilder<TToken, TTree> parseTreeBuilder)
        {
            #region Contract
            if (parseTreeBuilder == null)
                throw new ArgumentNullException(nameof(parseTreeBuilder));
            #endregion

            var result = new TTree[this.Depth];
            StackPath<TState> n = this;
            int pos = 0;
            while (n?.Link != null)
            {
                var trees = n.Link.Trees;
                result[pos++] = parseTreeBuilder.Merge(trees.Cast<TTree>().ToArray());
                n = n.Next;
            }
            Debug.Assert(pos == this.Depth);
            return result;
        }

        /// <summary>
        /// Gets all the symbols for this path.
        /// </summary>
        /// <returns>A list of symbols, one for each path node.</returns>
        public IEnumerable<ISymbol> GetSymbols()
        {
            var result = new ISymbol[this.Depth];
            StackPath<TState> n = this;
            int pos = 0;
            while (n?.Link != null)
            {
                result[pos++] = n.Link.Symbol;
                n = n.Next;
            }
            Debug.Assert(pos == this.Depth);
            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("<");
            StackPath<TState> p = this;
            sb.Append(p.Frame.State);
            p = p.Next;
            while (p != null)
            {
                sb.Append(", ");
                sb.Append(p.Frame.State);
                p = p.Next;
            }
            sb.Append(">");
            return sb.ToString();
        }
    }
}
