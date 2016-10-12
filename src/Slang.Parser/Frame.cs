using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parser
{
    /// <summary>
    /// A stack frame on the Graph-Structured Stack.
    /// </summary>
    internal sealed class Frame<TState>
    {
        /// <summary>
        /// Gets the state associated with this frame.
        /// </summary>
        /// <value>The state.</value>
        public TState State { get; }

        /// <summary>
        /// Gets the height of the frame.
        /// </summary>
        /// <value>The height of the frame. The first frame has a height of 0.</value>
        /// <remarks>
        /// The height is directly related to the number of tokens shifted onto
        /// the stack at any one point. For example, the height of a top frame after
        /// shifting three tokens onto the stack is 3.
        /// </remarks>
        public int Height { get; }

        private List<FrameLink<TState>> links = new List<FrameLink<TState>>(1);

        /// <summary>
        /// Gets a list of links to parent frames.
        /// </summary>
        /// <value>A list of frame links.</value>
        public IReadOnlyList<FrameLink<TState>> Links => this.links;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Frame{TState}"/> class.
        /// </summary>
        /// <param name="state">The state of the frame.</param>
        /// <param name="height">The height of the frame.</param>
        internal Frame(TState state, int height)
        {
            #region Contract
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height));
            #endregion

            this.State = state;
            this.Height = height;
        }
        #endregion

        /// <summary>
        /// Adds a link.
        /// </summary>
        public void AddLink(FrameLink<TState> link)
        {
            #region Contract
            if (link == null)
                throw new ArgumentNullException(nameof(link));
            #endregion

            this.links.Add(link);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.State.ToString();
        }
    }
}
