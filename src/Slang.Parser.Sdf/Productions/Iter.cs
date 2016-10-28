using System;
using JetBrains.Annotations;
using Slang.Parsing;

namespace Slang.Parser.Sdf.Productions
{
	public sealed class Iter : ISort, IEquatable<Iter>
    {
		/// <summary>
		/// Gets the type.
		/// </summary>
		public IterType Type
		{ get; }

		public object Separator
		{ get; }

		public ISymbol Child
		{ get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Iter"/> class.
		/// </summary>
		public Iter(IterType type, [CanBeNull] object separator, ISymbol child)
		{
			this.Type = type;
			this.Separator = separator;
			this.Child = child;
		}
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Iter);

        /// <inheritdoc />
        public bool Equals(Iter other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return Object.Equals(this.Type, other.Type)
                && Object.Equals(this.Separator, other.Separator)
                && Object.Equals(this.Child, other.Child);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Type.GetHashCode();
                hash = hash * 29 + this.Separator?.GetHashCode() ?? 0;
                hash = hash * 29 + this.Child.GetHashCode();
            }
            return hash;
        }
        #endregion

        public override string ToString()
		{
			if (this.Separator != null)
				return $"{{{this.Child} \"{this.Separator}\"}}{Iter.TypeToString(this.Type)}";
			else
				return $"{this.Child}{Iter.TypeToString(this.Type)}";
		}

		private static string TypeToString(IterType type)
		{
			switch (type)
			{
				case IterType.None:
					return "";
//					throw new NotImplementedException();
				case IterType.ZeroOrMore:
					return "*";
				case IterType.OneOrMore:
					return "+";
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
