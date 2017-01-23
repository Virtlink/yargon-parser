using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Yargon.Parsing;

namespace Yargon.Parser
{
    /// <summary>
    /// Describes a parse result.
    /// </summary>
    public sealed class ParseResult<TTree>
    {
        /// <summary>
        /// Gets whether parsing was successful.
        /// </summary>
        /// <value><see langword="true"/> when no syntax errors occurred;
        /// otherwise, <see langword="false"/>.</value>
        public bool Success { get; }

        /// <summary>
        /// Gets the parse tree.
        /// </summary>
        /// <value>The parse tree;
        /// or the default of <typeparamref name="TTree"/> if parsing failed.</value>
        [CanBeNull]
        public TTree Tree { get; }

        /// <summary>
        /// Gets the messages that the parser produced.
        /// </summary>
        /// <value>A collection of messages.</value>
        public IReadOnlyCollection<IMessage> Messages { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseResult{TTree}"/> class.
        /// </summary>
        /// <param name="success">Whether parsing was successful.</param>
        /// <param name="tree">The resulting parse tree.</param>
        /// <param name="messages">The parse messages.</param>
        public ParseResult(bool success, [CanBeNull] TTree tree, IReadOnlyCollection<IMessage> messages)
        {
            #region Contract
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));
            #endregion

            this.Success = success;
            this.Tree = tree;
            this.Messages = messages;
        }
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Success ? "success" : "failed" + " " + this.Tree;
        }
    }
}
