using System;
using Slang.Parsing;

namespace Slang.Parser.Sdf.Productions
{
	public sealed class Alt : ISort
	{
		public ISymbol Left
		{ get; }

		public ISymbol Right
		{ get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Alt"/> class.
		/// </summary>
		public Alt(ISymbol left, ISymbol right)
		{
			#region Contract
			if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
			#endregion

			this.Left = left;
			this.Right = right;
		}
		#endregion

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{this.Left} | {this.Right}";
		}
	}
}
