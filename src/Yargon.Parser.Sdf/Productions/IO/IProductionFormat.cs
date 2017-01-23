using System;
using System.Diagnostics.Contracts;

namespace Yargon.Parser.Sdf.Productions.IO
{
	/// <summary>
	/// A format of production rules.
	/// </summary>
	/// <typeparam name="T">The type of representation.</typeparam>
	public interface IProductionFormat<T>
	{
		/// <summary>
		/// Reads the representation of a production rule.
		/// </summary>
		/// <param name="representation">The representation.</param>
		/// <returns>The read production rule.</returns>
		Production Read(T representation);

		/// <summary>
		/// Writes a representation of a production rule.
		/// </summary>
		/// <param name="production">The production to represent.</param>
		/// <returns></returns>
		T Write(Production production);
	}
}
