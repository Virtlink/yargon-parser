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
    public sealed class Frame<TState>
    {
        /// <summary>
        /// Gets the state associated with this frame.
        /// </summary>
        /// <value>The state.</value>
        public TState State { get; }

        /// <summary>
        /// Gets the phase of the frame.
        /// </summary>
        /// <value>The phase of the frame. The first frame is added in phase 0.</value>
        /// <remarks>
        /// The phase is directly related to the number of tokens shifted onto
        /// the stack at any one point. For example, the phase of a top frame after
        /// shifting three tokens onto the stack is 3.
        /// </remarks>
        public int Phase { get; }

        /// <summary>
        /// Gets the minimum height of this frame.
        /// </summary>
        /// <value>The minimum height of this frame.</value>
        public int MinHeight => this.Links.Count > 0 ? this.Links.Min(l => l.Parent.MinHeight) + 1 : 0;

        /// <summary>
        /// Gets the maximum height of this frame.
        /// </summary>
        /// <value>The maximum height of this frame.</value>
        public int MaxHeight => this.Links.Count > 0 ? this.Links.Max(l => l.Parent.MaxHeight) + 1 : 0;

        private ExtHashSet<FrameLink<TState>> links = new ExtHashSet<FrameLink<TState>>();

        /// <summary>
        /// Gets a list of links to parent frames.
        /// </summary>
        /// <value>A list of frame links.</value>
        public IReadOnlyCollection<FrameLink<TState>> Links => this.links;

        /// <summary>
        /// Gets whether this frame has been rejected.
        /// </summary>
        /// <value><see langword="true"/> when all links to this frame have been rejected;
        /// otherwise, <see langword="false"/>.</value>
        /// <remarks>
        /// This property also returns <see langword="false"/> when the frame has no links.
        /// </remarks>
        public bool IsRejected => this.Links.Count > 0 && this.Links.All(l => l.IsRejected);

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Frame{TState}"/> class.
        /// </summary>
        /// <param name="state">The state of the frame.</param>
        /// <param name="phase">The phase of the frame.</param>
        internal Frame(TState state, int phase)
        {
            #region Contract
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (phase < 0)
                throw new ArgumentOutOfRangeException(nameof(phase));
            #endregion

            this.State = state;
            this.Phase = phase;
        }
        #endregion


        /// <summary>
        /// Adds a link.
        /// </summary>
        public void AddLink(FrameLink<TState> link)
        {
            FrameLink<TState> actualLink;
            AddLink(link, out actualLink);
        }

        /// <summary>
        /// Adds a link.
        /// </summary>
        /// <param name="actualLink">The link that was actually put on the stacks.</param>
        /// <returns><see langword="true"/> when <paramref name="actualLink"/> is a newly added link;
        /// otherwise, <see langword="false"/> when <paramref name="actualLink"/> is an existing link..</returns>
        public bool AddLink(FrameLink<TState> link, out FrameLink<TState> actualLink)
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
                actualLink = existingLink;
                return false;
            }
            else
            {
                // Add a new link.
                this.links.Add(link);
                actualLink = link;
                return true;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.State.ToString();
        }
    }
}
