using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Spoofax.Terms
{
	/// <summary>
	/// A list of terms.
	/// </summary>
	public interface IListTerm : ITerm
	{
		/// <summary>
		/// Gets the number of elements in the list.
		/// </summary>
		/// <value>The number of elements in the list.</value>
		int Count { get; }

		/// <summary>
		/// Gets whether the list is empty.
		/// </summary>
		/// <value><see langword="true"/> when the list is empty;
		/// otherwise, <see langword="false"/>.</value>
		bool IsEmpty { get; }

		/// <summary>
		/// Gets the head term of the list.
		/// </summary>
		/// <value>The head term of the list;
		/// or <see langword="null"/> when it is an empty list.</value>
		ITerm Head { get; }

		/// <summary>
		/// Gets the tail of the list.
		/// </summary>
		/// <value>The tail of the list;
		/// or <see langword="null"/> when it is an empty list..</value>
		IListTerm Tail { get; }
	}
}
