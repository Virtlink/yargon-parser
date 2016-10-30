using System;
using System.Diagnostics.Contracts;
using Virtlink.ATerms;

namespace Slang.Parser.Sdf.Productions.IO
{
	/// <summary>
	/// Reads and writes production rules represented as terms.
	/// </summary>
	public sealed partial class TermProductionFormat : IProductionFormat<ITerm>
	{
		/// <summary>
		/// The term factory to use.
		/// </summary>
		private readonly TermFactory factory;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="TermProductionFormat"/> class.
		/// </summary>
		/// <param name="termFactory">The term factory to use.</param>
		public TermProductionFormat(TermFactory termFactory)
		{
			#region Contract
			if (termFactory == null)
                throw new ArgumentNullException(nameof(termFactory));
			#endregion

			this.factory = termFactory;
		}
		#endregion
	}
}
