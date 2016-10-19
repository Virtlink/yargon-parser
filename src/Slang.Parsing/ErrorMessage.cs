using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parsing
{
    /// <summary>
    /// An error message.
    /// </summary>
    public class ErrorMessage : SourceMessage
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage"/> class.
        /// </summary>
        /// <param name="location">The source location.</param>
        /// <param name="message">The message text.</param>
        public ErrorMessage(SourceRange location, string message)
            : base(location, message)
        {
            // Nothing to do.
        }
        #endregion

        /// <inheritdoc />
        public override string ToString() => $"{this.Location.Start}: error: {this.Message}";
    }
}
