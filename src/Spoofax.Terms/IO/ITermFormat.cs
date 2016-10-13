using System;
using System.Diagnostics.Contracts;

namespace Spoofax.Terms.IO
{
	/// <summary>
	/// Represents a format in which terms can be read and written.
	/// </summary>
	public interface ITermFormat
	{
		/// <summary>
		/// Gets whether there is a reader for this format.
		/// </summary>
		/// <value><see langword="true"/> when there's a reader;
		/// otherwise, <see langword="false"/>.</value>
		bool CanRead { get; }

		/// <summary>
		/// Gets whether there is a writer for this format.
		/// </summary>
		/// <value><see langword="true"/> when there's a writer;
		/// otherwise, <see langword="false"/>.</value>
		bool CanWrite { get; }

		/// <summary>
		/// Creates a reader for this format.
		/// </summary>
		/// <param name="termFactory">The term factory to use.</param>
		/// <returns>The created <see cref="ITermReader"/>.</returns>
		/// <exception cref="InvalidOperationException">
		/// There is no reader for this format.
		/// </exception>
		ITermReader CreateReader(TermFactory termFactory);
		
		/// <summary>
		/// Creates a writer for this format.
		/// </summary>
		/// <returns>The created <see cref="ITermWriter"/>.</returns>
		/// <exception cref="InvalidOperationException">
		/// There is no writer for this format.
		/// </exception>
		ITermWriter CreateWriter();
	}
}
