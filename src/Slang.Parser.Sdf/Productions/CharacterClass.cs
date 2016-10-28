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
	public sealed class CharacterClass : ITokenType, IEquatable<CharacterClass>
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

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as CharacterClass);

        /// <inheritdoc />
        public bool Equals(CharacterClass other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return this.Characters.SetEquals(other.Characters);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return 17;
        }
        #endregion

		/// <inheritdoc />
		public override string ToString()
		{
			return this.Characters.ToString();
		}
	}
}
