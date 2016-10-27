using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parsing
{
    /// <summary>
	/// The parse table file is invalid.
	/// </summary>
	[Serializable]
    public class InvalidParseTableException : InvalidOperationException
    {
        private const string DefaultMessage = "The parse table file is invalid.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidParseTableException"/> class.
        /// </summary>
        public InvalidParseTableException()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidParseTableException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidParseTableException(string message)
            : this(message, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidParseTableException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public InvalidParseTableException(string message, Exception inner)
            : base(message ?? InvalidParseTableException.DefaultMessage, inner)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidParseTableException"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The serialization context.</param>
        protected InvalidParseTableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
