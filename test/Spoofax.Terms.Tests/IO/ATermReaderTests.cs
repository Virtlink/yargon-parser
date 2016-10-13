using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Spoofax.Terms;
using Spoofax.Terms.IO;

namespace Spoofax.Terms.IO
{
	[TestFixture]
	public sealed class ATermReaderTests : TestBase
	{
		#region SUT
		private static readonly ATermFormat Format = ATermFormat.Instance;

		/// <summary>
		/// Creates the Subject Under Test.
		/// </summary>
		/// <returns>The instance to test.</returns>
		public ATermReader CreateSUT()
		{
			return Format.CreateReader(Factory);
		}

		/// <summary>
		/// Creates a writer, used in the test.
		/// </summary>
		/// <returns>The writer.</returns>
		public ATermWriter CreateWriter()
		{
			return Format.CreateWriter();
		}

		[TestFixture]
		public sealed class ATermReaderTests_TermTextReaderTests : TermTextReaderTests
		{
			public override TermTextReader CreateSUT()
			{
				return new ATermReaderTests().CreateSUT();
			}

			public override TermTextWriter CreateWriter()
			{
				return new ATermReaderTests().CreateWriter();
			}
		}

		[TestFixture]
		public sealed class ATermReaderTests_ITermReaderTests : ITermReaderTests
		{
			public override ITermReader CreateSUT()
			{
				return new ATermReaderTests().CreateSUT();
			}

			public override ITermWriter CreateWriter()
			{
				return new ATermReaderTests().CreateWriter();
			}
		}
		#endregion

		[Test]
		public void ReadsCons()
		{
			// Arrange
			var sut = CreateSUT();
			string consName = "Cons";
			var term = Factory.Cons(consName, Factory.Int(0), Factory.Int(1));
			string input = this.CreateWriter().ToString(term);

			// Act
			var result = sut.FromString(input);

			// Assert
			Assert.AreEqual(term, result);
		}

		[Test]
		public void ReadsTuple()
		{
			// Arrange
			var sut = CreateSUT();
			var term = Factory.Tuple(Factory.Int(0), Factory.Int(1));
			string input = this.CreateWriter().ToString(term);

			// Act
			var result = sut.FromString(input);

			// Assert
			Assert.AreEqual(term, result);
		}

		[Test]
		public void ReadsInt()
		{
			// Arrange
			var sut = CreateSUT();
			var term = Factory.Int(1);
			string input = this.CreateWriter().ToString(term);

			// Act
			var result = sut.FromString(input);

			// Assert
			Assert.AreEqual(term, result);
		}

		[Test]
		public void ReadsReal()
		{
			// Arrange
			var sut = CreateSUT();
			var term = Factory.Real(4.2f);
			string input = this.CreateWriter().ToString(term);

			// Act
			var result = sut.FromString(input);

			// Assert
			Assert.AreEqual(term, result);
		}

		[Test]
		public void ReadsString()
		{
			// Arrange
			var sut = CreateSUT();
			var term = Factory.String("abc");
			string input = this.CreateWriter().ToString(term);

			// Act
			var result = sut.FromString(input);

			// Assert
			Assert.AreEqual(term, result);
		}

		[Test]
		public void ReadsList()
		{
			// Arrange
			var sut = CreateSUT();
			var term = Factory.List(Factory.Int(0), Factory.Int(1), Factory.Int(2));
			string input = this.CreateWriter().ToString(term);

			// Act
			var result = sut.FromString(input);

			// Assert
			Assert.AreEqual(term, result);
		}

		[Test]
		public void ReadsPlaceholder()
		{
			// Arrange
			var sut = CreateSUT();
			var term = Factory.Placeholder(Factory.Int(0));
			string input = this.CreateWriter().ToString(term);

			// Act
			var result = sut.FromString(input);

			// Assert
			Assert.AreEqual(term, result);
		}
	}
}
