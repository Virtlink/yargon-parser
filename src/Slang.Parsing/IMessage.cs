using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parsing
{
    /// <summary>
    /// A message about a source file.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets the kind of message.
        /// </summary>
        /// <value>A member of the <see cref="MessageKind"/> enumeration.</value>
        MessageKind Kind { get; }

        /// <summary>
        /// Gets the location of the message.
        /// </summary>
        /// <value>A source range; or <see langword="null"/> when the source range is not known.</value>
        SourceRange? Location { get; }

        /// <summary>
        /// Gets the message text.
        /// </summary>
        /// <value>The message text.</value>
        string Text { get; }
    }
}
