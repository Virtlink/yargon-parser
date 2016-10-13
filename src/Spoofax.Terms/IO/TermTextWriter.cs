using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;

namespace Spoofax.Terms.IO
{
	/// <summary>
	/// Base class for a term text writer.
	/// </summary>
	public abstract class TermTextWriter : ITermWriter
	{
		/// <summary>
		/// Gets the default culture of a text writer.
		/// </summary>
		/// <value>The default culture.</value>
		internal static CultureInfo DefaultCulture => CultureInfo.InvariantCulture;
		
		/// <summary>
		/// Gets the current culture of the writer.
		/// </summary>
		/// <value>The current culture.</value>
		public CultureInfo Culture { get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="TermTextWriter"/> class.
		/// </summary>
		/// <param name="culture">The culture of the writer.</param>
		protected TermTextWriter(CultureInfo culture)
        {
            #region Contract
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));
            #endregion

			this.Culture = culture;
		}
		#endregion

		/// <summary>
		/// Writes a term to a string.
		/// </summary>
		/// <param name="term">The term to write.</param>
		/// <returns>The string representation of the term.</returns>
		public string ToString(ITerm term)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            #endregion

            using (var writer = new StringWriter(this.Culture))
			{
				Write(term, writer);
				return writer.ToString();
			}
		}

		/// <inheritdoc />
		public void Write(ITerm term, Stream output)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            #endregion

            using (var writer = new StreamWriter(output, Encoding.Default, 4096, true))
			{
				Write(term, writer);
			}
		}

		/// <summary>
		/// Writes term to a <see cref="TextWriter"/>.
		/// </summary>
		/// <param name="term">The term to write.</param>
		/// <param name="writer">The writer to write to.</param>
		/// <remarks>
		/// The writer is not closed by this method.
		/// </remarks>
		public virtual void Write(ITerm term, TextWriter writer)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            #endregion

            // TODO: Implement in F#?
            if (term is IConsTerm)
				this.WriteConsTerm((IConsTerm)term, writer);
			else if (term is IIntTerm)
				this.WriteIntTerm((IIntTerm)term, writer);
			else if (term is IRealTerm)
				this.WriteRealTerm((IRealTerm)term, writer);
			else if (term is IStringTerm)
				this.WriteStringTerm((IStringTerm)term, writer);
			else if (term is IListTerm)
				this.WriteListTerm((IListTerm)term, writer);
			else if (term is IPlaceholderTerm)
				this.WritePlaceholderTerm((IPlaceholderTerm)term, writer);
			else
				throw new NotImplementedException();
		}

		/// <summary>
		/// Writes a application term to the specified writer.
		/// </summary>
		/// <param name="term">The term to write.</param>
		/// <param name="writer">The text writer to write to.</param>
		protected abstract void WriteConsTerm(IConsTerm term, TextWriter writer);

		/// <summary>
		/// Writes an integer number term to the specified writer.
		/// </summary>
		/// <param name="term">The term to write.</param>
		/// <param name="writer">The text writer to write to.</param>
		protected abstract void WriteIntTerm(IIntTerm term, TextWriter writer);

		/// <summary>
		/// Writes a floating-point number term to the specified writer.
		/// </summary>
		/// <param name="term">The term to write.</param>
		/// <param name="writer">The text writer to write to.</param>
		protected abstract void WriteRealTerm(IRealTerm term, TextWriter writer);

		/// <summary>
		/// Writes a string term to the specified writer.
		/// </summary>
		/// <param name="term">The term to write.</param>
		/// <param name="writer">The text writer to write to.</param>
		protected abstract void WriteStringTerm(IStringTerm term, TextWriter writer);

		/// <summary>
		/// Writes a list term to the specified writer.
		/// </summary>
		/// <param name="term">The term to write.</param>
		/// <param name="writer">The text writer to write to.</param>
		protected abstract void WriteListTerm(IListTerm term, TextWriter writer);

		/// <summary>
		/// Writes a placeholder term to the specified writer.
		/// </summary>
		/// <param name="term">The term to write.</param>
		/// <param name="writer">The text writer to write to.</param>
		protected abstract void WritePlaceholderTerm(IPlaceholderTerm term, TextWriter writer);

		/// <summary>
		/// Writes a separated list of terms to the writer.
		/// </summary>
		/// <param name="terms">The terms to write.</param>
		/// <param name="separator">The separator to use.</param>
		/// <param name="writer">The text writer to write to.</param>
		protected void WriteTermsList(IEnumerable<ITerm> terms, string separator, TextWriter writer)
        {
            #region Contract
            if (terms == null)
                throw new ArgumentNullException(nameof(terms));
            if (separator == null)
                throw new ArgumentNullException(nameof(separator));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            #endregion

			using (var enumerator = terms.GetEnumerator())
			{
				if (!enumerator.MoveNext())
					return;

				this.Write(enumerator.Current, writer);
				while (enumerator.MoveNext())
				{
					writer.Write(separator);
					this.Write(enumerator.Current, writer);
				}
			}
		}
	}
}
