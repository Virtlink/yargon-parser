using NUnit.Framework;

namespace Spoofax.Terms.Tests
{
	[TestFixture]
	public sealed partial class TrivialTermFactoryTests
	{
		#region SUT
		public TrivialTermFactory CreateSUT()
		{
			return new TrivialTermFactory();
		}

		[TestFixture]
		public sealed class TrivialTermFactory_TermFactory : TermFactoryTests
		{
			public override TermFactory CreateSUT()
			{
				return new TrivialTermFactoryTests().CreateSUT();
			}
		}
		#endregion
	}
}
