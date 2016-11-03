using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser.Sdf
{
    /// <summary>
    /// Provides character tokens.
    /// </summary>
    public sealed class CharacterProvider : ITokenProvider<Token<CodePoint>>
    {
        private readonly TextReader reader;
        private SourceLocation currentLocation;
        private bool done = false;

        /// <inheritdoc />
        public Token<CodePoint> Current { get; private set; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterProvider"/> class.
        /// </summary>
        /// <param name="reader">The text reader.</param>
        public CharacterProvider(TextReader reader)
        {
            #region Contract
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            #endregion

            this.reader = reader;
            this.currentLocation = new SourceLocation(0, 1, 1);
        }
        #endregion

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (this.done)
                return false;

            var token = ReadToken();
            if (token == null)
            {
                // Cop out when reading failed.
                this.done = true;
                return false;
            }

            // For EOF we return `true` the first time,
            // and `false` for all times after that.
            this.Current = (Token<CodePoint>) token;
            this.done = this.Current.Value == CodePoint.Eof;
            return true;
        }

        /// <summary>
        /// Reads a token from the text reader.
        /// </summary>
        /// <returns>The read token, or the EOF token; or <see langword="null"/> when reading failed.</returns>
        private Token<CodePoint>? ReadToken()
        {
            int read;
            try
            {
                read = this.reader.Read();
            }
            catch (ObjectDisposedException)
            {
                return null;
            }

            if (read == -1)
                return new Token<CodePoint>(CodePoint.Eof, null);

            var from = this.currentLocation;
            // FIXME: Not only does this not deal with CodePoints correctly,
            // it won't count lines properly, treating \r\n as two lines.
            var to = this.currentLocation = this.currentLocation.AddString(((char) read).ToString());
            return new Token<CodePoint>(new CodePoint(read), new SourceRange(from, to));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.reader.Dispose();
        }
    }
}
