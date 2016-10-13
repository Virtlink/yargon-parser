using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;

namespace Spoofax.Terms.IO
{
	// TODO: Support unicode.

	/// <summary>
	/// Reads terms from a texual format.
	/// </summary>
	public class ATermReader : TermTextReader
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ATermReader"/> class.
		/// </summary>
		/// <param name="termFactory">The term factory to use.</param>
		internal ATermReader(TermFactory termFactory)
			: this(termFactory, TermTextReader.DefaultCulture)
		{
			#region Contract
            if (termFactory == null)
                throw new ArgumentNullException(nameof(termFactory));
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ATermReader"/> class.
		/// </summary>
		/// <param name="termFactory">The term factory to use.</param>
		/// <param name="culture">The culture of the reader.</param>
		internal ATermReader(TermFactory termFactory, CultureInfo culture)
			: base(termFactory, culture)
		{
            #region Contract
            if (termFactory == null)
                throw new ArgumentNullException(nameof(termFactory));
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));
			#endregion
		}
		#endregion

		/// <inheritdoc />
		public override ITerm Read(TextReader reader)
        {
            #region Contract
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            #endregion

            this.ReadWhitespace(reader);

			// TODO: Handle the case where Peek() returns -1.
			char ch = (char)reader.Peek();

			switch (ch)
			{
				case '[': return ReadList(reader);
				case '(': return ReadTuple(reader);
				case '"': return ReadString(reader);
				case '<': return ReadPlaceholder(reader);
			}

			if (Char.IsNumber(ch))
				return ReadNumber(reader);
			else if (Char.IsLetter(ch))
				return ReadCons(reader);
			
			// TODO: Use the new string interpolation from C# 6.
			throw new TermParseException("Invalid term starting with '" + ch + "'.");
		}

		/// <summary>
		/// Reads a list.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <remarks>
		/// The reader must be positioned at the opening `[` character
		/// of the list, and will be positioned at the character following the closing `]`
		/// character of the string or the closing `}` bracket of its annotations (if any).
		/// </remarks>
		private IListTerm ReadList(TextReader reader)
        {
            #region Contract
		    Debug.Assert(reader != null);
            #endregion

            // TODO: Handle the case where Read() returns -1.
            char ch = (char)reader.Read();
			if (ch != '[')
				// TODO: Use C# 6 string interpolation.
				throw new TermParseException("Expected list, got '" + ch + "' character.");

			var terms = this.ReadTermSequence(reader, ',', ']');
			var annotations = this.ReadAnnotations(reader);

			return TermFactory.List(terms, annotations);
		}

	    /// <summary>
		/// Reads a tuple.
		/// </summary>
		/// <param name="reader">The reader.</param>
		private IConsTerm ReadTuple(TextReader reader)
        {
            #region Contract
            Debug.Assert(reader != null);
            #endregion

            return this.ReadCons(reader);
		}

		/// <summary>
		/// Reads a string.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <remarks>
		/// The reader must be positioned at the opening `"` character
		/// of the string, and will be positioned at the character following the closing `"`
		/// character of the string or the closing `}` bracket of its annotations (if any).
		/// </remarks>
		private IStringTerm ReadString(TextReader reader)
        {
            #region Contract
            Debug.Assert(reader != null);
            #endregion

            // TODO: Handle the case where Read() returns -1.
            char ch = (char)reader.Read();
			if (ch != '"')
				// TODO: Use C# 6 string interpolation.
				throw new TermParseException("Expected string, got '" + ch + "' character.");

			var stringBuilder = new StringBuilder();

			// TODO: Handle the case where Read() returns -1.
			ch = (char)reader.Read();

			while (ch != '"')
			{
				if (ch == '\\')
				{
					// Escaped character.
					// TODO: Handle the case where Read() returns -1.
					ch = (char)reader.Read();

					switch (ch)
					{
						case 'n':	// Line feed
							stringBuilder.Append('\n');
							break;
						case 'r':	// Carriage return
							stringBuilder.Append('\r');
							break;
						case 'f':	// Form feed
							stringBuilder.Append('\f');
							break;
						case 't':	// Horizontal tab
							stringBuilder.Append('\t');
							break;
						case 'v':	// Vertical tab
							stringBuilder.Append('\v');
							break;
						case 'b':	// Backspace
							stringBuilder.Append('\b');
							break;
						case '\\':	// Backslash
							stringBuilder.Append('\\');
							break;
						case '\'':	// Single quote
							stringBuilder.Append('\'');
							break;
						case '"':	// Double quote
							stringBuilder.Append('"');
							break;
						// TODO: Support Unicode escapes \x and \u and \U.
						default:
							// TODO: Use C# 6 string interpolation.
							throw new TermParseException("Unrecognized escape sequence: '\\" + ch + "'.");
					}
				}
				else
				{
					// Any non-escaped character.
					stringBuilder.Append(ch);
				}

				// TODO: Handle the case where Read() returns -1.
				ch = (char)reader.Read();
			}

			var annotations = this.ReadAnnotations(reader);

			return TermFactory.String(stringBuilder.ToString(), annotations);
		}

		/// <summary>
		/// Reads a number.
		/// </summary>
		/// <param name="reader">The reader.</param>
		private ITerm ReadNumber(TextReader reader)
        {
            #region Contract
            Debug.Assert(reader != null);
            #endregion

            string ints = ReadDigits(reader);
			string frac = String.Empty;
			string exp = String.Empty;

			// TODO: Handle the case where Peek() returns -1.
			char ch = (char)reader.Peek();
			if (ch == '.')
			{
				// TODO: Handle the case where Read() returns -1.
				reader.Read();

				frac = ReadDigits(reader);

				// TODO: Handle the case where Peek() returns -1.
				ch = (char)reader.Peek();
				if (ch == 'e' || ch == 'E')
				{
					// TODO: Handle the case where Read() returns -1.
					reader.Read();

					exp = ReadDigits(reader);
				}
			}

			if (ints == String.Empty && frac == String.Empty)
				throw new TermParseException("Expected number, got something else.");

			var annotations = this.ReadAnnotations(reader);

			if (frac != String.Empty)
			{
				double value = Double.Parse(
					(!String.IsNullOrWhiteSpace(ints) ? ints : "0") + "." +
					(!String.IsNullOrWhiteSpace(frac) ? frac : "0") + "e" +
					(!String.IsNullOrWhiteSpace(exp) ? exp : "0"),
					CultureInfo.InvariantCulture);
				// TODO: Support doubles instead of floats.
				return TermFactory.Real((float)value, annotations);
			}
			else
			{
				int value = Int32.Parse(ints, CultureInfo.InvariantCulture);
				return TermFactory.Int(value);
			}
		}

		/// <summary>
		/// Reads a placeholder.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <remarks>
		/// The reader must be positioned at the opening `&lt;` character
		/// of the placeholder, and will be positioned at the character following the closing `&gt;`
		/// character of the placeholder or the closing `}` bracket of its annotations (if any).
		/// </remarks>
		private IPlaceholderTerm ReadPlaceholder(TextReader reader)
        {
            #region Contract
            Debug.Assert(reader != null);
            #endregion

            // TODO: Handle the case where Read() returns -1.
            char ch = (char)reader.Read();
			if (ch != '<')
				// TODO: Use C# 6 string interpolation.
				throw new TermParseException("Expected placeholder, got '" + ch + "' character.");

			var template = this.Read(reader);

			this.ReadWhitespace(reader);
			
			// TODO: Handle the case where Read() returns -1.
			ch = (char)reader.Read();
			if (ch != '>')
				// TODO: Use C# 6 string interpolation.
				throw new TermParseException("Expected placeholder end, got '" + ch + "' character.");

			var annotations = this.ReadAnnotations(reader);

			return TermFactory.Placeholder(template, annotations);
		}

		/// <summary>
		/// Reads an application (or tuple).
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <remarks>
		/// The reader must be positioned at the first character of the application or tuple,
		/// and will be positioned at the character following the the application/tuple
		/// or the closing `}` bracket of its annotations (if any).
		/// </remarks>
		private IConsTerm ReadCons(TextReader reader)
        {
            #region Contract
            Debug.Assert(reader != null);
            #endregion

            string name = this.ReadIdentifier(reader);

			this.ReadWhitespace(reader);

			// TODO: Handle the case where Peek() returns -1.
			char ch = (char) reader.Peek();
			IReadOnlyList<ITerm> terms;
			if (ch == '(')
			{
				// TODO: Handle the case where Read() returns -1.
				reader.Read();
				terms = this.ReadTermSequence(reader, ',', ')');
			}
			else
			{
				terms = TermFactory.EmptyTermList;
			}

			var annotations = this.ReadAnnotations(reader);

			return TermFactory.Cons(name, terms, annotations);
		}

		/// <summary>
		/// Reads annotations.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>A list of annotations.</returns>
		private IReadOnlyList<ITerm> ReadAnnotations(TextReader reader)
        {
            #region Contract
            Debug.Assert(reader != null);
            #endregion

            // TODO: Handle the case where Peek() returns -1.
            char ch = (char) reader.Peek();
			if (ch != '{')
				// No annotations to read.
				return TermFactory.EmptyTermList;

			// TODO: Handle the case where Read() returns -1.
			reader.Read();

			return ReadTermSequence(reader, ',', '}');
		}

		/// <summary>
		/// Reads an identifier.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>The read identifier, which may be an empty string.</returns>
		/// <remarks>
		/// The reader must be positioned at the first character of the identifier,
		/// and will be positioned at the first non-identifier character following the identifier.
		/// </remarks>
		private string ReadIdentifier(TextReader reader)
        {
            #region Contract
            Debug.Assert(reader != null);
            #endregion

            StringBuilder stringBuilder = new StringBuilder();

			// TODO: Handle the case where Peek() returns -1.
			char ch = (char)reader.Peek();
			if (ATermReader.IsValidIdentifierFirstChar(ch))
			{
				// TODO: Handle the case where Read() returns -1.
				ch = (char) reader.Read();
				stringBuilder.Append(ch);

				// TODO: Handle the case where Peek() returns -1.
				ch = (char) reader.Peek();
				while (ATermReader.IsValidIdentifierChar(ch))
				{
					// TODO: Handle the case where Read() returns -1.
					ch = (char) reader.Read();
					stringBuilder.Append(ch);

					// TODO: Handle the case where Peek() returns -1.
					ch = (char) reader.Peek();
				}
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Reads a sequence of digits.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>The read digits, which may be an empty string.</returns>
		/// <remarks>
		/// The reader must be positioned at the first digit,
		/// and will be positioned at the first non-digit character following the digits.
		/// </remarks>
		private string ReadDigits(TextReader reader)
        {
            #region Contract
            Debug.Assert(reader != null);
            #endregion

            StringBuilder stringBuilder = new StringBuilder();

			// TODO: Handle the case where Peek() returns -1.
			char ch = (char)reader.Peek();
			while (Char.IsDigit(ch))
			{
				// TODO: Handle the case where Read() returns -1.
				ch = (char) reader.Read();
				stringBuilder.Append(ch);

				// TODO: Handle the case where Peek() returns -1.
				ch = (char)reader.Peek();
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Returns whether the specified character is valid as the first character
		/// of an identifier.
		/// </summary>
		/// <param name="ch">The character to check.</param>
		/// <returns><see langword="true"/> when it is valid;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		private static bool IsValidIdentifierFirstChar(char ch)
		{
			return Char.IsLetter(ch)
				|| ch == '_'
				|| ch == '-'
				|| ch == '+'
				|| ch == '*'
				|| ch == '$';
		}

		/// <summary>
		/// Returns whether the specified character is valid as a subsequent character
		/// of an identifier.
		/// </summary>
		/// <param name="ch">The character to check.</param>
		/// <returns><see langword="true"/> when it is valid;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		private static bool IsValidIdentifierChar(char ch)
		{
			return Char.IsLetterOrDigit(ch)
				|| ch == '_'
				|| ch == '-'
				|| ch == '+'
				|| ch == '*'
				|| ch == '$';
		}

		/// <summary>
		/// Reads a sequence of terms.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="separator">The separator character.</param>
		/// <param name="end">The end character.</param>
		/// <returns>The read terms.</returns>
		/// <remarks>
		/// The <paramref name="end"/> character is also consumed.
		/// </remarks>
		private IReadOnlyList<ITerm> ReadTermSequence(TextReader reader, char separator, char end)
        {
            #region Contract
            Debug.Assert(reader != null);
            #endregion

            this.ReadWhitespace(reader);

			// TODO: Handle the case where Peek() returns -1.
			char ch = (char)reader.Peek();
			if (ch == end)
			{
				// TODO: Handle the case where Read() returns -1.
				reader.Read();
				return TermFactory.EmptyTermList;
			}

			var terms = new List<ITerm>();
			do
			{
				var term = this.Read(reader);
				terms.Add(term);

				this.ReadWhitespace(reader);
				// TODO: Handle the case where Peek() returns -1.
				ch = (char) reader.Read();
			} while (ch == separator);

			if (ch != end)
				// TODO: Use C# string interpolation.
				throw new TermParseException($"Term sequence didn't end with '{end}': '{ch}'.");

			return terms;
		}

		/// <summary>
		/// Reads whitespace.
		/// </summary>
		/// <param name="reader">The reader.</param>
		private void ReadWhitespace(TextReader reader)
        {
            #region Contract
            Debug.Assert(reader != null);
            #endregion

            // TODO: Handle the case where Peek() returns -1.
            while (Char.IsWhiteSpace((char) reader.Peek()))
				reader.Read();
		}
	}
}
