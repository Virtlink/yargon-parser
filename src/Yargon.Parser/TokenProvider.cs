using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yargon.Parsing;

namespace Yargon.Parser
{
    /// <summary>
    /// Functions for working with token providers.
    /// </summary>
    public static class TokenProvider
    {
        /// <summary>
        /// Returns a new token provider created from the specified enumerable.
        /// </summary>
        /// <typeparam name="T">The type of tokens.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>The resulting token provider.</returns>
        public static ITokenProvider<T> From<T>(IEnumerable<T> enumerable)
            where T : IToken
        {
            #region Contract
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            #endregion

            return new EnumerableTokenProvider<T>(enumerable);
        }
    }
}
