using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Spoofax.Terms
{
	/// <summary>
	/// A placeholder term.
	/// </summary>
	public interface IPlaceholderTerm : ITerm
	{
		/// <summary>
		/// Gets the template of the placeholder.
		/// </summary>
		/// <value>The template <see cref="ITerm"/>.</value>
		ITerm Template { get; }
	}
}
