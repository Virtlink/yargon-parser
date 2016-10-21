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
    /// An example that has a grammar with a reject production.
    /// </summary>
    [TestFixture]
    public sealed class Example3
    {
        [Test]
        public void Test()
        {
            // Arrange
            // Sorts
            var S = new Sort("S");
            var E = new Sort("E");
            var I = new Sort("I");
            var T = new Sort("T");
            // Token types
            var ths = new TokenType("ths");
            var eof = new TokenType("$");
            var tokens = new[] {ths, eof};
            // Reductions
            var r0 = new Reduction(S, 2);                   // S → E $
            var r1 = new Reduction(E, 1);                   // E → I
            var r2 = new Reduction(E, 1);                   // E → T
            var r3 = new Reduction(I, 1);                   // I → ths
            var r4 = new Reduction(T, 1);                   // T → ths
            var r5 = new Reduction(I, 1, rejects: true);    // I → ths {reject}
            // States
            var s0 = new State("s0");
            var s1 = new State("s1");
            var s2 = new State("s2");
            var s3 = new State("s3");
            var s4 = new State("s4");
            var s5 = new State("s5");

            // Parse table
            var startState = s0;
            var gotos = new Dictionary<Tuple<State, ISymbol>, State>()
            {
                [Tuple.Create(s0, (ISymbol)ths)] = s1,
                [Tuple.Create(s0, (ISymbol)I)] = s2,
                [Tuple.Create(s0, (ISymbol)T)] = s3,
                [Tuple.Create(s0, (ISymbol)E)] = s4,
                
                [Tuple.Create(s4, (ISymbol)eof)] = s5,
            };
            var reductions = new Dictionary<State, IReadOnlyCollection<Reduction>>()
            {
                [s1] = new[] { r3, r4, r5 },
                [s2] = new[] { r1 },
                [s3] = new[] { r2 },
                [s5] = new[] { r0 },
            };
            var parseTable = new ParseTable(startState, gotos, Expand(reductions, tokens));
            var parseTreeBuilder = new ParseTreeBuilder();
            var parser = new SlangParser<State, Token, IParseTree>(parseTable, parseTreeBuilder, new FailingErrorHandler<State, Token>());

            // Input: 1-(2-3)$
            var input = new[]
            {
                new Token(ths, "this"),
                new Token(eof, "$"),
            };

            // Act
            var result = parser.Parse(TokenProvider.From(input));

            // Assert: E(T("this"))
            Assert.That(result.Success, Is.True);
            Assert.That(result.Tree, Is.EqualTo(Node(E,Node(T, Token(ths, "this")))));
        }

        private ParseTreeNode Node(Sort symbol, params IParseTree[] children) => new ParseTreeNode(symbol, children);
        private Token Token(TokenType type, string value) => new Token(type, value);

        private Dictionary<Tuple<State, ITokenType>, IReadOnlyCollection<IReduction>> Expand(Dictionary<State, IReadOnlyCollection<Reduction>> reductions, IReadOnlyCollection<TokenType> tokens)
        {
            var result = new Dictionary<Tuple<State, ITokenType>, IReadOnlyCollection<IReduction>>();
            foreach (var pair in reductions)
            {
                foreach (var token in tokens)
                {
                    result.Add(Tuple.Create(pair.Key, (ITokenType)token), pair.Value);
                }
            }
            return result;
        }
    }
}
