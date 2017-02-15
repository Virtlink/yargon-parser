using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Yargon.Parser
{
    /// <summary>
    /// The Graph-Structured Stack.
    /// </summary>
    public sealed class Stacks<TState>
    {
        /// <summary>
        /// The stack tops.
        /// </summary>
        private Dictionary<TState, Frame<TState>> tops = new Dictionary<TState, Frame<TState>>();

        /// <summary>
        /// The stack workspace (after the stack tops).
        /// </summary>
        private Dictionary<TState, Frame<TState>> workspace = new Dictionary<TState, Frame<TState>>();

        /// <summary>
        /// Gets the height of the stacks.
        /// </summary>
        /// <value>The height of the stacks.</value>
        /// <remarks>
        /// The height is directly related to the number of tokens shifted onto
        /// the stack at any one point. For example, the height of a top frame after
        /// shifting three tokens onto the stack is 3.
        /// </remarks>
        public int Height { get; private set; } = 0;

        /// <summary>
        /// Gets the current stack frames on top of the stack.
        /// </summary>
        /// <value>The stack frames on top of the stack.</value>
        /// <remarks>
        /// There may be more active stacks than stack tops.
        /// </remarks>
        public IReadOnlyCollection<Frame<TState>> Tops => this.tops.Values;

        /// <summary>
        /// Gets the current stack frames in the workspace of the stack.
        /// </summary>
        /// <value>The stack frames in the workspace of the stack.</value>
        /// <remarks>
        /// Call the <see cref="Advance"/> method to make the workspace the new stack tops.
        /// </remarks>
        public IReadOnlyCollection<Frame<TState>> Workspace => this.workspace.Values;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Stacks"/> class.
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        public Stacks(TState initialState)
        {
            #region Contract
            if (initialState == null)
                throw new ArgumentNullException(nameof(initialState));
            #endregion

            this.tops.Add(initialState, new Frame<TState>(initialState, 0));
        }
        #endregion

        /// <summary>
        /// Adds the specified link to the frame with the specified state on top of the stack.
        /// If no frame with that state exists, a new frame is created and added to the top
        /// of the stack.
        /// </summary>
        /// <param name="state">The state of the frame to which to add the link.</param>
        /// <param name="link">The link to add.</param>
        /// <returns>The frame to which the link was added; or <see langword="null"/> if an existing link was re-used.</returns>
        [CanBeNull]
        public Tuple<Frame<TState>, FrameLink<TState>> AddLinkToTopFrame(TState state, FrameLink<TState> link)
        {
            #region Contract
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (link == null)
                throw new ArgumentNullException(nameof(link));
            if (link.Parent.Phase > this.Height)
                throw new ArgumentException("The linked parent frame is not a frame at or before the current stack tops.", nameof(link));
            #endregion

            Frame<TState> frame;
            if (!this.tops.TryGetValue(state, out frame))
            {
                frame = new Frame<TState>(state, this.Height);
                this.tops.Add(state, frame);
            }
            FrameLink<TState> actualLink;
            if (frame.AddLink(link, out actualLink))
                return Tuple.Create(frame, actualLink);
            else
                return null;
        }

        /// <summary>
        /// Adds the specified link to the frame with the specified state in the workspace of the stack.
        /// If no frame with that state exists, a new frame is created and added to the workspace
        /// of the stack.
        /// </summary>
        /// <param name="state">The state of the frame to which to add the link.</param>
        /// <param name="link">The link to add.</param>
        /// <returns>The frame to which the link was added.</returns>
        public Frame<TState> AddLinkToWorkspaceFrame(TState state, FrameLink<TState> link)
        {
            #region Contract
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (link == null)
                throw new ArgumentNullException(nameof(link));
            if (link.Parent.Phase >= this.Height + 1)
                throw new ArgumentException(
                    "The linked parent frame is not a frame before or at the current stack tops.", nameof(link));
            #endregion

            Frame<TState> frame;
            if (!this.workspace.TryGetValue(state, out frame))
            {
                frame = new Frame<TState>(state, this.Height + 1);
                this.workspace.Add(state, frame);
            }
            
            frame.AddLink(link);
            return frame;
        }

        /// <summary>
        /// Makes the stack workspace the new stack top. Any stacks that
        /// are not reachable from the new stack top are automatically discarded.
        /// The new stack workspace is empty.
        /// </summary>
        /// <returns><see langword="true"/> when there are still stacks after advancing;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Advance()
        {
            if (this.workspace.Count == 0)
            {
                // Advancing would discard all stacks.
                return false;
            }

            // Switch the dictionaries.
            var newWorkspace = this.tops;
            newWorkspace.Clear();

            this.tops = this.workspace;
            this.workspace = newWorkspace;
            this.Height += 1;

            Debug.Assert(this.tops.Count > 0);

            return true;
        }

        /// <summary>
        /// Pops the top states and links from the stacks.
        /// </summary>
        /// <returns><see langword="true"/> when the top states and links were successfully popped from all stacks;
        /// otherwise, <see langword="false"/> when the stacks are empty.</returns>
        /// <remarks>
        /// <para>This discards the top token.</para>
        /// <para>This doesn't reinstante any previously discarded stacks.</para>
        /// </remarks>
        public bool Retreat()
        {
            #region Contract
            Debug.Assert(this.workspace.Count == 0, "The workspace must be empty.");
            #endregion

            int newHeight = this.Height - 1;

            if (newHeight < 0)
            {
                // There are no more states to pop.
                return false;
            }

            var newTops = from t in this.Tops
                          from l in t.Links
                          where l.Parent.Phase == newHeight
                          select l.Parent;

            this.tops = newTops.ToDictionary(t => t.State, t => t);

            this.Height = newHeight;

            return true;
        }

        /// <summary>
        /// Gets all paths with the specified length from the stack tops.
        /// </summary>
        /// <param name="length">The length of the path to find, which may be 0.</param>
        /// <param name="topFrame">The top frame of the path to find; or <see langword="null"/> to find paths with any top frame.</param>
        /// <param name="topLink">The top frame link of the path to find; or <see langword="null"/> to find path with any top link.</param>
        /// <returns>An enumerable sequence of paths.</returns>
        public IEnumerable<StackPath<TState>> GetPaths(int length, [CanBeNull] Frame<TState> topFrame, [CanBeNull] FrameLink<TState> topLink)
        {
            #region Contract
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (topFrame != null && !this.Tops.Contains(topFrame))
                throw new ArgumentException("The end frame must be a currently active frame.", nameof(topFrame));
            if (topLink != null && topFrame == null)
                throw new ArgumentNullException(nameof(topFrame));
            if (topLink != null && !topFrame.Links.Contains(topLink))
                throw new ArgumentException("The end link is not a link of the end frame.", nameof(topLink));
            #endregion

            // NOTE: The link `topLink`, if specified, may be a rejected link.
            // We ignore that here. If you want a path with that link, be it rejected or not,
            // you'll get that path. However, any links discovered must not be rejected
            // or they will not form a path.

            // Top frames
            var tops = topFrame != null ? new[] { topFrame } : this.Tops;

            // Create the top path nodes.
            int depth = 0;
            var pathNodes = tops.Select(f => new StackPath<TState>(f)).ToArray();

            // Create the selected top path link.
            if (topFrame != null && topLink != null && depth < length)
            {
                depth += 1;
                pathNodes = new[] { new StackPath<TState>(topLink, pathNodes.Single()) };
            }

            // Create the rest of the path nodes.
            while (depth < length && pathNodes.Any())
            {
                depth += 1;
                pathNodes = (from path in pathNodes
                             from link in path.Frame.Links
                             where !link.IsRejected
                             select new StackPath<TState>(link, path))
                    .ToArray();
            }

            // Return only paths that are not rejected.
            return pathNodes.Where(p => !p.IsRejected);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var graphBuilder = new DotDigraphBuilder<Frame<TState>>();
            var nodes = new List<Frame<TState>>();
            var queue = new Queue<Frame<TState>>(this.Workspace.Concat(this.Tops));
            while (queue.Count > 0)
            {
                var frame = queue.Dequeue();
                nodes.Add(frame);
                graphBuilder.AddNode(frame);
                foreach (var link in frame.Links)
                {
                    graphBuilder.AddEdge(link.Parent, frame, String.Join(" | ", link.Trees));
                    if (!queue.Contains(link.Parent) && !nodes.Contains(link.Parent))
                        queue.Enqueue(link.Parent);
                }
            }
            return graphBuilder.ToString();
        }
    }
}
