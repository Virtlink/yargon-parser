using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Yargon.Parsing;
using Yargon.ATerms;
using Virtlink.Utilib.Collections;

namespace Yargon.Parser.Sdf.Productions.IO
{
	partial class TermProductionFormat
	{
		/// <inheritdoc />
		public Production Read(ITerm representation)
        {
            #region Contract
            if (representation == null)
                throw new ArgumentNullException(nameof(representation));
            #endregion

            IConsTerm prod = representation.ToCons("prod", 3);
			IListTerm rhs = (IListTerm)prod[0];
			ITerm lhs = prod[1];
			ITerm attr = prod[2];

			var reduction = ReadReduction(lhs);
			var expression = ReadExpression(rhs);
			string constructor = ReadConstructor(attr);
			var type = ReadProductionType(attr);
			var flags = ReadProductionFlags(attr);
			// TODO: Error on unknown attributes

			return new Production(reduction, expression, constructor, type, flags);
		}

		/// <summary>
		/// Reads a reduction from a term.
		/// </summary>
		/// <param name="term">The term.</param>
		/// <returns>The reduction.</returns>
		private INonTerminal ReadReduction(ITerm term)
        {
            #region Contract
            Debug.Assert(term != null);
            #endregion

            return (INonTerminal)ReadSymbol(term);
		}

		/// <summary>
		/// Reads an expression from a term.
		/// </summary>
		/// <param name="term">The term.</param>
		/// <returns>The expression.</returns>
		private IReadOnlyList<IProductionSymbol> ReadExpression(IListTerm term)
        {
            #region Contract
            Debug.Assert(term != null);
            #endregion

            return term.SubTerms.Select(ReadSymbol).ToArray();
		}

		/// <summary>
		/// Reads a symbol from a term.
		/// </summary>
		/// <param name="term">The term.</param>
		/// <returns>The symbol.</returns>
		private IProductionSymbol ReadSymbol(ITerm term)
        {
            #region Contract
            Debug.Assert(term != null);
            #endregion

            IConsTerm appl = (IConsTerm)term;
			switch (appl.Name)
			{
				case "sort": return new Sort(appl[0].ToString());
				case "char-class": return new CharacterClass(ReadCharacterRanges((IListTerm)appl[0]));
				case "alt": return new Alt(ReadSymbol(appl[0]), ReadSymbol(appl[1]));
				case "cf": return new Cf(ReadSymbol(appl[0]));
				case "lex": return new Lex(ReadSymbol(appl[0]));
				case "layout": return Layout.Instance;
				case "lit": return new Lit(appl[0].ToString());
				case "opt": return new Opt(ReadSymbol(appl[0]));
				case "parameterized-sort": throw new NotImplementedException();
				case "iter": return new Iter(IterType.None, null, ReadSymbol(appl[0]));
				case "iter-star": return new Iter(IterType.ZeroOrMore, null, ReadSymbol(appl[0]));
				case "iter-plus": return new Iter(IterType.OneOrMore, null, ReadSymbol(appl[0]));
				case "iter-sep": return new Iter(IterType.None, null, ReadSymbol(appl[0]));
				case "iter-star-sep": return new Iter(IterType.ZeroOrMore, null, ReadSymbol(appl[0]));
				case "iter-plus-sep": return new Iter(IterType.OneOrMore, null, ReadSymbol(appl[0]));
				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Reads the constructor name from a term.
		/// </summary>
		/// <param name="term">The term.</param>
		/// <returns>The constructor name.</returns>
		private string ReadConstructor(ITerm term)
        {
            #region Contract
            Debug.Assert(term != null);
            #endregion

            if (term.IsCons("no-attrs", 0))
				return null;

			IListTerm list = (IListTerm)term.ToCons("attrs", 1)[0];
			var consTerms = list.SubTerms.Select(t => t.AsCons("cons", 1)).Where(c => c != null);
			IConsTerm consTerm = consTerms.SingleOrDefault();
			return consTerm?[0]?.ToString();
		}

		/// <summary>
		/// Reads the production type from a term.
		/// </summary>
		/// <param name="term">The term.</param>
		/// <returns>The production type.</returns>
		private ProductionType ReadProductionType(ITerm term)
        {
            #region Contract
            Debug.Assert(term != null);
            #endregion

            if (term.IsCons("no-attrs", 0))
				return ProductionType.None;

			ProductionType type = ProductionType.None;
			IListTerm list = (IListTerm)term.ToCons("attrs", 1)[0];
			foreach (var t in list.SubTerms.OfType<IConsTerm>())
			{
				if (t.SubTerms.Count != 0)
					continue;

				// TODO: Error when the type is set more than once.

				switch (t.Name)
				{
					case "reject": type = ProductionType.Reject; break;
					case "prefer": type = ProductionType.Prefer; break;
					case "avoid": type = ProductionType.Avoid; break;
					case "bracket": type = ProductionType.Bracket; break;
					case "assoc":
						{
							var a = t[0];
							if (a.IsCons("left", 0) || a.IsCons("assoc", 0))
								type = ProductionType.LeftAssociative;
							else if (a.IsCons("right", 0))
								type = ProductionType.RightAssociative;
							else if (a.IsCons("non-assoc", 0))
								type = ProductionType.None;
							else
								throw new InvalidOperationException("Unknown associativity: " + a);
						}
						break;
				}
			}

			return type;
		}

		/// <summary>
		/// Reads the production flags from a term.
		/// </summary>
		/// <param name="term">The term.</param>
		/// <returns>The production flags.</returns>
		private ProductionFlags ReadProductionFlags(ITerm term)
        {
            #region Contract
            Debug.Assert(term != null);
            #endregion

            if (term.IsCons("no-attrs", 0))
				return ProductionFlags.None;

			ProductionFlags flags = ProductionFlags.None;
			IListTerm list = (IListTerm)term.ToCons("attrs", 1)[0];
			foreach (var t in list.SubTerms.OfType<IConsTerm>())
			{
				if (t.SubTerms.Count != 0)
					continue;

				switch (t.Name)
				{
					case "recover": flags |= ProductionFlags.Recover; break;
					case "completion": flags |= ProductionFlags.Completion; break;
					case "ignore-indent":
					case "ignore-layout": flags |= ProductionFlags.IgnoreLayout; break;
					case "enforce-newline": flags |= ProductionFlags.NewlineEnforced; break;
					case "longest-match": flags |= ProductionFlags.LongestMatch; break;
				}
			}

			return flags;
		}

		/// <summary>
		/// Reads a character set from a list term.
		/// </summary>
		/// <param name="listTerm">A list of characters and character ranges.</param>
		/// <returns>The read character set.</returns>
		/// <example>
		/// The list term might look like this:
		/// <code>
		/// [range(9,10),13,32]
		/// </code>
		/// </example>
		private IReadOnlySet<CodePoint> ReadCharacterRanges(IListTerm listTerm)
        {
            #region Contract
            Debug.Assert(listTerm != null);
            #endregion

            CodePointSet characters = new CodePointSet();
			foreach (var term in listTerm.SubTerms)
			{
				int? charValue = term.AsInt32();
				IConsTerm rangeTerm = term.AsCons("range", 2);
				if (charValue != null)
				{
					characters.Add(new CodePoint((int)charValue));
				}
				else if (rangeTerm != null)
				{
					int start = rangeTerm[0].ToInt32();
					int end = rangeTerm[1].ToInt32();
					characters.AddRange(new CodePoint(start), new CodePoint(end));
				}
				else
					throw new InvalidOperationException("Unrecognized term: " + term);
			}

			// If the range contains the 8-bit Eof, change it into the 16-bit Eof.
			if (characters.Contains(new CodePoint(256)))
			{
				characters.Remove(new CodePoint(256));
				characters.Add(CodePoint.Eof);
			}

			return characters;
		}
	}
}
