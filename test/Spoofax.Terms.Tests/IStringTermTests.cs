using NUnit.Framework;
using Moq;

namespace Spoofax.Terms
{
	public abstract class IStringTermTests : TestBase
	{
		/// <summary>
		/// Creates the Subject Under Test.
		/// </summary>
		/// <param name="value">The value of the term.</param>
		/// <returns>The instance to test.</returns>
		public abstract IStringTerm CreateSUT(string value);

		[Test]
		public void HasExpectedPropertyValues()
		{
			// Arrange
			const string value = "abc";
			var sut = CreateSUT(value);

			// Assert
			Assert.AreEqual(value, sut.Value);
			Assert.IsEmpty(sut.Annotations);
			Assert.IsEmpty(sut.SubTerms);
		}

		[Test]
		public void ToStringReturnsAString()
		{
			// Arrange
			const string value = "abc";
			var sut = CreateSUT(value);

			// Act
			var result = sut.ToString();

			// Assert
			Assert.AreEqual("\"abc\"", result);
		}

		[Test]
		public void TwoEqualTermsAreEqual()
		{
			// Arrange
			const string value = "abc";
			var sut = CreateSUT(value);
			var other = CreateSUT(value);

			// Act
			var result = sut.Equals(other);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void TwoDifferentTermsAreDifferent()
		{
			// Arrange
			const string value = "abc";
			const string otherValue = "def";
			var sut = CreateSUT(value);
			var other = CreateSUT(otherValue);

			// Act
			var result = sut.Equals(other);

			// Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void CallsVisitorMethod()
		{
			// Arrange
			var sut = CreateSUT("abc");
			var visitor = new Mock<ITermVisitor>();

			// Act
			sut.Accept(visitor.Object);

			// Assert
			visitor.Verify(v => v.VisitString(sut));
		}
	}
}
