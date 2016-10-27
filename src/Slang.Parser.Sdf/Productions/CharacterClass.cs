using System;
using System.Collections.Generic;
using Slang.Parser.Sdf;
using Slang.Parsing;
using Virtlink.Utilib.Collections;

namespace Slang.Parser.Sdf.Productions
{
	/// <summary>
	/// A character class.
	/// </summary>
	public sealed class CharacterClass : ITokenType
	{
		/// <summary>
		/// Gets the characters in the character class.
		/// </summary>
		/// <value>A set of characters.</value>
		public IReadOnlySet<CodePoint> Characters
		{ get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CharacterClass"/> class.
		/// </summary>
		public CharacterClass(IEnumerable<CodePoint> characters)
		{
			#region Contract
			if (characters == null)
                throw new ArgumentNullException(nameof(ArgumentNullException));
			#endregion

			this.Characters = new CodePointSet(characters);
		}
		#endregion

		/// <inheritdoc />
		public override string ToString()
		{
			return this.Characters.ToString();
		}
	}
}
