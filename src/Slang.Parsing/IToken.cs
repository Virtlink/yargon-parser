using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parsing
{
    /// <summary>
    /// A token.
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Gets the type of token.
        /// </summary>
        /// <value>The token type.</value>
        ITokenType Type { get; }
    }
}
