using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yargon.Parsing
{
    /// <summary>
    /// A token with an associated type.
    /// </summary>
    public interface ITypedToken : IToken
    {
        /// <summary>
        /// Gets the type of token.
        /// </summary>
        /// <value>The type of token.</value>
        ITokenType Type { get; }
    }
}
