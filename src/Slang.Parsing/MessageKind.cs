using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parsing
{
    /// <summary>
    /// Specifies the kind of message.
    /// </summary>
    public enum MessageKind
    {
        /// <summary>
        /// Unspecified.
        /// </summary>
        None = 0,
        /// <summary>
        /// An informational message.
        /// </summary>
        Info,
        /// <summary>
        /// A warning message.
        /// </summary>
        Warning,
        /// <summary>
        /// An error message.
        /// </summary>
        Error,
    }
}
