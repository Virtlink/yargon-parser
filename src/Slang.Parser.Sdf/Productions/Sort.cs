using System;
using Slang.Parsing;

namespace Slang.Parser.Sdf.Productions
{
	/// <summary>
	/// A syntactic sort.
	/// </summary>
	public sealed class Sort : ISort
    {
		/// <summary>
		/// Gets the name of the sort.
		/// </summary>
		/// <value>The name of the sort.</value>
		public string Name
		{ get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Sort"/> class.
		/// </summary>
		public Sort(string name)
		{
			#region Contract
			if (name == null)
                throw new ArgumentNullException(nameof(name));
			#endregion

			this.Name = name;
		}
		#endregion

		/// <inheritdoc />
		public override string ToString()
		{
			return this.Name;
		}
	}
}
