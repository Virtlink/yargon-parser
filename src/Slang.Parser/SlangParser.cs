using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// Slang GLR parser.
    /// </summary>
    /// <typeparam name="TState">The type of states.</typeparam>
    /// <typeparam name="TToken">The type of tokens.</typeparam>
    /// <typeparam name="TTree">The type of parse tree.</typeparam>
    public sealed class SlangParser<TState, TToken, TTree>
        where TToken : IToken
    {
        private readonly IParseTreeBuilder<TToken, TTree> parseTreeBuilder;

        /// <summary>
        /// Gets the parse table that this parser uses.
        /// </summary>
        /// <value>The parse table.</value>
        public IParseTable<TState, TToken> ParseTable { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SlangParser{TState, TToken, TTree}"/> class.
        /// </summary>
        /// <param name="parseTable">The parse table.</param>
        /// <param name="parseTreeBuilder">The parse tree builder.</param>
        public SlangParser(IParseTable<TState, TToken> parseTable, IParseTreeBuilder<TToken, TTree> parseTreeBuilder)
        {
            #region Contract
            if (parseTable == null)
                throw new ArgumentNullException(nameof(parseTable));
            if (parseTreeBuilder == null)
                throw new ArgumentNullException(nameof(parseTreeBuilder));
            #endregion

            this.ParseTable = parseTable;
            this.parseTreeBuilder = parseTreeBuilder;
        }
        #endregion

        /// <summary>
        /// Parses the output of the specified token provider into a parse tree.
        /// </summary>
        /// <param name="tokenProvider">The token provider to read the tokens from.</param>
        /// <returns>The resulting parse tree.</returns>
        public TTree Parse(IEnumerable<TToken> tokenProvider)
        {
            #region Contract
            if (tokenProvider == null)
                throw new ArgumentNullException(nameof(tokenProvider));
            #endregion

            var stacks = new Stacks<TState>(this.ParseTable.StartState);

            // For each token we first reduce all stacks (with the token as lookahead)
            // then shift the token onto the stacks. Only stacks that accept
            // the new token remain.

            foreach (var token in tokenProvider)
            {
                ReduceAll(token, stacks);
                ShiftAll(token, stacks);
            }

            // Assuming there's a reduction S' → S$, and $ (EOF) is the last token
            // returned by the token provider, the current stacks will have a length
            // of two (the reduction to S and the EOF token $). Note that the reduction
            // S' → S$ is never applied.

            // TODO: Apply the remaining reductions, then merge the stacks of size 1 instead.
            // This removes the need for a separate EOF token, and allows arbitrary reductions
            // that reduce to the start symbol.

            return MergeRemainingPaths(stacks);
        }


        /// <summary>
        /// Attempts to reduce all the stacks as much as possible.
        /// </summary>
        /// <param name="lookahead">The lookahead token.</param>
        /// <param name="stacks">The stacks.</param>
        private void ReduceAll(TToken lookahead, Stacks<TState> stacks)
        {
            // Get all the stack tops to try to reduce.
            var worklist = new Queue<Tuple<Frame<TState>, FrameLink<TState>>>(
                from f in stacks.Tops
                from l in f.Links
                select Tuple.Create(f, l));

            while (worklist.Count > 0)
            {
                var frameAndLink = worklist.Dequeue();
                var endFrame = frameAndLink.Item1;
                var endLink = frameAndLink.Item2;

                var reductions = this.ParseTable.GetReductions(endFrame.State, lookahead.Type);
                foreach (var reduction in reductions)
                {
                    int arity = reduction.Arity;
                    // Find all unique paths of length `arity`
                    // that end at `endFrame` and include `endLink`.
                    var paths = stacks.GetPaths(arity, endFrame, endLink);

                    foreach (var path in paths)
                    {
                        var newFrameAndLink = Reduce(reduction, path, stacks);
                        // We have to try and reduce the new stack top as well.
                        worklist.Enqueue(newFrameAndLink);
                    }
                }
            }
        }

        /// <summary>
        /// Applies the specified reduction through the specified path.
        /// </summary>
        /// <param name="reduction">The reduction to apply.</param>
        /// <param name="path">The path to apply the reduction to.</param>
        /// <param name="stacks">The stacks.</param>
        /// <returns>The frame and frame link resulting from the reduction.</returns>
        private Tuple<Frame<TState>, FrameLink<TState>> Reduce(IReduction reduction, StackPath<TState> path, Stacks<TState> stacks)
        {
            #region Contract
            Debug.Assert(reduction != null);
            Debug.Assert(path != null);
            Debug.Assert(path.Depth == reduction.Arity);
            Debug.Assert(stacks != null);
            #endregion
            // Apply the reduction to the path.
            // This will introduce a new link to a new or existing frame.
            var baseFrame = path.Frame; // The top frame after reduction.
            TState nextState;
            if (!this.ParseTable.TryGetGoto(baseFrame.State, reduction.Symbol, out nextState))
                throw new InvalidOperationException($"No goto action for ({baseFrame.State}, {reduction.Symbol}) pair.");
            var arguments = path.GetParseTrees(this.parseTreeBuilder);
            var tree = this.parseTreeBuilder.BuildProduction(reduction, arguments);
            var newLink = new FrameLink<TState>(baseFrame, tree);
            var newFrame = stacks.AddLinkToTopFrame(nextState, newLink);
            return Tuple.Create(newFrame, newLink);
        }

        /// <summary>
        /// Attempts to shift the specified token on top of all the stacks.
        /// </summary>
        /// <param name="token">The token to shift.</param>
        /// <param name="stacks">The stacks.</param>
        /// <remarks>
        /// This method attempts to shift the token onto each currently active stack.
        /// Where this succeeds, this results in a new set of active stacks. The newly
        /// active stacks have links to the previously active stacks, labelled with
        /// the token that was shifted.
        /// </remarks>
        private void ShiftAll(TToken token, Stacks<TState> stacks)
        {
            foreach (var frame in stacks.Tops)
            {
                TState nextState;
                if (!this.ParseTable.TryGetShift(frame.State, token.Type, out nextState))
                    continue;
                Debug.Assert(nextState != null);
                var tree = this.parseTreeBuilder.BuildToken(token);
                var link = new FrameLink<TState>(frame, tree);
                stacks.AddLinkToWorkspaceFrame(nextState, link);
            }
            stacks.Advance();
        }

        /// <summary>
        /// Merges the remaining paths.
        /// </summary>
        /// <param name="stacks">The stacks.</param>
        /// <returns>The tree resulting from merging the paths.</returns>
        private TTree MergeRemainingPaths(Stacks<TState> stacks)
        {
            var paths = stacks.GetPaths(2, null, null);
            var alternatives = paths.Select(p => p.GetParseTrees(this.parseTreeBuilder).First()).ToArray();
            return this.parseTreeBuilder.Merge(alternatives);
        }
    }
}
