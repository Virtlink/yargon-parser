using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtlink.Utilib.Collections;

namespace Slang.Parser.Sdf
{
    /// <summary>
	/// 
	/// </summary>
	public sealed class ActionSet : IEquatable<ActionSet>
    {
        private readonly CodePointSet characters;
        /// <summary>
        /// Gets the characters that are recognized.
        /// </summary>
        /// <value>A set of characters.</value>
        public IReadOnlySet<CodePoint> Characters => this.characters;

        /// <summary>
        /// Gets a collection of action items.
        /// </summary>
        /// <value>A collection of action items.</value>
        /// <remarks>
        /// When there's more than one action item, the parser is non-deterministic.
        /// </remarks>
        public IReadOnlyCollection<ActionItem> Items { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionSet"/> class.
        /// </summary>
        /// <param name="characters">The characters that are recognized.</param>
        /// <param name="items">The action items.</param>
        public ActionSet(IEnumerable<CodePoint> characters, IEnumerable<ActionItem> items)
        {
            #region Contract
            if (characters == null)
                throw new ArgumentNullException(nameof(characters));
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            #endregion

            this.characters = new CodePointSet(characters);
            this.Items = items.ToArray();
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as ActionSet);

        /// <inheritdoc />
        public bool Equals(ActionSet other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return this.characters.SetEquals(other.characters)
                && ListComparer<ActionItem>.Default.Equals(this.Items, other.Items);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + MultiSetComparer<CodePoint>.Default.GetHashCode(this.characters);
                hash = hash * 29 + ListComparer<ActionItem>.Default.GetHashCode(this.Items);
            }
            return hash;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="ActionSet"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(ActionSet left, ActionSet right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="ActionSet"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(ActionSet left, ActionSet right) => !(left == right);
        #endregion

        /// <summary>
        /// Gets whether this action accepts the specified character.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns><see langword="true"/> when the character is accepted;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Accepts(CodePoint c)
        {
            return this.Characters.Contains(c);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var items = String.Join(", ", this.Items);
            return $"action({this.Characters}; [{items}])";
        }
    }
}
