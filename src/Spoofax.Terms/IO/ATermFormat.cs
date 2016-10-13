using System;
using System.Diagnostics.Contracts;

namespace Spoofax.Terms.IO
{
	/// <summary>
	/// The ATerm format.
	/// </summary>
	public sealed class ATermFormat : ITermFormat
	{
		/// <summary>
		/// Gets the default instance of the <see cref="ATermFormat"/>.
		/// </summary>
		/// <value>>An <see cref="ATermFormat"/>.</value>
		public static ATermFormat Instance { get; } = new ATermFormat();

        /// <inheritdoc />
        public bool CanRead { get; } = true;

		/// <inheritdoc />
		public bool CanWrite { get; } = true;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ATermFormat"/> class.
		/// </summary>
		private ATermFormat()
		{
			
		}
		#endregion

		/// <summary>
		/// Creates a reader for this format.
		/// </summary>
		/// <param name="termFactory">The term factory to use.</param>
		/// <returns>The created <see cref="ATermReader"/>.</returns>
		public ATermReader CreateReader(TermFactory termFactory)
		{
			#region Contract
            if (termFactory == null)
                throw new ArgumentNullException(nameof(termFactory));
			#endregion

			return new ATermReader(termFactory);
		}

		/// <summary>
		/// Creates a writer for this format.
		/// </summary>
		/// <returns>The created <see cref="ITermWriter"/>.</returns>
		public ATermWriter CreateWriter()
		{
			return new ATermWriter();
		}

		/// <inheritdoc />
		ITermReader ITermFormat.CreateReader(TermFactory termFactory)
        {
            #region Contract
            if (termFactory == null)
                throw new ArgumentNullException(nameof(termFactory));
            #endregion

            return this.CreateReader(termFactory);
		}

		/// <inheritdoc />
		ITermWriter ITermFormat.CreateWriter()
		{
			return this.CreateWriter();
		}
	}
}
