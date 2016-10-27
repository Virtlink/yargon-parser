using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Slang.Parsing;
using Virtlink.ATerms;
using Virtlink.Utilib.Collections;

namespace Slang.Parser.Sdf
{
    /// <summary>
    /// Reads SDF3 parse tables.
    /// </summary>
    public sealed class SdfParseTableReader
    {
        /// <summary>
        /// The highest character code point.
        /// </summary>
        private const int CharacterMax = 256;
        /// <summary>
        /// The lowest index of a label.
        /// </summary>
        private const int LabelBase = 257;

        /// <summary>
        /// Reads a parse table from the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to read from.</param>
        /// <returns>The read parse table.</returns>
        public SdfParseTable Read(TextReader reader)
        {
            #region Contract
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            #endregion

            // TODO

            throw new NotImplementedException();
        }

        /// <summary>
		/// Parses the parse table from the specified AST.
		/// </summary>
		/// <param name="ast">The AST.</param>
		/// <returns>The resulting <see cref="ParseTable"/>.</returns>
		private SdfParseTable Parse(ITerm ast)
        {
            #region Contract
            Debug.Assert(ast != null);
            #endregion

            var pt = ast.ToCons("parse-table", 5);
            int version = pt[0].ToInt32();
            Assert(version == 4 || version == 6, "Only version 4 or 6 parse tables are supported.");

            var initialState = new SdfStateRef(pt[1].ToInt32());
            var productions = ParseProductions((IListTerm)pt[2]);
            var states = ParseStates((IListTerm)pt[3].ToCons("states", 1)[0], productions);
            var priorities = ParsePriorities((IListTerm)pt[4].ToCons("priorities", 1)[0]);

            // TODO: Injections?

            return new SdfParseTable(initialState, states, productions, priorities);
        }

        /// <summary>
        /// Parses the production rules from the parse table.
        /// </summary>
        /// <param name="listTerm">A list of label terms.</param>
        /// <returns>A list of parsed productions.</returns>
        /// <example>
        /// A label term might look like this:
        /// <code>
        /// label(prod([lit("\""),lex(iter-star(sort("StringChar"))),lit("\"")],lex(sort("STRING")),no-attrs),378)
        /// </code>
        /// </example>
        private IReadOnlyList<Production> ParseProductions(IListTerm listTerm)
        {
            #region Contract
            Debug.Assert(listTerm != null);
            #endregion

            // NOTE: We assume that all label indices are less than the number of labels + 255.
            var result = new Production[listTerm.Count];
            foreach (var term in from t in listTerm.SubTerms select t.ToCons("label", 2))
            {
                // NOTE: Labels and characters are indexed at the same time. Below 256 it denotes
                // a character literal, above 255 it denotes a label with a production.
                // Therefore, we have to subtract 255 since we're dealing with labels separately.
                int labelIndex = term[1].ToInt32() - LabelBase;
                var productionTerm = term[0].ToCons("prod", 3);
                var production = ReadProduction(productionTerm);

                result[labelIndex] = production;
            }

            return result;
        }

        /// <summary>
        /// Parses the states from the parse table.
        /// </summary>
        /// <param name="listTerm">A list of state terms.</param>
        /// <param name="productions">The productions in the parse table.</param>
        /// <returns>The parsed states.</returns>
        /// <example>
        /// A state term might look like this:
        /// <code>
        /// state-rec(0,[goto([47],22)],[action([256],[reduce(0,280,0)])])
        /// </code>
        /// </example>
        private IReadOnlyList<State> ParseStates(IListTerm listTerm, IReadOnlyList<Production> productions)
        {
            #region Contract
            Debug.Assert(listTerm != null);
            Debug.Assert(productions != null);
            #endregion

            // NOTE: We assume that all state indices are less than the number of states.
            var result = new State[listTerm.Count];
            foreach (var term in from t in listTerm.SubTerms select t.ToCons("state-rec", 3))
            {
                int stateIndex = term[0].ToInt32();
                var gotos = ParseGotos((IListTerm)term[1]);
                var actions = ParseActions((IListTerm)term[2], productions);

                result[stateIndex] = new State(gotos, actions);
            }

            return result;
        }

        /// <summary>
        /// Parses the gotos from the parse table.
        /// </summary>
        /// <param name="listTerm">A list of goto terms.</param>
        /// <returns>The parsed gotos.</returns>
        /// <example>
        /// A goto term might look like this:
        /// <code>
        /// goto([range(9,10),13,32],21)
        /// </code>
        /// </example>
        private IReadOnlyList<Goto> ParseGotos(IListTerm listTerm)
        {
            #region Contract
            Debug.Assert(listTerm != null);
            #endregion

            var result = new Goto[listTerm.Count];
            int index = 0;
            foreach (var term in from t in listTerm.SubTerms select t.ToCons("goto", 2))
            {
                var characters = ParseCharacterRanges((IListTerm)term[0]);
                var labels = ParseLabelRanges((IListTerm)term[0]);
                var nextState = new SdfStateRef(term[1].ToInt32());

                result[index++] = new Goto(nextState, characters, labels);
            }

            return result;
        }

        /// <summary>
        /// Parses a character set from the parse table.
        /// </summary>
        /// <param name="listTerm">A list of characters and character ranges.</param>
        /// <returns>The parsed character set.</returns>
        /// <example>
        /// The list term might look like this:
        /// <code>
        /// [range(9,10),13,32]
        /// </code>
        /// </example>
        private IReadOnlySet<CodePoint> ParseCharacterRanges(IListTerm listTerm)
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
                    if (charValue <= CharacterMax)
                        characters.Add(new CodePoint((int)charValue));
                    // Else: ignore. It's a label which we'll add later.
                }
                else if (rangeTerm != null)
                {
                    int start = rangeTerm[0].ToInt32();
                    // Clip the end to the max character we read.
                    int end = Math.Min(rangeTerm[1].ToInt32(), CharacterMax);
                    if (start <= CharacterMax)
                        characters.AddRange(new CodePoint(start), new CodePoint(end));
                    // Else: ignore. It's a range of labels which we'll add later.
                }
                else
                    throw new InvalidParseTableException("Unrecognized term: " + term);
            }

            // If the range contains the 8-bit Eof, change it into the 16-bit Eof.
            if (characters.Contains(new CodePoint(256)))
            {
                characters.Remove(new CodePoint(256));
                characters.Add(CodePoint.Eof);
            }

            return characters;
        }

        /// <summary>
        /// Parses a set of labels from the parse table.
        /// </summary>
        /// <param name="listTerm">A list of label indices and label index ranges.</param>
        /// <returns>The parsed labels set.</returns>
        /// <example>
        /// The list term might look like this:
        /// <code>
        /// [range(309,310),313,332]
        /// </code>
        /// </example>
        private IEnumerable<Label> ParseLabelRanges(IListTerm listTerm)
        {
            #region Contract
            Debug.Assert(listTerm != null);
            #endregion
            // NOTE: Labels have index 256 or higher, so we subtract 256 (LabelOffset).

            var labels = new List<Label>();
            foreach (var term in listTerm.SubTerms)
            {
                int? labelIndex = term.AsInt32();
                IConsTerm rangeTerm = term.AsCons("range", 2);
                if (labelIndex != null)
                {
                    if (labelIndex >= LabelBase)
                        labels.Add(new Label(((int)labelIndex) - LabelBase));
                    // Else: ignore. It's a character which we'll add later.
                }
                else if (rangeTerm != null)
                {
                    // Clip the start to the min label index we read.
                    int start = Math.Max(LabelBase, rangeTerm[0].ToInt32());
                    int end = rangeTerm[1].ToInt32();
                    if (end >= LabelBase)
                    {
                        for (int i = start - LabelBase; i <= end - LabelBase; i++)
                        {
                            labels.Add(new Label(i));
                        }
                    }
                    // Else: ignore. It's a range of characters which we'll add later.
                }
                else
                    throw new InvalidOperationException("Unrecognized term: " + term);
            }
            return labels;
        }

        /// <summary>
        /// Parses the actions from the parse table.
        /// </summary>
        /// <param name="listTerm">A list of action terms.</param>
        /// <param name="productions">The productions in the parse table.</param>
        /// <returns>The parsed actions.</returns>
        /// <example>
        /// An action term might look like this:
        /// <code>
        /// action([47],[shift(22),reduce(0,280,0,[follow-restriction([char-class([42,47])])])])
        /// </code>
        /// </example>
        private IEnumerable<ActionSet> ParseActions(IListTerm listTerm, IReadOnlyList<Production> productions)
        {
            #region Contract
            Debug.Assert(listTerm != null);
            Debug.Assert(productions != null);
            #endregion

            var result = new ActionSet[listTerm.Count];
            int index = 0;
            foreach (var term in from t in listTerm.SubTerms select t.ToCons("action", 2))
            {
                var characters = ParseCharacterRanges((IListTerm)term[0]);
                var actionItems = ParseActionItems((IListTerm)term[1], productions);

                result[index++] = new ActionSet(characters, actionItems);
            }

            return result;
        }

        /// <summary>
        /// Parses the action items from the parse table.
        /// </summary>
        /// <param name="listTerm">A list of action item terms.</param>
        /// <param name="labels">The labels in the parse table.</param>
        /// <returns>The parsed action items.</returns>
        /// <example>
        /// The list term might look like this:
        /// <code>
        /// [shift(22),reduce(0,280,0,[follow-restriction([char-class([42,47])])])]
        /// </code>
        /// </example>
        private IEnumerable<ActionItem> ParseActionItems(IListTerm listTerm, IReadOnlyList<Production> productions)
        {
            #region Contract
            Debug.Assert(listTerm != null);
            Debug.Assert(productions != null);
            #endregion

            var result = new ActionItem[listTerm.Count];
            int index = 0;
            foreach (var term in listTerm.SubTerms)
            {
                ActionItem actionItem;
                if (term.IsCons("accept", 0))
                    actionItem = ParseAccept((IConsTerm)term);
                else if (term.IsCons("shift", 1))
                    actionItem = ParseShift((IConsTerm)term);
                else if (term.IsCons("reduce", 3))
                    actionItem = ParseReduce((IConsTerm)term, productions);
                else if (term.IsCons("reduce", 4))
                    actionItem = ParseReduce((IConsTerm)term, productions);
                else
                    throw new InvalidParseTableException("Unrecognized term: " + term);

                result[index++] = actionItem;
            }

            return result;
        }

        /// <summary>
        /// Parses an accept action item.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns>The parsed action item.</returns>
        /// <example>
        /// The term might look like this:
        /// <code>
        /// accept()
        /// </code>
        /// </example>
        private ActionItem ParseAccept(IConsTerm term)
        {
            #region Contract
            Debug.Assert(term != null);
            Debug.Assert(term.IsCons("accept", 0));
            #endregion

            return new AcceptActionItem();
        }

        /// <summary>
        /// Parses a shift action item.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns>The parsed action item.</returns>
        /// <example>
        /// The term might look like this:
        /// <code>
        /// shift(22)
        /// </code>
        /// </example>
        private ActionItem ParseShift(IConsTerm term)
        {
            #region Contract
            Debug.Assert(term != null);
            Debug.Assert(term.IsCons("shift", 1));
            #endregion

            var nextState = new SdfStateRef(term[0].ToInt32());

            return new ShiftActionItem(nextState);
        }

        /// <summary>
        /// Parses a reduce action item.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="labels">The labels in the parse table.</param>
        /// <returns>The parsed action item.</returns>
        /// <example>
        /// The term might look like this (without lookahead)
        /// <code>
        /// reduce(0,280,0)
        /// </code>
        /// or this (with lookahead)
        /// <code>
        /// reduce(0,280,0,[follow-restriction([char-class([42,47])])])
        /// </code>
        /// </example>
        private ActionItem ParseReduce(IConsTerm term, IReadOnlyList<Production> productions)
        {
            #region Contract
            Debug.Assert(term != null);
            Debug.Assert(term.IsCons("reduce", 3) || term.IsCons("reduce", 4));
            Debug.Assert(productions != null);
            #endregion

            int productionArity = term[0].ToInt32();
            var label = new Label(term[1].ToInt32() - LabelBase);
            ProductionType status = (ProductionType)term[2].ToInt32();
            bool isRecover = productions[label.Index].IsRecover;
            bool isCompletion = productions[label.Index].IsCompletion;
            var followRestriction = term.SubTerms.Count == 4 ? ParseLookaheadCharRanges((IListTerm)term[3]) : Arrays.Empty<IReadOnlySet<CodePoint>>();

            // Redundant information. Let's check it while we're at it.
            var production = productions[label.Index];
            Contract.Assert(production.Arity == productionArity);
            Contract.Assert(production.IsCompletion == isCompletion);
            Contract.Assert(production.IsRecover == isRecover);
            Contract.Assert(production.Type == status);

            return new ReduceActionItem(label, followRestriction);
        }

        /// <summary>
        /// Parses a list of lookahead character ranges.
        /// </summary>
        /// <param name="listTerm">The list term.</param>
        /// <returns>The parse character ranges.</returns>
        /// <example>
        /// The term might look like this:
        /// <code>
        /// [follow-restriction([char-class([42,47])])]
        /// </code>
        /// </example>
        private IReadOnlyList<IReadOnlySet<CodePoint>> ParseLookaheadCharRanges(IListTerm listTerm)
        {
            #region Contract
            Debug.Assert(listTerm != null);
            #endregion

            var result = new List<IReadOnlySet<CodePoint>>(listTerm.Count);
            foreach (var term in listTerm.SubTerms)
            {
                IListTerm l, n;
                if (term.IsCons("look", 2)) // sdf2bundle 2.4
                {
                    l = (IListTerm)term[0][0];
                    n = (IListTerm)term[1];
                }
                else if (term.IsCons("follow-restriction", 1)) // sdf2bundle 2.6
                {
                    l = (IListTerm)term[0][0].ToCons("char-class", 1)[0];
                    n = ((IListTerm)term[0]).Tail;
                }
                else
                    throw new InvalidParseTableException("Unknown term: " + term);

                result.Add(ParseCharacterRanges(l));

                // FIXME: multiple lookahead are not fully supported or tested
                //        (and should work for both 2.4 and 2.6 tables)
                if (n.SubTerms.Count > 0)
                    throw new InvalidParseTableException("Multiple lookahead not fully supported.");

                foreach (var nt in n.SubTerms)
                {
                    result.Add(ParseCharacterRanges((IListTerm)nt[0]));
                }
            }

            return result;
        }

        /// <summary>
        /// Parses the priorities items from the parse table.
        /// </summary>
        /// <param name="listTerm">A list of priority terms.</param>
        /// <returns>The parsed priorities.</returns>
        /// <example>
        /// The term might look like this:
        /// <code>
        /// [arg-gtr-prio(286,2,286), gtr-prio(301,295)]
        /// </code>
        /// </example>
        private IReadOnlyList<Priority> ParsePriorities(IListTerm listTerm)
        {
            #region Contract
            Debug.Assert(listTerm != null);
            #endregion

            var result = new List<Priority>(listTerm.Count);
            foreach (var term in listTerm.SubTerms.Cast<IConsTerm>())
            {
                switch (term.Name)
                {
                    case "left-prio":
                    case "right-prio":
                    case "non-assoc":
                        // Not supported.
                        break;
                    case "gtr-prio":
                        {
                            var left = new Label(term[0].ToInt32() - LabelBase);
                            var right = new Label(term[1].ToInt32() - LabelBase);
                            if (left != right)
                                result.Add(new Priority(left, right, PriorityType.Greater));
                            break;
                        }
                    case "arg-gtr-prio":
                        {
                            var left = new Label(term[0].ToInt32() - LabelBase);
                            var arg = term[1].ToInt32();
                            var right = new Label(term[2].ToInt32() - LabelBase);
                            if (left != right)
                                result.Add(new Priority(left, right, arg, PriorityType.Greater));
                            break;
                        }
                    default:
                        throw new InvalidParseTableException("Unknown priority: " + term);
                }
            }
            return result;
        }

        private Production ReadProduction(ITerm representation)
        {
            // CONTRACT: Inherited from IProductionFormat<T>

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
        private ISort ReadReduction(ITerm term)
        {
            #region Contract
            Debug.Assert(term != null);
            #endregion

            return (ISort)ReadSymbol(term);
        }

        /// <summary>
        /// Reads an expression from a term.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns>The expression.</returns>
        private IReadOnlyList<ISymbol> ReadExpression(IListTerm term)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(term != null);
            Contract.Ensures(Contract.Result<IReadOnlyList<ISymbol>>() != null);
            #endregion

            return term.SubTerms.Select(ReadSymbol).ToArray();
        }

        /// <summary>
        /// Reads a symbol from a term.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns>The symbol.</returns>
        private ISymbol ReadSymbol(ITerm term)
        {
            #region Contract
            Contract.Requires<ArgumentNullException>(term != null);
            Contract.Ensures(Contract.Result<ISymbol>() != null);
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
            Contract.Requires<ArgumentNullException>(term != null);
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
            Contract.Requires<ArgumentNullException>(term != null);
            Contract.Ensures(Enum.IsDefined(typeof(ProductionType), Contract.Result<ProductionType>()));
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
            Contract.Requires<ArgumentNullException>(term != null);
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
            Contract.Requires<ArgumentNullException>(listTerm != null);
            Contract.Ensures(Contract.Result<IReadOnlySet<CodePoint>>() != null);
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

        /// <summary>
        /// Asserts that a condition is <see langword="true"/>,
        /// otherwise throws an <see cref="InvalidParseTableException"/>.
        /// </summary>
        /// <param name="assertion">The assertion to check.</param>
        private void Assert(bool assertion)
        {
            // ReSharper disable once IntroduceOptionalParameters.Local
            Assert(assertion, message: null);
        }

        /// <summary>
        /// Asserts that a condition is <see langword="true"/>,
        /// otherwise throws an <see cref="InvalidParseTableException"/>.
        /// </summary>
        /// <param name="assertion">The assertion to check.</param>
        /// <param name="message">The message to put in the thrown exception; or <see langword="null"/>.</param>
        // ReSharper disable once UnusedParameter.Local
        private void Assert(bool assertion, [CanBeNull] string message)
        {
            if (!assertion)
                throw new InvalidParseTableException(message);
        }
    }
}
