using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yargon.Parser.Sdf
{
    /// <summary>
	/// A code point.
	/// </summary>
	/// <remarks>
	/// This is a code point for no specific encoding.
	/// </remarks>
	[Serializable]
    public struct CodePoint : IEquatable<CodePoint>, IComparable<CodePoint>, IComparable, IFormattable
    {
        /// <summary>
        /// The Eof code point.
        /// </summary>
        public static readonly CodePoint Eof = new CodePoint(unchecked((uint)-1));

        /// <summary>
        /// Gets the numeric value of the code point.
        /// </summary>
        /// <value>The numeric value.</value>
        public uint Value { get; }

        /// <summary>
        /// Gets whether this code point represents Eof.
        /// </summary>
        /// <value><see langword="true"/> when it represents Eof;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsEof => this == CodePoint.Eof;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CodePoint"/> class.
        /// </summary>
        /// <param name="value">The value of the code point.</param>
        public CodePoint(uint value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodePoint"/> class.
        /// </summary>
        /// <param name="value">The value of the code point.</param>
        public CodePoint(int value)
            : this(unchecked((uint)value))
        {
            #region Contract
            if (value >= Int32.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value));
            #endregion
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is CodePoint && Equals((CodePoint)obj);

        /// <inheritdoc />
        public bool Equals(CodePoint other)
        {
            return this.Value == other.Value;
        }

        /// <inheritdoc />
        public override int GetHashCode() => this.Value.GetHashCode();

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="CodePoint"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(CodePoint left, CodePoint right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="CodePoint"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(CodePoint left, CodePoint right) => !(left == right);
        #endregion

        #region Comparity
        /// <inheritdoc />
        public int CompareTo(CodePoint other)
        {
            return this.Value.CompareTo(other.Value);
        }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (!(obj is CodePoint))
                throw new ArgumentException("Type mismatch.");
            return this.CompareTo((CodePoint)obj);
        }

        /// <summary>
        /// Returns a value that indicates whether one <see cref="CodePoint"/> is greater than another.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >(CodePoint left, CodePoint right) => left.CompareTo(right) > 0;

        /// <summary>
        /// Returns a value that indicates whether one <see cref="CodePoint"/> is greater than or equal to another.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(CodePoint left, CodePoint right) => left.CompareTo(right) >= 0;

        /// <summary>
        /// Returns a value that indicates whether one <see cref="CodePoint"/> is less than another.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <(CodePoint left, CodePoint right) => left.CompareTo(right) < 0;

        /// <summary>
        /// Returns a value that indicates whether one <see cref="CodePoint"/> is less than or equal to another.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(CodePoint left, CodePoint right) => left.CompareTo(right) <= 0;
        #endregion

        #region Arithmetic
        /// <summary>
        /// Computes adding an offset to a <see cref="CodePoint"/>.
        /// </summary>
        /// <param name="codePoint">The code point.</param>
        /// <param name="offset">The offset to add.</param>
        /// <returns>The resulting code point.</returns>
        public static CodePoint operator +(CodePoint codePoint, int offset)
        {
            return new CodePoint(unchecked((uint)(codePoint.Value + offset)));
        }

        /// <summary>
        /// Computes incrementing a <see cref="CodePoint"/>.
        /// </summary>
        /// <param name="codePoint">The code point.</param>
        /// <returns>The resulting code point.</returns>
        public static CodePoint operator ++(CodePoint codePoint)
        {
            return codePoint + 1;
        }

        /// <summary>
        /// Computes subtracting an offset from a <see cref="CodePoint"/>.
        /// </summary>
        /// <param name="codePoint">The code point.</param>
        /// <param name="offset">The offset to subtarct.</param>
        /// <returns>The resulting code point.</returns>
        public static CodePoint operator -(CodePoint codePoint, int offset)
        {
            return new CodePoint(unchecked((uint)(codePoint.Value - offset)));
        }

        /// <summary>
        /// Computes decrementing a <see cref="CodePoint"/>.
        /// </summary>
        /// <param name="codePoint">The code point.</param>
        /// <returns>The resulting code point.</returns>
        public static CodePoint operator --(CodePoint codePoint)
        {
            return codePoint - 1;
        }

        /// <summary>
        /// Computes subtracting a <see cref="CodePoint"/> from another <see cref="CodePoint"/>.
        /// </summary>
        /// <param name="left">The first code point.</param>
        /// <param name="right">The second code point.</param>
        /// <returns>The resulting difference.</returns>
        public static int operator -(CodePoint left, CodePoint right)
        {
            return unchecked((int)(left.Value - right.Value));
        }
        #endregion

        /// <inheritdoc />
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case null:
                case "G":
                    return this.IsEof ? "Eof" : Char.ConvertFromUtf32(unchecked((int)this.Value));
                default:
                    return this.IsEof ? "Eof" : this.Value.ToString(format, formatProvider);
            }
        }

        /// <summary>
        /// Formats the value of the current instance using the specified format provider.
        /// </summary>
        /// <param name="formatProvider">The provider to use to format the value;
        /// or <see langword="null"/> to use the current culture.</param>
        /// <returns>The value of the current instance in the specified format.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return this.ToString(null, formatProvider);
        }

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">The format to use; or <see langword="null"/> to use
        /// the default format defined for the type of the System.IFormattable implementation.</param>
        /// <returns>The value of the current instance in the specified format.</returns>
        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.ToString(null, CultureInfo.CurrentCulture);
        }
    }
}
