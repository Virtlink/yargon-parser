using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using System.Reflection;
using Yargon.Parser.Sdf.ParseTrees;
using Yargon.Parser.Sdf.Productions.IO;
using Yargon.Parsing;
using Virtlink.ATerms;
using Virtlink.ATerms.IO;

namespace Yargon.Parser.Sdf
{
	[TestFixture]
	public class SdfParseTableReaderTests
    {
		[Test]
		public void ReadParseTableFromFile()
		{
            // Arrange
		    var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Yargon.Parser.Sdf.Parsetable1.tbl");
            Debug.Assert(stream != null);
            var termFactory = new TrivialTermFactory();
            var productionFormat = new TermProductionFormat(termFactory);
            var reader = new SdfParseTableReader(productionFormat);
            
            // Act/Assert
            Assert.That(() =>
            {
                var parseTable = reader.Read(new StreamReader(stream));
            }, Throws.Nothing);

            // Cleanup
            stream.Dispose();
		}

        [Test, Ignore("Test is not correct.")]
	    public void ParseString()
        {
            // Arrange
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Yargon.Parser.Sdf.Parsetable1.tbl");
            var termFactory = new TrivialTermFactory();
            var productionFormat = new TermProductionFormat(termFactory);
            var reader = new SdfParseTableReader(productionFormat);
            var parseTable = reader.Read(new StreamReader(stream));            
            var parseTreeBuilder = new TrivialParseNodeFactory();
            var errorHandler = new FailingErrorHandler<SdfStateRef, Token<CodePoint>>();
            var parser = new YargonParser<SdfStateRef, Token<CodePoint>, IParseNode>(parseTable, parseTreeBuilder, errorHandler);

            // Act
            string input = "module Test entity X {}";
            var result = parser.Parse(new CharacterProvider(new StringReader(input)));

            // Assert
            Assert.That(result.Success, Is.True);
            // TODO: Result is not null, and should not be null.
            Assert.That(result.Tree, Is.Null);

            // Cleanup
            stream.Dispose();
        }
    }
}
