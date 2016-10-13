using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;

namespace Spoofax.Terms.IO
{
	/// <summary>
	/// Base class for a term text reader.
	/// </summary>
	public abstract class TermTextReader : ITermReader
	{
		/// <summary>
		/// Gets the default culture of a text reader.
		/// </summary>
		/// <value>The default culture.</value>
		internal static CultureInfo DefaultCulture
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<CultureInfo>() != null);
				#endregion
				return CultureInfo.InvariantCulture;
			}
		}
		
		/// <summary>
		/// Gets the current culture of the reader.
		/// </summary>
		/// <value>The current culture.</value>
		public CultureInfo Culture { get; }
		
		/// <summary>
		/// Gets the current term factory of the reader.
		/// </summary>
		/// <value>THe current term factory.</value>
		protected TermFactory TermFactory { get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="TermTextReader"/> class.
		/// </summary>
		/// <param name="termFactory">The term factory to use.</param>
		/// <param name="culture">The culture of the reader.</param>
		protected TermTextReader(TermFactory termFactory, CultureInfo culture)
        {
            #region Contract
            if (termFactory == null)
                throw new ArgumentNullException(nameof(termFactory));
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));
            #endregion

			this.TermFactory = termFactory;
			this.Culture = culture;
		}
		#endregion

		/// <summary>
		/// Reads a term from a string.
		/// </summary>
		/// <param name="str">The string to read.</param>
		/// <returns>The read term.</returns>
		public ITerm FromString(string str)
        {
            #region Contract
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            #endregion

            var reader = new StringReader(str);
			return Read(reader);
		}

		/// <inheritdoc />
		public ITerm Read(Stream input)
        {
            #region Contract
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            #endregion

            using (var reader = new StreamReader(input, Encoding.Default, true, 4096, true))
			{
				return Read(reader);
			}
		}

		/// <summary>
		/// Reads term from a <see cref="TextReader"/>.
		/// </summary>
		/// <param name="reader">The reader to read to.</param>
		/// <returns>The term that was read.</returns>
		/// <remarks>
		/// The reader is not closed by this method.
		/// </remarks>
		public abstract ITerm Read(TextReader reader);
	}
}
