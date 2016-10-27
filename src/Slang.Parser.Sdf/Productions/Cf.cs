using System;
using Slang.Parsing;

namespace Slang.Parser.Sdf.Productions
{
	public sealed class Cf : ISort
    {
		public ISymbol Child
		{ get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Cf"/> class.
		/// </summary>
		public Cf(ISymbol child)
		{
			#region Contract
			if (child == null)
                throw new ArgumentNullException(nameof(child));
			#endregion

			this.Child = child;
		}
		#endregion

		/// <inheritdoc />
		public override string ToString()
		{
			return $"CF-{this.Child}";
		}
	}
}
