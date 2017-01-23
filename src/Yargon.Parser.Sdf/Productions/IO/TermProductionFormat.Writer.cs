using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Yargon.Parsing;
using Virtlink.ATerms;
using Virtlink.Utilib.Collections;

namespace Yargon.Parser.Sdf.Productions.IO
{
	partial class TermProductionFormat : IProductionSymbolVisitor<ITerm>
	{
		/// <inheritdoc />
		public ITerm Write(Production production)
		{
            #region Contract
            if (production == null)
                throw new ArgumentNullException(nameof(production));
            #endregion

            IListTerm rhs = WriteExpression(production.Expression);
			ITerm lhs = WriteReduction(production.Symbol);
			ITerm attr = WriteAttributes(production.Constructor, production.Type, production.Flags);

			return this.factory.Cons("prod", rhs, lhs, attr);
		}

		/// <summary>
		/// Writes an expression into a term.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <returns>The resulting term.</returns>
		private IListTerm WriteExpression(IReadOnlyList<IProductionSymbol> expression)
		{
			#region Contract
			Debug.Assert(expression != null);
			#endregion

			return factory.List(expression.Select(WriteSymbol));
		}

		/// <summary>
		/// Writes a reduction into a term.
		/// </summary>
		/// <param name="reduction">The reduction.</param>
		/// <returns>The resulting term.</returns>
		private ITerm WriteReduction(INonTerminal reduction)
		{
            #region Contract
            Debug.Assert(reduction != null);
			#endregion

			return WriteSymbol(reduction);
		}

		private ITerm WriteSymbol(IProductionSymbol symbol)
		{
            #region Contract
            Debug.Assert(symbol != null);
			#endregion

			return symbol.Accept(this);
		}

		/// <inheritdoc />
		ITerm IProductionSymbolVisitor<ITerm>.VisitSymbol(IProductionSymbol symbol)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            throw new NotSupportedException();
		}

		/// <inheritdoc />
		ITerm IProductionSymbolVisitor<ITerm>.VisitAlt(Alt symbol)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            return factory.Cons("alt", symbol.Left.Accept(this), symbol.Right.Accept(this));
		}

		/// <inheritdoc />
		ITerm IProductionSymbolVisitor<ITerm>.VisitCf(Cf symbol)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            return factory.Cons("cf", symbol.Child.Accept(this));
		}

		/// <inheritdoc />
		ITerm IProductionSymbolVisitor<ITerm>.VisitCharacterClass(CharacterClass symbol)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            return factory.Cons("char-class", WriteCharacterRanges(symbol.Characters));
		}

		/// <summary>
		/// Writes the list of characters into a term.
		/// </summary>
		/// <param name="characters">The collection of characters.</param>
		/// <returns>A list term.</returns>
		private IListTerm WriteCharacterRanges(IReadOnlySet<CodePoint> characters)
		{
            #region Contract
            Debug.Assert(characters != null);
			#endregion

			// Put all characters in one set (that tracks ranges).
			var set = new OrderedRangedSet<int>();
			set.AddAll(characters.Where(c => !c.IsEof).Select(c => unchecked((int)c.Value)));

			// If the range contains the 16-bit Eof, add the 8-bit Eof.
			if (characters.Any(c => c.IsEof))
				set.Add(256);
			
			// Turn each range into a term.
			List<ITerm> result = new List<ITerm>();
			foreach (var range in set.GetRanges())
			{
				if (range.IsEmpty)
					// ReSharper disable once RedundantJumpStatement
					continue;
				else if (range.Start == range.End - 1)
					result.Add(factory.Int(range.Start));
				else
					result.Add(factory.Cons("range", factory.Int(range.Start), factory.Int(range.End - 1)));
			}
			return factory.List(result);
		}

		/// <inheritdoc />
		ITerm IProductionSymbolVisitor<ITerm>.VisitIter(Iter symbol)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            if (symbol.Separator == null)
			{
				switch (symbol.Type)
				{
					case IterType.None: return factory.Cons("iter", symbol.Child.Accept(this));
					case IterType.ZeroOrMore: return factory.Cons("iter-star", symbol.Child.Accept(this));
					case IterType.OneOrMore: return factory.Cons("iter-plus", symbol.Child.Accept(this));
					default: throw new InvalidOperationException();
				}
			}
			else
			{
				throw new NotImplementedException();
				// iter-sep
				// iter-star-sep
				// iter-plus-sep
			}
		}

		/// <inheritdoc />
		ITerm IProductionSymbolVisitor<ITerm>.VisitLayout(Layout symbol)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            return factory.Cons("layout");
		}

		/// <inheritdoc />
		ITerm IProductionSymbolVisitor<ITerm>.VisitLit(Lit symbol)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            return factory.Cons("lit", factory.String(symbol.Text));
		}

		/// <inheritdoc />
		ITerm IProductionSymbolVisitor<ITerm>.VisitLex(Lex symbol)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            return factory.Cons("lex", symbol.Child.Accept(this));
		}

		/// <inheritdoc />
		ITerm IProductionSymbolVisitor<ITerm>.VisitOpt(Opt symbol)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            return factory.Cons("opt", symbol.Child.Accept(this));
		}

		/// <inheritdoc />
		ITerm IProductionSymbolVisitor<ITerm>.VisitSort(Sort symbol)
		{
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            return factory.Cons("lit", factory.String(symbol.Name));
		}

		/// <summary>
		/// Writes attributes into a term.
		/// </summary>
		/// <param name="constructor">The constructor name; or <see langword="null"/>.</param>
		/// <param name="type">The production type.</param>
		/// <param name="flags">The production flags.</param>
		/// <returns>The resulting term.</returns>
		private ITerm WriteAttributes(string constructor, ProductionType type, ProductionFlags flags)
		{
            #region Contract
            Debug.Assert(Enum.IsDefined(typeof(ProductionType), type));
            #endregion

			if (constructor == null && type == ProductionType.None && flags == ProductionFlags.None)
				return factory.Cons("no-attrs");

			var attrs = new List<ITerm>();

			if (constructor != null)
				attrs.Add(factory.Cons("cons", factory.String(constructor)));

			switch (type)
			{
				case ProductionType.Reject:
					attrs.Add(factory.Cons("reject"));
					break;
				case ProductionType.Prefer:
					attrs.Add(factory.Cons("prefer"));
					break;
				case ProductionType.Bracket:
					attrs.Add(factory.Cons("bracket"));
					break;
				case ProductionType.Avoid:
					attrs.Add(factory.Cons("avoid"));
					break;
				case ProductionType.LeftAssociative:
					attrs.Add(factory.Cons("assoc", factory.Cons("left")));
					break;
				case ProductionType.RightAssociative:
					attrs.Add(factory.Cons("assoc", factory.Cons("right")));
					break;
				case ProductionType.None:
					break;
				default: throw new NotSupportedException();
			}

			if (flags.HasFlag(ProductionFlags.Recover))
				attrs.Add(factory.Cons("recover"));
			if (flags.HasFlag(ProductionFlags.Completion))
				attrs.Add(factory.Cons("completion"));
			if (flags.HasFlag(ProductionFlags.IgnoreLayout))
				attrs.Add(factory.Cons("ignore-layout"));
			if (flags.HasFlag(ProductionFlags.NewlineEnforced))
				attrs.Add(factory.Cons("enforce-newline"));
			if (flags.HasFlag(ProductionFlags.LongestMatch))
				attrs.Add(factory.Cons("longest-match"));

			return factory.Cons("attrs", factory.List(attrs));
		}
	}
}
