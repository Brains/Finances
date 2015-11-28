using Microsoft.Practices.Unity;
using UI.Interfaces;
using UI.ViewModels;

namespace Loader.Factories
{
	public class FormFactory : IFormFactory
	{
		private readonly IUnityContainer container;

		public FormFactory(IUnityContainer container)
		{
			this.container = container;
		}

		public IForm Create()
		{
			return container.Resolve<IForm>();
		}
	}
}