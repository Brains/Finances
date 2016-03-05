using Common;
using Microsoft.Practices.Unity;
using UI.Services;

namespace Loader.Factories
{
	public class AmountFactory : IAmountFactory
	{
		private readonly IUnityContainer container;

		public AmountFactory(IUnityContainer container)
		{
			this.container = container;
		}

		public IAmount Create(Record.Types type)
		{
			if (type == Record.Types.Shared)
				return container.Resolve<IAmount>("Shared");

			return container.Resolve<IAmount>();
		}
	}
}