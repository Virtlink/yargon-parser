using System.Diagnostics.Contracts;
using System.IO;
using System;
using SCG = System.Collections.Generic;

namespace Spoofax.Terms.IO
{
	/// <summary>
	/// Reads terms from a stream.
	/// </summary>
	public interface ITermReader
	{
		/// <summary>
		/// Reads a term from a stream.
		/// </summary>
		/// <param name="input">The stream to read from.</param>
		/// <returns>A term; or <see langword="null"/>
		/// when there are no more terms to read.</returns>
		/// <remarks>
		/// The stream is not closed by this method.
		/// </remarks>
		ITerm Read(Stream input);
	}

	/// <summary>
	/// Extensions to the <see cref="ITermReader"/> interface.
	/// </summary>
	public static class ITermReaderExtensions
	{
		/// <summary>
		/// Reads the term from a file.
		/// </summary>
		/// <param name="reader">The term reader.</param>
		/// <param name="path">The path to the file to read from.</param>
		/// <returns>The read term.</returns>
		public static ITerm Read(this ITermReader reader, string path)
        {
            #region Contract
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            #endregion

			using (var input = File.OpenRead(path))
			{
				return reader.Read(input);
			}
		}
	}
}
