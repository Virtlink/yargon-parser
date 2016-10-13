using NUnit.Framework;
using Moq;

namespace Spoofax.Terms
{
	public abstract class IPlaceholderTermTests : TestBase
	{
		/// <summary>
		/// Creates the Subject Under Test.
		/// </summary>
		/// <param name="template">The template of the placeholder.</param>
		/// <returns>The instance to test.</returns>
		public abstract IPlaceholderTerm CreateSUT(ITerm template);

		[Test]
		public void HasExpectedPropertyValues()
		{
			// Arrange
			var template = Factory.Int(0);
			var sut = CreateSUT(template);

			// Assert
			Assert.AreEqual(new ITerm[] { template }, sut.SubTerms);
			Assert.IsEmpty(sut.Annotations);
		}

		[Test]
		public void ToStringReturnsAString()
		{
			// Arrange
			var template = Factory.Int(0);
			var sut = CreateSUT(template);

			// Act
			var result = sut.ToString();

			// Assert
			Assert.AreEqual("<0>", result);
		}

		[Test]
		public void TwoEqualTermsAreEqual()
		{
			// Arrange
			var template = Factory.Int(0);
			var sut = CreateSUT(template);
			var other = CreateSUT(template);

			// Act
			var result = sut.Equals(other);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void TwoDifferentTermsAreDifferent()
		{
			// Arrange
			var template0 = Factory.Int(0);
			var template1 = Factory.Int(1);
			var sut = CreateSUT(template0);
			var other = CreateSUT(template1);

			// Act
			var result = sut.Equals(other);

			// Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void CallsVisitorMethod()
		{
			// Arrange
			var sut = CreateSUT(Factory.Int(0));
			var visitor = new Mock<ITermVisitor>();

			// Act
			sut.Accept(visitor.Object);

			// Assert
			visitor.Verify(v => v.VisitPlaceholder(sut));
		}
	}
}
