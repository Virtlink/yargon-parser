using NUnit.Framework;

namespace Spoofax.Terms
{
	partial class TrivialTermFactoryTests
	{
		[TestFixture]
		public sealed class RealTermTests : TestBase
		{
			#region SUT
			public TrivialTermFactory.RealTerm CreateSUT(float value)
			{
				return (TrivialTermFactory.RealTerm)Factory.Real(value);
			}

			public sealed class RealTerm_IRealTerm : IRealTermTests
			{
				public override IRealTerm CreateSUT(float value)
				{
					return new RealTermTests().CreateSUT(value);
				}
			}
			#endregion
		}
	}
}
