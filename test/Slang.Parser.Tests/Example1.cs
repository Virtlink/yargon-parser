using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// An example that parses a string with a simple LR(0) grammar.
    /// </summary>
    [TestFixture]
    public sealed class Example1
    {
        [Test]
        public void Test()
        {
            // Arrange
            // Sorts
            var S = new Sort("S");
            var E = new Sort("E");
            var T = new Sort("T");
            // Token types
            var min = new TokenType("-");
            var nnn = new TokenType("n");
            var lbr = new TokenType("(");
            var rbr = new TokenType(")");
            var eof = new TokenType("$");
            var tokens = new[] {min, nnn, lbr, rbr, eof};
            // Reductions
            var r1 = new Reduction(S, 2);   // S → E $
            var r2 = new Reduction(E, 3);   // E → E - T
            var r3 = new Reduction(E, 1);   // E → T
            var r4 = new Reduction(T, 1);   // T → n
            var r5 = new Reduction(T, 3);   // T → ( E )
            // States
            var s1 = new State("s1");
            var s2 = new State("s2");
            var s3 = new State("s3");
            var s4 = new State("s4");
            var s5 = new State("s5");
            var s6 = new State("s6");
            var s7 = new State("s7");
            var s8 = new State("s8");
            var s9 = new State("s9");
            var s10 = new State("s10");

            // Parse table (LR(0))
            var startState = s1;
            var gotos = new Dictionary<Tuple<State, ISymbol>, State>()
            {
                [Tuple.Create(s1, (ISymbol)nnn)] = s3,
                [Tuple.Create(s1, (ISymbol)lbr)] = s6,
                [Tuple.Create(s1, (ISymbol)E)] = s4,
                [Tuple.Create(s1, (ISymbol)T)] = s2,

                [Tuple.Create(s4, (ISymbol)min)] = s7,
                [Tuple.Create(s4, (ISymbol)eof)] = s5,

                [Tuple.Create(s6, (ISymbol)nnn)] = s3,
                [Tuple.Create(s6, (ISymbol)lbr)] = s6,
                [Tuple.Create(s6, (ISymbol)E)] = s9,
                [Tuple.Create(s6, (ISymbol)T)] = s2,

                [Tuple.Create(s7, (ISymbol)nnn)] = s3,
                [Tuple.Create(s7, (ISymbol)lbr)] = s6,
                [Tuple.Create(s7, (ISymbol)T)] = s8,

                [Tuple.Create(s9, (ISymbol)min)] = s7,
                [Tuple.Create(s9, (ISymbol)rbr)] = s10,
            };
            var reductions = new Dictionary<State, Reduction>()
            {
                [s2] = r3,
                [s3] = r4,
                [s5] = r1,
                [s8] = r2,
                [s10] = r5,
            };
            var parseTable = new ParseTable(startState, gotos, Expand(reductions, tokens));
            var parseTreeBuilder = new ParseTreeBuilder();
            var parser = new SlangParser<State, Token, IParseTree>(parseTable, parseTreeBuilder);

            // Input: 1-(2-3)$
            var input = new[]
            {
                new Token(nnn, "1"),
                new Token(min, "-"),
                new Token(lbr, "("),
                new Token(nnn, "2"),
                new Token(min, "-"),
                new Token(nnn, "3"),
                new Token(rbr, ")"),
                new Token(eof, "$"),
            };

            // Act
            var tree = parser.Parse(input);

            // Assert: E(E(T("1")), "-", T("(", E(E(T("2")), "-", T("3")), ")"))
            Assert.That(tree, Is.EqualTo(
                Node(E,
                    Node(E, Node(T, Token(nnn, "1"))),
                    Token(min, "-"),
                    Node(T,
                        Token(lbr, "("),
                        Node(E,
                            Node(E, Node(T, Token(nnn, "2"))),
                            Token(min, "-"),
                            Node(T, Token(nnn, "3"))
                        ),
                        Token(rbr, ")")
                    )
                )
            ));
        }

        private ParseTreeNode Node(Sort symbol, params IParseTree[] children) => new ParseTreeNode(symbol, children);
        private Token Token(TokenType type, string value) => new Token(type, value);

        private Dictionary<Tuple<State, ITokenType>, IReadOnlyCollection<IReduction>> Expand(Dictionary<State, Reduction> reductions, IReadOnlyCollection<TokenType> tokens)
        {
            var result = new Dictionary<Tuple<State, ITokenType>, IReadOnlyCollection<IReduction>>();
            foreach (var pair in reductions)
            {
                foreach (var token in tokens)
                {
                    result.Add(Tuple.Create(pair.Key, (ITokenType)token), new [] { pair.Value });
                }
            }
            return result;
        }
    }
}
