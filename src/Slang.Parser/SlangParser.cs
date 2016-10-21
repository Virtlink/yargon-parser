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
    /// <remarks>
    /// This parser accepts LR(1) non-cyclic grammars
    /// that have a single start symbol S,
    /// an EOF token $, and a production S → X$,
    /// where X is a single symbol from the grammar.
    /// </remarks>
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
        /// <returns>The parse result.</returns>
        public ParseResult<TTree> Parse(ITokenProvider<TToken> tokenProvider)
        {
            #region Contract
            if (tokenProvider == null)
                throw new ArgumentNullException(nameof(tokenProvider));
            #endregion

            var stacks = new Stacks<TState>(this.ParseTable.StartState);

            // For each token we first reduce all stacks (with the token as lookahead)
            // then shift the token onto the stacks. Only stacks that accept
            // the new token remain.

            while (tokenProvider.MoveNext())
            {
                var token = tokenProvider.Current;

                ReduceAll(token, stacks);
                bool success = ShiftAll(token, stacks);
                if (!success)
                {
                    bool corrected = HandleSyntaxError(token, stacks);
                    if (!corrected)
                        return new ParseResult<TTree>(false, default(TTree));
                }
            }

            // Assuming $ (EOF) is the last token returned by the token provider,
            // shifting it onto the stacks caused all stacks to be discarded except
            // those that accept $. The new states on top are those that result from
            // shifting $, i.e. the accept states.

            var tree = MergeRemainingStacks(stacks);
            return new ParseResult<TTree>(true, tree);
        }


        /// <summary>
        /// Attempts to reduce all the stacks as much as possible.
        /// </summary>
        /// <param name="lookahead">The lookahead token.</param>
        /// <param name="stacks">The stacks.</param>
        private void ReduceAll(TToken lookahead, Stacks<TState> stacks)
        {
            // NOTE: A reduction may introduce a new state with
            // a rejected link, rejecting the state, while a later reduction
            // may add another link that is not rejected to the same state,
            // essentially un-rejecting the state. Therefore we must
            // reduce everything, and we'll just not shift on rejected stacks
            // in the next phase.

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
        private Tuple<Frame<TState>, FrameLink<TState>> Reduce(IReduction reduction, StackPath<TState> path,
            Stacks<TState> stacks)
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

            // TODO: We could check the right-hand symbols of the reduction using path.GetSymbols().
            // Since I've proven that these should always match, a mismatch would indicate an error
            // in the built parse table. Therefore this should be a Debug.Assert().

            var arguments = path.GetParseTrees(this.parseTreeBuilder);
            var tree = this.parseTreeBuilder.BuildProduction(reduction, arguments);
            var newLink = new FrameLink<TState>(baseFrame, reduction.Symbol, reduction.Rejects, tree);
            return stacks.AddLinkToTopFrame(nextState, newLink);
        }

        /// <summary>
        /// Attempts to shift the specified token on top of all the stacks.
        /// </summary>
        /// <param name="token">The token to shift.</param>
        /// <param name="stacks">The stacks.</param>
        /// <returns><see langword="true"/> when all went well;
        /// otherwise, <see langword="false"/> when the token could not be shifted.</returns>
        /// <remarks>
        /// This method attempts to shift the token onto each currently active stack.
        /// Where this succeeds, this results in a new set of active stacks. The newly
        /// active stacks have links to the previously active stacks, labelled with
        /// the token that was shifted.
        /// </remarks>
        private bool ShiftAll(TToken token, Stacks<TState> stacks)
        {
            #region Contract
            Debug.Assert(token != null);
            Debug.Assert(stacks != null);
            #endregion

            // We try to shift the token on all non-rejected stacks.

            foreach (var frame in stacks.Tops.Where(f => !f.IsRejected))
            {
                TState nextState;
                if (!this.ParseTable.TryGetShift(frame.State, token.Type, out nextState))
                    continue;

                Debug.Assert(nextState != null);
                var tree = this.parseTreeBuilder.BuildToken(token);
                var link = new FrameLink<TState>(frame, token.Type, false, tree);
                stacks.AddLinkToWorkspaceFrame(nextState, link);
            }

            return stacks.Advance();
        }


        /// <summary>
        /// Handles a syntax error.
        /// </summary>
        /// <param name="token">The token that could not be shifted.</param>
        /// <param name="stacks">The stacks.</param>
        /// <param name="errorConsumer">The error consumer.</param>
        /// <returns><see langword="true"/> when the error was corrected and parsing can continue;
        /// otherwise, <see langword="false"/> when the error was not corrected.</returns>
        private bool HandleSyntaxError(TToken token, Stacks<TState> stacks)
        {
            #region Contract
            Debug.Assert(token != null);
            Debug.Assert(stacks != null);
            #endregion

            // TODO: Implement error handling.
            
            // TODO: Report error.

            return false;
        }

        /// <summary>
        /// Merges the remaining stacks.
        /// </summary>
        /// <param name="stacks">The stacks.</param>
        /// <returns>The tree resulting from merging the stacks.</returns>
        private TTree MergeRemainingStacks(Stacks<TState> stacks)
        {
            #region Contract
            Debug.Assert(stacks != null);
            #endregion

            // At this point the last symbol shifted onto the should be $, the EOF symbol.

            // Ideally the grammar includes a production S' → S$, where
            // the shifting of $ results in stacks that have a height of two.
            // The relevant parse tree is then on the link between the start state
            // and the next state. However, a poorly constructed grammar may have
            // a production S' → ABC$, resulting in higher stacks. Since it wouldn't
            // be clear which tree we have to return, we'll just error.
            if (stacks.Tops.Any(t => t.MaxHeight > 2))
            {
                throw new InvalidOperationException(
                    "The remaining stacks are too high. The grammar should have a production S' → S$, " +
                    "where S' is the start symbol and $ is the EOF token.");
            }

            // Since at least S$ are on the stacks, the minimum height must be 2.
            Debug.Assert(stacks.Tops.Any(t => t.MinHeight == 2));
            // Since at most S$ are on the stack (due to the previous check), the maximum height must be 2.
            Debug.Assert(stacks.Tops.Any(t => t.MaxHeight == 2));

            var paths = stacks.GetPaths(2, null, null);
            var alternatives = paths.Select(p => p.GetParseTrees(this.parseTreeBuilder).First()).ToArray();
            return this.parseTreeBuilder.Merge(alternatives);
        }
    }
}
