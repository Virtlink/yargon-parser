using Moq;
using NUnit.Framework;

namespace Spoofax.Terms
{
	public abstract class IRealTermTests : TestBase
	{
		/// <summary>
		/// Creates the Subject Under Test.
		/// </summary>
		/// <param name="value">The value of the term.</param>
		/// <returns>The instance to test.</returns>
		public abstract IRealTerm CreateSUT(float value);

		[Test]
		public void HasExpectedPropertyValues()
		{
			// Arrange
			const float value = 4.2f;
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
			const float value = 4.2f;
			var sut = CreateSUT(value);

			// Act
			var result = sut.ToString();

			// Assert
			Assert.AreEqual("4.2", result);
		}

		[Test]
		public void TwoEqualTermsAreEqual()
		{
			// Arrange
			const float value = 4.2f;
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
			const float value = 4.2f;
			const float otherValue = 2.4f;
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
			var sut = CreateSUT(4.2f);
			var visitor = new Mock<ITermVisitor>();

			// Act
			sut.Accept(visitor.Object);

			// Assert
			visitor.Verify(v => v.VisitReal(sut));
		}
	}
}
