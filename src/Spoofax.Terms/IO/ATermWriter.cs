using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using Spoofax.Terms.Extra;

namespace Spoofax.Terms.IO
{
	/// <summary>
	/// Writes a term as a string.
	/// </summary>
	public sealed class ATermWriter : TermTextWriter
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ATermWriter"/> class
		/// with the default culture.
		/// </summary>
		internal ATermWriter()
			: this(TermTextWriter.DefaultCulture)
		{
			// Nothing to do.
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ATermWriter"/> class.
		/// </summary>
		/// <param name="culture">The culture of the writer.</param>
		internal ATermWriter(CultureInfo culture)
			: base(culture)
		{
			#region Contract
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));
			#endregion
		}
		#endregion


		/// <inheritdoc />
		protected override void WriteConsTerm(IConsTerm term, TextWriter writer)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            #endregion

            // TODO: Escape constructor name.
            writer.Write(term.Name);
			writer.Write('(');
			this.WriteTermsList(term.SubTerms, ",", writer);
			writer.Write(')');
			this.WriteAnnotations(term.Annotations, writer);
		}

		/// <inheritdoc />
		protected override void WriteIntTerm(IIntTerm term, TextWriter writer)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            #endregion

            writer.Write(term.Value.ToString(CultureInfo.InvariantCulture));
			this.WriteAnnotations(term.Annotations, writer);
		}

		/// <inheritdoc />
		protected override void WriteRealTerm(IRealTerm term, TextWriter writer)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            #endregion

            writer.Write(term.Value.ToString(CultureInfo.InvariantCulture));
			this.WriteAnnotations(term.Annotations, writer);
		}

		/// <inheritdoc />
		protected override void WriteStringTerm(IStringTerm term, TextWriter writer)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            #endregion

            writer.Write('"');
			string escaped = StringEscaper.Escape(term.Value, new Dictionary<char, string>()
			{
				{ '\n', "\\n" },	// Line feed
				{ '\r', "\\r" },	// Carriage return
				{ '\f', "\\f" },	// Form feed
				{ '\t', "\\t" },	// Horizontal tab
				{ '\v', "\\v" },	// Vertical tab
				{ '\b', "\\b" },	// Backspace
				{ '\\', "\\\\" },	// Backslash
				{ '\'', "\\'" },	// Single quote
				{ '"',  "\\\"" },	// Double quote
			});
			writer.Write(escaped);
			writer.Write('"');
			this.WriteAnnotations(term.Annotations, writer);
		}

		/// <inheritdoc />
		protected override void WriteListTerm(IListTerm term, TextWriter writer)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            #endregion

            writer.Write('[');
			this.WriteTermsList(term.SubTerms, ",", writer);
			writer.Write(']');
			this.WriteAnnotations(term.Annotations, writer);
		}

		/// <inheritdoc />
		protected override void WritePlaceholderTerm(IPlaceholderTerm term, TextWriter writer)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            #endregion

            writer.Write('<');
			this.Write(term.Template, writer);
			writer.Write('>');
			this.WriteAnnotations(term.Annotations, writer);
		}

		/// <summary>
		/// Writes a set of annotations to the writer,
		/// when the set is not empty.
		/// </summary>
		/// <param name="annotations">The set of annotations.</param>
		/// <param name="writer">The text writer to write to.</param>
		private void WriteAnnotations(IReadOnlyCollection<ITerm> annotations, TextWriter writer)
        {
            #region Contract
            if (annotations == null)
                throw new ArgumentNullException(nameof(annotations));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            #endregion

			if (annotations.Count == 0)
				return;

			writer.Write("{");
			this.WriteTermsList(annotations, ",", writer);
			writer.Write("}");
		}
	}
}
