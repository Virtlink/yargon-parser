using System;
using Slang.Parsing;

namespace Slang.Parser.Sdf.Productions
{
	public sealed class Iter : ISort
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
		public Iter(IterType type, object separator, ISymbol child)
		{
			this.Type = type;
			this.Separator = separator;
			this.Child = child;
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
