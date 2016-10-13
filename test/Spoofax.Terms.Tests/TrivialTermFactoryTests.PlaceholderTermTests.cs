using NUnit.Framework;

namespace Spoofax.Terms
{
	partial class TrivialTermFactoryTests
	{
		[TestFixture]
		public sealed class PlaceholderTermTests : TestBase
		{
			#region SUT
			public TrivialTermFactory.PlaceholderTerm CreateSUT(ITerm template)
			{
				return (TrivialTermFactory.PlaceholderTerm)Factory.Placeholder(template);
			}

			public sealed class PlaceholderTerm_IConsTerm : IPlaceholderTermTests
			{
				public override IPlaceholderTerm CreateSUT(ITerm template)
				{
					return new PlaceholderTermTests().CreateSUT(template);
				}
			}
			#endregion
		}
	}
}
