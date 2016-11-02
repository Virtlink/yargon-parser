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
    public sealed class Example2 : ExampleBase
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
            var shifts = new Dictionary<Tuple<State, ITokenType>, State>()
            {
                [Tuple.Create(s0, (ITokenType)ddd)] = s2,

                [Tuple.Create(s1, (ITokenType)pls)] = s4,
                [Tuple.Create(s1, (ITokenType)eof)] = s3,

                [Tuple.Create(s4, (ITokenType)ddd)] = s2,

                [Tuple.Create(s5, (ITokenType)pls)] = s4,
            };
            var gotos = new Dictionary<Tuple<State, ISort>, State>()
            {
                [Tuple.Create(s0, (ISort)S)] = s6,
                [Tuple.Create(s0, (ISort)E)] = s1,
                
                [Tuple.Create(s4, (ISort)E)] = s5,
            };
            var reductions = new Dictionary<State, Reduction>()
            {
                [s2] = r2,
                [s3] = r0,
                [s5] = r1,
            };
            var parseTable = new ParseTable(startState, shifts, gotos, Expand(reductions, tokens));
            var parseTreeBuilder = new ParseTreeBuilder();
            var parser = new SlangParser<State, TypedToken<String>, IParseTree>(parseTable, parseTreeBuilder, new FailingErrorHandler<State, TypedToken<String>>());

            // Input: 1+(2+3)$
            var input = Collect(new []
            {
                Tuple.Create("1", ddd),
                Tuple.Create("+", pls),
                Tuple.Create("2", ddd),
                Tuple.Create("+", pls),
                Tuple.Create("3", ddd),
                Tuple.Create("$", eof),
            });

            // Act
            var result = parser.Parse(TokenProvider.From(input));

            // Assert: Amb(
            //             E(E(E("1"), "+", E("2")), "+", E("3")),
            //             E(E("1"), "+", E(E("2"), "+", E("3")))
            //         )
            Assert.That(result.Success, Is.True);
            var tree = (ParseTreeNode)StripLocations(result.Tree);
            Assert.That(tree.Symbol, Is.EqualTo(ParseTreeBuilder.Amb));
            Assert.That(tree.Children, Is.EquivalentTo(new []
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
    }
}
