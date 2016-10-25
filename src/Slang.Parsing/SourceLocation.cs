using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtlink.Utilib;

namespace Slang.Parsing
{
    /// <summary>
    /// A source location.
    /// </summary>
    public struct SourceLocation
    {
        /// <summary>
        /// The zero-based line offset.
        /// </summary>
        private readonly int line;

        /// <summary>
        /// The zero-based character offset on the line.
        /// </summary>
        private readonly int character;

        /// <summary>
        /// Gets the zero-based character offset in the source.
        /// </summary>
        /// <value>The zero-based character offset.</value>
        public int Offset { get; }

        // This is to ensure default(SourceLocation).Line == 1.
        /// <summary>
        /// Gets the one-based line offset.
        /// </summary>
        /// <value>The one-based line offset.</value>
        public int Line => this.line + 1;

        // This is to ensure default(SourceLocation).Character == 1.
        /// <summary>
        /// Gets the one-based character offset on the line.
        /// </summary>
        /// <value>The one-based character offset.</value>
        public int Character => this.character + 1;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceLocation"/> class.
        /// </summary>
        /// <param name="offset">The zero-based character offset.</param>
        /// <param name="line">The one-based line offset.</param>
        /// <param name="character">The one-based character offset on the line.</param>
        public SourceLocation(int offset, int line, int character)
        {
            #region Contract
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + 1 < character)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (line < 1)
                throw new ArgumentOutOfRangeException(nameof(line));
            if (character < 1)
                throw new ArgumentOutOfRangeException(nameof(character));
            #endregion

            this.Offset = offset;
            this.line = line - 1;
            this.character = character - 1;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is SourceLocation && Equals((SourceLocation)obj);

        /// <inheritdoc />
        public bool Equals(SourceLocation other)
        {
            return this.Offset == other.Offset
                && this.Line == other.Line
                && this.Character == other.Character;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Offset.GetHashCode();
                hash = hash * 29 + this.Line.GetHashCode();
                hash = hash * 29 + this.Character.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="SourceLocation"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(SourceLocation left, SourceLocation right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="SourceLocation"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(SourceLocation left, SourceLocation right) => !(left == right);
        #endregion

        /// <summary>
        /// Adds the length and newlines of the specified string to the current source location.
        /// </summary>
        /// <param name="str">The string whose properties to add.</param>
        /// <returns>The new source location.</returns>
        public SourceLocation AddString(string str)
        {
            #region Contract
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            #endregion
            
            int offset = this.Offset + str.Length;
            int line = this.Line + str.CountNewlines();

            int character;
            int lastLineIndex = str.LastIndexOfAny(new char[] { '\r', '\n' });
            if (lastLineIndex >= 0)
                character = str.Length - lastLineIndex;
            else
                character = this.Character + str.Length;
            
            return new SourceLocation(offset, line, character);
        }

        /// <inheritdoc />
        public override string ToString() => $"{this.Line}:{this.Character}";
    }
}
