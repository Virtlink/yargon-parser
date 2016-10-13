using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Spoofax.Terms
{
	// TODO: Serializable
	/// <summary>
	/// An integer number term.
	/// </summary>
	public interface IIntTerm : ITerm
	{
		/// <summary>
		/// Gets the integer value of the term.
		/// </summary>
		/// <value>The value.</value>
		int Value { get; }
	}
}
