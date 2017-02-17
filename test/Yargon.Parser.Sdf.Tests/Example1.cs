using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using System.Reflection;
using Yargon.Parser.Sdf.ParseTrees;
using Yargon.Parser.Sdf.Productions.IO;
using Yargon.Parsing;
using Yargon.ATerms;
using Yargon.ATerms.IO;

namespace Yargon.Parser.Sdf
{
	[TestFixture]
	public class Example1
	{
	    private const string Name = "Example1";

        [Test]
		public void ReadParseTableFromFile()
		{
            // Arrange
		    var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Yargon.Parser.Sdf.{Name}.tbl");
            Debug.Assert(stream != null);
            var termFactory = new TrivialTermFactory();
            var productionFormat = new TermProductionFormat(termFactory);
            var reader = new SdfParseTableReader(productionFormat);

            // Act
            var parseTable = reader.Read(new StreamReader(stream));

            // Assert
            // No exceptions.

            // Cleanup
            stream.Dispose();
		}

        [Test]
	    public void ParseString()
        {
            // Arrange
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Yargon.Parser.Sdf.{Name}.tbl");
            var termFactory = new TrivialTermFactory();
            var productionFormat = new TermProductionFormat(termFactory);
            var reader = new SdfParseTableReader(productionFormat);
            var parseTable = reader.Read(new StreamReader(stream));            
            var parseTreeBuilder = new TrivialParseNodeFactory();
            var errorHandler = new FailingErrorHandler<SdfStateRef, Token<CodePoint>>();
            var parser = new YargonParser<SdfStateRef, Token<CodePoint>, IParseNode>(parseTable, parseTreeBuilder, errorHandler);

            // Act
            string input = "keyword";
            var result = parser.Parse(new CharacterProvider(new StringReader(input)));

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Tree.ToString(), Is.EqualTo("appl(\"<START>\" -> LEX-\"Start\"; [appl(LEX-\"Start\" -> \"\"keyword\"\"; [appl(\"\"keyword\"\" -> [107][101][121][119][111][114][100]; [107, 101, 121, 119, 111, 114, 100])])])"));

            // Cleanup
            stream.Dispose();
        }
    }
}
