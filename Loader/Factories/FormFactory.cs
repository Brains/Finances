using UI.ViewModels;

namespace Loader.Factories
{
	public class FormFactory : IFormFactory
	{
		public IForm Create()
		{
			return new Form();
		}
	}
}