using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// Provides tokens.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EnumerableTokenProvider<T> : ITokenProvider<T>
    {
        private readonly IEnumerator<Token<T>> enumerator;
        
        /// <inheritdoc />
        public Token<T> Current => this.enumerator.Current;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerableTokenProvider{T}"/> class.
        /// </summary>
        /// <param name="enumerable">The enumerable to wrap.</param>
        public EnumerableTokenProvider(IEnumerable<Token<T>> enumerable)
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
