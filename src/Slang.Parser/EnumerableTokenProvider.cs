using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parser
{
    /// <summary>
    /// Provides tokens.
    /// </summary>
    /// <typeparam name="TToken"></typeparam>
    public sealed class EnumerableTokenProvider<TToken> : ITokenProvider<TToken>
    {
        private readonly IEnumerator<TToken> enumerator;
        
        /// <inheritdoc />
        public TToken Current => this.enumerator.Current;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerableTokenProvider{TToken}"/> class.
        /// </summary>
        /// <param name="enumerable">The enumerable to wrap.</param>
        public EnumerableTokenProvider(IEnumerable<TToken> enumerable)
        {
            #region Contract
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            #endregion

            this.enumerator = enumerable.GetEnumerator();
        }
        #endregion

        /// <inheritdoc />
        public void Dispose() => this.enumerator.Dispose();

        /// <inheritdoc />
        public bool MoveNext() => this.enumerator.MoveNext();
    }
}
