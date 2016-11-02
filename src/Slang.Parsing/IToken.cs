using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Slang.Parsing
{
    /// <summary>
    /// A token.
    /// </summary>
    public interface IToken : ISymbol
    {
        /// <summary>
        /// Gets the location of the token in the source.
        /// </summary>
        /// <value>The token location; or <see langword="null"/> when the token is virtual.</value>
        [CanBeNull]
        SourceRange? Location { get; }

        /// <summary>
        /// Gets whether the token is virtual.
        /// </summary>
        /// <value><see langword="true"/> when the token is virtual;
        /// otherwise, <see langword="false"/>.</value>
        bool IsVirtual { get; }
    }
}
