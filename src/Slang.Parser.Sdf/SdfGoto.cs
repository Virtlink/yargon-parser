using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtlink.Utilib.Collections;

namespace Slang.Parser.Sdf
{
    /// <summary>
	/// Specifies the state to go to given a state and token or symbol.
	/// </summary>
	public sealed class Goto : IEquatable<Goto>
    {
        private readonly CodePointSet characters;
        /// <summary>
        /// Gets the characters that are recognized.
        /// </summary>
        /// <value>A collection of characters.</value>
        public IReadOnlySet<CodePoint> Characters => this.characters;

        /// <summary>
        /// Gets the recognized labels.
        /// </summary>
        /// <value>A collection of label references.</value>
        public IReadOnlyCollection<Label> Labels { get; }

        /// <summary>
        /// Gets the state to which to jump.
        /// </summary>
        /// <value>A state reference.</value>
        public SdfStateRef NextState { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Goto"/> class.
        /// </summary>
        /// <param name="nextState">The next state.</param>
        /// <param name="characters">The recogized characters.</param>
        /// <param name="labels">The recognized labels.</param>
        public Goto(SdfStateRef nextState, IEnumerable<CodePoint> characters, IEnumerable<Label> labels)
        {
            #region Contract
            if (characters == null)
                throw new ArgumentNullException(nameof(characters));
            if (labels == null)
                throw new ArgumentNullException(nameof(labels));
            #endregion

            this.NextState = nextState;
            this.characters = new CodePointSet(characters);
            this.Labels = labels.ToArray();
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(Goto other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return EqualityComparer<SdfStateRef>.Default.Equals(this.NextState, other.NextState)
                && MultiSetComparer<CodePoint>.Default.Equals(this.Characters, other.Characters)
                && MultiSetComparer<Label>.Default.Equals(this.Labels, other.Labels);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + EqualityComparer<SdfStateRef>.Default.GetHashCode(this.NextState);
                hash = hash * 29 + MultiSetComparer<CodePoint>.Default.GetHashCode(this.Characters);
                hash = hash * 29 + MultiSetComparer<Label>.Default.GetHashCode(this.Labels);
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as Goto);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Goto"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Goto left, Goto right)
        {
            return Object.Equals(left, right);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Goto"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Goto left, Goto right)
        {
            return !(left == right);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public bool HasProduction(Label label)
        {
            return this.Labels.Contains(label);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var labels = String.Join(",", this.Labels);
            return $"goto({this.Characters}; [{labels}]; {this.NextState})";
        }
    }
}
