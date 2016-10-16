using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtlink.Utilib.Collections;

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

        private ExtHashSet<FrameLink<TState>> links = new ExtHashSet<FrameLink<TState>>();

        /// <summary>
        /// Gets a list of links to parent frames.
        /// </summary>
        /// <value>A list of frame links.</value>
        public IReadOnlyCollection<FrameLink<TState>> Links => this.links;

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
        /// <returns>The link that's added to the frame.</returns>
        public FrameLink<TState> AddLink(FrameLink<TState> link)
        {
            #region Contract
            if (link == null)
                throw new ArgumentNullException(nameof(link));
            #endregion

            FrameLink<TState> existingLink;
            if (this.links.TryGet(link, out existingLink))
            {
                // Merge the new link with the existing link.
                existingLink.MergeWith(link);
                return existingLink;
            }
            else
            {
                // Add a new link.
                this.links.Add(link);
                return link;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.State.ToString();
        }
    }
}
