using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Spoofax.Terms.IO
{
	/// <summary>
	/// Writes terms to a stream.
	/// </summary>
	public interface ITermWriter
	{
		/// <summary>
		/// Writes the term to a <see cref="Stream"/>.
		/// </summary>
		/// <param name="term">The term to write.</param>
		/// <param name="output">The stream to write to.</param>
		/// <remarks>
		/// The stream is not closed by this method.
		/// </remarks>
		void Write(ITerm term, Stream output);
	}

	/// <summary>
	/// Extensions to the <see cref="ITermWriter"/> interface.
	/// </summary>
	public static class ITermWriterExtensions
	{
		/// <summary>
		/// Writes the term to a file.
		/// </summary>
		/// <param name="writer">The term writer.</param>
		/// <param name="term">The term to write.</param>
		/// <param name="path">The path to the file to write to.</param>
		/// <remarks>
		/// The file is overwritten by this method.
		/// </remarks>
		public static void Write(this ITermWriter writer, ITerm term, string path)
        {
            #region Contract
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            #endregion

			using (var output = File.Create(path))
			{
				writer.Write(term, output);
			}
		}
	}
}
