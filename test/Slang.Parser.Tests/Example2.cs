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
    /// An example that parses a string with an ambiguous grammar.
    /// </summary>
    [TestFixture]
    public sealed class Example2
    {
        [Test]
        public void Test()
        {
            // Arrange
            // Sorts
            var S = new Sort("S");
            var E = new Sort("E");
            // Token types
            var pls = new TokenType("+");
            var ddd = new TokenType("d");
            var eof = new TokenType("$");
            var tokens = new[] {pls, ddd, eof};
            // Reductions
            var r0 = new Reduction(S, 2);   // S → E $
            var r1 = new Reduction(E, 3);   // E → E + E
            var r2 = new Reduction(E, 1);   // T → d
            // States
            var s0 = new State("s0");
            var s1 = new State("s1");
            var s2 = new State("s2");
            var s3 = new State("s3");
            var s4 = new State("s4");
            var s5 = new State("s5");
            var s6 = new State("s6");

            // Parse table
            var startState = s0;
            var gotos = new Dictionary<Tuple<State, ISymbol>, State>()
            {
                [Tuple.Create(s0, (ISymbol)ddd)] = s2,
                [Tuple.Create(s0, (ISymbol)S)] = s6,
                [Tuple.Create(s0, (ISymbol)E)] = s1,

                [Tuple.Create(s1, (ISymbol)pls)] = s4,
                [Tuple.Create(s1, (ISymbol)eof)] = s3,
                
                [Tuple.Create(s4, (ISymbol)ddd)] = s2,
                [Tuple.Create(s4, (ISymbol)E)] = s5,

                [Tuple.Create(s5, (ISymbol)pls)] = s4,
            };
            var reductions = new Dictionary<State, Reduction>()
            {
                [s2] = r2,
                [s3] = r0,
                [s5] = r1,
            };
            var parseTable = new ParseTable(startState, gotos, Expand(reductions, tokens));
            var parseTreeBuilder = new ParseTreeBuilder();
            var parser = new SlangParser<State, Token, IParseTree>(parseTable, parseTreeBuilder);

            // Input: 1+(2+3)$
            var input = new[]
            {
                new Token(ddd, "1"),
                new Token(pls, "+"),
                new Token(ddd, "2"),
                new Token(pls, "+"),
                new Token(ddd, "3"),
                new Token(eof, "$"),
            };

            // Act
            var result = parser.Parse(TokenProvider.From(input));

            // Assert: Amb(
            //             E(E(E("1"), "+", E("2")), "+", E("3")),
            //             E(E("1"), "+", E(E("2"), "+", E("3")))
            //         )
            Assert.That(result.Success, Is.True);
            Assert.That(((ParseTreeNode)result.Tree).Symbol, Is.EqualTo(ParseTreeBuilder.Amb));
            Assert.That(((ParseTreeNode)result.Tree).Children, Is.EquivalentTo(new []
            {
                Node(E,
                    Node(E, Token(ddd, "1")),
                    Token(pls, "+"),
                    Node(E,
                        Node(E, Token(ddd, "2")),
                        Token(pls, "+"),
                        Node(E, Token(ddd, "3"))
                    )
                ),
                Node(E,
                    Node(E,
                        Node(E, Token(ddd, "1")),
                        Token(pls, "+"),
                        Node(E, Token(ddd, "2"))
                    ),
                    Token(pls, "+"),
                    Node(E, Token(ddd, "3"))
                )
            }));
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
