using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Spoofax.Terms
{
	/// <summary>
	/// A floating-point number term.
	/// </summary>
	public interface IRealTerm : ITerm
	{
		/// <summary>
		/// Gets the floating-point value of the term.
		/// </summary>
		/// <value>The value.</value>
		float Value { get; }
	}
}
