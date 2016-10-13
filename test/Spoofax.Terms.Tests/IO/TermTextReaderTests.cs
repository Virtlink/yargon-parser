using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Spoofax.Terms.IO
{
	public abstract class TermTextReaderTests : TestBase
	{
		/// <summary>
		/// Creates the Subject Under Test.
		/// </summary>
		/// <returns>The instance to test.</returns>
		public abstract TermTextReader CreateSUT();

		/// <summary>
		/// Creates a writer, used in the test.
		/// </summary>
		/// <returns>The writer.</returns>
		public abstract TermTextWriter CreateWriter();

		[Test]
		public void CanReadFromTextReader()
		{
			// Arrange
			var sut = CreateSUT();
			var term = Factory.Int(1);
			var input = new MemoryStream();
			this.CreateWriter().Write(term, input);
			input.Position = 0;
			var reader = new StreamReader(input);

			// Act
			var result = sut.Read(reader);

			// Assert
			Assert.IsNotNull(result);
		}

		[Test]
		public void CanReadFromString()
		{
			// Arrange
			var sut = CreateSUT();
			var term = Factory.Int(1);
			var input = new MemoryStream();
			this.CreateWriter().Write(term, input);
			input.Position = 0;
			var reader = new StreamReader(input);
			var str = reader.ReadToEnd();

			// Act
			var result = sut.FromString(str);

			// Assert
			Assert.IsNotNull(result);
		}
	}
}
